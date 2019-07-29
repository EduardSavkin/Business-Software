using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public String ProductName { get; set; }
        public String Type { get; set; }
        public float Price { get; set; }

        [Display(Name = "Image")]
        public string image { get; set; }

        [Display(Name = "Description")]
        public String Desc { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Products Products { get; set; }
    }
}