using DS3_Sprint1.Models;
using DS3_Sprint1.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string name = null;

        public ActionResult Index()
        {
            //if (ModelState.IsValid)
            //{
            //    string userId = User.Identity.GetUserName();
            //    var res = db.Users.ToList().Find(x => x.Email == userId);
            //    if (res != null && res.Active == false)
            //    {
            //        res.Active = true;
            //        db.Entry(res).State = EntityState.Modified;
            //        db.SaveChanges();


            //        try
            //        {

            //            var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
            //            var recieveremail = new MailAddress(userId, "Reciever");
            //            var password = "hzea bfpq aihm vyiz";

            //            int date = DateTime.Now.Year;
            //            string thisyear = Convert.ToString(date);

            //            var sub = "Confirmation of Reactivation";
            //            var body = res.fullname + " , Welcome back to Elenas Delicacies. Your account is now a reactivated and can utilize our system for your own benefits. Please note that this email is auto generated. DO NOT reply to this email.  The Elenas Delicacies team " + thisyear + ". All rights reserved.";

            //            var smtp = new SmtpClient
            //            {
            //                Host = "smtp.gmail.com",
            //                Port = 587,
            //                EnableSsl = true,
            //                DeliveryMethod = SmtpDeliveryMethod.Network,
            //                UseDefaultCredentials = false,
            //                Credentials = new NetworkCredential(senderEmail.Address, password)
            //            };

            //            using (var msg = new MailMessage(senderEmail, recieveremail)
            //            {
            //                Subject = sub,
            //                Body = body
            //            }
            //            )
            //            {
            //                smtp.Send(msg);
            //            }


            //        }
            //        catch (Exception)
            //        {

            //        }
            //    }
            //}
            return View();
        }

        public ActionResult Index2(int? Id, int? page)
        {
            string userId = User.Identity.GetUserName();
            var name = db.Users.ToList().Find(x => x.Email == userId);
            string fname = name.fullname;
            ViewBag.Name = fname;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel();
            var count = db.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = db.Messages
                                       .OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (Id != null)
            {

                var replies = db.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = db.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }

                }
                else
                {
                    vm.Replies.Add(null);
                }


                ViewBag.MessageId = Id.Value;
            }

            return View(vm);
        }
        public ActionResult Index1(int? Id, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel();
            var count = db.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = db.Messages
                                       .OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (Id != null)
            {

                var replies = db.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = db.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }

                }
                else
                {
                    vm.Replies.Add(null);
                }


                ViewBag.MessageId = Id.Value;
            }

            return View(vm);
        }
        [HttpPost]
        //[Authorize]
        public ActionResult DeleteMessage(int messageId)
        {
            Message _messageToDelete = db.Messages.Find(messageId);
            db.Messages.Remove(_messageToDelete);
            db.SaveChanges();

            // also delete the replies related to the message
            var _repliesToDelete = db.Replies.Where(i => i.MessageId == messageId).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    db.Replies.Remove(rep);
                    db.SaveChanges();
                }
            }


            return View();
        }
        
       
        //[Authorize]
        public ActionResult DeletePost(int Id)
        {

            var _repliesToDelete = db.Replies.Where(i => i.Id == Id).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    db.Replies.Remove(rep);
                    db.SaveChanges();
                }
            }


            return View();
        }


        public ActionResult DeleteP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reply replys = db.Replies.Find(id);
            if (replys == null)
            {
                return HttpNotFound();
            }
            return View(replys);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("DeleteP")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedP(int id)
        {
            Reply replys = db.Replies.Find(id);
            db.Replies.Remove(replys);
            db.SaveChanges();
            return RedirectToAction("Index2");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message messages = db.Messages.Find(id);
            db.Messages.Remove(messages);
            db.SaveChanges();
            return RedirectToAction("Index2");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message messages = db.Messages.Find(id);
            name = messages.From;
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,MessageToPost,From,DatePosted")] Message messages)
        {
            //var name = db.Messages.Find(messages.Id);
            if (ModelState.IsValid)
            {
                
               // messages.From = messages.From;
                messages.DatePosted = DateTime.Now;
                db.Entry(messages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }
            return View(messages);
        }

        public ActionResult Edit2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reply replies = db.Replies.Find(id);
           // name = messages.From;
            if (replies == null)
            {
                return HttpNotFound();
            }
            return View(replies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "Id,MessageId,ReplyFrom,ReplyMessage,ReplyDateTime")] Reply replies)
        {
            //var name = db.Messages.Find(messages.Id);
            if (ModelState.IsValid)
            {

                // messages.From = messages.From;
                replies.ReplyDateTime = DateTime.Now;
                db.Entry(replies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }
            return View(replies);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Notifications()
        {
            var prods = db.Products.ToList();
            var Orders = db.OrderDetails.ToList();
            var del = db.delivery.ToList();

            List<string> prodOrders = new List<string>();

            int qty = 0;

            foreach (var p in prods)
            {
                foreach (var o in Orders)
                {
                    foreach (var d in del)
                    {
                        if (d.OrderId == o.OrderId)
                        {
                            if (d.DeliveryStatus == "Undelivered")
                            {
                                if (p.ProductId == o.ProductId)
                                {
                                    qty = qty + o.Quantity;
                                }
                            }
                        }
                    }
                }
                if (qty >= 1)
                {
                    prodOrders.Add(p.ProductName + ": Currently has  " + qty.ToString() + " Orders");
                }
                qty = 0;
            }
            return View(prodOrders);
        }

        [ChildActionOnly]
        public ActionResult NotifCount()
        {
            //var order = from o in db.OrderDetails.ToList()
            //     where o.Quantity >= 5
            //           select o;

            var prods = db.Products.ToList();
            var Orders = db.OrderDetails.ToList();

            List<string> prodOrders = new List<string>();

            int qty = 0;

            foreach (var p in prods)
            {
                foreach (var o in Orders)
                {
                    if (p.ProductId == o.ProductId)
                    {
                        qty = qty + o.Quantity;
                    }
                }

                if (qty >= 1)
                {
                    prodOrders.Add(p.ProductName + "    " + qty.ToString());
                }
            }
            ViewData["NotiCount"] = prodOrders.Count();

            return PartialView("NotifCount");
        }
    }
}