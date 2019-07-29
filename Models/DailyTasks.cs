using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class DailyTasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        public string Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime Stime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Finish Time")]
        public DateTime Etime { get; set; }

        public bool Complete { get; set; }

        public bool done { get; set; }

        [Display(Name = "Time Completed")]
        public string CompTime { get; set; }

        public List<Products> products { get; set; }
    }
}