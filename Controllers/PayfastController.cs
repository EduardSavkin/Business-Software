using ElenasDelicacies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.Controllers;
using DS3_Sprint1.Models;

namespace ElenasDelicacies.Controllers
{
    public class PayfastController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Notification()
        {
            return View();
        }

        //public ActionResult Index(string status)
        //{
        //    List<SelectListItem> ObjItem = new List<SelectListItem>()
        //    {
        //        new SelectListItem {Text="Select payment method",Value="0",Selected=true },
        //        new SelectListItem {Text="Pay online",Value="1" },
        //        new SelectListItem {Text="Pay on delivery",Value="2"},
        //    };
        //    ViewBag.ListItem = ObjItem;
        //    return View();
        //}
    }
}
