using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.Models;
using SelectPdf;
using System.Data.Entity.Validation;
using System.Text;

namespace DS3_Sprint1.Controllers
{
    public class CustomerInvoicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerInvoies
        //public ActionResult Index()
        //{
        //    return View(db.customerInvoice.ToList());
        //}

        public ActionResult Index(String searchtext, String name, string Emp_Dept)
        {
            var DeptList = new List<string>();
            var DeptQuery = from q in db.customerInvoice
                            where q.BusinessName != null
                            orderby q.BusinessName
                            select q.BusinessName;
            var d = from s in db.customerInvoice.ToList() select s;

            DeptList.AddRange(DeptQuery.Distinct());

            ViewBag.Emp_Dept = new SelectList(DeptList);

            IList<CustomerInvoice> empList = new List<CustomerInvoice>();
            var emp = from q in db.customerInvoice.ToList() select q;

            if (!String.IsNullOrEmpty(Emp_Dept))
            {
                emp = emp.Where(s => s.BusinessName == Emp_Dept);
            }

            var myEmpList = emp.ToList();

            //foreach (var empData in myEmpList)
            //{
            //    empList.Add(new Supplies()
            //    {
            //        SupplyId = empData.SupplyId,
            //        SupplyName = empData.SupplyName,
            //        //Add line below 
            //        Barcode = empData.Barcode,
            //        Qty = empData.Qty,
            //        Type = empData.Type,
            //        SupName = empData.SupName,
            //        PurPrice = empData.PurPrice,
            //        SupplierId = empData.SupplierId,
            //    });
            //}

            if (!string.IsNullOrEmpty(name))
            {
                emp = emp.Where(s => s.FullName.Contains(name) || s.Phone.Contains(name));
            }

            return View(emp/*.ToList().Where(x => x.Paid == true)*/);
        }

        public ActionResult Index2(int? id, string sub, string vat, string tot, string paid)
        {
            CustomerInvoice cust = db.customerInvoice.Find(id);

            List<OrderDetail> ordList = db.OrderDetails.Where(x => x.OrderId.Equals(cust.OrderId)).ToList();

            sub = cust.ExclTotal.ToString();
            tot = cust.InclTotal.ToString();
            decimal total = Convert.ToDecimal(tot);
            decimal subtot = Convert.ToDecimal(sub);
            vat = (total - subtot).ToString();

            if(cust.Paid == true)
            {
                paid = "Y";
            }
            if(cust.Paid == false)
            {
                paid = "N";
            }

            //var Invoice = from o in db.customerInvoice
            //              join o2 in db.OrderDetails
            //              on o.OrderId equals o2.OrderId
            //              where o.OrderId.Equals(o2.OrderId)
            //              select new Invoice { Customers = o, Orders = o2 }
            //              ;

            ViewBag.paid = paid;
            ViewBag.total = tot;
            ViewBag.subtot = sub;
            ViewBag.totvat = vat;

            ViewBag.Customer = cust;
            ViewBag.Order = ordList;

            return View(ordList);
        }

        public ActionResult Index3(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var asd = from s in db.customerInvoice.Where(x => x.Username == User.Identity.Name)
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
            return View(asd.ToList()/*.Where(x => x.Paid == true)*/);
        }

        public ActionResult IndexReturns(String name/*, string Emp_Dept*/)
        {
            //var DeptList = new List<string>();
            //var DeptQuery = from q in db.customerInvoice
            //                where q.BusinessName != null
            //                orderby q.BusinessName
            //                select q.BusinessName;
            //var d = from s in db.customerInvoice.ToList() select s;

            //DeptList.AddRange(DeptQuery.Distinct());

            IList<CustomerInvoice> empList = new List<CustomerInvoice>();
            var emp = from q in db.customerInvoice.ToList() select q;

            //if (!String.IsNullOrEmpty(Emp_Dept))
            //{
            //    emp = emp.Where(s => s.BusinessName == Emp_Dept);
            //}

            var myEmpList = emp.ToList();

            if (!string.IsNullOrEmpty(name))
            {
                emp = emp.Where(s => s.FullName.Contains(name) || s.Phone.Contains(name)).OrderByDescending(x => x.OrderDate);
            }

            return View(emp);
        }

