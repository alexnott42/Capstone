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
//todo: partialview orders on users page
//todo: more comments
//todo: password hashing
//todo: make the dropdownlists on items change based on user selections