using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using DAL;
using ElderScrollsOnlineCraftingOrders.Logging;
using DAL.dalMappers;
using DAL.dalModels;
using ElderScrollsOnlineCraftingOrders;
using ElderScrollsOnlineCraftingOrders.Mapping;
using ElderScrollsOnlineCraftingOrders.Models;
using ElderScrollsOnlineCraftingOrders.Security;

namespace ElderScrollsOnlineCraftingOrders.Controllers
{
    public class ItemsController : Controller
    {
        //establishing connections, file locations, data access, etc
        private readonly string errorLogPath;
        private readonly string connectionString;
        private ItemsDAO _ItemsDAO;


        //constructor
        public ItemsController()
        {
            errorLogPath = ConfigurationManager.AppSettings["errorLogPath"];
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _ItemsDAO = new ItemsDAO(connectionString, errorLogPath);
            Logger.errorLogPath = errorLogPath;
        }

        //view item by ID
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewItemByID(int ItemID)
        {
            ActionResult response;
            ItemsPO itemDetails = new ItemsPO();
            try
            {
                //mapping to presentation layer
                itemDetails = Mapper.ItemsDOtoItemsPO(_ItemsDAO.ViewItemByID(ItemID));
                response = View(itemDetails);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View(itemDetails);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View(itemDetails);
            }
            return response;
        }

        //view item by Order ID
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewItemByOrder(int OrderID)
        {
            ActionResult response;
            List<ItemsDO> _Items = new List<ItemsDO>();
            List<ItemsPO> Items = new List<ItemsPO>();
            try
            {
                //mapping to presentation layer
                _Items = _ItemsDAO.ItemsByOrderID(OrderID);
                Items = Mapper.ItemsListDOtoPO(_Items);
                response = View(Items);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View(Items);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View(Items);
            }
            return response;
        }

        //create item
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult CreateNewItem()
        {
            ActionResult response;
            try
            {
                response = View();
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View();
            }
            return response;
        }

        //create new item continued
        [HttpPost]
        public ActionResult CreateNewItem(ItemsPO form)
        {
            ActionResult response;
            if (ModelState.IsValid)
            {

                try
                {
                    //taking user input and mapping it to the database
                    ItemsDO newItem = Mapper.ItemsPOtoItemsDO(form);
                    _ItemsDAO.CreateNewItemEntry(newItem);
                    //setting response view
                    response = RedirectToAction("ViewOrderByID", "Orders");
                }
                //logging errors
                catch (SqlException sqlEx)
                {
                    Logger.SqlErrorLog(sqlEx);
                    response = View(form);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    response = View(form);
                }
            }
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            //return view page
            return response;
        }

        //update item
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult UpdateItem(int ItemID)
        {
            ActionResult response;
            ItemsPO ItemPO = new ItemsPO();
            try
            {
                //retrieving data and displaying to user
                ItemsDO ItemDO = _ItemsDAO.ViewItemByID(ItemID);
                ItemPO = Mapper.ItemsDOtoItemsPO(ItemDO);
                //setting response view
                response = View(ItemPO);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View(ItemPO);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View(ItemPO);
            }
            //return view
            return response;
        }

        //update item continued
        [SecurityFilter(3)]
        [HttpPost]
        public ActionResult UpdateItem(ItemsPO form)
        {
            ActionResult response;
            if (ModelState.IsValid)
            {

                try
                {
                    {
                        //storing data to database
                        ItemsDO ItemDO = Mapper.ItemsPOtoItemsDO(form);
                        _ItemsDAO.UpdateItemEntryInformation(ItemDO);
                        //setting response page
                        response = RedirectToAction("ViewOrderByID", "Orders");
                    }
                }
                //logging errors
                catch (SqlException sqlEx)
                {
                    Logger.SqlErrorLog(sqlEx);
                    response = View(form);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    response = View(form);
                }
            }
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            return response;
        }

        //delete item
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult DeleteItem(int ItemID)
        {
            ActionResult response;
            try
            {
                //executing procedure
                _ItemsDAO.DeleteItemEntry(ItemID);
                //setting response view
                response = RedirectToAction("ViewOrderByID", "Orders");
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("ViewOrderByID", "Orders");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("ViewOrderByID", "Orders");
            }
            //returning view
            return response;
        }
    }
}