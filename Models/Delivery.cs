using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Models
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryID { get; set; }

        [Display(Name = "Order Number")]
        public int OrderId { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Scheduled Date")]
        [DataType(DataType.Date)]
        public string sched { get; set; }

        public IEnumerable<SelectListItem> TypeList
        {
            get
            {
                return new List<SelectListItem>
        {
            new SelectListItem { Text = "Undelivered", Value = "Undelivered", Selected=true },
            new SelectListItem { Text = "Ready", Value = "Ready"},
            new SelectListItem { Text = "Delivered", Value = "Delivered"}
        };
            }
        }

        [Display(Name = "Delivery Status")]
        public string DeliveryStatus { get; set; }

        public string Driver { get; set; }
    }
}