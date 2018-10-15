using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.dalModels
{
    //Defining what a User is and its attributes
    public class UsersDO
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ESOname { get; set; }

        public byte RoleID { get; set; }

        public string Server { get; set; }
    }
}
