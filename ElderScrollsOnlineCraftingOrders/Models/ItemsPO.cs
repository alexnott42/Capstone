using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class ItemsPO
    {
        [Required]
        public int ItemID { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string SubType { get; set; }

        [Required]
        public string Trait { get; set; }

        [Required]
        public string Style { get; set; }

        [Required]
        public string Set { get; set; }

        [Required]
        public string Level { get; set; }

        [Required]
        public string Quality { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int? Price { get; set; }
    }
}