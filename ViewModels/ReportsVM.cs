using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS3_Sprint1.ViewModels
{
    public class ReportsVM
    {
        [Required, Display(Name = "Business Name")]
        public string BusinessName { get; set;  }

        [Required, Display(Name = "Range")]
        public string Range { get; set; }
      
    }
}
