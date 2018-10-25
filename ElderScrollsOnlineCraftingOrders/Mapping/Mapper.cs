using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Models;
using DAL.dalModels;
using ElderScrollsOnlineCraftingOrders.Models;




namespace ElderScrollsOnlineCraftingOrders.Mapping
{
    //moving information between layers through mapping

    public static class Mapper
    {
        //mapping Items to presentation layer from DAL
        public static ItemsPO ItemsDOtoItemsPO(ItemsDO from)
        {
            //mapping each individual attribute
            ItemsPO to = new ItemsPO();
                to.ItemID = from.ItemID;
                to.Type = from.Type;
                to.SubType = from.SubType;
                to.Trait = from.Trait;
                to.Style = from.Style;
                to.Set = from.Set;
                to.Level = from.Level;
                to.Quality = from.Quality;
                to.OrderID = from.OrderID;
                to.Price = from.Price;
            //making it usable in the future
            return to;
        }

        //mapping Items to presentation layer from BLL
        public static ItemsPO ItemsBOtoItemsPO(ItemsBO from)
        {
            //mapping each individual attribute
            ItemsPO to = new ItemsPO();
            to.ItemID = from.ItemID;
            to.Type = from.Type;
            to.SubType = from.SubType;
            to.Trait = from.Trait;
            to.Style = from.Style;
            to.Set = from.Set;
            to.Level = from.Level;
            to.Quality = from.Quality;
            to.OrderID = from.OrderID;
            to.Price = from.Price;
            //making it usable in the future
            return to;
        }

        //mapping from Presentation from Business
        public static ItemsBO ItemsPOtoItemsBO(ItemsPO from)
        {
            //mapping each individual attribute
            ItemsBO to = new ItemsBO();
            to.ItemID = from.ItemID;
            to.Type = from.Type;
            to.SubType = from.SubType;
            to.Trait = from.Trait;
            to.Style = from.Style;
            to.Set = from.Set;
            to.Level = from.Level;
            to.Quality = from.Quality;
            to.OrderID = from.OrderID;
            to.Price = from.Price;
            //making it usable in future
            return to;
        }

        //mapping back to Data Access layer
        public static ItemsDO ItemsPOtoItemsDO(ItemsPO from)
        {
            //mapping each individual attribute
            ItemsDO to = new ItemsDO();
            to.ItemID = from.ItemID;
            to.Type = from.Type;
            to.SubType = from.SubType;
            to.Trait = from.Trait;
            to.Style = from.Style;
            to.Set = from.Set;
            to.Level = from.Level;
            to.Quality = from.Quality;
            to.OrderID = from.OrderID;
            to.Price = from.Price;
            //making it usable in future
            return to;
        }

        //Mapping to presentation layer from DLL
        public static OrdersPO OrdersDOtoOrdersPO(OrdersDO from)
        {
            //mapping each individual attribute
            OrdersPO to = new OrdersPO();
            to.OrderID = from.OrderID;
            to.UserID = from.UserID;
            to.Requested = from.Requested;
            to.Due = from.Due;
            to.CrafterID = from.CrafterID;
            to.Status = from.Status;
            to.Username = from.Username;
            to.Crafter = from.Crafter;
            to.Pricetotal = from.Pricetotal;
            //making it usable in future
            return to;
        }

        //Mapping to presentation layer from BLL
        public static OrdersPO OrdersBOtoOrdersPO(OrdersBO from)
        {
            //mapping each individual attribute
            OrdersPO to = new OrdersPO();
            to.OrderID = from.OrderID;
            to.UserID = from.UserID;
            to.Requested = from.Requested;
            to.Due = from.Due;
            to.CrafterID = from.CrafterID;
            to.Status = from.Status;
            to.Username = from.Username;
            to.Crafter = from.Crafter;
            to.Pricetotal = from.Pricetotal;

            //making it usable in future
            return to;
        }

