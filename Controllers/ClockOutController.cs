using DS3_Sprint1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Controllers
{
    public class ClockOutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int count = 0;
        // GET: ClockOut
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserName();
            var details = db.Users.ToList().Find(x => x.UserName == userId);
            var name = db.attendance.ToList();
            DateTime today = DateTime.Today;
            DateTime time = DateTime.Now;
            Attendance ob = db.attendance.ToList().Find(x => x.Name == details.fullname);

         
                if (count == 0)
                {
                    ob.TimeOut = time;
                    ob.ClockedOut = true;
                    db.Entry(ob).State = EntityState.Modified;
                    db.SaveChanges();
                    count++;
                   
            
            }
            return RedirectToAction("Index", "Home");
        }
    }
}