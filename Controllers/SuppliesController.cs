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
    public class SuppliesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(String searchtext, string Emp_Dept)
        {
            var DeptList = new List<string>();
            var DeptQuery = from q in db.supplies orderby q.SupName select q.SupName;
            var d = from s in db.supplies.ToList() select s;

            DeptList.AddRange(DeptQuery.Distinct());

            ViewBag.Emp_Dept = new SelectList(DeptList);

            IList<Supplies> empList = new List<Supplies>();
            var emp = from q in db.supplies.ToList()
                      select q;

            if (!String.IsNullOrEmpty(Emp_Dept))
            {
                emp = emp.Where(s => s.SupName == Emp_Dept);
            }

            var myEmpList = emp.ToList();

            foreach (var empData in myEmpList)
            {
                empList.Add(new Supplies()
                {
                    SupplyId = empData.SupplyId,
                    SupplyName = empData.SupplyName,
                    //Add line below 
                    Barcode = empData.Barcode,
                    Qty = empData.Qty,
                    Type = empData.Type,
                    SupName = empData.SupName,
                    PurPrice = empData.PurPrice,
                    SupplierId = empData.SupplierId,
                });
            }
            return View(emp);
        }

        public ActionResult LowStock()
        {
            var List = db.supplies.ToList().FindAll(x => x.Qty <= 10);

            return View(List);
        }

        // GET: Supplies
        //public ActionResult Index()
        //{
        //    return View(db.supplies.ToList());
        //}

        // GET: Supplies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplies supplies = db.supplies.Find(id);
            if (supplies == null)
            {
                return HttpNotFound();
            }
            return View(supplies);
        }

        // GET: Supplies/Create
        public ActionResult Create(string Emp_Dept)
        {
            var DeptList = new List<string>();
            ApplicationDbContext edc = new ApplicationDbContext();
            var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
            DeptList.AddRange(DeptQuery.Distinct());
            ViewBag.Emp_Dept = new SelectList(DeptList);
            return View();
        }

        // POST: Supplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Barcode Scanner Code ... Copy entire method 
        public ActionResult Create([Bind(Include = "SupplyId,SupplyName,Barcode,Type,Qty,NetWeight,PurPrice,SupName,SupplierId")] Supplies supplies, string Emp_Dept)
        {
            if (ModelState.IsValid)
            {
                var supply = db.supplies.ToList().Find(x => x.Barcode == supplies.Barcode);
                if (supply == null)
                {
                    var DeptList = new List<string>();
                    ApplicationDbContext edc = new ApplicationDbContext();
                    var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
                    DeptList.AddRange(DeptQuery.Distinct());
                    ViewBag.Emp_Dept = new SelectList(DeptList);

                    var ss = edc.suppliers.ToList().Find(x => x.CompanyName == Emp_Dept);

                    supplies.SupName = Emp_Dept;
                    supplies.SupplierId = ss.SupplierId;

                    db.supplies.Add(supplies);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                   
                    var DeptList = new List<string>();
                    ApplicationDbContext edc = new ApplicationDbContext();
                    var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
                    DeptList.AddRange(DeptQuery.Distinct());
                    ViewBag.Emp_Dept = new SelectList(DeptList);

                    var ss = edc.suppliers.ToList().Find(x => x.CompanyName == Emp_Dept);

                    supplies.SupName = Emp_Dept;
                    supplies.SupplierId = ss.SupplierId;


                    ViewBag.Message = "Barcode In Use  ";
                }
            }

            return View(supplies);
        }

        // GET: Supplies/Edit/5
        public ActionResult Edit(int? id, string Emp_Dept)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplies supplies = db.supplies.Find(id);
            if (supplies == null)
            {
                return HttpNotFound();
            }

            var DeptList = new List<string>();
            ApplicationDbContext edc = new ApplicationDbContext();
            var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
            DeptList.AddRange(DeptQuery.Distinct());
            ViewBag.Emp_Dept = new SelectList(DeptList);

            return View(supplies);
        }

        // POST: Supplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplyId,SupplyName,Type,Qty,NetWeight,PurPrice,SupName,SupplierId,Barcode")] Supplies supplies, string Emp_Dept)
        {
            if (ModelState.IsValid)
            {
                var DeptList = new List<string>();
                ApplicationDbContext edc = new ApplicationDbContext();
                var DeptQuery = from q in edc.suppliers orderby q.CompanyName select q.CompanyName;
                DeptList.AddRange(DeptQuery.Distinct());
                ViewBag.Emp_Dept = new SelectList(DeptList);

                var ss = edc.suppliers.ToList().Find(x => x.CompanyName == Emp_Dept);

                supplies.SupName = Emp_Dept;
                supplies.SupplierId = ss.SupplierId;

                db.Entry(supplies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplies);
        }

        // GET: Supplies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplies supplies = db.supplies.Find(id);
            if (supplies == null)
            {
                return HttpNotFound();
            }
            return View(supplies);
        }

        // POST: Supplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplies supplies = db.supplies.Find(id);
            db.supplies.Remove(supplies);
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
