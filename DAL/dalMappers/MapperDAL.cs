using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAL.dalModels;
namespace DAL.dalMappers
{
    public static class MapperDAL
    {

        //Getting User data from the database
        public static UsersDO ReaderToUser(SqlDataReader from)
        {

            UsersDO to = new UsersDO
            {
                UserID = from.GetInt32(0),
                Username = from.GetValue(1) as string,
                Email = from.GetValue(2) as string,
                Password = from.GetValue(3) as string,
                ESOname = from.GetValue(4) as string,
                RoleID = from.GetByte(5),
                Server = from.GetValue(6) as string,
            };

            //returning the User Data
            return to;

        }

        //Getting Item Data from the Database
        public static ItemsDO ReaderToItem(SqlDataReader from)
        {
            ItemsDO to = new ItemsDO
            {
                ItemID = from.GetInt32(0),
                Type = from.GetValue(1) as string,
                SubType = from.GetValue(2) as string,
                Trait = from.GetValue(3) as string,
                Style = from.GetValue(4) as string,
                Set = from.GetValue(5) as string,
                Level = from.GetValue(6) as string,
                Quality = from.GetValue(7) as string,
                OrderID = from.GetInt32(8),
                Price = from.GetInt32(9)
            };

            //Returning Item Data
            return to;
        }

        //Getting Order Data from the Database
        public static OrdersDO ReaderToOrder(SqlDataReader from)
        {
            OrdersDO to = new OrdersDO
            {
                OrderID = from.GetInt32(0),
                UserID = from.GetInt32(1),
                Requested = from.GetDateTime(2),
                Due = from.GetDateTime(3),
                CrafterID = from.GetInt32(4),
                Status = from.GetByte(5),
                PriceTotal = from.GetInt32(6)
            };

            //Returning Order Data
            return to;
        }










    }
}
