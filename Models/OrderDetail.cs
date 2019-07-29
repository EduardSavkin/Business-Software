using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string name { get; set; }

        public string desc { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public virtual Products Products { get; set; }
        public virtual Orderd Order { get; set; }
    }
}