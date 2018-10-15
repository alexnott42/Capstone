using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.dalModels;
using ElderScrollsOnlineCraftingOrders.Models;

namespace ElderScrollsOnlineCraftingOrders.Mapping
{
    //moving information between layers through mapping

    public static class Mapper
    {
        //mapping Items to presentation layer
        public static ItemsPO ItemsDOtoItemsPO(ItemsDO from)
        {
            //mapping each individual attribute
            ItemsPO to = new ItemsPO
            {
                ItemID = from.ItemID,
                Type = from.Type,
                SubType = from.SubType,
                Trait = from.Trait,
                Style = from.Style,
                Set = from.Set,
                Level = from.Level,
                Quality = from.Quality,
                OrderID = from.OrderID,
                Price = from.Price
            };
            //making it usable in the future
            return to;
        }

        //mapping back to Data Access layer
        public static ItemsDO ItemsPOtoItemsDO(ItemsPO from)
        {
            //mapping each individual attribute
            ItemsDO to = new ItemsDO
            {
                ItemID = from.ItemID,
                Type = from.Type,
                SubType = from.SubType,
                Trait = from.Trait,
                Style = from.Style,
                Set = from.Set,
                Level = from.Level,
                Quality = from.Quality,
                OrderID = from.OrderID,
                Price = from.Price
            };
            //making it usable in future
            return to;
        }

        //Mapping to presentation layer 
        public static OrdersPO OrdersDOtoOrdersPO(OrdersDO from)
        {
            //mapping each individual attribute
            OrdersPO to = new OrdersPO
            {
                OrderID = from.OrderID,
                UserID = from.UserID,
                Requested = from.Requested,
                Due = from.Due,
                CrafterID = from.CrafterID,
                Status = from.Status,
                PriceTotal = from.PriceTotal
            };
            //making it usable in future
            return to;
        }

        //mapping back to Data Access layer
        public static OrdersDO OrdersPOtoOrdersDO(OrdersPO from)
        {
            //mapping each individual attribute
            OrdersDO to = new OrdersDO
            {
                OrderID = from.OrderID,
                UserID = from.UserID,
                Requested = from.Requested,
                Due = from.Due,
                CrafterID = from.CrafterID,
                Status = from.Status,
                PriceTotal = from.PriceTotal
            };
            //making it usable in future
            return to;
        }

        //mapping to presentation layer
        public static UsersPO UsersDOtoUsersPO(UsersDO from)
        {
            //mapping each individual attribute
            UsersPO to = new UsersPO
            {
                UserID = from.UserID,
                Username = from.Username,
                Email = from.Email,
                Password = from.Password,
                ESOname = from.ESOname,
                RoleID = from.RoleID,
                Server = from.Server
            };
            return to;
        }

        //mapping back to Data Access layer
        public static UsersDO UsersPOtoUsersDO(UsersPO from)
        {
            //mapping each individual attribute
            UsersDO to = new UsersDO
            {
                UserID = from.UserID,
                Username = from.Username,
                Email = from.Email,
                Password = from.Password,
                ESOname = from.ESOname,
                RoleID = from.RoleID,
                Server = from.Server
            };
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
    }
}