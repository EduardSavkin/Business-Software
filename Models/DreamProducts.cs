using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class DreamProducts
    {
        public DreamProducts()
        {
            this.Supplies = new HashSet<Supplies>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProdName { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ICollection<Supplies> Supplies { get; set; }
    }
}