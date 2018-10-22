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
            errorLogPath = ConfigurationManager.AppSettings["errorLogPath"];
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _UsersDAO = new UsersDAO(connectionString, errorLogPath);
            Logger.errorLogPath = errorLogPath;

        }

        //show only one user's information
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult Users(int UserID)
        {
            ActionResult response;
            UsersPO userDetails = new UsersPO();
            try
            {
                //mapping all the data to the view page
                UsersDO userDO = _UsersDAO.ViewUserByID(UserID);
                userDetails = Mapper.UsersDOtoUsersPO(userDO);
                response = View(userDetails);
            }
            //logging exceptions and redirecting to error page
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
            //returning response view
            return response;
        }

        //List of all users
        [SecurityFilter(5)]
        [HttpGet]
        public ActionResult AllUsers()
        {
            ActionResult response;
            List<UsersPO> userList = new List<UsersPO>();

            try
            {
                //mapping all the data to the view page
                List<UsersDO> allUsers = _UsersDAO.ViewAllUsers();
                userList = Mapper.UsersListDOtoPO(allUsers);
                response = View(userList);
            }
            //logging errors
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
            //returning view
            return response;
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
            //checking modelstate
            if (ModelState.IsValid)
            {
                try
                {
                    //taking user input and mapping it to the database
                    form.RoleID = 3;
                    
                    UsersDO newUser = Mapper.UsersPOtoUsersDO(form);
                    _UsersDAO.CreateNewUserEntry(newUser);
                    //setting response view
                    response = RedirectToAction("Login", "Users");
                }
                //logging errors
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
            //return view page
            return response;
        }

        //update user
        [SecurityFilter(5)]
        [HttpGet]
        public ActionResult UpdateUser(int UserID)
        {
            UsersPO UserPO = new UsersPO();
            ActionResult response;
            try
            {
                //retrieving data and displaying to user
                UsersDO UserDO = _UsersDAO.ViewUserByID(UserID);
                UserPO = Mapper.UsersDOtoUsersPO(UserDO);
                response = View(UserPO);
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

        //update user continued
        [SecurityFilter(5)]
        [HttpPost]
        public ActionResult UpdateUser(UsersPO form)
        {
            ActionResult response;
            //checking modelstate
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
                Logger.SqlErrorLog(sqlEx);
                response = RedirectToAction("Error", "Shared");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                response = RedirectToAction("Error", "Shared");
            }
            //returning view
            return response;
        }

        //view users by role
        [SecurityFilter(5)]
        [HttpGet]
        public ActionResult UsersByRole (byte RoleID)
        {
            ActionResult response;
            List<UsersPO> userList = new List<UsersPO>();

            try
            {
                //mapping all the data to the view page
                List<UsersDO> allUsers = _UsersDAO.ViewUserByRole(RoleID);
                userList = Mapper.UsersListDOtoPO(allUsers);
                //return view with data 
                //todo: more comments
                response = View(userList);
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
        [SecurityFilter(5)]
        [HttpGet]
        public ActionResult UsersByServer(string server)
        {
            ActionResult response;
            List<UsersPO> userList = new List<UsersPO>();
            //todo: unnecessary isntantiation
            List<UsersDO> allUsers = new List<UsersDO>();

            try
            {
                //mapping all the data to the view page
                allUsers = _UsersDAO.ViewUserByServer(server);
                userList = Mapper.UsersListDOtoPO(allUsers);
                response = View(userList);
            }
            //logging errors
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
    }
}