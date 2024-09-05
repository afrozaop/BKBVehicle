using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BKBVehicle.Models.BKBManager;
using BKBVehicle.Models.ViewModel;
using BKBVehicle.DataModel;


namespace BKBVehicle.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModels _login) //Log check
        {
            if (ModelState.IsValid) //validating the user inputs
            {
                SessionLookup slv = new SessionLookup();
                //var isExist = false;
                try
                {
                    using (BKBVehicleEntities _entity = new BKBVehicleEntities())  // out Entity name is "SampleMenuMasterDBEntites"
                    {
                        string ip = GetIp();
                        string password = PasswordManager.encryption(_login.UserId.Trim() + _login.Password).ToString();
                        var isExist = _entity.LoginUsers.Where(x => x.userID.Trim().ToLower() == _login.UserId.Trim().ToLower() && x.userPass.Equals(password)).FirstOrDefault(); //validating the user name in tblLogin table whether the user name is exist or not
                        if (isExist != null)
                        {
                            if (isExist.user_status == "I")
                            {
                                TempData["retMsg"] = "Please Active Your User ID";
                                return View();
                            }
                            LoginModels _loginCredentials = _entity.LoginUsers.Where
                                (x => x.userID.Trim().ToLower() == _login.UserId.Trim().ToLower()).Select
                                (x => new LoginModels
                                {
                                    UserId = x.userID,
                                    userName = x.userName,

                                    //RoleName = x.Role.role_name,
                                    UserRoleId = x.userRole,
                                    BranchCode = x.branch_code,
                                    UserStatus = x.user_status,
                                    UserMobile = x.UserMobile,
                              
                                    BankID = x.bankID,
                                    //role_priority = x.Role.role_priority.ToString()
                                }).FirstOrDefault();

                            List<MenuModels> _menus = _entity.RoleMenuMappings.Where(x => x.UserRoleId == _loginCredentials.UserRoleId).Select(x => new MenuModels
                            {
                                MainMenuId = x.SubMenu.main_menu_id,
                                MainMenuName = x.SubMenu.MainMenu.main_menu,
                                SubMenuId = x.SubMenuId,
                                SubMenuName = x.SubMenu.sub_menu_name,
                                ControllerName = x.SubMenu.menu_controller,
                                ActionName = x.SubMenu.menu_action,
                                RoleId = x.UserRoleId,
                                RoleName = x.Role.role_name,
                                MenuStatus = x.IsActive
                            }).OrderBy(x => x.MainMenuId).ToList();//Get the Menu details from entity and bind it in MenuModels list.
                            FormsAuthentication.SetAuthCookie(_loginCredentials.UserId, false); // set the formauthentication cookie
                            Session["LoginCredentials"] = _loginCredentials; // Bind the _logincredentials details to "LoginCredentials" session

                            if (isExist.first_login == "Y")
                            {
                                return RedirectToAction("FirstLogin", "Account");
                            }

                            _loginCredentials.BranchName = slv.getBranchName(_loginCredentials.BranchCode);
                            _loginCredentials.division = slv.getDivisionName(_loginCredentials.BranchCode);
                            _loginCredentials.region = slv.getRegionName(_loginCredentials.BranchCode);


                            Session["MenuMaster"] = _menus; //Bind the _menus list to MenuMaster session
                            Session["UserID"] = _loginCredentials.UserId;
                            Session["BranchCode"] = _loginCredentials.BranchCode;
                            Session["Branch"] = slv.getBranchName(_loginCredentials.BranchCode);
                            Session["Division"] = slv.getDivisionName(_loginCredentials.BranchCode);
                            Session["Region"] = slv.getRegionName(_loginCredentials.BranchCode);
                            Session["UserRoleId"] = _loginCredentials.UserRoleId;
                            Session["userName"]= _loginCredentials.userName;

                            return RedirectToAction("Welcome", "Home");
                        }
                        else
                        {
                            TempData["retMsg"] = "Please enter the valid credentials!...";
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["retMsg"] = "Error! " + ex.Message;
                    return View();
                }
            }
            return View();
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }
    }
}