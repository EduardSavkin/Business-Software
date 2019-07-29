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
    public class ClockingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int count = 0;
        // GET: Clocking
        public ActionResult Index()
        {


            string userId = User.Identity.GetUserName();
            var name = db.Users.ToList().Find(x => x.Email == userId);
            DateTime date = DateTime.Today;
            DateTime time = DateTime.Now;
            var ob = db.attendance.ToList();
            bool clocked = true;
            // int day2 = Convert.ToInt16(ob.Date.Day);
            //int day = date.Day - day2;
            Attendance obj = new Attendance();
            //  int i = DateTime.Compare(ob.Date, date);
            //foreach (var a in ob)
            //{

            //    if (a.Name == name.fullname && a.ClockedIn == true && a.Date == date)
            //    {
            //        ViewBag.Message = "You have clocked in already";
            //        clocked = true;
            //    }
            //    else
            //    {
            //        clocked = false;
            //    }


            //}
            ////   return RedirectToAction("Index", "Home");
            //if (clocked == false)


            if (count == 0)
            {
                obj.Name = Convert.ToString(name.fullname);
                obj.Date = date;
                obj.Time = time;
                obj.TimeOut = Convert.ToDateTime("5:00:00 PM");
                obj.ClockedIn = true;
                obj.ClockedOut = false;

                db.attendance.Add(obj);
                db.SaveChanges();
                count++;
            }


            return RedirectToAction("Index", "Home");



        }
    }
}


