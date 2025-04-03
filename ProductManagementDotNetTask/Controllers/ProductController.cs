using ProductManagementDotNetTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace ProductManagementDotNetTask.Controllers
{
    public class ProductController : Controller
    {
         DotNetTasksEntities DB = new DotNetTasksEntities();

        // GET: Product
        public ActionResult Index()
        {
            List<tblproduct> lst = DB.tblproducts.ToList();
            return View(lst);
        }

        // Display Create View
        public ActionResult Create()
        {
            ViewBag.Categories = new List<string> { "Electronics", "Clothing", "Books", "Furniture" };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblproduct tbp, HttpPostedFileBase[] photo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new List<string> { "Electronics", "Clothing", "Books", "Furniture" };
                return View(tbp);
            }

            string imagenames = "";
            string folderpath = Server.MapPath("~/Photos/" + tbp.Name);

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            if (photo != null && photo.Length > 0)
            {
                foreach (HttpPostedFileBase h in photo)
                {
                    if (h != null && h.ContentLength > 0)
                    {
                        string ImgName = tbp.Name + "_" + Path.GetFileName(h.FileName);
                        string Imgpath = Path.Combine(folderpath, ImgName);
                        h.SaveAs(Imgpath);
                        imagenames += "," + ImgName;
                    }
                }

                if (!string.IsNullOrEmpty(imagenames))
                {
                    imagenames = imagenames.Substring(1); // Remove leading comma
                }
            }

            tbp.Images = imagenames;
            DB.tblproducts.Add(tbp);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            tblproduct t = DB.tblproducts.Find(Id);
            if (t == null)
            {
                return HttpNotFound();
            }
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblproduct tbp)
        {
            if (!ModelState.IsValid)
            {
                return View(tbp);
            }

            DB.Entry(tbp).State = EntityState.Modified;
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            tblproduct t = DB.tblproducts.Find(Id);
            if (t == null)
            {
                return HttpNotFound();
            }

            DB.tblproducts.Remove(t);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
