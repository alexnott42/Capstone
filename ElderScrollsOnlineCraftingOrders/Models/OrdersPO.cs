using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class OrdersPO
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public DateTime Requested { get; set; }

        public DateTime Due { get; set; }

        public int CrafterID { get; set; }

        public byte Status { get; set; }

        public int PriceTotal { get; set; }
    }
}