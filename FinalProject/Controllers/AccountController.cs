using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext db = new AppDbContext();

        #region user registration 

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User t)
        {
            User u = new User();
            if (ModelState.IsValid)
            {
                u.Name = t.Name;
                u.Email = t.Email;
                u.Password = t.Password;
                u.RoleType = 2;
                db.Users.Add(u);
                db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["msg"] = "Not Register!!";
            }
            return View();
        }

        #endregion

        #region user login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User t)
        {
            var query = db.Users.SingleOrDefault(m => m.Email == t.Email && m.Password == t.Password);
            if (query != null)
            {
                if (query.RoleType == 1)
                {
                    Session["uid"] = query.UserId;
                    FormsAuthentication.SetAuthCookie(query.Email, false);
                    Session["User"] = query.Name;
                    return RedirectToAction("Index", "Home");
                }
                else if (query.RoleType == 2)
                {
                    Session["uid"] = query.UserId;
                    FormsAuthentication.SetAuthCookie(query.Email, false);
                    Session["User"] = query.Name;
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                TempData["msg"] = "Invalid Username or Password";
            }

            return View();
        }

        #endregion

        #region logout 

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}