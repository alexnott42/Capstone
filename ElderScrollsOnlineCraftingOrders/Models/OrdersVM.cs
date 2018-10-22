using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class OrdersVM
    {
        public OrdersPO Order { get; set; }

        public List<ItemsPO> Items { get; set; }
    }
}