using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
namespace DS3_Sprint1.Controllers
{
    public class EmpMonthController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Attendance obj = new Attendance();
        Points objj = new Points();
        // GET: EmpMonth
        public ActionResult Index(string name)
        {
            int maxAge = db.points.Select(p => p.EmPoints).DefaultIfEmpty(0).Max();

            var disp = db.points.ToList().Find(x => x.EmPoints == maxAge);

            ViewBag.name = disp.Name;

            //try
            //{
            //    if (DateTime.Now.Day == 09)
            //    {

            //        var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
            //        var recieveremail = new MailAddress(disp.UserName, "Reciever");
            //        var password = "hzea bfpq aihm vyiz";

            //        int date = DateTime.Now.Year;
            //        string thisyear = Convert.ToString(date);

            //        var sub = "Confirmation of Delivery";
            //        var body = "Dear " + disp.Name + " , This email is to confirm that you have been selected as the employee of the month. Congratulations!";

            //        var smtp = new SmtpClient
            //        {
            //            Host = "smtp.gmail.com",
            //            Port = 587,
            //            EnableSsl = true,
            //            DeliveryMethod = SmtpDeliveryMethod.Network,
            //            UseDefaultCredentials = false,
            //            Credentials = new NetworkCredential(senderEmail.Address, password)
            //        };

            //        using (var msg = new MailMessage(senderEmail, recieveremail)
            //        {
            //            Subject = sub,
            //            Body = body
            //        }
            //        )
            //        {
            //            smtp.Send(msg);
            //        }
            //    }
            //}
            //catch (Exception)
            //{ }

            return View(disp);
        }

    }
}