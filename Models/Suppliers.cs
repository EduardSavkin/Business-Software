using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Suppliers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "SupplierID")]
        public int SupplierId { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public ICollection<Supplies> supplies { get; set; }
        public ICollection<ReceiveStock> receivestock { get; set; }
    }
}