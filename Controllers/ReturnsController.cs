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
using System.Web.UI.WebControls;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace DS3_Sprint1.Controllers
{
    public class ReturnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Returns
        public ActionResult Index(String name)
        {
            var r = from q in db.returns.ToList() select q;

            if (!string.IsNullOrEmpty(name))
            {
                r = r.Where(s => s.Name.Contains(name));
            }

            return View(r);
        }

        public ActionResult Index2(int? id, string sub, string vat, string tot)
        {
            decimal t = 0;
            decimal perc = 0.14M;

            Returns r = db.returns.Find(id);

            List<ReturnItems> items = db.returnItems.Where(x => x.RId == r.ReturnId).ToList();

            var items2 = db.returnItems.Where(x => x.RId == r.ReturnId && x.Qty != 0).ToList();

            var invoice = db.customerInvoice.ToList().Find(x => x.InvoiceId == r.InvoiceId);

            var details = db.OrderDetails.ToList().Where(x => x.OrderId == invoice.OrderId);

            var prices = db.Products.ToList();

            for(int p = 0; p < prices.Count; p++)
            {
                foreach (var d in items)
                {
                    if (prices[p].ProductName == d.Item)
                    {
                        t += Convert.ToDecimal(prices[p].Price * items[p].Qty);
                    }
                }                
            }

            sub = t.ToString();
            decimal v = (t * perc);
            decimal convert = Convert.ToDecimal(sub);
            decimal amt = convert + v;
            vat = v.ToString();
            tot = amt.ToString();

            //var Invoice = from o in db.customerInvoice
            //              join o2 in db.OrderDetails
            //              on o.OrderId equals o2.OrderId
            //              where o.OrderId.Equals(o2.OrderId)
            //              select new Invoice { Customers = o, Orders = o2 }
            //              ;

            ViewBag.total = tot;
            ViewBag.subtot = sub;
            ViewBag.totvat = vat;

            ViewBag.inv = invoice;
            ViewBag.r = r;
            ViewBag.items = items2;

            return View(items);
        }

        public ActionResult Items(int? id)
        {
            var items = db.returnItems.ToList().Where(x => x.RId == id && x.Qty != 0);
            return View(items);
        }

        // GET: Returns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        // GET: Returns/Create
        public ActionResult Create(string Emp_Dept)
        {
            //var DeptList = new List<string>();
            //ApplicationDbContext adb = new ApplicationDbContext();
            //var DeptQuery = from q in adb.Users
            //                where (q.businessname != "Admin" && q.businessname != "Employee" && q.businessname != null)
            //                orderby q.businessname
            //                select q.businessname;
            //DeptList.AddRange(DeptQuery.Distinct());
            //ViewBag.Emp_Dept = new SelectList(DeptList);
            return View();
        }

        // POST: Returns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReturnId,msg")] Returns returns, string Emp_Dept)
        {
            if (ModelState.IsValid)
            {
                //TotalReturns tr = new TotalReturns();
                //var DeptList = new List<string>();
                //ApplicationDbContext adb = new ApplicationDbContext();
                //var DeptQuery = from q in adb.Users
                //                where (q.businessname != "Admin" && q.businessname != "Employee" && q.businessname != null)
                //                orderby q.businessname
                //                select q.businessname;
                //DeptList.AddRange(DeptQuery.Distinct());
                //ViewBag.Emp_Dept = new SelectList(DeptList);

                //var ss = adb.Users.ToList().Find(x => x.businessname == Emp_Dept);

                //TotalReturns mp = db.totalreturns.ToList().Find(x => x.Business == Emp_Dept);

                //if (mp != null)
                //{
                //    mp.totalReturns = mp.totalReturns + returns.Qty;

                //}
                //else
                //{
                //    tr.Business = Emp_Dept;
                //    tr.totalReturns = returns.Qty;
                //    tr.Status = "False";
                //    db.totalreturns.Add(tr);

                //}
                //returns.Name = Emp_Dept;
                //returns.UserNAME = ss.UserName;
                returns.Date = DateTime.Now.Date;
               
                
                db.returns.Add(returns);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(returns);
        }
        public async System.Threading.Tasks.Task<ActionResult> CustLotReturn()
        {



            TotalReturns tttlR = new TotalReturns();
            ApplicationDbContext ab = new ApplicationDbContext();
            var ss = ab.Users.ToList();
            List<ApplicationUser> ls = new List<ApplicationUser>();
            var dd = db.totalreturns.ToList();

            foreach (var item in ss)
            {
                foreach (var item1 in dd)
                {
                    if (item1.totalReturns > 50 && item.businessname == item1.Business)
                    {
                        ls.Add(item);

                        if (item1.Status == "False") {

                            var from = "elenasdelicacies2@gmail.com";
                            var pass = "Elenas02#";
                            MailMessage message = new MailMessage();
                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential(from, pass);
                            client.EnableSsl = true;


                            var mail = new MailMessage(from, item.Email);
                            mail.Subject = "Warning regarding exceeding returns";
                            mail.Body = "PLEASE NOTE: You have been returning excess stock back to Elenas Delicacies for just over a short period of time. <br/> You are therefore warned that shall you continue doing this, we will have to remove you from the system and cancel our business deal. <br/> We will contact you in order to discuess this issue further. Also you will not get another warning and the only next step will be to remove you from the system as mentioned above. <br/><br/> Regards <br/>Elenas Delicacies";
                            mail.IsBodyHtml = true;
                            item1.Status = "True";
                            db.SaveChanges();
                            client.Send(mail);

                        }


                    }
                }
            }



            return View(ls);
        }

        // GET: Returns/Edit/5
        public ActionResult Edit(int? id, string Emp_Dept)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }

            var DeptList = new List<string>();
            ApplicationDbContext adb = new ApplicationDbContext();
            var DeptQuery = from q in adb.Users
                            where (q.businessname != "Admin" && q.businessname != "Employee" && q.businessname != null)
                            orderby q.businessname
                            select q.businessname;
            DeptList.AddRange(DeptQuery.Distinct());
            ViewBag.Emp_Dept = new SelectList(DeptList);

            return View(returns);
        }

        // POST: Returns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReturnId,CustName,Qty,msg,Date,UserNAME")] Returns returns, string Emp_Dept)
        {            
            if (ModelState.IsValid)
            {
                var DeptList = new List<string>();
                ApplicationDbContext adb = new ApplicationDbContext();
                var DeptQuery = from q in adb.Users
                                where (q.businessname != "Admin" && q.businessname != "Employee" && q.businessname != null)
                                orderby q.businessname
                                select q.businessname;
                DeptList.AddRange(DeptQuery.Distinct());
                ViewBag.Emp_Dept = new SelectList(DeptList);

                //var ss = adb.Users.ToList().Find(x => x.businessname == Emp_Dept);

                //returns.CustName = Emp_Dept;
                //returns.UserNAME = ss.UserName;

                //int dif = 0;
                //ApplicationDbContext db2 = new ApplicationDbContext();
                //var current = db2.returns.ToList().Find(x => x.ReturnId == returns.ReturnId);
                //var tot = db.totalreturns.ToList().Find(x => x.Business == returns.CustName);
                //if (current.Qty > returns.Qty)
                //{
                //    dif = dif + (current.Qty - returns.Qty);
                //    tot.totalReturns = tot.totalReturns - dif;
                //}
                //else if (current.Qty < returns.Qty)
                //{
                //    dif = dif + (returns.Qty - current.Qty);
                //    tot.totalReturns = tot.totalReturns + dif;
                //}
                //else if (current.Qty == returns.Qty)
                //{
                //    tot.totalReturns = tot.totalReturns;
                //}

                db.Entry(returns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(returns);
        }

        // GET: Returns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        // POST: Returns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Returns returns = db.returns.Find(id);

            //TotalReturns tot = db.totalreturns.ToList().Find(x => x.Business == returns.CustName);
            //tot.totalReturns = tot.totalReturns - returns.Qty;

            //TotalReturns remove = db.totalreturns.ToList().Find(x => x.totalReturns == 0);
            //if (tot.totalReturns == 0)
            //{
            //    db.totalreturns.Remove(remove);
            //}

            db.returns.Remove(returns);
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
