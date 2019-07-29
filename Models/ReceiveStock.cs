using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class ReceiveStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ReceivingID")]
        public int ReceivingId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        public float TotPrice { get; set; }

        [Display(Name = "SupplierID")]
        public int SupplierId { get; set; }

        public List<Suppliers> suppliers { get; set; }
    }
}