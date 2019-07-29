using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class CustomerInvoice
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        //[Required(ErrorMessage = "First Name is required")]
        //[DisplayName("First Name")]
        //[StringLength(160)]
        //public string FirstName { get; set; }
        //[Required(ErrorMessage = "Last Name is required")]
        //[DisplayName("Last Name")]
        //[StringLength(160)]
        //public string LastName { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [DisplayName("Full Name")]
        [StringLength(160)]
        public string FullName { get; set; }

        [DisplayName("Business Name")]
        [StringLength(160)]
        public string BusinessName { get; set; }

        [DisplayName("Vat Number")]
        [StringLength(160)]
        public string VatNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; }
 
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DisplayName("Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Exclusive Total")]
        public float ExclTotal { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Inclusive Total")]
        public float InclTotal { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        public System.DateTime DeliveryDate { get; set; }

        public bool Paid { get; set; }

        public string Status { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public virtual Orderd order { get; set; }
        public virtual ICollection<Returns> Return { get; set; }
    }
}