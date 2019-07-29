using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.Models;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

namespace DS3_Sprint1.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        string date = null;

        // GET: Orders
        public ActionResult Index(string sortOrder, string searchString, Orderd order)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var asd = from s in db.Orderss/*.Where(x=>x.Username==User.Identity.Name)*/
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                asd = asd.Where(s => s.FullName.Contains(searchString) || s.Phone.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    asd = asd.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    asd = asd.OrderBy(s => s.OrderDate);
                    break;
                case "date_desc":
                    asd = asd.OrderByDescending(s => s.OrderDate);
                    break;
                default:
                    asd = asd.OrderBy(s => s.FullName);
                    break;
            }
            return View(asd.ToList());
        }

        public ActionResult paidOrders(string sortOrder, string searchString, Orderd order)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var paid = from s in db.Orderss.Where(x => x.Paid == true && x.scheduled == false)
                       select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                paid = paid.Where(s => s.FullName.Contains(searchString) || s.Phone.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    paid = paid.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    paid = paid.OrderBy(s => s.OrderDate);
                    break;
                case "date_desc":
                    paid = paid.OrderByDescending(s => s.OrderDate);
                    break;
                default:
                    paid = paid.OrderBy(s => s.FullName);
                    break;
            }
            return View(paid.ToList());
            //var paid = db.Orderss.ToList().Where(x => x.Paid == "Y" && x.scheduled == false);
            //return View(paid);
        }

        public ActionResult OrderData(int? id)
        {
            Orderd order = db.Orderss.Find(id);

            Session["id"] = order.OrderId.ToString();
            Session["user"] = order.Username;
            Session["name"] = order.FullName;
            Session["add"] = order.Address;
            Session["phone"] = order.Phone;

            return View(order);
        }

        public ActionResult Index3(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var asd = from s in db.Orderss.Where(x => x.Username == User.Identity.Name)
                      select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                asd = asd.Where(s => s.FullName.Contains(searchString) || s.Phone.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    asd = asd.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    asd = asd.OrderBy(s => s.OrderDate);
                    break;
                case "date_desc":
                    asd = asd.OrderByDescending(s => s.OrderDate);
                    break;
                default:
                    asd = asd.OrderBy(s => s.FullName);
                    break;
            }
            return View(asd.ToList());
        }

        public ActionResult Index2(int id)
        {
            return View(db.OrderDetails.ToList().Where(x=>x.OrderId==id));
        }
        public ActionResult Index4(int id)
        {
            return View(db.OrderDetails.ToList().Where(x => x.OrderId == id));
        }
        //public ActionResult Index3(int id)
        //{
        //    var a = db.Orders.ToList().Where(x => x.OrderId == id);
        //    decimal total = 0;
        //    foreach (var item in a)
        //    {
        //        total = item.Total;
        //    }
        //    return Redirect("http://localhost:2551/payment.aspx?total");
        //}        

        public ActionResult CancelledOrders()
        {
            var orders = db.Orderss.ToList().FindAll(x => x.Status == "Cancelled");
            return View(orders);
        }

        public ActionResult ComfirmCancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderd order = db.Orderss.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("ComfirmCancel")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmed(int id)
        {
            Orderd order = db.Orderss.Find(id);
            var invoice = db.customerInvoice.ToList().Find(x => x.OrderId == order.OrderId);

            order.Status = "Cancelled";
            invoice.Status = "Cancelled";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index3");
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderd order = db.Orderss.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderd order = db.Orderss.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orderd order = db.Orderss.Find(id);
            var delivery = db.delivery.ToList().Find(x => x.OrderId == id);
            db.Orderss.Remove(order);
            if(delivery != null)
            {
                db.delivery.Remove(delivery);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
