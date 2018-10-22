using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.dalModels
{
    //defining what an Order is and its attributes
    public class OrdersDO
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public string Username { get; set; }

        public DateTime Requested { get; set; }

        public DateTime Due { get; set; }

        public int? CrafterID { get; set; }

        public string Crafter { get; set; }

        public byte Status { get; set; }
    }
}
