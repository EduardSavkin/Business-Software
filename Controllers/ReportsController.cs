using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.ViewModels;
using DS3_Sprint1.Models;

namespace DS3_Sprint1.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetReport()
        {
            ReportsVM report = new ReportsVM();

            ViewBag.BusinessName = new SelectList(db.Users, "businessname", "businessname");

            return View(report);
        }


        public ActionResult GenerateReport()
        {
            ReportsVM report = new ReportsVM();

            //ViewBag.BusinessName = new SelectList(db.Users, "businessname", "businessname");
            var users = new List<string>();
            var Query = from q in db.Users
                        where q.businessname != "Employee" && q.businessname != "Admin" && q.businessname != null
                        orderby q.businessname
                        select q.businessname;
            users.AddRange(Query.Distinct());
            ViewBag.BusinessName = new SelectList(users);

            return View(report);
        }

        [HttpPost]
        public ActionResult GenerateReport(FormCollection form)
        {
            //if(ModelState.IsValid)
            //{
                List<Orderd> sales = new List<Orderd>();
                int range = 0;
                string period = form["Range"].ToString();
                string business = form["BusinessName"].ToString();

            ReportsVM rvm = new ReportsVM();
            rvm.BusinessName = business;
            rvm.Range = period;
            Session["rvm"] = rvm;

            switch (period)
                {
                    case "Week":
                        range = 7;
                        break;
                    case "Month":
                        range = 30;
                        break;
                    default:
                        range = 365;
                        break;
                }


                foreach (var s in db.Orderss)
                {
                    if (business.Equals(s.BusName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (DateTime.Now.Subtract(s.OrderDate).Days <= range)
                        {
                            sales.Add(s);
                        }
                    }
                }


            //int hours = range * 24;
            //TimeSpan time = new TimeSpan(hours, 0, 0);


            //foreach(var s in sales)
            //{
            //    SalesReport r = new SalesReport();

            // }
                Session["list"] = sales;
                return RedirectToAction("Report");
            //}

            //ViewBag.BusinessName = new SelectList(db.Users, "businessname", "businessname");

            //return View(report);
        }


        public ActionResult ReportList(string name)
        {
            ViewBag.Business = name;
            List<Orderd> para = (List<Orderd>) Session["list"];

            List<Orderd> report = new List<Orderd>();

            foreach(var o in para)
            {
                report.Add(o);
            }

            return View(report.AsEnumerable());
        }


        public ActionResult Report()
        {
            ReportsVM vm = (ReportsVM) Session["rvm"];

            ViewBag.Business = vm.BusinessName;
            List<Orderd> para = (List<Orderd>)Session["list"];

            List<Orderd> report = new List<Orderd>();

            foreach (var o in para)
            {
                report.Add(o);
            }

            return View(report);
        }





    }
}