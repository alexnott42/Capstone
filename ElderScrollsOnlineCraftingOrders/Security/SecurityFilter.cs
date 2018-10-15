using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElderScrollsOnlineCraftingOrders.Security
{
    public class SecurityFilter : ActionFilterAttribute
    {
        //this allows us to pass in a RoleID to check it
        private readonly int _Role;
        public SecurityFilter(int Role)
        {
            _Role = Role;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            if (session["Role"] is null || (session["Role"] != null && (int)session["Role"] < _Role))
            {
                filterContext.Result = new RedirectResult("/Account/Login", false);
            }

            base.OnActionExecuted(filterContext);
        }

    }
}