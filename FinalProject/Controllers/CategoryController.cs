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

        #region showing categories for admin

        public ActionResult Index()
        {
            var query = db.Categories.ToList();

            return View(query);
        }

        #endregion


        #region add categories

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

        #endregion


        #region edit category

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

        #endregion

        #region delete category

        public ActionResult Delete(int id)
        {
            var query = db.Categories.SingleOrDefault(m => m.CategoryId == id);
            db.Categories.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        #endregion
    }
}