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
//TODO: create register view, create Users views, CREATE orders views, create items views