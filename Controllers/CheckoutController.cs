using DS3_Sprint1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DS3_Sprint1.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        //
        // GET: /Checkout/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Orderd();
            decimal perc = 0.14M;

            var id = User.Identity.GetUserId();
            var cust = db.Users.ToList().Find(x => x.Id == id);

            TryUpdateModel(order);
            order.Username = User.Identity.Name;
            order.OrderDate = DateTime.Now;
            order.FullName = cust.fullname;
            order.Address = cust.address;
            order.Phone = cust.contactnumber;
            order.Email = cust.Email;
            order.BusName = cust.businessname;
            order.BusNum = cust.businessnumber;
            ShoppingCart s = new ShoppingCart();
            var cart1 = ShoppingCart.GetCart(this.HttpContext);
            order.ExclTotal = cart1.GetTotal();
            order.InclTotal = cart1.GetTotal() + (cart1.GetTotal() * (float)perc);
            order.Paid = false;

            db.Orderss.Add(order);
            db.SaveChanges();

            //Process the order
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.CreateOrder(order);

            Session["id"] = order.OrderId.ToString();
            return RedirectToAction("Complete", "Checkout");
            //return RedirectToAction("Index3", "Orders");
        }

        public ActionResult Complete()
        {
            var ordid = (string)Session["id"];
            var ord = Convert.ToInt16(ordid);

            var order = db.Orderss.ToList().Find(x => x.OrderId == ord);
            CustomerInvoice c = new CustomerInvoice();

            c.Address = order.Address;
            c.BusinessName = order.BusName;
            c.OrderId = ord;
            c.DeliveryDate = DateTime.Today;
            c.Email = order.Email;
            c.FullName = order.FullName;
            c.OrderDate = order.OrderDate;
            c.Phone = order.Phone;
            c.ExclTotal = order.ExclTotal;
            c.InclTotal = order.InclTotal;
            c.Username = order.Username;
            c.VatNumber = order.BusNum;

            db.customerInvoice.Add(c);
            db.SaveChanges();

            return RedirectToAction("Index3", "Orders");
            // Validate customer owns this order
            //bool isValid = db.Orderss.Any(o => o.OrderId == id && o.Username == User.Identity.Name);

            //if (isValid)
            //{
            //    return View(id);
            //}
            //else
            //{
            //    return View("Error");
            //}
        }
    }
}