        public ActionResult ReturnInvoice(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReturnInvoice(Returns returns, int id)
        {
            if (ModelState.IsValid)
            {
                var invoice = db.customerInvoice.ToList().Find(x => x.InvoiceId == id);

                try
                {
                    returns.InvoiceId = invoice.InvoiceId;
                    returns.Date = DateTime.Now.Date;
                    returns.Name = invoice.FullName;
                    returns.Business = invoice.BusinessName;
                    returns.UserNAME = invoice.Username;

                    db.returns.Add(returns);
                    db.SaveChanges();

                    Session["Rid"] = returns.ReturnId.ToString();
                    Session["Iid"] = invoice.InvoiceId.ToString();

                    return RedirectToAction("AddItems");
                }
                catch (Exception)
                { }
            }
            return View(returns);
        }

        public ActionResult AddItems()
        {
            var idR = (string)Session["Rid"];
            var Rid = Convert.ToInt16(idR);

            var idI = (string)Session["Iid"];
            var Iid = Convert.ToInt16(idI);

            var invoice = db.customerInvoice.ToList().Find(x => x.InvoiceId == Iid);
            var returns = db.returns.ToList().Find(x => x.ReturnId == Rid);

            var ordList = db.OrderDetails.Where(x => x.OrderId.Equals(invoice.OrderId)).ToList();

            try
            {
                var list = new List<ReturnItems>();

                if (ordList != null)
                {
                    foreach (var item in ordList)
                    {
                        list.Add(new ReturnItems
                        {
                            RId = returns.ReturnId,
                            Item = item.name
                        });
                        db.returnItems.AddRange(list);
                    }

                    Session["id"] = Rid.ToString();
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
            return RedirectToAction("ReturnItems");
        }

        public ActionResult ReturnItems()
        {
            var idR = (string)Session["id"];
            var id = Convert.ToInt16(idR);

            return View(db.returnItems.ToList().Where(x => x.RId == id));
        }

        [HttpPost]
        public ActionResult ReturnItems(FormCollection c)
        {
            int i = 0;
            int a = 0;
            int sum = 0;
            string m = "";
            if (ModelState.IsValid)
            {
                var ReturnItemIDArray = c.GetValues("item.Id");
                var ItemNameArray = c.GetValues("item.Item");
                var QtyArray = c.GetValues("item.Qty");

                for (i = 0; i < ReturnItemIDArray.Count(); i++)
                {
                    ReturnItems items = db.returnItems.Find(Convert.ToInt32(ReturnItemIDArray[i]));
                    foreach (var q in QtyArray[i])
                    {
                        if (Convert.ToString(q) == null)
                        {
                            Exception e = new Exception();
                            m = e.Message;
                            ViewBag.m = m;
                        }
                        else if (Convert.ToString(q) != null)
                        {
                            items.Item = ItemNameArray[i];
                            items.Qty = Convert.ToInt16(QtyArray[i]);
                            db.Entry(items).State = EntityState.Modified;

                            //Returns r = db.returns.ToList().Find(x => x.ReturnId == items.RId);
                            //for (a = 0; a < QtyArray[i].Length; a++)
                            //{
                            //    foreach (var s in QtyArray[a])
                            //    {
                            //        if (Convert.ToString(s) != null)
                            //        {
                            //            sum += Convert.ToInt16(QtyArray[a]);

                            //            TotalReturns tr = new TotalReturns();
                            //            TotalReturns mp = db.totalreturns.ToList().Find(x => x.Name == r.Name);

                            //            if (mp != null)
                            //            {
                            //                mp.totalReturns = mp.totalReturns + sum;
                            //            }
                            //            else
                            //            {
                            //                tr.Name = r.Name;
                            //                tr.Business = r.Business;
                            //                tr.totalReturns = sum;
                            //                tr.Status = "False";
                            //                db.totalreturns.Add(tr);
                            //            }
                            //        }
                            //    }
                            //}

                            db.SaveChanges();
                        }
                    }

                }
                db.SaveChanges();
                return RedirectToAction("Index", "Returns");
            }
            return View(db.returnItems.ToList());
        }       

        // GET: CustomerInvoies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInvoice customerInvoie = db.customerInvoice.Find(id);
            if (customerInvoie == null)
            {
                return HttpNotFound();
            }
            return View(customerInvoie);
        }

        // GET: CustomerInvoies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerInvoies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,OrderId,Username,FullName,BusinessName,VatNumber,Address,Phone,Email,Total,OrderDate,DeliveryDate,Path")] CustomerInvoice customerInvoie)
        {
            if (ModelState.IsValid)
            {
                db.customerInvoice.Add(customerInvoie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerInvoie);
        }

        // GET: CustomerInvoies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInvoice customerInvoie = db.customerInvoice.Find(id);
            if (customerInvoie == null)
            {
                return HttpNotFound();
            }
            return View(customerInvoie);
        }

        // POST: CustomerInvoies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,OrderId,Username,FullName,BusinessName,VatNumber,Address,Phone,Email,Total,OrderDate,DeliveryDate,Path")] CustomerInvoice customerInvoie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerInvoie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerInvoie);
        }

        // GET: CustomerInvoies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInvoice customerInvoie = db.customerInvoice.Find(id);
            if (customerInvoie == null)
            {
                return HttpNotFound();
            }
            return View(customerInvoie);
        }

        // POST: CustomerInvoies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerInvoice customerInvoie = db.customerInvoice.Find(id);
            db.customerInvoice.Remove(customerInvoie);
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

        public ActionResult GetPdf()
        {
            var converter = new HtmlToPdf();
            var doc = converter.ConvertUrl("http://localhost:59130/CustomerInvoices/Index2");
            doc.Save(System.Web.HttpContext.Current.Response, true, "Invoice.pdf");
            doc.Close();

            return null;
        }
    }
}
