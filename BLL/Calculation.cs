using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL
{
    //finding price total
    public class Calculation
    {
        public static OrdersBO PriceTotalCalculator(List<ItemsBO> calcItems)
        {
            int? totalPrice = 0;
            OrdersBO Order = new OrdersBO();
            foreach (ItemsBO item in calcItems)
            {
                
                totalPrice += item.Price;
            }
            Order.Pricetotal = totalPrice;
            return Order;
        }
        //todo: calculation: price totals, every user and single user
        //todo: calculation: defaulted payments
        //todo: calculation: uncompleted orders
    }
}
