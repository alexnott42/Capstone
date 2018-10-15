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
    public class UsersDAO
    {
        //Establishing connections and file locations
        public readonly string connectionString;
        public readonly string errorLogPath;

        //constructor
        public UsersDAO(string _connectionString, string _errorLogPath)
        {
            this.connectionString = _connectionString;
            this.errorLogPath = _errorLogPath;
        }

        //Retrieving User Data from the Database
        public List<UsersDO> ViewAllUsers()
        {
            //Creating a list of users
            List<UsersDO> usersList = new List<UsersDO>();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand viewUserTable = new SqlCommand("USERS_SELECT_ALL", sqlConnection))
                {
                    //giving up after 60 seconds
                    viewUserTable.CommandType = CommandType.StoredProcedure;
                    viewUserTable.CommandTimeout = 60;

                    sqlConnection.Open();

                    //Reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewUserTable.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //creating a new user object for each entry and adding them to a list
                            UsersDO user = MapperDAL.ReaderToUser(reader);
                            usersList.Add(user);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning list of all users
            return usersList;
        }

        //retrieve a single user entry from database
        public UsersDO ViewUserByID(int UserID)
        {
            UsersDO userData = new UsersDO();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand viewByID = new SqlCommand("USERS_SELECT_BY_ID", sqlConnection))
                {
                    //give up after 60 seconds
                    viewByID.CommandType = CommandType.StoredProcedure;
                    viewByID.CommandTimeout = 60;

                    //inserting the UserID to sort through entries
                    viewByID.Parameters.AddWithValue("UserID", UserID);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByID.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = MapperDAL.ReaderToUser(reader);
                        }
                    }
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning information
            return userData;
        }

        //Creating a new User entry
        public int CreateNewUserEntry(UsersDO userInfo)
        {
            int rowsAffected = 0;

            try
            {
                //defining commands
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand createUser = new SqlCommand("USERS_CREATE_NEW", sqlConnection))
                {
                    //timing out after 60 seconds
                    createUser.CommandType = CommandType.StoredProcedure;
                    createUser.CommandTimeout = 60;

                    //inserting information
                    createUser.Parameters.AddWithValue("Username", userInfo.Username);
                    createUser.Parameters.AddWithValue("Email", userInfo.Email);
                    createUser.Parameters.AddWithValue("Password", userInfo.Password);
                    createUser.Parameters.AddWithValue("ESOname", userInfo.ESOname);
                    createUser.Parameters.AddWithValue("RoleID", userInfo.RoleID);
                    createUser.Parameters.AddWithValue("Server", userInfo.Server);

                    //Saving information to database
                    sqlConnection.Open();
                    rowsAffected = createUser.ExecuteNonQuery();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            return rowsAffected;
        }

        //Updating an existing user
        public int UpdateUserInformation(UsersDO userInfo)
        {
            int rowsAffected = 0;

            try
            {
                //defining some commands
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand updateUser = new SqlCommand("USERS_UPDATE_ACCOUNT", sqlConnection))
                {
                    //timing out after 60 seconds
                    updateUser.CommandType = CommandType.StoredProcedure;
                    updateUser.CommandTimeout = 60;

                    //inserting information
                    updateUser.Parameters.AddWithValue("Username", userInfo.Username);
                    updateUser.Parameters.AddWithValue("Email", userInfo.Email);
                    updateUser.Parameters.AddWithValue("Password", userInfo.Password);
                    updateUser.Parameters.AddWithValue("ESOname", userInfo.ESOname);
                    updateUser.Parameters.AddWithValue("RoleID", userInfo.RoleID);
                    updateUser.Parameters.AddWithValue("Server", userInfo.Server);

                    //Saving information to database
                    sqlConnection.Open();
                    rowsAffected = updateUser.ExecuteNonQuery();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            return rowsAffected;
        }

        //deleting a user entry
        public int DeleteUserEntry(int UserID)
        {
            int rowsAffected = 0;

            try
            {
                //defining connections
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand deleteUser = new SqlCommand("USERS_DELETE_ACCOUNT", sqlConnection))
                {
                    //giving up after 60 seconds
                    deleteUser.CommandType = CommandType.StoredProcedure;
                    deleteUser.CommandTimeout = 60;
                    deleteUser.Parameters.AddWithValue("UserID", UserID);

                    //opening connection and executing procedure
                    sqlConnection.Open();
                    rowsAffected = deleteUser.ExecuteNonQuery();
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            return rowsAffected;
        }

        //retrieve a single user entry from database
        public UsersDO ViewUserByRole(Int16 RoleID)
        {
            UsersDO userData = new UsersDO();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand viewByRole = new SqlCommand("USERS_SELECT_BY_ROLE", sqlConnection))
                {
                    //giving up after 60 seconds
                    viewByRole.CommandType = CommandType.StoredProcedure;
                    viewByRole.CommandTimeout = 60;

                    //inserting the UserID to sort through entries
                    viewByRole.Parameters.AddWithValue("RoleID", RoleID);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByRole.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = MapperDAL.ReaderToUser(reader);
                        }
                    }
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning data
            return userData;
        }

        //retrieve a single user entry from database
        public UsersDO ViewUserByUsername(string Username)
        {
            UsersDO userData = new UsersDO();
            try
            {
                //defining commands to access the database
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand viewByUsername = new SqlCommand("USERS_SELECT_BY_USERNAME", sqlConnection))
                {
                    //time out after 60 seconds
                    viewByUsername.CommandType = CommandType.StoredProcedure;
                    viewByUsername.CommandTimeout = 60;

                    //inserting the UserID to sort through entries
                    viewByUsername.Parameters.AddWithValue("@Username", @Username);

                    //reading the data and using Mapper to store it
                    sqlConnection.Open();
                    using (SqlDataReader reader = viewByUsername.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = MapperDAL.ReaderToUser(reader);
                        }
                    }
                }
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.SqlErrorLog(sqlEx);
                throw sqlEx;
            }
            catch (Exception ex)
            {
                LoggerDAL.errorLogPath = errorLogPath;
                LoggerDAL.ErrorLog(ex);
                throw ex;
            }
            //returning data
            return userData;
        }
    }
}
