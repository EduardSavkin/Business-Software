using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public partial class Orderd
    {
        [ScaffoldColumn(false)]
        [Key]
        public int OrderId { get; set; }
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Full Name is required")]
        [DisplayName("FullName")]
        [StringLength(160)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Delivery Address is required")]
        [DisplayName("Delivery Address")]
        [StringLength(70)]
        public string Address { get; set; }
 

        public string Phone { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DisplayName("Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [ScaffoldColumn(false)]

        [DisplayName("Business Name")]
        [StringLength(160)]
        public string BusName { get; set; }

        [DisplayName("Business Number")]
        [StringLength(160)]
        public string BusNum { get; set; }

        [DisplayName("Exclusive Total")]
        public float ExclTotal { get; set; }

        [DisplayName("Inclusive Total")]
        public float InclTotal { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        //[DisplayName("Payment Method")]
        //[StringLength(160)]
        //public string PmtMethod { get; set; }

        public bool Paid { get; set; }

        public bool scheduled { get; set; }

        public string Status { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<CustomerInvoice> customerInvoice { get; set; }
    }
}