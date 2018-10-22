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
        private readonly string errorLogPath;
        private readonly string connectionString;
        private OrdersDAO _OrdersDAO;
        private ItemsDAO _ItemsDAO;


        //constructor
        public OrdersController()
        {
            errorLogPath = ConfigurationManager.AppSettings["errorLogPath"];
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _OrdersDAO = new OrdersDAO(connectionString, errorLogPath);
            _ItemsDAO = new ItemsDAO(connectionString, errorLogPath);
            Logger.errorLogPath = errorLogPath;
        }
        //view all orders
        [SecurityFilter(4)]
        //todo: create view
        public ActionResult ViewAllOrders()
        {
            ActionResult response;
            List<OrdersDO> _Orders = new List<OrdersDO>();
            List<OrdersPO> Orders = new List<OrdersPO>();
            try
            {
                //mapping all the data to the view page
                _Orders = _OrdersDAO.ViewAllOrders();
                Orders = Mapper.OrdersListDOtoPO(_Orders);
                response = View(Orders);
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            return response;
        }
        //View order by status
        [SecurityFilter(4)]
        //todo: create view

        [HttpGet]
        public ActionResult ViewByStatus(byte status)
        {
            ActionResult response;
            List<OrdersDO> _Orders = new List<OrdersDO>();
            List<OrdersPO> Orders = new List<OrdersPO>();
            try
            {
                //mapping all the data to the view page
                _Orders = _OrdersDAO.ViewOrderByStatus(status);
                Orders = Mapper.OrdersListDOtoPO(_Orders);
                response = View(Orders);
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            return response;
        }

        //view order by ID
        [SecurityFilter(3)]
        //todo: create view

        [HttpGet]
        public ActionResult ViewOrderByID(int OrderID)
        {
            ActionResult response;
            OrdersPO order = new OrdersPO();
            List<ItemsPO> orderItems = new List<ItemsPO>();
            OrdersVM orderDetails = new OrdersVM();

            try
            {
                //mapping all the data to the view page
                order = Mapper.OrdersDOtoOrdersPO(_OrdersDAO.ViewOrderByID(OrderID));
                orderItems = Mapper.ItemsListDOtoPO(_ItemsDAO.ItemsByOrderID(OrderID));
                orderDetails.Order = order;
                orderDetails.Items = orderItems;

                response = View(orderDetails);
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            //return view
            return response;
        }

        //view order by user
        [SecurityFilter(3)]
        //todo: create view

        [HttpGet]
        public ActionResult ViewOrderByUserID(int UserID)
        {
            List<OrdersDO> _Orders = new List<OrdersDO>();
            List<OrdersPO> Orders = new List<OrdersPO>();
            ActionResult response;
            try
            {
                //mapping all the data to the view page
                _Orders = _OrdersDAO.ViewOrderByUserID(UserID);
                Orders = Mapper.OrdersListDOtoPO(_Orders);
                response = View(Orders);
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            return response;
        }

        //view order by crafter
        [SecurityFilter(4)]
        //todo: create view
        [HttpGet]
        public ActionResult ViewOrderByCrafterID(int CrafterID)
        {
            ActionResult response;
            List<OrdersDO> _Orders = new List<OrdersDO>();
            List<OrdersPO> Orders = new List<OrdersPO>();
            try
            {
                //mapping all the data to the view page
                _Orders = _OrdersDAO.ViewOrderByCrafterID(CrafterID);
                Orders = Mapper.OrdersListDOtoPO(_Orders);
                //returning form view
                response = View(Orders);
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            //return view
            return response;
        }

        //Create order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult CreateNewOrder()
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
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            //return view
            return response;
        }

        //create order continued
        [SecurityFilter(3)]
        [HttpPost]
        public ActionResult CreateNewOrder(OrdersPO form)
        {
            ActionResult response;
            //checking modelstate
            if (ModelState.IsValid)
            {

                try
                {
                    //taking user input and saving it to the database
                    OrdersDO newOrder = Mapper.OrdersPOtoOrdersDO(form);
                    _OrdersDAO.CreateNewOrder(newOrder);
                    //redirecting to Order page when finished
                    response = RedirectToAction("ViewOrderByUserID", "Orders");
                }
                //logging errors and redirecting
                catch (SqlException sqlEx)
                {
                    Logger.SqlErrorLog(sqlEx);
                    response = RedirectToAction("Error", "Shared");
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    response = RedirectToAction("Error", "Shared");
                }
            }
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            //returning to home page
            return response;
        }

        //update order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult UpdateOrder(int UserID)
        {
            OrdersPO orderPO = new OrdersPO(); 
            ActionResult response;
            try
            {
                //mapping user input to database
                OrdersDO orderDO = _OrdersDAO.ViewOrderByID(UserID);
                orderPO = Mapper.OrdersDOtoOrdersPO(orderDO);
                response = View(orderPO);
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            //return view
            return response;
        }

        //update order continued
        [SecurityFilter(3)]
        [HttpPost]
        public ActionResult UpdateOrder(OrdersPO form)
        {
            ActionResult response;

            //checking modelstate
            if (ModelState.IsValid)
            {
                try
                {
                    //mapping user inpit to database
                    OrdersDO OrderDO = Mapper.OrdersPOtoOrdersDO(form);
                    _OrdersDAO.UpdateOrder(OrderDO);
                    response = RedirectToAction("ViewOrderByID", "Orders");

                }
                //logging errors and redirecting
                catch (SqlException sqlEx)
                {
                    Logger.SqlErrorLog(sqlEx);
                    response = RedirectToAction("Error", "Shared");
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    response = RedirectToAction("Error", "Shared");
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
                response = RedirectToAction("ViewOrderByUserID", "Orders");
            }
            //logging errors and redirecting
            catch (SqlException sqlEx)
            {
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            //returning response page
            return response;
        }
    }
}