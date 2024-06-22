using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class CategoryController : Controller
    {
        AppDbContext db = new AppDbContext();

     

        public ActionResult Index()
        {
            var query = db.Categories.ToList();

            return View(query);
        }

      

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category c)
        {
            if (ModelState.IsValid)
            {
                Category cat = new Category();
                cat.Name = c.Name;
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Not Inserted Category";
            }
            return View();
        }

      

        public ActionResult Edit(int id)
        {
            var query = db.Categories.SingleOrDefault(m => m.CategoryId == id);
            return View(query);
        }

        [HttpPost]
        public ActionResult Edit(Category c)
        {
            try
            {

                db.Entry(c).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index");
        }

      

        public ActionResult Delete(int id)
        {
            var query = db.Categories.SingleOrDefault(m => m.CategoryId == id);
            db.Categories.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



      
    }
}
