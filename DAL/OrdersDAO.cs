using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.dalModels;
using System.Data.SqlClient;
using DAL.dalMappers;
using System.Data;
using DAL.dalLogger;

namespace DAL
{
    public class OrdersDAO
    {
        //Establishing connections and file locations
        private readonly string _ConnectionString;
        private readonly string _ErrorLogPath;

        //constructor
        public OrdersDAO(string connectionString, string errorLogPath)
        {
            this._ConnectionString = connectionString;
            this._ErrorLogPath = errorLogPath;
        }

        //creating a new order
        public void CreateNewOrder(OrdersDO orderInfo)
        {
            try
            {
                //defining commands
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand createOrder = new SqlCommand("ORDERS_CREATE_NEW", sqlConnection))
                {
                    //timing out after 60 seconds
                    createOrder.CommandType = CommandType.StoredProcedure;
                    createOrder.CommandTimeout = 60;

                    //inserting information
                    createOrder.Parameters.AddWithValue("UserID", orderInfo.UserID);
                    createOrder.Parameters.AddWithValue("Requested", orderInfo.Requested);
                    createOrder.Parameters.AddWithValue("Due", orderInfo.Due);
                    createOrder.Parameters.AddWithValue("Status", orderInfo.Status);

                    //Saving information to database
                    sqlConnection.Open();
                    createOrder.ExecuteNonQuery();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }

        }

        //retrieving all orders from the database
        public List<OrdersDO> ViewAllOrders()
        {
            //Creating a list of users
            List<OrdersDO> ordersList = new List<OrdersDO>();
            try
            {
                //defining commands and accessing the database
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewAllOrders = new SqlCommand("ORDERS_SELECT_ALL", sqlConnection))
                {
                    // gives up after 60 seconds
                    viewAllOrders.CommandType = CommandType.StoredProcedure;
                    viewAllOrders.CommandTimeout = 60;

                    //reading the database and using a mapper to store it to memory
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewAllOrders.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //creating objects to add them to a list
                            ordersList.Add(MapperDAL.ReaderToOrder(reader));
                        }
                    }
                }
            }
            //logging any errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            return ordersList;
        }

        //Retrieving a single order from the database
        public OrdersDO ViewOrderByID(int OrderID)
        {
            OrdersDO orderData = new OrdersDO();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewByID = new SqlCommand("ORDERS_SELECT_BY_ID", sqlConnection))
                {
                    //give up after 60 seconds
                    viewByID.CommandType = CommandType.StoredProcedure;
                    viewByID.CommandTimeout = 60;

                    //inserting the UserID to sort through entries
                    viewByID.Parameters.AddWithValue("OrderID", OrderID);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByID.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orderData = MapperDAL.ReaderToOrder(reader);
                        }
                    }
                }
            }
            //logging any errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            // returning order
            return orderData;
        }

        //updating an existing order
        public int UpdateOrder(OrdersDO orderInfo)
        {
            int rowsAffected = 0;

            try
            {
                //defining some commands
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand updateOrder = new SqlCommand("ORDERS_UPDATE", sqlConnection))
                {
                    //timing out after 60 seconds
                    updateOrder.CommandType = CommandType.StoredProcedure;
                    updateOrder.CommandTimeout = 60;

                    //inserting information
                    updateOrder.Parameters.AddWithValue("OrderID", orderInfo.OrderID);
                    updateOrder.Parameters.AddWithValue("Due", orderInfo.Due);
                    updateOrder.Parameters.AddWithValue("Status", orderInfo.Status);

                    //Saving information to database
                    sqlConnection.Open();
                    rowsAffected = updateOrder.ExecuteNonQuery();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            return rowsAffected;
        }

        //Deleting an order from the database
        public int DeleteOrder(int OrderID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand deleteOrder = new SqlCommand("ORDERS_DELETE", sqlConnection))
                {
                    //giving up after 60 seconds and adding the ID to sort by
                    deleteOrder.CommandType = CommandType.StoredProcedure;
                    deleteOrder.CommandTimeout = 60;
                    deleteOrder.Parameters.AddWithValue("OrderID", OrderID);

                    //opening connection and completing procedure
                    sqlConnection.Open();
                    rowsAffected = deleteOrder.ExecuteNonQuery();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            return rowsAffected;

        }

        //Retrieving orders by a specific user
        public List<OrdersDO> ViewOrderByUserID(int UserID)
        {
            List<OrdersDO> orderData = new List<OrdersDO>();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewByUserID = new SqlCommand("ORDERS_SELECT_BY_USER_ID", sqlConnection))
                {
                    //giving up after 60 seconds
                    viewByUserID.CommandType = CommandType.StoredProcedure;
                    viewByUserID.CommandTimeout = 60;

                    //inserting the UserID to sort entries
                    viewByUserID.Parameters.AddWithValue("UserID", UserID);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByUserID.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderData.Add(MapperDAL.ReaderToOrder(reader));
                        }
                    }
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning order
            return orderData;
        }

        //Retrieving orders by a specific user
        public List<OrdersDO> ViewOrderByCrafterID(int CrafterID)
        {
            List<OrdersDO> orderData = new List<OrdersDO>();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewByCrafterID = new SqlCommand("ORDERS_SELECT_BY_CRAFTER_ID", sqlConnection))
                {
                    //giving up after 60 seconds
                    viewByCrafterID.CommandType = CommandType.StoredProcedure;
                    viewByCrafterID.CommandTimeout = 60;

                    //inserting the UserID to sort through entries
                    viewByCrafterID.Parameters.AddWithValue("CrafterID", CrafterID);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByCrafterID.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orderData.Add(MapperDAL.ReaderToOrder(reader));
                        }
                    }
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning information
            return orderData;
        }

        //Retrieving orders by a specific user
        public List<OrdersDO> ViewOrderByStatus(byte status)
        {
            List<OrdersDO> orderData = new List<OrdersDO>();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewByStatus = new SqlCommand("ORDERS_SELECT_BY_STATUS", sqlConnection))
                {
                    //giving up after 60 seconds
                    viewByStatus.CommandType = CommandType.StoredProcedure;
                    viewByStatus.CommandTimeout = 60;

                    //inserting the UserID to sort entries
                    viewByStatus.Parameters.AddWithValue("Status", status);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByStatus.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orderData.Add(MapperDAL.ReaderToOrder(reader));
                        }
                    }
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.ErrorLogPath = _ErrorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning order
            return orderData;
        }
    }
}
