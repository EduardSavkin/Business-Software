using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Issues
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IssueId { get; set; }

        [Required]
        [Display(Name = "Issue Type")]
        public string IssueType { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "From")]
        public string Sender { get; set; }

        [Display(Name = "Date Created")]
        public DateTime dateSent { get; set; }
    }
}