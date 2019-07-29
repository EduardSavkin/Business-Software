using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.ViewModels
{
    public class ProductIngredientsViewModel
    {
        public Products products { get; set; }
        public IEnumerable<SelectListItem> IngredientsList { get; set; }

        private List<int> Supplies;
        public List<int> supplies
        {
            get
            {
                if (Supplies == null)
                {
                    Supplies = products.supplies.Select(m => m.SupplyId).ToList();
                }
                return Supplies;
            }
            set { Supplies = value; }
        }
    }
}