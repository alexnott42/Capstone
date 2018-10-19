using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class OrdersPO
    {
        [Required]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime Requested { get; set; }

        [Required]
        public DateTime Due { get; set; }

        public int CrafterID { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Status { get; set; }
    }
}