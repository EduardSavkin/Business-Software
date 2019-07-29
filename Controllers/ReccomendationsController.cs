using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Controllers
{
    public class ReccomendationsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reccomendations
        public ActionResult Reccomendations(string prod)
        {
            var task2 = new List<string>();
            var Query2 = from q in db.Products orderby q.ProductId select q.ProductName;
            task2.AddRange(Query2);
            ViewBag.prod = new SelectList(task2);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reccomendations([Bind(Include = "Product,Description")] Recommendations Rec, string prod)
        {
            var task2 = new List<string>();
            var Query2 = from q in db.Products orderby q.ProductId select q.ProductName;
            task2.AddRange(Query2);
            ViewBag.prod = new SelectList(task2);

            var ss = db.Products.ToList().Find(x => x.ProductName == prod);

            Rec.dateSent = DateTime.Now;
            Rec.Product = prod;
            Rec.Sender = User.Identity.Name;

            if (ModelState.IsValid)

            {
                db.recommendations.Add(Rec);
                db.SaveChanges();
                return RedirectToAction("Successful");

            }
            return View(Rec);




        }

        //public ActionResult ViewReccomendations()
        //{
        //    return View(db.recommendations.ToList());
        //}

        public ActionResult ViewReccomendations(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var asd = from s in db.recommendations
                      select s;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    asd = asd.Where(s => s.FullName.Contains(searchString));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    asd = asd.OrderByDescending(s => s.Sender);
                    break;
                case "Date":
                    asd = asd.OrderBy(s => s.dateSent);
                    break;
                case "date_desc":
                    asd = asd.OrderByDescending(s => s.dateSent);
                    break;
                default:
                    asd = asd.OrderBy(s => s.Sender);
                    break;
            }
            return View(asd.ToList());
        }

        public ActionResult Successful()
        {
            return View();


        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recommendations I = db.recommendations.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            return View(I);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recommendations I = db.recommendations.Find(id);
            db.recommendations.Remove(I);
            db.SaveChanges();
            return RedirectToAction("ViewReccomendations");
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