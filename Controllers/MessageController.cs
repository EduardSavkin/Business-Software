using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SendGrid;
using System.Net;
using System.Net.Mail;
using DS3_Sprint1.Models;
using DS3_Sprint1.ViewModels;
//using SendGrid.Helpers.Mail;

namespace DS3_Sprint1.Controllers
{
    public class MessageController : ApplicationBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        [HttpPost]
        [Authorize]
        public ActionResult PostMessage(MessageReplyViewModel vm)
        {
            var username = User.Identity.Name;
            string fullName = "";
            int msgid = 0;
            if (!string.IsNullOrEmpty(username))
            {
                var user = dbContext.Users.SingleOrDefault(u => u.UserName == username);
                fullName = user.fullname;
            }
            Message messagetoPost = new Message();
            if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = vm.Message.Subject;
                messagetoPost.MessageToPost = vm.Message.MessageToPost;
                messagetoPost.From = fullName;

                db.Messages.Add(messagetoPost);
                db.SaveChanges();
                msgid = messagetoPost.Id;
            }

            return RedirectToAction("Index2", "Home", new { Id = msgid });
        }

        public ActionResult Create(string subje, string mes)
        {
            MessageReplyViewModel vm = new MessageReplyViewModel();
            return View(vm);
        }


        [HttpPost]
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> ReplyMessage(MessageReplyViewModel vm, int messageId)
        {
            var username = User.Identity.Name;
            string fullName = "";
            if (!string.IsNullOrEmpty(username))
            {
                var user = dbContext.Users.SingleOrDefault(u => u.UserName == username);
                fullName = user.fullname;
            }
            if (vm.Reply.ReplyMessage != null)
            {
                Reply _reply = new Reply();
                _reply.ReplyDateTime = DateTime.Now;
                _reply.MessageId = messageId;
                _reply.ReplyFrom = fullName;
                _reply.ReplyMessage = vm.Reply.ReplyMessage;
                db.Replies.Add(_reply);
                db.SaveChanges();
            }
            //reply to the message owner - using email template

            var messageOwner = db.Messages.Where(x => x.Id == messageId).Select(ssa => ssa.From).FirstOrDefault();
            var users = from user in dbContext.Users
                        orderby user.fullname
                        select new
                        {
                            FullName = user.name + " " + user.lastname,
                            UserEmail = user.Email
                        };

            var uemail = users.Where(x => x.FullName == messageOwner).Select(q => q.UserEmail).FirstOrDefault();
            //SendGridMessage replyMessage = new SendGridMessage();
            //replyMessage.From = new MailAddress(username);
            String sSubject = "Reply for your message :" + db.Messages.Where(i => i.Id == messageId).Select(x => x.Subject).FirstOrDefault();
            //replyMessage.Text = vm.Reply.ReplyMessage;


            //replyMessage.AddTo(uemail);



            //string credentials = "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";
            //var transportweb = new Web(credentials);
            //transportweb.DeliverAsync(replyMessage);
            //return RedirectToAction("Index", "Home", new { Id = messageId });

            //string apiKey = "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";

            //dynamic sendGridClient = new SendGridAPIClient(apiKey);

            //string Body = vm.Reply.ReplyMessage;

            //SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
            //SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(uemail);
            //Content content = new Content("text/plain", Body);
            //SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, sSubject, toEmail, content);

            //dynamic response = await sendGridClient.client.mail.send.post(requestBody: mail.Get());
            return RedirectToAction("Index2", "Home", new { Id = messageId });

        }
    }
}