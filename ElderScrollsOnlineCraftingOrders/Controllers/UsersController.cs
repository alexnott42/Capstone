﻿using System;
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
        private readonly string filePath;
        private readonly string errorLogPath;
        private readonly string connectionString;
        private UsersDAO _UsersDAO;
        private List<UsersPO> userList = new List<UsersPO>();

        //constructor
        public UsersController()
        {
            filePath = Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~"));
            errorLogPath = filePath + @"\ErrorLog.txt";
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _UsersDAO = new UsersDAO(connectionString, errorLogPath);
        }
        // GET: Users


        //show only one user's information
        [SecurityFilter(3)]
        [HttpGet]
        public ActionResult Users(int UserID)
        {
            UsersPO userDetails = Mapper.UsersDOtoUsersPO(_UsersDAO.ViewUserByID(UserID));
            return View(userDetails);
        }

        //List of all users
        [SecurityFilter(5)]
        [HttpGet]
        public PartialViewResult _Users()
        {
            try
            {
                List<UsersDO> allUsers = _UsersDAO.ViewAllUsers();
                userList = Mapper.UsersListDOtoPO(allUsers);
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
            return PartialView("_Users", userList);
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
            ActionResult response = new ViewResult();
            try
            {
                //checks if model is valid
                if (ModelState.IsValid)
                {
                    //checking if valid user
                    UsersDO user = _UsersDAO.ViewUserByUsername(form.Username);
                    if (!(user.UserID == 0))
                    {
                        //checking if password is correct
                        if (form.Password.Equals(user.Password))
                        {
                            //setting session data
                            Session["UserID"] = user.UserID;
                            Session["Username"] = user.Username;
                            Session["ESOname"] = user.ESOname;
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
                    else
                    {
                        //informing user that information was incorrect and returning to form view
                        ModelState.AddModelError("Password", "Username or password was incorrect");
                        response = View(form);
                    }
                }
                else
                {
                    //returning to form view if model state is invalid
                    response = View(form);
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
            //returning response view
            return response;
        }

        //logout
        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                //clearing session data and redirecting to home page
                Session.Abandon();
                return RedirectToAction("Index", "Home");
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

        //create new account
        [HttpGet]
        public ActionResult CreateNewAccount()
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

        //create new account continued
        [HttpPost]
        public ActionResult CreateNewAccount(UsersPO form)
        {
            ActionResult response;
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
            try
            {
                //storing data to database
                UsersDO UserDO = Mapper.UsersPOtoUsersDO(form);
                _UsersDAO.UpdateUserInformation(UserDO);
                //setting response page
                response = RedirectToAction("Users", "Users");
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
                response = RedirectToAction("Users", "Users");
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