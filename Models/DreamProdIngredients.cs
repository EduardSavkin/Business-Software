using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class DreamProdIngredients
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DreamProdId { get; set; }

        public int SupplyId { get; set; }

        public string SupplyName { get; set; }

        public virtual DreamProducts DreamProducts { get; set; }
        public virtual Supplies Supplies { get; set; }
    }
}