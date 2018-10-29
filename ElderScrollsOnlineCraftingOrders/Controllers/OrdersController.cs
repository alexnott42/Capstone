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
using BLL;
using BLL.Models;
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
        public ActionResult ViewAllOrders()
        {
            ActionResult response;
            try
            {
                //mapping all the data to the view page
                List<OrdersDO> _Orders = _OrdersDAO.ViewAllOrders();
                List<OrdersPO> Orders = Mapper.OrdersListDOtoPO(_Orders);
                response = View(Orders);
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
            return response;
        }
        //View order by status
        [SecurityFilter(4)]
        [HttpGet]
        public ActionResult ViewByStatus(byte status)
        {
            ActionResult response;
            try
            {
                //mapping all the data to the view page
                List<OrdersDO> _Orders = _OrdersDAO.ViewOrderByStatus(status);
                List<OrdersPO> Orders = Mapper.OrdersListDOtoPO(_Orders);
                response = View(Orders);
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
            return response;
        }

        //view order by ID
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewOrderByID(int OrderID)
        {
            ActionResult response;
            OrdersVM orderDetails = new OrdersVM();

            try
            {
                //mapping all the data to the view page
                OrdersPO order = Mapper.OrdersDOtoOrdersPO(_OrdersDAO.ViewOrderByID(OrderID));
                List<ItemsPO> orderItems = Mapper.ItemsListDOtoPO(_ItemsDAO.ItemsByOrderID(OrderID));

                //sending to business logic layer
                OrdersBO calcOrder = Mapper.OrdersPOtoOrdersBO(order);
                List<ItemsBO> calcItems = Mapper.ItemsListPOtoBO(orderItems);

                //doing valculations
                calcOrder = Calculation.PriceTotalCalculator(calcOrder, calcItems);
                order.Pricetotal = calcOrder.Pricetotal;

                //assigning new total price
                OrdersDO newTotal = Mapper.OrdersPOtoOrdersDO(order);
                _OrdersDAO.UpdateOrderPricetotal(newTotal);

                //assigning objects to viewmodel
                orderDetails.Order = order;
                orderDetails.Items = orderItems;

                response = View(orderDetails);
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

        //view order by user
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult ViewOrderByUserID(int UserID)
        {
            ActionResult response;
            try
            {
                //mapping all the data to the view page
                List<OrdersDO> _Orders = _OrdersDAO.ViewOrderByUserID(UserID);
                List<OrdersPO> Orders = Mapper.OrdersListDOtoPO(_Orders);
                response = View(Orders);
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
            return response;
        }

        //view order by crafter
        [SecurityFilter(4)]
        [HttpGet]
        public ActionResult ViewOrderByCrafterID(int CrafterID)
        {
            ActionResult response;
            try
            {
                //mapping all the data to the view page
                List<OrdersDO> _Orders = _OrdersDAO.ViewOrderByCrafterID(CrafterID);
                List<OrdersPO> Orders = Mapper.OrdersListDOtoPO(_Orders);
                //returning form view
                response = View(Orders);
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

        //Create order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult CreateNewOrder()
        {
            ActionResult response;
            try
            {
                OrdersPO newOrder = new OrdersPO();
                newOrder.Requested = DateTime.Now;
                newOrder.Status = 1;
                newOrder.UserID = (int)Session["UserID"];
                newOrder.Due = DateTime.Now.AddDays(7);
               response = View(newOrder);
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
                    response = RedirectToAction("ViewOrderByUserID", "Orders", new { newOrder.UserID });
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
            //returning to home page
            return response;
        }

        //update order
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult UpdateOrder(int OrderID)
        {
            ActionResult response;
            try
            {
                //mapping user input to database
                OrdersDO orderDO = _OrdersDAO.ViewOrderByID(OrderID);
                OrdersPO orderPO = Mapper.OrdersDOtoOrdersPO(orderDO);

                response = View(orderPO);
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
                    response = RedirectToAction("ViewOrderByID", "Orders", new { OrderDO.OrderID });

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
                int UserID = (int)Session["UserID"];
                response = RedirectToAction("ViewOrderByUserID", "Orders", new { UserID });
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
            //returning response page
            return response;
        }

        [SecurityFilter(4)]
        [HttpGet]
        public ActionResult ConfirmAssign(int OrderID)
        {
            OrdersDO Order = _OrdersDAO.ViewOrderByID(OrderID);
            OrdersPO orderDetails = Mapper.OrdersDOtoOrdersPO(Order);
            orderDetails.CrafterID = (int)Session["UserID"];
            orderDetails.Crafter = (string)Session["Username"];
            return View(orderDetails);
        }

        //todo make "AssignMeToOrder" actually work
        //Assigning user to order
        [SecurityFilter(4)]
        [HttpPost]
        //todo: return a view showing the information on this page
        public ActionResult AssignMeToOrder(OrdersPO form)
        {
            ActionResult response;
            if (ModelState.IsValid)
            {
                try
                {
                    OrdersDO newInfo = Mapper.OrdersPOtoOrdersDO(form);
                    //running the stored procedure
                    newInfo.Status = 2;
                    _OrdersDAO.UpdateOrderCrafter(newInfo);

                    response = RedirectToAction("ViewOrderByID", "Orders", new { form.OrderID });
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
            //returning response page
            return response;
        }
    }
}