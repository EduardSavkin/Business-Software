using DS3_Sprint1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DS3_Sprint1.Controllers
{
    public class DeleteCustomerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult ManageAcc()
        {
            return View();
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        // GET: DeleteCustomer
        public ActionResult Index(String pword)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));

            if (pword != null)
            {
                var user = userManager.Find(User.Identity.Name, pword);

                if (user != null)
                {

                    try
                    {

                        string userId = User.Identity.GetUserName();


                        var customer = db.Users.ToList().Find(x => x.Email == userId);
                        customer.Active = false;

                        db.Entry(customer).State = EntityState.Modified;
                        db.SaveChanges();

                        var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                        var recieveremail = new MailAddress(userId, "Reciever");
                        var password = "hzea bfpq aihm vyiz";

                        int date = DateTime.Now.Year;
                        string thisyear = Convert.ToString(date);

                        var sub = "Confirmation of Deactivation";
                        var body = "Dear " + customer.fullname + " , Your account has successfully been deactivated. Login to reactivate your account. Please note that this email is auto generated. DO NOT reply to this email.  The Elenas Delicacies team " + thisyear + ". All rights reserved.";

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
                    // ViewBag.Message = string.Format("Request for deleteion of your account has been sent to Admin, you will be informed shortly");
                    //  return new JavaScriptResult { Script = ("Your request to delete your account has been sent to Administration. You will be notified when your account has been deleted.") };
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    //return new JavaScriptResult { Script = ("Error one") };
                    ViewBag.Message = string.Format("Error, Either you have not entered your password OR your password is incorrect");
                }


            }
            return View();
            //return RedirectToAction("Index", "Home");
        }
    }
}
    