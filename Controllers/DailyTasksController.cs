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
    public class DailyTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DailyTasks
        public ActionResult Index(String name)
        {
            var d = from s in db.dailyTasks.ToList() select s;
            if (!string.IsNullOrEmpty(name))
            {
                d = d.Where(s => (s.Name == name));
            }
            return View(d);
        }

        public ActionResult Index2(String name)
        {
            var d = from s in db.dailyTasks.ToList() select s;
            if (!string.IsNullOrEmpty(name))
            {
                d = d.Where(s => (s.Name == name));
            }
            return View(d);
        }

        // GET: DailyTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTasks dailyTasks = db.dailyTasks.Find(id);
            if (dailyTasks == null)
            {
                return HttpNotFound();
            }
            return View(dailyTasks);
        }

        // GET: DailyTasks/Create
        public ActionResult Create(string name, string prod)
        {
            ApplicationDbContext cont = new ApplicationDbContext();

            var task = new List<string>();
            var Query = from q in cont.Users
                        where q.businessname == "Employee"
                        orderby q.fullname
                        select q.fullname;
            task.AddRange(Query.Distinct());
            ViewBag.name = new SelectList(task);

            var task2 = new List<string>();
            var Query2 = from q in cont.Products orderby q.ProductId select q.ProductName;
            task2.AddRange(Query2);
            ViewBag.prod = new SelectList(task2);

            return View();
        }

        // POST: DailyTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,ProductId,Product,Quantity,Name,Date,Stime,Etime,Complete")] DailyTasks dailyTasks, string name, string prod)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext cont = new ApplicationDbContext();

                var task = new List<string>();
                var Query = from q in cont.Users
                            where q.businessname == "Employee"
                            orderby q.fullname
                            select q.fullname;
                task.AddRange(Query.Distinct());
                ViewBag.name = new SelectList(task);

                var List2 = new List<string>();
                var Query2 = from q in cont.Products orderby q.ProductId select q.ProductName;
                List2.AddRange(Query2);
                ViewBag.prod = new SelectList(List2);

                var ss = cont.Products.ToList().Find(x => x.ProductName == prod);

                dailyTasks.Name = name;
                dailyTasks.Product = prod;
                dailyTasks.ProductId = ss.ProductId;
                dailyTasks.Complete = false;
                dailyTasks.done = false;

                db.dailyTasks.Add(dailyTasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dailyTasks);
        }

        // GET: DailyTasks/Edit/5
        public ActionResult Edit(int? id, string name, string prod)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTasks dailyTasks = db.dailyTasks.Find(id);
            if (dailyTasks == null)
            {
                return HttpNotFound();
            }

            ApplicationDbContext cont = new ApplicationDbContext();

            var task = new List<string>();
            var Query = from q in cont.Users
                        where q.businessname == "Employee"
                        orderby q.fullname
                        select q.fullname;
            task.AddRange(Query.Distinct());
            ViewBag.name = new SelectList(task);

            var task2 = new List<string>();
            var Query2 = from q in cont.Products orderby q.ProductId select q.ProductName;
            task2.AddRange(Query2);
            ViewBag.prod = new SelectList(task2);

            return View(dailyTasks);
        }

        // POST: DailyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,ProductId,Product,Quantity,Name,Date,Stime,Etime,Complete")] DailyTasks dailyTasks, string name, string prod)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext cont = new ApplicationDbContext();

                var task = new List<string>();
                var Query = from q in cont.Users
                            where q.businessname == "Employee"
                            orderby q.fullname
                            select q.fullname;
                task.AddRange(Query.Distinct());
                ViewBag.name = new SelectList(task);

                var List2 = new List<string>();
                var Query2 = from q in cont.Products orderby q.ProductId select q.ProductName;
                List2.AddRange(Query2);
                ViewBag.prod = new SelectList(List2);

                var ss = cont.Products.ToList().Find(x => x.ProductName == prod);

                dailyTasks.Name = name;
                dailyTasks.Product = prod;
                dailyTasks.ProductId = ss.ProductId;

                Products pro = db.Products.ToList().Find(x => x.ProductId == dailyTasks.ProductId);

                ApplicationDbContext db2 = new ApplicationDbContext();
                var daily = db2.dailyTasks.ToList().Find(x => x.TaskId == dailyTasks.TaskId && x.done == false);

                ApplicationDbContext db3 = new ApplicationDbContext();
                var current = db3.dailyTasks.ToList().Find(x => x.TaskId == dailyTasks.TaskId);

                //Points obj = new Points();
                //DateTime today = DateTime.Today;
                //DateTime time1 = DateTime.Now;

                //DateTime t2 = Convert.ToDateTime(t);
                //DateTime t1 = dailyTasks.Etime;
                //Points name1 = db.points.ToList().Find(x => x.Name == dailyTasks.Name);

                //int i = DateTime.Compare(t1, t2);
                //if (name1 != null)
                //{
                //    if (i < 0)
                //    {
                //        name1.EmPoints = name1.EmPoints + 1;

                //    }
                //    if (i == 0)
                //    {
                //        name1.EmPoints = name1.EmPoints + 0;

                //    }
                //    if (i > 0)
                //    {
                //        name1.EmPoints = name1.EmPoints - 1;

                //    }


                //}
                //else
                //{
                //    obj.Name = dailyTasks.Name;

                //    if (i < 0)
                //    {
                //        obj.EmPoints = obj.EmPoints + 1;
                //    }
                //    if (i == 0)
                //    {
                //        obj.EmPoints = obj.EmPoints + 0;
                //    }
                //    if (i > 0)
                //    {
                //        obj.EmPoints = obj.EmPoints - 1;
                //    }
                //}

                if (daily != null)
                {
                    if (dailyTasks.Complete == true)
                    {
                        pro.qtyh = pro.qtyh + dailyTasks.Quantity;
                        dailyTasks.done = true;
                    }
                }
                else if (dailyTasks.Complete == true)
                {
                    int dif = 0;

                    if (current.Quantity > dailyTasks.Quantity)
                    {
                        dif = dif + (current.Quantity - dailyTasks.Quantity);
                        pro.qtyh = pro.qtyh - dif;
                    }
                    else if (current.Quantity < dailyTasks.Quantity)
                    {
                        dif = dif + (dailyTasks.Quantity - current.Quantity);
                        pro.qtyh = pro.qtyh + dif;
                    }
                    else if (current.Quantity == dailyTasks.Quantity)
                    {
                        pro.qtyh = pro.qtyh;
                    }
                }

                db.Entry(dailyTasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dailyTasks);
        }

        public ActionResult EditEmp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTasks dailyTasks = db.dailyTasks.Find(id);
            if (dailyTasks == null)
            {
                return HttpNotFound();
            }
            return View(dailyTasks);
        }

        // POST: DailyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmp([Bind(Include = "TaskId,ProductId,Product,Quantity,Name,Date,Stime,Etime,Complete")] DailyTasks dailyTasks)
        {
            if (ModelState.IsValid)
            {
                Products pro = db.Products.ToList().Find(x => x.ProductId == dailyTasks.ProductId);

                var now = DateTime.Now;
                var time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
                var t = time.TimeOfDay;

                //Points obj = new Points();
                //DateTime today = DateTime.Today;
                //DateTime time1 = DateTime.Now;

                //DateTime t2 = Convert.ToDateTime(t);
                //DateTime t1 = dailyTasks.Etime;
                //Points name1 = db.points.ToList().Find(x => x.Name == dailyTasks.Name);

                //int i = DateTime.Compare(t1, t2);
                //if (name1 != null)
                //{
                //    if (i < 0)
                //    {
                //        name1.EmPoints = name1.EmPoints + 1;

                //    }
                //    if (i == 0)
                //    {
                //        name1.EmPoints = name1.EmPoints + 0;

                //    }
                //    if (i > 0)
                //    {
                //        name1.EmPoints = name1.EmPoints - 1;

                //    }


                //}
                //else
                //{
                //    obj.Name = dailyTasks.Name;

                //    if (i < 0)
                //    {
                //        obj.EmPoints = obj.EmPoints + 1;
                //    }
                //    if (i == 0)
                //    {
                //        obj.EmPoints = obj.EmPoints + 0;
                //    }
                //    if (i > 0)
                //    {
                //        obj.EmPoints = obj.EmPoints - 1;
                //    }
                //}

                if (dailyTasks.Complete == true)
                {
                    pro.qtyh = pro.qtyh + dailyTasks.Quantity;
                }

                dailyTasks.CompTime = t.ToString();
                db.Entry(dailyTasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }
            return View(dailyTasks);
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            db.dailyTasks.RemoveRange(db.dailyTasks.Where(c => c.Complete == true));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTasks dailyTasks = db.dailyTasks.Find(id);
            if (dailyTasks == null)
            {
                return HttpNotFound();
            }
            return View(dailyTasks);
        }

        // POST: ReceiveStock/Delete/5
        [HttpPost, ActionName("Delete2")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed2(int id)
        {
            DailyTasks dailyTasks = db.dailyTasks.Find(id);
            db.dailyTasks.Remove(dailyTasks);
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