        //Mapping to BLL from presentation layer
        public static OrdersBO OrdersPOtoOrdersBO(OrdersPO from)
        {
            //mapping each individual attribute
            OrdersBO to = new OrdersBO();
            to.OrderID = from.OrderID;
            to.UserID = from.UserID;
            to.Requested = from.Requested;
            to.Due = from.Due;
            to.CrafterID = from.CrafterID;
            to.Status = from.Status;
            to.Username = from.Username;
            to.Crafter = from.Crafter;
            to.Pricetotal = from.Pricetotal;

            //making it usable in future
            return to;
        }

        //mapping back to Data Access layer
        public static OrdersDO OrdersPOtoOrdersDO(OrdersPO from)
        {
            //mapping each individual attribute
            OrdersDO to = new OrdersDO();
            to.OrderID = from.OrderID;
            to.UserID = from.UserID;
            to.Requested = from.Requested;
            to.Due = from.Due;
            to.CrafterID = from.CrafterID;
            to.Status = from.Status;
            to.Username = from.Username;
            to.Crafter = from.Crafter;
            to.Pricetotal = from.Pricetotal;

            //making it usable in future
            return to;
        }

        //mapping to presentation layer
        public static UsersPO UsersDOtoUsersPO(UsersDO from)
        {
            //mapping each individual attribute
            UsersPO to = new UsersPO();
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.Email = from.Email;
            to.Password = from.Password;
            to.ESOname = from.ESOname;
            to.RoleID = from.RoleID;
            to.Server = from.Server;
            return to;
        }

        //mapping back to Data Access layer
        public static UsersDO UsersPOtoUsersDO(UsersPO from)
        {
            //mapping each individual attribute
            UsersDO to = new UsersDO();
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.Email = from.Email;
            to.Password = from.Password;
            to.ESOname = from.ESOname;
            to.RoleID = from.RoleID;
            to.Server = from.Server;
            return to;
        }

        //mapping item list to presentation layer
        public static List<ItemsPO> ItemsListDOtoPO(List<ItemsDO> from)
        {
            List<ItemsPO> to = new List<ItemsPO>();
            foreach (ItemsDO Item in from)
            {
                to.Add(Mapper.ItemsDOtoItemsPO(Item));
            }
            return to;
        }

        //mapping order list to presentation layer
        public static List<OrdersPO> OrdersListDOtoPO(List<OrdersDO> from)
        {
            List<OrdersPO> to = new List<OrdersPO>();
            foreach (OrdersDO Order in from)
            {
                to.Add(Mapper.OrdersDOtoOrdersPO(Order));
            }
            return to;
        }

        //mapping user list to presentation layer
        public static List<UsersPO> UsersListDOtoPO(List<UsersDO> from)
        {
            List<UsersPO> to = new List<UsersPO>();
            foreach (UsersDO User in from)
            {
                to.Add(Mapper.UsersDOtoUsersPO(User));
            }
            return to;
        }

        //mapping item list to Business layer
        public static List<ItemsBO> ItemsListPOtoBO(List<ItemsPO> from)
        {
            List<ItemsBO> to = new List<ItemsBO>();
            foreach (ItemsPO Item in from)
            {
                to.Add(Mapper.ItemsPOtoItemsBO(Item));
            }
            return to;
        }

        //mapping order list to business layer
        public static List<OrdersBO> OrdersListPOtoBO(List<OrdersPO> from)
        {
            List<OrdersBO> to = new List<OrdersBO>();
            foreach (OrdersPO Order in from)
            {
                to.Add(Mapper.OrdersPOtoOrdersBO(Order));
            }
            return to;
        }

        //mapping item list from Business layer to presentation
        public static List<ItemsPO> ItemsListBOtoPO(List<ItemsBO> from)
        {
            List<ItemsPO> to = new List<ItemsPO>();
            foreach (ItemsBO Item in from)
            {
                to.Add(Mapper.ItemsBOtoItemsPO(Item));
            }
            return to;
        }

        //mapping order list from business layer to presentation
        public static List<OrdersPO> OrdersListBOtoPO(List<OrdersBO> from)
        {
            List<OrdersPO> to = new List<OrdersPO>();
            foreach (OrdersBO Order in from)
            {
                to.Add(Mapper.OrdersBOtoOrdersPO(Order));
            }
            return to;
        }
    }
}