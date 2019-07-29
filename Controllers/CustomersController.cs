using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElenasDelicacies.Models
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationDbContext db1 = new ApplicationDbContext();

        // GET: BusinessCustomers

        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var users = context.Users.ToList().Where(x => x.businessname != null && x.businessname != "Admin" && x.businessname != "Employee" && x.businessname != "Driver");
            return View(users);
        }

        public ActionResult Index2()
        {
            var context = new ApplicationDbContext();
            var users = context.Users.ToList().Where(x => x.businessname == null && x.businessname == null);
            return View(users);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db1.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        public ActionResult Details2(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db1.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,fullname,contactnumber,address,businessname,businessnumber,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db1.Users.Add(applicationUser);
                db1.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db1.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fullname,contactnumber,address,businessname,businessnumber,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            try {
                if (ModelState.IsValid)
                {
                    db1.Entry(applicationUser).State = EntityState.Modified;
                    db1.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            return View(applicationUser);
            
            //if (ModelState.IsValid)
            //{
            //    db1.Entry(applicationUser).State = EntityState.Modified;
            //    db1.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit2(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db1.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "Id,fullname,contactnumber,address,businessname,businessnumber,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db1.Entry(applicationUser).State = EntityState.Modified;
                    db1.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            return View(applicationUser);

            //if (ModelState.IsValid)
            //{
            //    db1.Entry(applicationUser).State = EntityState.Modified;
            //    db1.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db1.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db1.Users.Find(id);
            db1.Users.Remove(applicationUser);
            db1.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete2(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db1.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete2")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed2(string id)
        {
            ApplicationUser applicationUser = db1.Users.Find(id);
            db1.Users.Remove(applicationUser);
            db1.SaveChanges();
            return RedirectToAction("Index2");
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
