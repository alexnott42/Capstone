using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class UsersPO
    {
        public int UserID { get; set; }
        //todo: annotations

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ESOname { get; set; }

        public byte RoleID { get; set; }

        public string Server { get; set; }
    }
}