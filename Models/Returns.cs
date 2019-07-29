using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Returns
    {
        public Returns()
        {
            returnItems = new List<ReturnItems>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ReturnID")]
        public int ReturnId { get; set; }

        [Display(Name = "Customer Name")]
        public string Name { get; set; }

        [Display(Name = "Business Name")]
        public string Business { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Reason for return")]
        public string msg { get; set; }

        public float Amount { get; set; }

        [Display(Name = "UserName")]
        public string UserNAME { get; set; }

        [Display(Name = "Invoice ID")]
        public int InvoiceId { get; set; }

        public List<ReturnItems> returnItems { get; set; }
        public virtual CustomerInvoice Invoices { get; set; }
    }
}