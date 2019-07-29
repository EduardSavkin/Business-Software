using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ElenasDelicacies.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public void Form(string recieverEmail)
        {
            try
            {

                var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                var recieveremail = new MailAddress(recieverEmail, "Reciever");
                var password = "hzea bfpq aihm vyiz";

                int date = DateTime.Now.Year;
                string thisyear = Convert.ToString(date);

                var sub = "Confirmation of Registration";
                var body = "Welcome to Elenas Delicacies. You are now a registered user and can utilize our system for your own benefits. Please note that this email is auto generated. DO NOT reply to this email.  The Elenas Delicacies team " + thisyear + ". All rights reserved.";

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
    }       
}