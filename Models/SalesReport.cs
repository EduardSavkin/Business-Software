using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class SalesReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTime Date { get; set; }

        [Display(Name = "Business Name")]
        public string Business { get; set; }

        [Display(Name = "Amount")]
        public float Amount { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}