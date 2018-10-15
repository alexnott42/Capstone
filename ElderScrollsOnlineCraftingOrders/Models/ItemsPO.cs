using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class ItemsPO
    {
        public int ItemID { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public string Trait { get; set; }

        public string Style { get; set; }

        public string Set { get; set; }

        public string Level { get; set; }

        public string Quality { get; set; }

        public int OrderID { get; set; }

        public int Price { get; set; }
    }
}