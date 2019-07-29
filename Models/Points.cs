using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace DS3_Sprint1.Models
{
    public class Points
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Points")]
        public int EmPoints { get; set; }

        public string UserName { get; set; }
    }
}