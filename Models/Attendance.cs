using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [DataType(DataType.Time)]
        public DateTime TimeOut { get; set; }

        public bool ClockedIn { get; set; }

        public bool ClockedOut { get; set; }


    }
}