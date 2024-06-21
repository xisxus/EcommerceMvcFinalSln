using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class ProductsController : Controller
    {
        AppDbContext db = new AppDbContext();

        #region showing all products for admin 

        public ActionResult Index()
        {
            var query = db.Products.ToList();
            return View(query);
        }

        #endregion


        #region products add for admin

        public ActionResult Create()
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");
            return View();
        }



        [HttpPost]
        public ActionResult Create(Product p, HttpPostedFileBase Image)
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");


            if (ModelState.IsValid)
            {


                Product pro = new Product();
                pro.Name = p.Name;
                pro.Description = p.Description;
                pro.Unit = p.Unit;
                pro.Image = Image.FileName.ToString();
                pro.CategoryId = p.CategoryId;

                //image upload
                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));

                db.Products.Add(pro);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Product Not Upload";
            }
            return View();
        }


        #endregion


        #region edit products

        public ActionResult Edit(int id)
        {

            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");

            var query = db.Products.SingleOrDefault(m => m.ProductId == id);

            return View(query);
        }


        [HttpPost]
        public ActionResult Edit(Product p, HttpPostedFileBase Image)
        {
            List<Category> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CategoryId", "Name");

            try
            {

                p.Image = Image.FileName.ToString();
                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                db.Entry(p).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }

            return RedirectToAction("Index");

        }

        #endregion


        #region delete product 

        public ActionResult Delete(int id)
        {
            var query = db.Products.SingleOrDefault(m => m.ProductId == id);
            db.Products.Remove(query);

            db.SaveChanges();


            return RedirectToAction("Index");

        }

        #endregion
    }
}