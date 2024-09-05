using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BKBVehicle.DataModel;
using BKBVehicle.Models.ViewModel;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using BKBVehicle.Models.BKBManager;



namespace BKBVehicle.Controllers

   {
    public class AccountController : Controller
    {

        BKBVehicleEntities db = new BKBVehicleEntities();

        public string pageSized = ConfigurationManager.AppSettings["PageSize"];

        [HttpGet]
        public ActionResult BranchList(int? page, string currentFilter, string searchString)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            if (login != null)
            {
                var branchDet = from x in db.Branches
                                join b in db.Banks on x.bankID equals b.id
                                where b.id == login.BankID
                                select new BranchViewModel
                                {
                                    branch_code = x.branch_code,
                                    branch_name = x.branch_name,
                                    region = x.region,
                                    BBCode = x.BBCode,
                                    RoutingNo = x.RoutingNo,

                                    //agentID = x.agentID,
                                    //agentSequence = x.agentSequence,
                                    //token = x.token,
                                    //Address1 = x.Address1,
                                    //Address2 = x.Address2,
                                    //Address3 = x.Address3,
                                    City = x.City,
                                    //MainOffice = x.MainOffice,
                                    Status = x.Status,
                                    //StoreNum = x.StoreNum,
                                    ISO = x.ISO,
                                    ZIP = x.ZIP,
                                    //POSStatus = x.POSStatus,
                                    //UnitProfileID = x.UnitProfileID,
                                    //AgentPIN = x.AgentPIN,
                                    //RAMS = x.RAMS,
                                    //Partyno = x.Partyno,
                                    //HQPartyno = x.HQPartyno,
                                    //UnitOffice = x.UnitOffice,
                                    //Legacy = x.Legacy
                                };
                if (login.UserRoleId != 1)
                {
                    branchDet = branchDet.Where(o => o.branch_code.Equals(login.BranchCode));
                }
                if (searchString != null)
                {
                    branchDet = branchDet.Where(o => o.branch_code.Equals(searchString) || o.branch_name.Contains(searchString) || o.agentID.Equals(searchString));
                }
                List<BranchViewModel> userList = branchDet.OrderBy(x => x.branch_code).ToList();
                int pageSize = Convert.ToInt16(pageSized);
                int pageNumber = (page ?? 1);
                return View(userList.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        //[HttpGet]
        public ActionResult Index(int? page, string currentFilter, string searchString)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            if (login != null)
            {
                var userDet = from x in db.LoginUsers
                              join br in db.Branches on x.branch_code equals br.branch_code
                              join rl in db.Roles on x.userRole equals rl.id
                              join b in db.Banks on x.bankID equals b.id
                              where b.id == login.BankID
                              select new UserSignUpModel
                              {
                                  userID = x.userID,
                                  userName = x.userName,
                                  userRole = x.userRole,
                                 
                                  roleName = rl.role_name,
                                  branch_code = x.branch_code,
                                  branchName = br.branch_name,
                                  user_status = x.user_status.Equals("A") ? "Active" : "In-Active"
                                  //UserMobile = br.e

                              };
                if (login.UserRoleId != 1)
                {
                    userDet = userDet.Where(o => o.branch_code.Equals(login.BranchCode));
                }
                if (searchString != null)
                {
                    userDet = userDet.Where(o => o.userID.Contains(searchString) || o.userName.Contains(searchString));
                }
                List<UserSignUpModel> userList = userDet.OrderBy(x => x.userRole).ToList();
                int pageSize = Convert.ToInt16(pageSized);
                int pageNumber = (page ?? 1);
                return View(userList.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

  
        public ActionResult Create()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                //int rolP = Convert.ToInt32(login.role_priority);
                UserSignUpModel usm = new UserSignUpModel();
                var queryRole = from q in db.Roles
                                select new
                                {
                                    value = q.id,
                                    description = q.role_name
                                };
                usm.roleList = new SelectList(queryRole.ToList(), "value", "description");

                var query = from q in db.Banks
                            select new
                            {
                                value = q.id,
                                description = q.bankShortName + "--" + q.bankName
                            };

                if (login.BankID != 1)
                {
                    query = query.Where(o => o.value.Equals(login.BankID));
                }


                usm.BankList = new SelectList(query.ToList(), "value", "description");

                return View(usm);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public JsonResult getBankBranch(int bankId)
        {

            var ddlbankId = db.Branches.Where(x => x.bankID == bankId).ToList();


            List<SelectListItem> liBranches = new List<SelectListItem>();

            liBranches.Add(new SelectListItem { Text = "Select a Branch", Value = "0" });
            if (ddlbankId != null)
            {
                foreach (var x in ddlbankId)
                {
                    liBranches.Add(new SelectListItem { Text = x.branch_code + "-" + x.branch_name, Value = x.branch_code });
                }
            }
            return Json(new SelectList(liBranches, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        [HttpPost]
        public ActionResult Create(UserSignUpModel USM)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];

            LoginUser LU = new LoginUser();

            if (login != null)
            {

                var userExists = db.LoginUsers.Where(o => o.userID.Equals(USM.userID)).FirstOrDefault();
                if (userExists != null)
                {
                    TempData["retMsg"] = "User ID already exists! Please try different user ID.";
                    return RedirectToAction("Create", "Account");
                }
                //int rolP = Convert.ToInt32(login.role_priority.ToString());
                try
                {
                    LU.userID = USM.userID;
                    LU.userName = USM.userName;
                    LU.userPass = PasswordManager.encryption(USM.userID + USM.userPass).ToString();
                    LU.branch_code = USM.branch_code;
                    LU.userRole = USM.userRole;
                    LU.user_status = USM.user_status;
                    LU.UserMobile = USM.UserMobile;
                    LU.first_login = "Y";
                    LU.makerID = login.UserId;
                    LU.makerTime = DateTime.Now;
                    LU.bankID = USM.BankID;

                    db.LoginUsers.Add(LU);

                    db.SaveChanges();

                    TempData["retMsg"] = USM.userID + " user created successfully.";
                    return RedirectToAction("Index", "Account");
                }
                catch (Exception ex)
                {
                    TempData["retMsg"] = "Error Occured!" + ex.Message;
                    return RedirectToAction("Index", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public ActionResult Edit(string userID)
        {
            LoginModels logIn = new LoginModels();
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];

            UserSignUpModel usm = new UserSignUpModel();
            if (login != null)
            {
                //int rolP = Convert.ToInt32(login.role_priority);
                try
                {
                    var userDet = from x in db.LoginUsers
                                  where x.userID == userID
                                  select new UserSignUpModel
                                  {
                                      userID = x.userID,
                                      userName = x.userName,
                                      userRole = x.userRole,
                                      branch_code = x.branch_code,
                                      BankID = x.bankID,
                                      user_status = x.user_status,
                                      UserMobile = x.UserMobile
                                  };

                    usm = userDet.SingleOrDefault();

                    var queryRole = from q in db.Roles
                                    select new
                                    {
                                        value = q.id,
                                        description = q.role_name
                                    };
                    usm.roleList = new SelectList(queryRole.ToList(), "value", "description");

                    var query = from q in db.Banks
                                select new
                                {
                                    value = q.id,
                                    description = q.bankShortName + "--" + q.bankName
                                };

                    if (login.BankID != 1)
                    {
                        query = query.Where(o => o.value.Equals(login.BankID));
                    }

                    usm.BankList = new SelectList(query.ToList(), "value", "description");

                    var queryBr = from q in db.Branches
                                  where q.bankID == login.BankID
                                  select new
                                  {
                                      value = q.branch_code,
                                      description = q.branch_code + "--" + q.branch_name
                                  };

                    if (login.UserRoleId != 1)
                    {
                        query = query.Where(o => o.value.Equals(login.BranchCode));
                    }

                    usm.BranchList = new SelectList(queryBr.ToList(), "value", "description");

                    return View(usm);
                }
                catch (Exception ex)
                {
                    TempData["retMsg"] = "Error!" + ex.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Edit(UserSignUpModel usm)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                bool txnStatus = true;
                var txn = db.Database.BeginTransaction();
                try
                {
                    LoginUser LU = db.LoginUsers.Find(usm.userID);

                    LU.userName = usm.userName;
                    LU.branch_code = usm.branch_code;
                    LU.userRole = usm.userRole;
                    LU.user_status = usm.user_status;
                    LU.UserMobile = usm.UserMobile;
                    LU.updatedBY = login.UserId;
                    LU.updateTime = DateTime.Now;
                    //LU.bankID = usm.BankID;

                    db.SaveChanges();
                    TempData["retMsg"] = usm.userID + " Updated successfully!";
                    return RedirectToAction("Index", "Account");
                }
                catch (Exception ex)
                {
                    txnStatus = false;
                    TempData["retMsg"] = "Error occured!" + ex.Message;
                    return RedirectToAction("Index", "Account");
                }
                finally
                {
                    if (txnStatus == true)
                    {
                        txn.Commit();
                    }
                    else
                    {
                        txn.Rollback();
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeModel pcm)
        {
            var _login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            string userID = _login.UserId.ToString();
            try
            {
                string password = PasswordManager.encryption(_login.UserId.Trim() + pcm.OldPassword).ToString();
                var isExist = db.LoginUsers.Where(x => x.userID.Trim().ToLower() == _login.UserId.Trim().ToLower() && x.userPass.Equals(password)).SingleOrDefault();
                if (isExist != null)
                {
                    LoginUser lu = db.LoginUsers.Find(userID);

                    lu.userPass = PasswordManager.encryption(lu.userID + pcm.NewPassword);
                    lu.first_login = "N";
                    db.SaveChanges();
                    TempData["retMsg"] = "Password has been changed successfully.";
                    return View();
                }
                else
                {
                    TempData["retMsg"] = "Old password is incorrect.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["retMsg"] = "Unknown Error! Please try again.";
                return RedirectToAction("ChangePassword", "Account");
            }
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                PasswordResetModel psm = new PasswordResetModel();
                var queryUsr = from q in db.LoginUsers
                               join b in db.Branches on q.branch_code equals b.branch_code
                               select new
                               {
                                   value = q.userID,
                                   description = q.userID + "-" + q.userName + "-" + b.branch_name
                                   //bankID = q.bankID
                               };

                //if (login.BankID != 1)
                //{
                //    queryUsr = queryUsr.Where(o => o.bankID.Equals(login.BankID));
                //}

                psm.userList = new SelectList(queryUsr.ToList(), "value", "description");
                return View(psm);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(PasswordResetModel prm)
        {
            var _login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            //string userID = _login.UserId.ToString();
            try
            {
                //string password = PasswordManager.encryption(prm.userID.Trim() + prm.NewPassword).ToString();
                var isExist = db.LoginUsers.Where(x => x.userID.Trim().ToLower() == prm.userID.Trim().ToLower()).SingleOrDefault();
                if (isExist != null)
                {
                    LoginUser lu = db.LoginUsers.Find(prm.userID);

                    lu.userPass = PasswordManager.encryption(prm.userID + "123456");
                    lu.first_login = "Y";
                    db.SaveChanges();
                    TempData["retMsg"] = "Your password has been changed.New password 123456";
                    return RedirectToAction("ResetPassword", "Account");
                }
                else
                {
                    TempData["retMsg"] = "User does not exists in the system!";
                    return RedirectToAction("ResetPassword", "Account");
                }
            }
            catch (Exception ex)
            {
                TempData["retMsg"] = "Unknown Error! Please try again.";
                return RedirectToAction("ResetPassword", "Account");
            }
        }
        //[HttpGet]
        //public ActionResult ResetPassword()
        //{
        //    var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
        //    if (login != null)
        //    {
        //        PasswordResetModel psm = new PasswordResetModel();
        //        var queryUsr = from q in db.LoginUsers
        //                       select new
        //                       {
        //                           value = q.userID,
        //                           description = q.userID + "--" + q.userName,
        //                           //bankID = q.bankID
        //                       };

        //        //if (login.BankID != 1)
        //        //{
        //        //    queryUsr = queryUsr.Where(o => o.bankID.Equals(login.BankID));
        //        //}

        //        psm.userList = new SelectList(queryUsr.ToList(), "value", "description");
        //        return View(psm);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}

        //[HttpPost]
        //public ActionResult ResetPassword(PasswordResetModel prm)
        //{
        //    var _login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
        //    //string userID = _login.UserId.ToString();
        //    try
        //    {
        //        //string password = PasswordManager.encryption(prm.userID.Trim() + prm.NewPassword).ToString();
        //        var isExist = db.LoginUsers.Where(x => x.userID.Trim().ToLower() == prm.userID.Trim().ToLower()).SingleOrDefault();
        //        if (isExist != null)
        //        {
        //            LoginUser lu = db.LoginUsers.Find(prm.userID);

        //            lu.userPass = PasswordManager.encryption(prm.userID + "123456");
        //            lu.first_login = "Y";
        //            db.SaveChanges();
        //            TempData["retMsg"] = "Your password has been changed.New password 123456";
        //            return RedirectToAction("ResetPassword", "Account");
        //        }
        //        else
        //        {
        //            TempData["retMsg"] = "User does not exists in the system!";
        //            return RedirectToAction("ResetPassword", "Account");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["retMsg"] = "Unknown Error! Please try again.";
        //        return RedirectToAction("ResetPassword", "Account");
        //    }
        //}
        [HttpGet]
        public ActionResult FirstLogin()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult FirstLogin(PasswordChangeModel pcm)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            string userID = login.UserId.ToString();
            try
            {
                LoginUser lu = db.LoginUsers.Find(userID);

                lu.userPass = PasswordManager.encryption(lu.userID.Trim() + pcm.NewPassword).ToString();
                lu.first_login = "N";
                db.SaveChanges();

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                Console.Write("Error " + ex);
                return View("Index", "Login");
            }
        }
    }
}