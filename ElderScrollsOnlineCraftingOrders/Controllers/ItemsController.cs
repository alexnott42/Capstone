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
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View("Error");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View("Error");
            }
            //return view
            return response;
        }

        //create new item continued
        [HttpPost]
        public ActionResult CreateNewItem(ItemsPO form, int OrderID)
        {
            ActionResult response;
            //checking model
            if (ModelState.IsValid)
            {

                try
                {
                    //taking user input and mapping it to the database
                    ItemsDO newItem = Mapper.ItemsPOtoItemsDO(form);
                    newItem.OrderID = OrderID;
                    _ItemsDAO.CreateNewItemEntry(newItem);
                    //setting response view
                    response = RedirectToAction("ViewOrderByID", "Orders");
                }
                //logging errors and redirecting
                catch (SqlException sqlEx)
                {
                    Logger.SqlErrorLog(sqlEx);
                    response = View("Error");
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    response = View("Error");
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
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View("Error");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View("Error");
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
            //checking modelstate
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
                //logging errors and redirecting
                catch (SqlException sqlEx)
                {
                    Logger.SqlErrorLog(sqlEx);
                    response = View("Error");
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    response = View("Error");
                }
            }
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            //return view
            return response;
        }

        //delete item
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult DeleteItem(int ItemID, int OrderID)
        {
            ActionResult response;
            try
            {
                //executing procedure
                _ItemsDAO.DeleteItemEntry(ItemID);
                //setting response view
                response = RedirectToAction("ViewOrderByID", "Orders", new { OrderID });
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = View("Error");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = View("Error");
            }
            //returning view
            return response;
        }
    }
}