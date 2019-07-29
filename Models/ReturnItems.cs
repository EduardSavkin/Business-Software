using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class ReturnItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string Item { get; set; }

        //[Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal to or bigger than {0}")]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        public int RId { get; set; }

        public virtual CustomerInvoice invoice { get; set; }
        public virtual Returns returns { get; set; }
    }
}