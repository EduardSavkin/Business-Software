using DS3_Sprint1.Models;
using DS3_Sprint1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext storeDB = new ApplicationDbContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }

        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedItem = storeDB.Products.Single(x => x.ProductId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        //[HttpPost]
        //public ActionResult RemoveFromCart(int id)
        //{
        //    // Remove the item from the cart
        //    var cart = ShoppingCart.GetCart(this.HttpContext);

        //    // Get the name of the album to display confirmation
        //    int itemName = storeDB.Carts.Single(item => item.RecordId == id).lid;

        //    // Remove from cart
        //    int itemCount = cart.RemoveFromCart(id);

        //    // Display the confirmation message
        //    var results = new ShoppingCartRemoveModel
        //    {
        //        Message ="Item has been removed from your shopping cart.",
        //        CartTotal = cart.GetTotal(),
        //        CartCount = cart.GetCount(),
        //        ItemCount = itemCount,
        //        DeleteId = id
        //    };
        //    return Json(results);
        //}
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            var itemName = storeDB.Carts.Single(item => item.RecordId == id);

            // Remove from cart
            cart.RemoveFromCart(id);

            // Display the confirmation message
            //var results = new ShoppingCartRemoveModel
            //{
            //    Message = Server.HtmlEncode("Item has been removed from your shopping cart."),
            //    CartTotal = cart.GetTotal(),
            //    CartCount = cart.GetCount(),
            //    ItemCount = itemCount,
            //    DeleteId = id
            //};
            return RedirectToAction("Index", "ShoppingCart");
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            ViewData["CartTot"] = cart.GetTotal();
            return PartialView("CartSummary");
        }
    }
}