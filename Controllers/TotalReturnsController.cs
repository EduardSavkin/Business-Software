using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.Models;
using ElenasDelicacies.Models;

namespace DS3_Sprint1.Controllers
{
    public class TotalReturnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TotalReturns
        public ActionResult Index(String Qty, String name)
        {
            var d = from s in db.totalreturns.ToList() select s;
            if (!string.IsNullOrEmpty(Qty))
            {
                int qty = Convert.ToInt16(Qty);
                d = d.Where(s => (s.totalReturns >= qty));
            }

            if (!string.IsNullOrEmpty(name))
            {
                d = d.Where(s => s.Business.Contains(name));
            }

            return View(d);
        }

        // GET: TotalReturns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalReturns totalReturns = db.totalreturns.Find(id);
            if (totalReturns == null)
            {
                return HttpNotFound();
            }
            return View(totalReturns);
        }

        // POST: TotalReturns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TotalReturns totalReturns = db.totalreturns.Find(id);
            db.totalreturns.Remove(totalReturns);
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
