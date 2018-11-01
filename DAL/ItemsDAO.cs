using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.dalMappers;
using DAL.dalModels;
using System.Data.SqlClient;
using System.Data;
using DAL.dalLogger;

namespace DAL
{
    public class ItemsDAO
    {
        //Establishing connection and file location
        private readonly string _ConnectionString;
        private readonly string _ErrorLogPath;

        //constructor
        public ItemsDAO(string connectionString, string errorLogPath)
        {
            this._ConnectionString = connectionString;
            this._ErrorLogPath = errorLogPath;
        }

        //Viewing a single item entry
        public ItemsDO ViewItemByID(int ItemID)
        {
            ItemsDO singleItem = new ItemsDO();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewItemByID = new SqlCommand("ITEMS_SELECT_BY_ID", sqlConnection))
                {
                    //telling it to time out after 60 seconds
                    viewItemByID.CommandType = CommandType.StoredProcedure;
                    viewItemByID.CommandTimeout = 60;

                    //using the ItemID to return only that item
                    viewItemByID.Parameters.AddWithValue("ItemID", ItemID);

                    sqlConnection.Open();

                    //collecting and storing the data
                    using (SqlDataReader reader = viewItemByID.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            singleItem = MapperDAL.ReaderToItem(reader);
                        }
                    }
                }
            }
            //Logging any errors
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

            //returning data
            return singleItem;
        }

        //Retrieving all items under a single order
        public List<ItemsDO> ItemsByOrderID(int orderID)
        {
            List<ItemsDO> items = new List<ItemsDO>();
            try
            {
                //Defining some commands again
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand viewByOrder = new SqlCommand("ITEMS_SELECT_BY_ORDER_ID", sqlConnection))
                {
                    //after 60 seconds, it stops trying to complete
                    viewByOrder.CommandType = CommandType.StoredProcedure;
                    viewByOrder.CommandTimeout = 60;

                    //Sorting the table by OrderID, so only items in an order are returned
                    viewByOrder.Parameters.AddWithValue("OrderID", orderID);

                    sqlConnection.Open();

                    //gathering and storing data
                    using (SqlDataReader reader = viewByOrder.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //creating a new item object for each entry and adding them all to a list
                            ItemsDO item = MapperDAL.ReaderToItem(reader);
                            items.Add(item);
                        }
                    }
                }
            }
            //keeping a log of any errors
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
            //returning list of items under that order
            return items;
        }

        //creating a new item entry
        public int CreateNewItemEntry(ItemsDO itemInfo)
        {
            int rowsAffected = 0;

            try
            {
                //defining commands
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand createItem = new SqlCommand("ITEMS_CREATE_NEW", sqlConnection))
                {

                    //timing out after 60 seconds
                    createItem.CommandType = CommandType.StoredProcedure;
                    createItem.CommandTimeout = 60;

                    //inserting information
                    createItem.Parameters.AddWithValue("Type", itemInfo.Type);
                    createItem.Parameters.AddWithValue("SubType", itemInfo.SubType);
                    createItem.Parameters.AddWithValue("Trait", itemInfo.Trait);
                    createItem.Parameters.AddWithValue("Style", itemInfo.Style);
                    createItem.Parameters.AddWithValue("Set", itemInfo.Set);
                    createItem.Parameters.AddWithValue("Level", itemInfo.Level);
                    createItem.Parameters.AddWithValue("Quality", itemInfo.Quality);
                    createItem.Parameters.AddWithValue("OrderID", itemInfo.OrderID);
                    createItem.Parameters.AddWithValue("Price", itemInfo.Price);

                    //Saving information to database
                    sqlConnection.Open();
                    rowsAffected = createItem.ExecuteNonQuery();
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
        //updating an existing item entry
        public int UpdateItemEntryInformation(ItemsDO itemInfo)
        {
            int rowsAffected = 0;

            try
            {
                //defining some commands
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand updateItem = new SqlCommand("ITEMS_UPDATE", sqlConnection))
                {
                    //timing out after 60 seconds
                    updateItem.CommandType = CommandType.StoredProcedure;
                    updateItem.CommandTimeout = 60;

                    //inseting information
                    updateItem.Parameters.AddWithValue("ItemID", itemInfo.ItemID);
                    updateItem.Parameters.AddWithValue("Type", itemInfo.Type);
                    updateItem.Parameters.AddWithValue("SubType", itemInfo.SubType);
                    updateItem.Parameters.AddWithValue("Trait", itemInfo.Trait);
                    updateItem.Parameters.AddWithValue("Style", itemInfo.Style);
                    updateItem.Parameters.AddWithValue("Set", itemInfo.Set);
                    updateItem.Parameters.AddWithValue("Level", itemInfo.Level);
                    updateItem.Parameters.AddWithValue("Quality", itemInfo.Quality);
                    updateItem.Parameters.AddWithValue("OrderID", itemInfo.OrderID);
                    updateItem.Parameters.AddWithValue("Price", itemInfo.Price);

                    //opening connection and completing procedure
                    sqlConnection.Open();
                    rowsAffected = updateItem.ExecuteNonQuery();
                }
            }
            //Logging errors
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

        //deleting an item entry
        public int DeleteItemEntry(int ItemID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
                using (SqlCommand deleteItem = new SqlCommand("ITEMS_DELETE", sqlConnection))
                {
                    //Defining procedure and timing out after 60 seconds 
                    deleteItem.CommandType = CommandType.StoredProcedure;
                    deleteItem.CommandTimeout = 60;

                    //adding ItemID as parameter
                    deleteItem.Parameters.AddWithValue("ItemID", ItemID);

                    //opening connection and executing procedure
                    sqlConnection.Open();
                    rowsAffected = deleteItem.ExecuteNonQuery();
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
            }
            // logging errors
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
    }
}
