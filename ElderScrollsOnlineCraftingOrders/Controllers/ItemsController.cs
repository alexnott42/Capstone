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
        private readonly string filePath;
        private readonly string errorLogPath;
        private readonly string connectionString;
        private ItemsDAO _ItemsDAO;
        private List<ItemsDO> _Items = new List<ItemsDO>();
        private List<ItemsPO> Items = new List<ItemsPO>();

        //constructor
        public ItemsController()
        {
            filePath = Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~"));
            errorLogPath = filePath + @"\ErrorLog.txt";
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _ItemsDAO = new ItemsDAO(connectionString, errorLogPath);
        }

        //view item by ID
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewItemByID(int ItemID)
        {
            try
            {
                //mapping to presentation layer
                ItemsPO itemDetails = Mapper.ItemsDOtoItemsPO(_ItemsDAO.ViewItemByID(ItemID));
                return View(itemDetails);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
            }
        }

        //view item by Order ID
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewItemByOrder(int OrderID)
        {
            try
            {
                //mapping to presentation layer
                _Items = _ItemsDAO.ItemsByOrderID(OrderID);
                Items = Mapper.ItemsListDOtoPO(_Items);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
            }
            return View();
        }

        //create item
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult CreateNewItem()
        {
            try
            {
                return View();
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
            }
        }

        //create new item continued
        [HttpPost]
        public ActionResult CreateNewItem(ItemsPO form)
        {
            ActionResult response;
            try
            {
                //taking user input and mapping it to the database
                ItemsDO newItem = Mapper.ItemsPOtoItemsDO(form);
                _ItemsDAO.CreateNewItemEntry(newItem);
                //setting response view
                response = RedirectToAction("Index", "Home");
                //todo: make createnewitem redirect to order page
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
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
            try
            {
                //retrieving data and displaying to user
                ItemsDO ItemDO = _ItemsDAO.ViewItemByID(ItemID);
                ItemsPO ItemPO = Mapper.ItemsDOtoItemsPO(ItemDO);
                //setting response view
                response = View(ItemPO);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
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
            try
            {
                {
                    //storing data to database
                    ItemsDO ItemDO = Mapper.ItemsPOtoItemsDO(form);
                    _ItemsDAO.UpdateItemEntryInformation(ItemDO);
                    //setting response page
                    response = RedirectToAction("Index", "Home");
                    //TODO: set update item to redirect to order page
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
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
                response = RedirectToAction("Index", "Home");
                //TODO: set deleteitem to redirect to order page
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                throw ex;
            }
            //returning view
            return response;
        }
    }
}