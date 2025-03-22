using ProductManagementDotNetTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ProductManagementDotNetTask.Models;
using System.Web.UI;
using System.IO;

namespace ProductManagementDotNetTask.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        DotNetTasksEntities  DB = new DotNetTasksEntities();    
        public ActionResult Index()
        {
            List<tblproduct> lst = DB.tblproducts.ToList();
            return View(lst);
        }

        public ActionResult Create() { //  To display Create View

            ViewBag.Categories = new List<string> { "Electronics", "Clothing", "Books", "Furniture" };
            return View();
   
        }
        [HttpPost]

        public ActionResult Create(tblproduct tbp,HttpPostedFileBase [] photo) {
            //if (photo.ContentLength>0) {
            //    String PhotoName = tbp.Name + Path.GetExtension(photo.FileName);
            //    string Imgpath = Server.MapPath("~/Photos/"+PhotoName);
            //    photo.SaveAs(Imgpath);
            //    tbp.Images = PhotoName; 
            //}

            string imagenames = "";
            string folderpath = Server.MapPath("~/Photos/"+tbp.Name);
            if (!Directory.Exists(folderpath)) { 
                Directory.CreateDirectory(folderpath);  

            
            }
            int i = 1;
            foreach (HttpPostedFileBase h in photo) { 
                string ImgName = tbp.Name+Path.GetFileName(h.FileName);
                string Imgpath = folderpath + "/" + ImgName;
                h.SaveAs(Imgpath);
                i++;
                imagenames = imagenames + "," + ImgName;
            }
            imagenames = imagenames.Substring(1, imagenames.Length - 1);
            tbp.Images = imagenames;
            DB.tblproducts.Add(tbp);
            DB.SaveChanges();   
            DB.tblproducts.Add(tbp);
            DB.SaveChanges();
            return RedirectToAction("Index");  
        }

        public ActionResult Edit(int Id) {
            tblproduct t = DB.tblproducts.Find(Id);
            DB.SaveChanges();
            


            return View(t);
        }
        [HttpPost]
        public ActionResult Edit(tblproduct tbp) {
            DB.Entry<tblproduct>(tbp).State= EntityState.Modified;
            DB.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id) {

            tblproduct t = DB.tblproducts.Find(Id);
                DB.tblproducts.Remove(t);
            DB.SaveChanges();

            return RedirectToAction("Index");

        
        }

        

    }
}