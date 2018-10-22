using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElderScrollsOnlineCraftingOrders.Controllers
{

    public class HomeController : Controller
    {
         
        public ActionResult Index()
        {
            return View();
        }


    }
}
//TODO: CREATE orders views, create items views
//todo: confirmations on deletes
//todo: partialview orders on users page
//todo: create item page linked from order details page
//todo: update item page
//todo: delete item link
//todo: create order page
//todo: update order page
//todo: delete order button
//todo: items on order details