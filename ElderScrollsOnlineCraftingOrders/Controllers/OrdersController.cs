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
    public class OrdersController : Controller
    {
        //establishing connections, file locations, Data Access Objects, etc
        private readonly string filePath;
        private readonly string errorLogPath;
        private readonly string connectionString;
        private OrdersDAO _OrdersDAO;
        private List<OrdersDO> _Orders = new List<OrdersDO>();
        private List<OrdersPO> Orders = new List<OrdersPO>();

        //constructor
        public OrdersController()
        {
            filePath = Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~"));
            errorLogPath = filePath + @"\ErrorLog.txt";
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _OrdersDAO = new OrdersDAO(connectionString, errorLogPath);
        }
        //view all orders
        [SecurityFilter(4)]
        [HttpGet]
        public ActionResult ViewAllOrders()
        {
            try
            {
                _Orders = _OrdersDAO.ViewAllOrders();
                Orders = Mapper.OrdersListDOtoPO(_Orders);
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
        //View order by status
        [SecurityFilter(4)]
        [HttpGet]
        public ActionResult ViewByStatus(byte status)
        {
            try
            {
                //mapping to presentation layer
                _Orders = _OrdersDAO.ViewOrderByStatus(status);
                Orders = Mapper.OrdersListDOtoPO(_Orders);
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

        //view order by ID
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewOrderByID(int OrderID)
        {
            try
            {
                //Mapping data to presentation layer
                OrdersPO orderDetails = Mapper.OrdersDOtoOrdersPO(_OrdersDAO.ViewOrderByID(OrderID));
                return View(orderDetails);
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

        //view order by user
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewOrderByUserID(int UserID)
        {
            try
            {
                //Mapping all the data from database, to data access, to presentation layer
                _Orders = _OrdersDAO.ViewOrderByUserID(UserID);
                Orders = Mapper.OrdersListDOtoPO(_Orders);
                return View(Orders);
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

        //view order by crafter
        [SecurityFilter(4)]
        [HttpGet]
        public ActionResult ViewOrderByCrafterID(int CrafterID)
        {
            try
            {
                //Mapping all the data from database, to data access, to presentation layer
                _Orders = _OrdersDAO.ViewOrderByCrafterID(CrafterID);
                Orders = Mapper.OrdersListDOtoPO(_Orders);
                //returning form view
                return View(Orders);
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

        //Create order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult CreateNewOrder()
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

        //create order continued
        [SecurityFilter(3)]
        [HttpPost]
        public ActionResult CreateNewOrder(OrdersPO form)
        {
            ActionResult response;
            try
            {
                //taking in user input and saving it to the database
                OrdersDO newOrder = Mapper.OrdersPOtoOrdersDO(form);
                _OrdersDAO.CreateNewOrder(newOrder);
                //redirecting to home page when finished
                response = RedirectToAction("Index", "Home");
                //TODO: make CreateNewOrder return user to orders screen
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
            //returning to home page
            return response;
        }

        //update order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult UpdateOrder(int UserID)
        {
            ActionResult response;
            try
            {
                OrdersDO OrderDO = _OrdersDAO.ViewOrderByID(UserID);
                OrdersPO OrderPO = Mapper.OrdersDOtoOrdersPO(OrderDO);
                response = View(OrderPO);
            }
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

        //update order continued
        [SecurityFilter(3)]
        [HttpPost]
        public ActionResult UpdateOrder(OrdersPO form)
        {
            ActionResult response;
            try
            {
                OrdersDO OrderDO = Mapper.OrdersPOtoOrdersDO(form);
                _OrdersDAO.UpdateOrder(OrderDO);
                response = RedirectToAction("Index", "Home");
                //todo: make updateorder return to order view

            }
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

        //delete order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult DeleteOrder(int OrderID)
        {
            ActionResult response;
            try
            {
                //running the stored procedure
                _OrdersDAO.DeleteOrder(OrderID);
                //setting response page
                response = RedirectToAction("Index", "Home");
                //todo: make deleteorder return to order view
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
            //returning response page
            return response;
        }
    }
}