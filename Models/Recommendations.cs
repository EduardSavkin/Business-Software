using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Recommendations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Product { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "From")]
        public string Sender { get; set; }

        [Display(Name = "Date Created")]
        public DateTime dateSent { get; set; }

        public List<Products> products { get; set; }
    }
}