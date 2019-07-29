using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DS3_Sprint1.Models
{
    public class Ingredients
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Ingredient { get; set; }

        public string SupplyName { get; set; }

        public virtual Supplies supplies { get; set; }
        public virtual Products products { get; set; }
    }
}