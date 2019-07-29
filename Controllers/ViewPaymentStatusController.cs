using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Controllers
{
    public class ViewPaymentStatusController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: ViewPaymentStatus
        public ActionResult Index()
        {
            return View();
        }

        public string updateStatus(int ordId, bool paid)
        {
            var order = db.Orderss.ToList().Find(x => x.OrderId == ordId);
            var invoice = db.customerInvoice.ToList().Find(x => x.OrderId == ordId);

            string msg = "Error, ";

            try
            {
                //var invoice = db.Orderss.ToList().Find(x => x.OrderId == ordId);
                //CustomerInvoice c = new CustomerInvoice();

                //c.Address = order.Address;
                //c.BusinessName = order.BusName;
                //c.OrderId = ordId;
                //c.DeliveryDate = DateTime.Today;
                //c.Email = order.Email;
                //c.FullName = order.FullName;
                //c.OrderDate = order.OrderDate;
                //c.Phone = order.Phone;
                //c.ExclTotal = order.ExclTotal;
                //c.InclTotal = order.InclTotal;
                //c.Username = order.Username;
                //c.VatNumber = order.BusNum;

                if (order != null)
                {
                    order.Paid = paid;
                    invoice.Paid = paid;
                    //db.customerInvoice.Add(c);
                    db.SaveChanges();
                    msg = "Your payment was successful. Thank you for buying from Elenas Delicacies";
                }
            }
            catch (Exception e)
            {
                msg = msg + e.Message;
            }
            return msg;
        }
    }
}