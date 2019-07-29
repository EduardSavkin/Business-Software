using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Products
    {
        public Products()
        {
            this.supplies = new HashSet<Supplies>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        public String ProductName { get; set; }
        public String Type { get; set; }
        public float Price { get; set; }

        [Display(Name ="Description")]
        public String Desc { get; set; }

        [Display(Name = "Barcode")]
        public string Ibarcode { get; set; }
        [Display(Name = "Image")]
        public string image { get; set; }
        [Display(Name = "QR Code")]
        public string qr { get; set; }

        [Display(Name = "Quantity On Hand")]
        public int qtyh { get; set; }

        public decimal NetWeight { get; set; }

        public String ChiliStat { get; set; }
        public String DairyStat { get; set; }
        public String NutsStat { get; set; }
        public String OliveStat { get; set; }

        public virtual ICollection<Supplies> supplies { get; set; }
    }
}