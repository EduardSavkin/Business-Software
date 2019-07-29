using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.Models;
using System.Net.Mail;
using System.IO;
using Microsoft.AspNet.Identity;

namespace DS3_Sprint1.Models
{
    public class DeliveriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        string date = null;

        // GET: Deliveries
        public ActionResult Index(String name)
        {
            var d = from q in db.delivery.ToList() select q;

            if (!string.IsNullOrEmpty(name))
            {
                d = d.Where(s => s.FullName.Contains(name) || s.Phone.Contains(name));
            }

            return View(d);
        }

        public ActionResult Index2()
        {
            var id = User.Identity.GetUserId();
            var cust = db.Users.ToList().Find(x => x.Id == id);
            var delivery = db.delivery.ToList().Where(x => x.Driver == cust.fullname);
            return View(delivery);
        }

        // GET: Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.delivery.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.delivery.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: Deliveries/Create
        public ActionResult Create(string n)
        {
            var ordid = (string)Session["id"];
            var ord = Convert.ToInt16(ordid);
            var user = (string)Session["user"];
            var name = (string)Session["name"];
            var add = (string)Session["add"];
            var ph = (string)Session["phone"];

            var deliveries = db.delivery.ToList().Find(x => x.OrderId == ord);

            var driver = new List<string>();
            var Query = from q in db.Users
                        where q.businessname == "Driver"
                        orderby q.fullname
                        select q.fullname;
            driver.AddRange(Query.Distinct());
            ViewBag.n = new SelectList(driver);

            return View(deliveries);
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeliveryID,OrderId,Username,FullName,Address,Phone,sched,DeliveryStatus,Driver")] Delivery delivery, string n)
        {
            if (ModelState.IsValid)
            {
                var driver = new List<string>();
                var Query = from q in db.Users
                            where q.businessname == "Driver"
                            orderby q.fullname
                            select q.fullname;
                driver.AddRange(Query.Distinct());
                ViewBag.n = new SelectList(driver);

                var order = db.Orderss.ToList().Find(x => x.OrderId == delivery.OrderId);
                date = Convert.ToString(delivery.sched);
                order.scheduled = true;

                var ordid = (string)Session["id"];
                var ord = Convert.ToInt16(ordid);
                var user = (string)Session["user"];
                var name = (string)Session["name"];
                var add = (string)Session["add"];
                var ph = (string)Session["phone"];

                var deliveries = db.delivery.ToList().Find(x => x.OrderId == ord);

                delivery.OrderId = Convert.ToInt16(ord);
                delivery.Username = user.ToString();
                delivery.FullName = name.ToString();
                delivery.Address = add.ToString();
                delivery.Phone = ph.ToString();

                delivery.Driver = n;
                delivery.DeliveryStatus = "Undelivered";
                db.delivery.Add(delivery);
                db.SaveChanges();

                try
                {
                    var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                    var recieveremail = new MailAddress(delivery.Username, "Reciever");
                    var password = "hzea bfpq aihm vyiz";

                    int date = DateTime.Now.Year;
                    string thisyear = Convert.ToString(date);

                    var sub = "Confirmation of Delivery";
                    var body = "Dear " + delivery.FullName + " , This email is to confirm that your delivery for order number " + ordid + " is scheduled to be delivered on "+delivery.sched+". Regards";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    using (var msg = new MailMessage(senderEmail, recieveremail)
                    {
                        Subject = sub,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(msg);
                    }


                }
                catch (Exception)
                {

                }
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public ActionResult Edit(int? id, string n)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.delivery.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }

            var driver = new List<string>();
            var Query = from q in db.Users
                        where q.businessname == "Driver"
                        orderby q.fullname
                        select q.fullname;
            driver.AddRange(Query.Distinct());
            ViewBag.n = new SelectList(driver);

            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeliveryID,OrderId,Username,FullName,Address,Phone,sched,DeliveryStatus,Driver")] Delivery delivery, string n)
        {
            

            if (ModelState.IsValid)
            {
                var driver = new List<string>();
                var Query = from q in db.Users
                            where q.businessname == "Driver"
                            orderby q.fullname
                            select q.fullname;
                driver.AddRange(Query.Distinct());
                ViewBag.n = new SelectList(driver);

                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                string date2 = System.DateTime.Now.ToString("YYYY-MM-DD");
                
            if (delivery.DeliveryStatus.Equals("Ready"))
            {
                    try
                    {
                        var prods = db.Products.ToList();
                        var det = db.OrderDetails.ToList();
                        var ord = db.Orderss.ToList();

                        foreach (var p in prods)
                        {
                            foreach (var d in det)
                            {
                                foreach (var o in ord)
                                {
                                    if (d.OrderId == o.OrderId)
                                    {
                                        if (delivery.OrderId == o.OrderId)
                                        {
                                            if (o.OrderId == d.OrderId)
                                            {
                                                if (p.ProductId == d.ProductId)
                                                {
                                                    p.qtyh = p.qtyh - d.Quantity;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                    var recieveremail = new MailAddress(delivery.Username, "Reciever");
                    var password = "hzea bfpq aihm vyiz";

                    var sub = "Confirmation of Delivery";
                    var body = "Dear " + delivery.FullName + " , Order number " + delivery.OrderId + " is ready and will be delivered to you tomorrow. Regards";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    using (var msg = new MailMessage(senderEmail, recieveremail)
                    {
                        Subject = sub,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(msg);
                    }


                }
                catch (Exception)
                {

                }
            }

            else if (date != delivery.sched && delivery.DeliveryStatus.Equals("Undelivered"))
            {
                try
                {

                    var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                    var recieveremail = new MailAddress(delivery.Username, "Reciever");
                    var password = "hzea bfpq aihm vyiz";

                    var sub = "Confirmation of Delivery";
                    var body = "Dear " + delivery.FullName + " , This email is to inform you that the delivery of Order number " + delivery.OrderId + " has been changed and is scheduled to be delivered on " + delivery.sched + " On behalf of Elena`s Delicacies we apologize for any inconvience. Regards.";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    using (var msg = new MailMessage(senderEmail, recieveremail)
                    {
                        Subject = sub,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(msg);
                    }


                }
                catch (Exception)
                {

                }
            }
                delivery.Driver = n;
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delivery);
        }
        
        public ActionResult Edit2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.delivery.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "DeliveryID,OrderId,Username,FullName,Address,Phone,sched,DeliveryStatus,Driver")] Delivery delivery)
        {


            if (ModelState.IsValid)
            {
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                string date2 = System.DateTime.Now.ToString("YYYY-MM-DD");

                if (delivery.DeliveryStatus.Equals("Ready"))
                {
                    try
                    {
                        var prods = db.Products.ToList();
                        var det = db.OrderDetails.ToList();
                        var ord = db.Orderss.ToList();

                        foreach (var p in prods)
                        {
                            foreach (var d in det)
                            {
                                foreach (var o in ord)
                                {
                                    if (d.OrderId == o.OrderId)
                                    {
                                        if (delivery.OrderId == o.OrderId)
                                        {
                                            if (o.OrderId == d.OrderId)
                                            {
                                                if (p.ProductId == d.ProductId)
                                                {
                                                    p.qtyh = p.qtyh - d.Quantity;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                        var recieveremail = new MailAddress(delivery.Username, "Reciever");
                        var password = "hzea bfpq aihm vyiz";

                        var sub = "Confirmation of Delivery";
                        var body = "Dear " + delivery.FullName + " , Order number " + delivery.OrderId + " is ready and will be delivered to you tomorrow. Regards";

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };

                        using (var msg = new MailMessage(senderEmail, recieveremail)
                        {
                            Subject = sub,
                            Body = body
                        }
                        )
                        {
                            smtp.Send(msg);
                        }


                    }
                    catch (Exception)
                    {

                    }
                }

                else if (date != delivery.sched && delivery.DeliveryStatus.Equals("Undelivered"))
                {
                    try
                    {

                        var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                        var recieveremail = new MailAddress(delivery.Username, "Reciever");
                        var password = "hzea bfpq aihm vyiz";

                        var sub = "Confirmation of Delivery";
                        var body = "Dear " + delivery.FullName + " , This email is to inform you that the delivery of Order number " + delivery.OrderId + " has been changed and is scheduled to be delivered on " + delivery.sched + " On behalf of Elena`s Delicacies we apologize for any inconvience. Regards.";

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };

                        using (var msg = new MailMessage(senderEmail, recieveremail)
                        {
                            Subject = sub,
                            Body = body
                        }
                        )
                        {
                            smtp.Send(msg);
                        }


                    }
                    catch (Exception)
                    {

                    }
                }
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.delivery.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Delivery delivery = db.delivery.Find(id);

            var order = db.Orderss.ToList().Find(x => x.OrderId == delivery.OrderId);
            order.scheduled = false;

            db.delivery.Remove(delivery);
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

        public ActionResult DueDeliveries()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return View(db.delivery.ToList().FindAll(x => x.sched.Equals(date)));
        }
    }
}
