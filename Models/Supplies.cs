using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public partial class Supplies
    {
        public Supplies()
        {
            this.dreamProducts = new HashSet<DreamProducts>();
            this.products = new HashSet<Products>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "SupplyID")]
        public int SupplyId { get; set; }

        [Required]
        [Display(Name = "Supply Name")]
        public string SupplyName { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        [Required]
        [Display(Name = "Net Weight(kg)")]
        public decimal NetWeight { get; set; }

        [Required]
        [Display(Name = "Purchase Price")]
        public decimal PurPrice { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupName { get; set; }

        [Display(Name = "SupplierID")]
        public int SupplierId { get; set; }

        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        public List<Suppliers> suppliers { get; set; }
        public virtual ICollection<Products> products { get; set; }
        public virtual ICollection<DreamProducts> dreamProducts { get; set; }
    }
}