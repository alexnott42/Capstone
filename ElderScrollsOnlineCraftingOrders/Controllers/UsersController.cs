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
    public class UsersController : Controller
    {
        //establishing connections, file locations, Data Access Objects, etc
        private readonly string errorLogPath;
        private readonly string connectionString;
        private UsersDAO _UsersDAO;

        //constructor
        public UsersController()
        {
            //TODO: figure out relative log paths
            //filePath = Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~"));
            //errorLogPath = filePath + @"\ErrorLog.txt";
            //this.filePath = ConfigurationManager.AppSettings["filePath"];
            errorLogPath = ConfigurationManager.AppSettings["errorLogPath"];
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _UsersDAO = new UsersDAO(connectionString, errorLogPath);
        }

        //show only one user's information
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult Users(int UserID)
        {
            UsersPO userDetails = new UsersPO();
            try
            {
                UsersDO userDO = _UsersDAO.ViewUserByID(UserID);
                userDetails = Mapper.UsersDOtoUsersPO(userDO);
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
            return View(userDetails);
        }

        //List of all users
        [SecurityFilter(5)]
        [HttpGet]
        public ActionResult AllUsers()
        {
            ActionResult result;
            List<UsersPO> userList = new List<UsersPO>();

            try
            {
                List<UsersDO> allUsers = _UsersDAO.ViewAllUsers();
                userList = Mapper.UsersListDOtoPO(allUsers);
                //todo: confirmations on deletes
                //todo: 
                result = View(userList);
            }
            //logging errors
            catch (SqlException sqlEx)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.SqlErrorLog(sqlEx);
                result = View(userList);
            }
            catch (Exception ex)
            {
                Logger.errorLogPath = errorLogPath;
                Logger.ErrorLog(ex);
                result = View(userList);
            }
            return View(userList);
        }

        //login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //login continued
        [HttpPost]
        public ActionResult Login(LoginPO form)
            //todo: implement password hashing
        {
            ActionResult response;
            //checks if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    //checking if valid user
                    UsersDO user = _UsersDAO.ViewUserByUsername(form.Username);
                    //checking if password is correct
                    if (!(user.UserID == 0) && form.Password.Equals(user.Password))
                    {
                        //setting session data
                        Session["UserID"] = user.UserID;
                        Session["Username"] = user.Username;
                        //Session["ESOname"] = user.ESOname;
                        Session["RoleID"] = user.RoleID;

                        //setting response to redirect to home page
                        response = RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //informing user that information was incorrect and returning to form view
                        ModelState.AddModelError("Password", "Username or password was incorrect");
                        response = View(form);
                    }
                }
                //logging errors
                catch (SqlException sqlEx)
                {
                    Logger.errorLogPath = errorLogPath;
                    Logger.SqlErrorLog(sqlEx);
                    //todo: redirect instead of throw
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Logger.errorLogPath = errorLogPath;
                    Logger.ErrorLog(ex);
                    throw ex;
                }
            }
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            //returning response view
            return response;
        }

        //logout
        [HttpGet]
        public ActionResult Logout()
        {
            //clearing session data and redirecting to home page
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //create new account
        [HttpGet]
        public ActionResult CreateNewAccount()
        {
                return View();
        }

        //create new account continued
        [HttpPost]
        public ActionResult CreateNewAccount(UsersPO form)
        {
            ActionResult response;
            if (ModelState.IsValid)
            {
                try
                {
                    //taking user input and mapping it to the database
                    UsersDO newUser = Mapper.UsersPOtoUsersDO(form);
                    _UsersDAO.CreateNewUserEntry(newUser);
                    //setting response view
                    response = RedirectToAction("Index", "Home");
                    //todo: make create new account log user in immediately after creation
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
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            //return view page
            return response;
        }

        //update user
        [SecurityFilter(5)]
        [HttpGet]
        public ActionResult UpdateUser(int UserID)
        {
            ActionResult response;
            try
            {
                //retrieving data and displaying to user
                UsersDO UserDO = _UsersDAO.ViewUserByID(UserID);
                UsersPO UserPO = Mapper.UsersDOtoUsersPO(UserDO);
                response = View(UserPO);
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

        //update user continued
        [SecurityFilter(5)]
        [HttpPost]
        public ActionResult UpdateUser(UsersPO form)
        {
            ActionResult response;
            if (ModelState.IsValid)
            {

                try
                {
                    //storing data to database
                    UsersDO UserDO = Mapper.UsersPOtoUsersDO(form);
                    _UsersDAO.UpdateUserInformation(UserDO);
                    //setting response page
                    response = RedirectToAction("AllUsers", "Users");
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
            else
            {
                //returning to form view if model state is invalid
                response = View(form);
            }
            //returning to user page
            return response;
        }

        //delete user
        [SecurityFilter(6)]
        [HttpGet]
        public ActionResult DeleteUser(int UserID)
        {
            ActionResult response;
            try
            {
                //executing procedure
                _UsersDAO.DeleteUserEntry(UserID);
                //setting response view
                response = RedirectToAction("AllUsers", "Users");
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