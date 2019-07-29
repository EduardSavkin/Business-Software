using DS3_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class TotalReturns
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ReturnID")]
        public int totalID { get; set; }

        [Display(Name = "Customer Name")]
        public string Name { get; set; }

        [Display(Name = "Business Name")]
        public string Business { get; set; }

        [Display(Name = "Total Returns")]
        public int totalReturns { get; set; }

        public String Status { get; set; }

        public List<Returns> treturns { get; set; }
    }
}