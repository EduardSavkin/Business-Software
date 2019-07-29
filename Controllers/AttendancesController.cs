using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Models
{
    public class AttendancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attendances
        public ActionResult Index(string sortDate, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortDate) ? "name_desc" : "";
            ViewBag.DateSortParm = sortDate == "Date" ? "date_desc" : "Date";
            var asd = from s in db.attendance
                      select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                asd = asd.Where(s => s.Name.Contains(searchString));
            }
            switch (sortDate)
            {
                case "name_desc":
                    asd = asd.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    asd = asd.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    asd = asd.OrderByDescending(s => s.Date);
                    break;
                default:
                    asd = asd.OrderBy(s => s.Name);
                    break;
            }
            return View(asd.ToList());

            //return View(db.attendance.ToList());
        }

        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create(string name)
        {
            var task = new List<string>();
            var Query = from q in db.Users
                        where q.businessname == "Employee"
                        orderby q.fullname
                        select q.fullname;
            task.AddRange(Query.Distinct());
            ViewBag.name = new SelectList(task);

            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Date,Time")] Attendance attendance, string name)
        {
            if (ModelState.IsValid)
            {
                var task = new List<string>();
                var Query = from q in db.Users
                            where q.businessname == "Employee"
                            orderby q.fullname
                            select q.fullname;
                task.AddRange(Query.Distinct());
                ViewBag.name = new SelectList(task);

                DateTime finished = DateTime.Now.Date;

                Points obj = new Points();
                DateTime today = DateTime.Today;
                DateTime time = DateTime.Now;

                DateTime t2 = Convert.ToDateTime("7:30:00 AM");
                DateTime t1 = attendance.Time;
                Points name1 = db.points.ToList().Find(x => x.Name == name);

                int i = DateTime.Compare(t1, t2);
                if (name1 != null)
                {
                    if (i < 0)
                    {
                        name1.EmPoints = name1.EmPoints + 1;

                    }
                    if (i == 0)
                    {
                        name1.EmPoints = name1.EmPoints + 0;

                    }
                    if (i > 0)
                    {
                        name1.EmPoints = name1.EmPoints - 1;

                    }


                }
                else
                {
                    obj.Name = name;

                    if (i < 0)
                    {
                        obj.EmPoints = obj.EmPoints + 1;
                    }
                    if (i == 0)
                    {
                        obj.EmPoints = obj.EmPoints + 0;
                    }
                    if (i > 0)
                    {
                        obj.EmPoints = obj.EmPoints - 1;
                    }

                    db.points.Add(obj);
                    db.SaveChanges();
                }

                attendance.Name = name;
                attendance.Date = finished;
                db.attendance.Add(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id, string name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }

            var task = new List<string>();
            var Query = from q in db.Users
                        where q.businessname == "Employee"
                        orderby q.fullname
                        select q.fullname;
            task.AddRange(Query.Distinct());
            ViewBag.name = new SelectList(task);

            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Date,Time,TimeOut")] Attendance attendance, string name)
        {
            if (ModelState.IsValid)
            {
                var task = new List<string>();
                var Query = from q in db.Users
                            where q.businessname == "Employee"
                            orderby q.fullname
                            select q.fullname;
                task.AddRange(Query.Distinct());
                ViewBag.name = new SelectList(task);

                DateTime today = DateTime.Now.Date;

                attendance.Name = name;
                attendance.Date = today;
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.attendance.Find(id);
            db.attendance.Remove(attendance);
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
