using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DS3_Sprint1.Models;
using System.Web.Mvc;

namespace DS3_Sprint1.ViewModels
{
    public class DreamProdIngredientsViewModel
    {
        public DreamProducts DreamProduct { get; set; }
        public IEnumerable<SelectListItem> SupplyList { get; set; }

        private List<int> Supplies;
        public List<int> supplies
        {
            get
            {
                if (Supplies == null)
                {
                    Supplies = DreamProduct.Supplies.Select(m => m.SupplyId).ToList();
                }
                return Supplies;
            }
            set { Supplies = value; }
        }
    }
}