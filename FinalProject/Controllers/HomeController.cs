using FinalProject.Models;
using FinalProject.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        /* Database Connection  */
        AppDbContext db = new AppDbContext();

        /* Add to Cart List use */
        List<Cart> li = new List<Cart>();

        #region home page in showing all products 

        public ActionResult Index()
        {

            if (TempData["cart"] != null)
            {
                int x = 0;

                List<Cart> li2 = TempData["cart"] as List<Cart>;
                foreach (var item in li2)
                {
                    x += item.bill;

                }
                TempData["total"] = x;
                TempData["item_count"] = li2.Count();
            }
            TempData.Keep();

            var query = db.Products.ToList();
            return View(query);
        }

        #endregion

        #region add to cart

        public ActionResult AddtoCart(int id)
        {
            var query = db.Products.Where(x => x.ProductId == id).SingleOrDefault();
            return View(query);
        }

        [HttpPost]
        public ActionResult AddtoCart(int id, int qty)
        {
            Product p = db.Products.Where(x => x.ProductId == id).SingleOrDefault();
            Cart c = new Cart();
            c.proid = id;
            c.proname = p.Name;
            c.price = Convert.ToInt32(p.Unit);
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.proid == c.proid)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                    }

                }
                if (flag == 0)
                {
                    li2.Add(c);
                    //new item
                }
                TempData["cart"] = li2;

            }

            TempData.Keep();

            return RedirectToAction("Index");
        }

        #endregion

        #region remove cart item

        public ActionResult remove(int? id)
        {
            if (TempData["cart"] == null)
            {
                TempData.Remove("total");
                TempData.Remove("cart");
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                Cart c = li2.Where(x => x.proid == id).SingleOrDefault();
                li2.Remove(c);
                int s = 0;
                foreach (var item in li2)
                {
                    s += item.bill;
                }
                TempData["total"] = s;

            }

            return RedirectToAction("Index");
        }
        #endregion

        #region checkout code

        public ActionResult Checkout()
        {
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string contact, string address)
        {
            if (ModelState.IsValid)
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                Invoice iv = new Invoice();
                iv.UserId = Convert.ToInt32(Session["uid"].ToString());
                iv.InvoiceDate = System.DateTime.Now;
                iv.Bill = (int)TempData["total"];
                iv.Payment = "cash";
                db.Invoices.Add(iv);
                db.SaveChanges();
                //order book
                foreach (var item in li2)
                {
                    Order od = new Order();
                    od.ProductId = item.proid;
                    od.Contact = contact;
                    od.Address = address;
                    od.OrderDate = System.DateTime.Now;
                    od.InvoiceId = iv.InvoiceId;
                    od.Qty = item.qty;
                    od.Unit = item.price;
                    od.Total = item.bill;

                    db.Orders.Add(od);
                    db.SaveChanges();

                }
                TempData.Remove("total");
                TempData.Remove("cart");
                // TempData["msg"] = "Order Book Successfully!!";
                return RedirectToAction("Index");
            }

            TempData.Keep();
            return View();
        }

        #endregion


        #region all orders for admin 

        public ActionResult GetAllOrderDetail()
        {
            var result = db.Invoices
                .Join(db.Users,
                      invoice => invoice.UserId,
                      user => user.UserId,
                      (invoice, user) => new OrderUserVM
                      {
                          InvoiceId = invoice.InvoiceId,
                          UserId = user.UserId,
                          Name = user.Name,
                          Bill = invoice.Bill,
                          Payment = invoice.Payment,
                          InvoiceDate = invoice.InvoiceDate,
                          Status = invoice.Status
                      })
                .ToList();

            return View(result);
        }


        #endregion

        #region  confirm order by admin

        public ActionResult ConfirmOrder(int InvoiceId)
        {
            // Retrieve the invoice and user data for the given InvoiceId
            var result = db.Invoices
                .Where(inv => inv.InvoiceId == InvoiceId)
                .Select(inv => new OrderUserVM
                {
                    InvoiceId = inv.InvoiceId,
                    UserId = inv.UserId,
                    Name = inv.User.Name, // Assuming User is a navigation property in Invoice
                    Bill = inv.Bill,
                    Payment = inv.Payment,
                    InvoiceDate = inv.InvoiceDate,
                    Status = inv.Status
                })
                .SingleOrDefault();

            if (result == null)
            {
                // Handle case where invoice with given InvoiceId was not found
                return HttpNotFound(); // You can return HttpNotFoundResult or handle appropriately
            }

            return View(result);
        }


        [HttpPost]
        public ActionResult ConfirmOrder(OrderUserVM o)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var inv = db.Invoices.Find(o.InvoiceId);

                    if (inv != null)
                    {
                        inv.Status = 1; 
                        inv.Payment = o.Payment; 

                        db.Entry(inv).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home"); 
                    }
                    else
                    {
                        return HttpNotFound(); 
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error occurred while confirming the order. Please try again.");
                }
            }

            return View(o);
        }


        #endregion

        #region orders for only user

        public ActionResult OrderDetail(int id)
        {
            var result = db.Invoices
                .Join(db.Users,
                      invoice => invoice.UserId,
                      user => user.UserId,
                      (invoice, user) => new OrderUserVM
                      {
                          InvoiceId = invoice.InvoiceId,
                          UserId = user.UserId,
                          Name = user.Name,
                          Bill = invoice.Bill,
                          Payment = invoice.Payment,
                          InvoiceDate = invoice.InvoiceDate,
                          Status = invoice.Status
                      })
                .Where(x => x.UserId == id)
                .ToList();

            return View(result);
        }


        #endregion


        #region  get all users 
        [Authorize(Roles = "1")]
        public ActionResult GetAllUser()
        {
            var query = db.Users.ToList();
            return View(query);
        }

        #endregion



        #region invoice for  user

        public ActionResult Invoice(int id)
        {
            var result = db.Invoices
                .Join(db.Users,
                      invoice => invoice.UserId,
                      user => user.UserId,
                      (invoice, user) => new UserInvoiceVM
                      {
                          InvoiceId = invoice.InvoiceId,
                          Name = user.Name,
                          Bill = invoice.Bill,
                          Payment = invoice.Payment,
                          InvoiceDate = invoice.InvoiceDate
                      })
                .Where(m => m.InvoiceId == id)
                .ToList();

            return View(result);
        }


        #endregion

    }
}