using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElenasDelicacies.Models
{
    public class ReceiveStockController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(String searchtext, string Emp_Dept)
        {
            var DeptList = new List<string>();
            var DeptQuery = from q in db.receivestock orderby q.CompanyName select q.CompanyName;
            var d = from s in db.receivestock.ToList() select s;

            DeptList.AddRange(DeptQuery.Distinct());

            ViewBag.Emp_Dept = new SelectList(DeptList);

            IList<ReceiveStock> empList = new List<ReceiveStock>();
            var emp = from q in db.receivestock.ToList()
                      select q;

            if (!String.IsNullOrEmpty(Emp_Dept))
            {
                emp = emp.Where(s => s.CompanyName == Emp_Dept);
            }

            var myEmpList = emp.ToList();

            foreach (var empData in myEmpList)
            {
                empList.Add(new ReceiveStock()
                {
                    ReceivingId = empData.ReceivingId,
                    CompanyName = empData.CompanyName,
                    TotPrice = empData.TotPrice,
                    Date = empData.Date,
                    SupplierId = empData.SupplierId,
                });
            }
            return View(emp);


            //var d = from s in db.returns.ToList() select s;
            //if (!string.IsNullOrEmpty(searchtext))
            //{
            //    d = d.Where(s => s.CustName.ToUpper().Contains(searchtext.ToUpper()));
            //}
            //return View(d);
        }

        public ActionResult Index2()
        {
            return View(db.receivestock.ToList());
        }

        // GET: ReceiveStock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiveStock receiveStock = db.receivestock.Find(id);
            if (receiveStock == null)
            {
                return HttpNotFound();
            }
            return View(receiveStock);
        }

        // GET: ReceiveStock/Create
        public ActionResult Create(string Emp_Dept)
        {
            var DeptList = new List<string>();
            ApplicationDbContext edc = new ApplicationDbContext();
            var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
            DeptList.AddRange(DeptQuery.Distinct());
            ViewBag.Emp_Dept = new SelectList(DeptList);
            return View();
        }

        // POST: ReceiveStock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReceivingId,CompanyName,Date,TotPrice,SupplierId")] ReceiveStock receiveStock, string Emp_Dept)
        {
            if (ModelState.IsValid)
            {
                var DeptList = new List<string>();
                ApplicationDbContext edc = new ApplicationDbContext();
                var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
                DeptList.AddRange(DeptQuery.Distinct());
                ViewBag.Emp_Dept = new SelectList(DeptList);

                var ss = edc.suppliers.ToList().Find(x => x.CompanyName == Emp_Dept);

                receiveStock.CompanyName = Emp_Dept;
                receiveStock.SupplierId = ss.SupplierId;

                db.receivestock.Add(receiveStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(receiveStock);
        }

        // GET: ReceiveStock/Edit/5
        public ActionResult Edit(int? id, string Emp_Dept)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiveStock receiveStock = db.receivestock.Find(id);
            if (receiveStock == null)
            {
                return HttpNotFound();
            }

            var DeptList = new List<string>();
            ApplicationDbContext edc = new ApplicationDbContext();
            var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
            DeptList.AddRange(DeptQuery.Distinct());
            ViewBag.Emp_Dept = new SelectList(DeptList);

            return View(receiveStock);
        }

        // POST: ReceiveStock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReceivingId,CompanyName,Date,TotPrice,SupplierId")] ReceiveStock receiveStock, string Emp_Dept)
        {
            if (ModelState.IsValid)
            {
                var DeptList = new List<string>();
                ApplicationDbContext edc = new ApplicationDbContext();
                var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
                DeptList.AddRange(DeptQuery.Distinct());
                ViewBag.Emp_Dept = new SelectList(DeptList);

                var ss = edc.suppliers.ToList().Find(x => x.CompanyName == Emp_Dept);

                receiveStock.CompanyName = Emp_Dept;
                receiveStock.SupplierId = ss.SupplierId;

                db.Entry(receiveStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(receiveStock);
        }

        // GET: ReceiveStock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiveStock receiveStock = db.receivestock.Find(id);
            if (receiveStock == null)
            {
                return HttpNotFound();
            }
            return View(receiveStock);
        }

        // POST: ReceiveStock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceiveStock receiveStock = db.receivestock.Find(id);
            db.receivestock.Remove(receiveStock);
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
