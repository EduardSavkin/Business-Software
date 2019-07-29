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
    public class ReportedIssuesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ReportedIssues
        public ActionResult ReportAnIssue()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportAnIssue([Bind(Include = "IssueType,Description")] Issues issue)
        {

            issue.dateSent = DateTime.Now;

            issue.Sender = User.Identity.Name;

            if (ModelState.IsValid)

            {
                db.issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Successful");

            }
            return View(issue);
        }

        //public ActionResult ViewIssues()
        //{
        //    return View(db.issues.ToList());
        //}

        public ActionResult ViewIssues(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var asd = from s in db.issues
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
            Issues I = db.issues.Find(id);
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
            Issues I = db.issues.Find(id);
            db.issues.Remove(I);
            db.SaveChanges();
            return RedirectToAction("ViewIssues");
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