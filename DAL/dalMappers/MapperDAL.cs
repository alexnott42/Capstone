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

            UsersDO to = new UsersDO();
            //mapping data
            to.UserID = (int)from["UserID"];
            to.Username = from["Username"] as string;
            to.Email = from["email"] as string;
            to.Password = from["Password"] as string;
            to.ESOname = from["ESOname"] as string;
            to.RoleID = (byte)from["RoleID"];
            to.Server = from["Server"] as string;
            

            //returning the User Data
            return to;

        }

        //Getting Item Data from the Database
        public static ItemsDO ReaderToItem(SqlDataReader from)
        {
            ItemsDO to = new ItemsDO();
            //mapping data
            to.ItemID = (int)from["ItemID"];
            to.Type = from["Type"] as string;
            to.SubType = from["SubType"] as string;
            to.Trait = from["Trait"] as string;
            to.Style = from["Style"] as string;
            to.Set = from["Set"] as string;
            to.Level = from["Level"] as string;
            to.Quality = from["Quality"] as string;
            to.OrderID = (int)from["OrderID"];
            to.Price = (int)from["Price"];
            

            //Returning Item Data
            return to;
        }

        //Getting Order Data from the Database
        public static OrdersDO ReaderToOrder(SqlDataReader from)
        {
            OrdersDO to = new OrdersDO();

            //mapping data
            to.OrderID = (int)from["OrderID"];
            to.UserID = (int)from["UserID"];
            to.Requested = (DateTime)from["Requested"];
            to.Due = (DateTime)from["Due"];
            to.CrafterID = from["CrafterId"] as int?;
            to.Status = (byte)from["Status"];
            to.Username = from["Username"] as string;
            to.Crafter = from["Crafter"] as string;
            //Returning Order Data
            return to;
        }










    }
}
