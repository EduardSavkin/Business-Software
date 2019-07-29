using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public float CartTotal { get; set; }
    }
}