using BKBVehicle.DataModel;
using BKBVehicle.Models.VehicleAppModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.Ajax.Utilities;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.IO;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.UI;
using System.Data.Entity.Infrastructure;
using System.Web.UI.WebControls;
using BKBVehicle.Models.ViewModel;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace BKBVehicle.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        BKBVehicleEntities db = new BKBVehicleEntities();


        public JsonResult GetPFList(string name)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<EmployeeInfo> PFENList = db.EmployeeInfoes.Where(x => x.PFBN == name).ToList();
            
            return Json(PFENList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegNoList(string name)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car_Details> RegNoList = db.Car_Details.ToList();
            var SumTK = db.Car_Details.Where(x => x.RegistrationNo == name).Select(x => x.Depriciation).FirstOrDefault();
            if (SumTK == null)
            {
                SumTK = '0';
            }

            RegNoList[0].Depriciation = SumTK;
            return Json(RegNoList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VehicleInfo()

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                {
                    var getDivision = db.Branches.Where(x => x.branch_code == login.BranchCode).FirstOrDefault();
                    login.division = getDivision.RoutingNo;
                    var getRegion = db.Branches.Where(x => x.branch_code == login.BranchCode).FirstOrDefault();
                    login.region = getDivision.region;
                    if (login.UserRoleId == 1 || login.UserRoleId == 10 || login.UserRoleId == 11)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where q.POSStatus == "H" || q.POSStatus == "D" || q.POSStatus == "R" || q.POSStatus == "DAO"
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 2 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.POSStatus == "D" || q.POSStatus == "R" || q.POSStatus == "DAO") && (q.RoutingNo == login.division)
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 3 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.POSStatus == "R" && q.region == login.region)
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 8 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.RoutingNo == "LPO")
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 9 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.branch_code == "4121")
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                }
                var VEHICLEList = from q in db.Varities_Table
                                  where q.Field == "Vehicle"
                                  select new
                                  {
                                      value = q.Value,
                                      description = q.Value
                                  };
                ViewBag.VEHICLEList = new SelectList(VEHICLEList.ToList(), "value", "description");
                var Color = from q in db.Varities_Table
                                where  q.Flag==2
                                select new
                                {
                                    value = q.Value,
                                    description = q.Value
                                };
                ViewBag.ColorList = new SelectList(Color.ToList(), "value", "description");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]

        public ActionResult VehicleInfo(CarInfoViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Car_Info dd = new Car_Info();
                dd.OfficeName = ci.OfficeName;
                dd.VehicleType = ci.VehicleType;
                dd.RegistrationNo = ci.RegistrationNo;
                dd.BrandName = ci.BrandName;
                dd.Manufacturer = ci.Manufacturer;
                dd.ProductionYear = ci.ProductionYear;
                dd.Remarks = ci.Remarks;
                dd.PurchaseValue = ci.PurchaseValue;
                dd.Color = ci.Color;
                dd.ChesisNo = ci.ChesisNo;
                dd.EngineNo = ci.EngineNo;
                dd.Comments = ci.Comments;
                dd.PurchaseDate = ci.PurchaseDate;
                dd.Flag = ci.Flag;
                dd.BrCode = login.BranchCode;
                dd.CreatedOn = System.DateTime.Now;
                dd.CreatedBy = login.UserId;
            
                db.Car_Info.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Vehicle Information Saved Successfully.";
                return RedirectToAction("VehicleInfo", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult VehicleInfoList()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                var carList = db.Car_Info.Where(o => o.ApprovedStatus != "Y").ToList().OrderBy(o => o.RegistrationNo);
                return View(carList);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }



        [HttpGet]
        public ActionResult CarDetails(int id)
        {
            var result = from i in db.Car_Info
                         where i.id == id
                         select new CarInfoViewModel
                         {

                             RegistrationNo = i.RegistrationNo,
                             OfficeName = i.OfficeName,
                             VehicleType = i.VehicleType,

                             BrandName = i.BrandName,
                             Manufacturer = i.Manufacturer,
                             ProductionYear = i.ProductionYear,
                             PurchaseValue = i.PurchaseValue,
                             Color = i.Color,
                             ChesisNo = i.ChesisNo,
                             EngineNo = i.EngineNo,
                             PurchaseDate = i.PurchaseDate,
                             BrCode = i.BrCode,
                         };
            CarInfoViewModel List = result.FirstOrDefault();
            return View(List);

        }




        [HttpGet]
        public ActionResult approvalCar(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var inq = db.Car_Info.Where(o => o.id == id).SingleOrDefault();
            return View(inq);
        }
        [HttpPost]
        public ActionResult approvalCar(CarInfoViewModel i, string btn)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (login != null)
            {

                try
                {
                    if (btn == "Approve" && i.id > 0)
                    {
                        Car_Info dd = new Car_Info();
                        dd = db.Car_Info.Find(i.id);

                        dd.ApprovedStatus = "Y";
                        dd.ApprovedBy = login.UserId;
                        dd.ApprovedOn = System.DateTime.Now;


                        db.SaveChanges();
                        TempData["retMsg"] = "Car Information approved Successfully.";

                        return RedirectToAction("VehicleInfoList", "Vehicle");

                    }

                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            TempData["errMsg"] = TempData["errMsg"] + " " + "Property: " + validationError.PropertyName
                                + " Error: " + validationError.ErrorMessage;
                            //logger.Error("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }

                    return RedirectToAction("VehicleInfoList", "Vehicle", null);
                }
                catch (Exception ex)
                {


                    TempData["errMsg"] = ex.Message;
                    return RedirectToAction("VehicleInfoList", "Vehicle", null);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }




        [HttpGet]
        public ActionResult VehicleinfoEdit(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                {
                    var getDivision = db.Branches.Where(x => x.branch_code == login.BranchCode).FirstOrDefault();
                    login.division = getDivision.RoutingNo;
                    var getRegion = db.Branches.Where(x => x.branch_code == login.BranchCode).FirstOrDefault();
                    login.region = getDivision.region;

                    if (login.UserRoleId == 1 || login.UserRoleId == 10 || login.UserRoleId == 11)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where q.POSStatus == "H" || q.POSStatus == "D" || q.POSStatus == "R" || q.POSStatus == "DAO"
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 2 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.POSStatus == "D" || q.POSStatus == "R" || q.POSStatus == "DAO") && (q.RoutingNo == login.division)
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 3 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.POSStatus == "R" && q.region == login.region)
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 8 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.RoutingNo == "LPO")
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                    else if (login.UserRoleId == 9 || login.UserRoleId == 10)
                    {
                        var OFFICELIST = from q in db.Branches

                                         where (q.branch_code == "4121")
                                         select new
                                         {
                                             value = q.branch_name,
                                             description = q.branch_name + " - " + q.branch_code
                                         };
                        ViewBag.OFFICELIST = new SelectList(OFFICELIST.ToList(), "value", "description");
                    }
                }

                var VEHICLEList = from q in db.Varities_Table
                                  where q.Field == "Vehicle"
                                  select new
                                  {
                                      value = q.Value,
                                      description = q.Value
                                  };
                ViewBag.VEHICLEList = new SelectList(VEHICLEList.ToList(), "value", "description");
                var ColorList = from q in db.Varities_Table
                                where q.Field == "Color"
                                select new
                                {
                                    value = q.Value,
                                    description = q.Value
                                };
                ViewBag.ColorList = new SelectList(ColorList.ToList(), "value", "description");

                CarInfoViewModel ci = new CarInfoViewModel();
                var Vehicleinfo = db.Car_Info.Find(id);

                ci.OfficeName = Vehicleinfo.OfficeName;
                ci.VehicleType = Vehicleinfo.VehicleType;
                ci.RegistrationNo = Vehicleinfo.RegistrationNo;
                ci.BrandName = Vehicleinfo.BrandName;
                ci.Manufacturer = Vehicleinfo.Manufacturer;
                ci.ProductionYear = Vehicleinfo.ProductionYear;
                ci.Remarks = Vehicleinfo.Remarks;
                ci.PurchaseValue = Vehicleinfo.PurchaseValue;
                ci.Color = Vehicleinfo.Color;
                ci.ChesisNo = Vehicleinfo.ChesisNo;
                ci.EngineNo = Vehicleinfo.EngineNo;

                ci.PurchaseDate = Vehicleinfo.PurchaseDate;


                return View(ci);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        [HttpGet]
        public ActionResult VehicleEdit(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            CarInfoViewModel usm = new CarInfoViewModel();
            if (login != null)
            {
                //int rolP = Convert.ToInt32(login.role_priority);
                try
                {
                    var userDet = from x in db.Car_Info
                                  where x.id == id
                                  select new CarInfoViewModel
                                  {
                                      PurchaseDate = x.PurchaseDate,
                                      Color = x.Color,
                                      ChesisNo = x.ChesisNo,
                                      RegistrationNo = x.RegistrationNo,
                                      PurchaseValue = x.PurchaseValue,
                                      VehicleType = x.VehicleType,
                                      ProductionYear = x.ProductionYear
                                  };

                    usm = userDet.SingleOrDefault();

                    var ColorList = from q in db.Varities_Table
                                    where q.Field == "Color"
                                    select new
                                    {
                                        value = q.Value,
                                        description = q.Value
                                    };
                    usm.ColorList = new SelectList(ColorList.ToList(), "value", "description");
                    var VEHICLEList = from q in db.Varities_Table
                                      where q.Field == "Vehicle"
                                      select new
                                      {
                                          value = q.Value,
                                          description = q.Value
                                      };
                    usm.VEHICLEList = new SelectList(VEHICLEList.ToList(), "value", "description");

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

        public ActionResult VehicleInfoEdit(CarInfoViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Car_Info dd = new Car_Info();
                dd = db.Car_Info.Find(ci.id);
                dd.OfficeName = ci.OfficeName;
                dd.VehicleType = ci.VehicleType;
                dd.RegistrationNo = ci.RegistrationNo;
                dd.BrandName = ci.BrandName; 
                dd.Manufacturer = ci.Manufacturer;
                dd.ProductionYear = ci.ProductionYear;
                dd.Remarks = ci.Remarks;
                dd.PurchaseValue = ci.PurchaseValue;
                dd.Color = ci.Color;
                dd.ChesisNo = ci.ChesisNo;
                dd.EngineNo = ci.EngineNo;

                dd.PurchaseDate = ci.PurchaseDate;

                db.SaveChanges();
                TempData["retMsg"] = "Vehicle Information Updated Successfully.";
                return RedirectToAction("VehicleInfoList", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }




        [HttpGet]
        public ActionResult VehicleInfoDetails()

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]

        public ActionResult VehicleInfoDetails(Car_DetailsViewModel cd)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Car_Details dd = new Car_Details();

                dd.RegistrationNo = cd.RegistrationNo;
                dd.TaxToken = cd.TaxToken;
                dd.Fitness = cd.Fitness;
                dd.RoutePermit = cd.RoutePermit;
                // var PurValue = db.Car_Info.Where(x =>x.RegistrationNo==cd.RegistrationNo).Select(x => x.PurchaseValue);
                //int DepCal = Convert.ToInt32(PurValue);
                //dd.Depriciation = DepCal * (20 / 100);

                dd.Depriciation = 20;
                dd.Insurance = cd.Insurance;
                dd.BookValue = cd.BookValue;
                dd.Remarks = cd.Remarks;
                dd.Comments = cd.Comments;
                dd.Flag = cd.Flag;
                dd.BookValue = cd.BookValue;
                dd.BrCode = login.BranchCode;
                dd.CreatedOn = System.DateTime.Now;
                dd.CreatedBy = login.UserId;
                db.Car_Details.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Information Saved Successfully.";
                return RedirectToAction("VehicleInfoDetails", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult VehicleInfoDetailsList()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                return View(db.Car_Details.ToList().OrderBy(o => o.RegistrationNo));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public ActionResult VehicleInfoDetailsEdit(int id)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Car_DetailsViewModel ci = new Car_DetailsViewModel();
                var VehicleinfoDetails = db.Car_Details.Find(id);
                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");


                ci.RegistrationNo = VehicleinfoDetails.RegistrationNo;
                ci.TaxToken = VehicleinfoDetails.TaxToken;
                ci.Insurance = VehicleinfoDetails.Insurance;
                ci.Fitness = VehicleinfoDetails.Fitness;
                ci.Remarks = VehicleinfoDetails.Remarks;
                ci.RoutePermit = VehicleinfoDetails.RoutePermit;
                ci.Depriciation = 20;
                ci.BookValue = VehicleinfoDetails.BookValue;

                ci.Comments = VehicleinfoDetails.Comments;
                ci.Flag = VehicleinfoDetails.Flag;
                ci.BookValueDate = VehicleinfoDetails.BookValueDate;

                return View(ci);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]

        public ActionResult VehicleInfoDetailsEdit(Car_DetailsViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Car_Details dd = new Car_Details();
                dd = db.Car_Details.Find(ci.id);
                dd.RegistrationNo = ci.RegistrationNo;
                dd.TaxToken = ci.TaxToken;
                dd.Insurance = ci.Insurance;
                dd.Fitness = ci.Fitness;
                dd.Remarks = ci.Remarks;
                dd.RoutePermit = ci.RoutePermit;
                dd.Depriciation = 20;
                dd.BookValue = ci.BookValue;
 
                dd.Comments = ci.Comments;
                dd.Flag = ci.Flag;
                db.SaveChanges();
                TempData["retMsg"] = "Information Updated Successfully.";
                return RedirectToAction("VehicleInfoDetailsList", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }






        [HttpGet]
        public ActionResult FuelInfo()

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                var PFLIST = from q in db.EmployeeInfoes

                                
                                 select new
                                 {
                                     value = q.PFBN,
                                     description = q.PFBN
                                 };
                ViewBag.PFLIST = new SelectList(PFLIST.ToList(), "value", "description");

                var DriverPFList = from q in db.EmployeeInfoes

                                 where q.Designation == "Driver"
                                 select new
                                 {
                                     value = q.PFBN,
                                     description = q.PFBN
                                 };
                ViewBag.DriverPFList = new SelectList(DriverPFList.ToList(), "value", "description");
                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");
                var FuelList = from q in db.Varities_Table
                               where q.Field == "Fuel"
                               select new
                               {
                                   value = q.Value,
                                   description = q.Value
                               };
                ViewBag.FuelList = new SelectList(FuelList.ToList(), "value", "description");

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]

        public ActionResult FuelInfo(Fuel_DetailsViewModel fd)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Fuel_Details_Table dd = new Fuel_Details_Table();
                dd.UsedBy = fd.UsedBy;
                dd.UsedByPF = fd.UsedByPF;
                dd.Degination = fd.Degination;
                dd.DriverPF = fd.DriverPF;
                dd.Driver = fd.Driver;
                dd.RegistrationNo = fd.RegistrationNo;
                dd.FuelDate = fd.FuelDate;
                dd.Rate = fd.Rate;
                dd.FuelConsumptionCost = fd.FuelConsumptionCost;
                dd.FuelType = fd.FuelType;
                dd.FuelQuantity = fd.FuelQuantity;
                dd.Remarks = fd.Remarks;
                dd.Comments = fd.Comments;
                dd.Flag = fd.Flag;
                dd.BrCode = login.BranchCode;
                dd.CreatedOn = System.DateTime.Now;
                dd.CreatedBy = login.UserId;
                db.Fuel_Details_Table.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Fuel Information Saved Successfully.";
                return RedirectToAction("FuelInfoList", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult FuelInfoList()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                //return View(db.Fuel_Details_Table.ToList().OrderByDescending(o => o.FuelDate));
                var FuelList = db.Fuel_Details_Table.Where(o => o.ApprovedStatus != "Y").ToList().OrderBy(o => o.RegistrationNo);
                    //.OrderByDescending(o => o.FuelDate);

                return View(FuelList);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public ActionResult FuelInfoEdit(int id)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                

                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");

                var FuelList = from q in db.Varities_Table
                               where q.Field == "Fuel"
                               select new
                               {
                                   value = q.Value,
                                   description = q.Value
                               };
                ViewBag.FuelList = new SelectList(FuelList.ToList(), "value", "description");

                Fuel_DetailsViewModel fd = new Fuel_DetailsViewModel();
                var FuelInfo = db.Fuel_Details_Table.Find(id);

                fd.RegistrationNo = FuelInfo.RegistrationNo;
                fd.UsedBy = FuelInfo.UsedBy;
                fd.UsedByPF = FuelInfo.UsedByPF;
                fd.Degination = FuelInfo.Degination;
                fd.Driver = FuelInfo.Driver;
                fd.DriverPF = FuelInfo.DriverPF;
                fd.FuelDate = FuelInfo.FuelDate;
                fd.Rate=FuelInfo.Rate;
                fd.FuelConsumptionCost = FuelInfo.FuelConsumptionCost;
                fd.FuelQuantity = FuelInfo.FuelQuantity;
                //fd.FuelQuantity = FuelInfo.FuelConsumptionCost/FuelInfo.Rate;
                fd.Remarks = FuelInfo.Remarks;
                fd.Comments = FuelInfo.Comments;
                fd.Flag = FuelInfo.Flag;
                fd.FuelType = FuelInfo.FuelType;
       

                return View(fd);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]

        public ActionResult FuelInfoEdit(Fuel_DetailsViewModel fd)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Fuel_Details_Table dd = new Fuel_Details_Table();

                dd = db.Fuel_Details_Table.Find(fd.id);

                dd.RegistrationNo = fd.RegistrationNo;
                dd.UsedBy = fd.UsedBy;
                dd.UsedByPF = fd.UsedByPF;
                dd.Degination = fd.Degination;
                dd.Driver  = fd.Driver;
                dd.DriverPF = fd.DriverPF;
                dd.FuelDate = fd.FuelDate;
                dd.FuelType = fd.FuelType;
                dd.FuelQuantity = fd.FuelQuantity;
                dd.FuelConsumptionCost = fd.FuelConsumptionCost;
                dd.Rate = fd.Rate;
                dd.Remarks = fd.Remarks;
                dd.Comments = fd.Comments;
     

                db.SaveChanges();
                TempData["retMsg"] = "Fuel Information Updated Successfully.";
                return RedirectToAction("FuelInfoList", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult FuelDelete(int? id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Fuel_Details_Table vdd = db.Fuel_Details_Table.Find(id);
                if (vdd == null)
                    return HttpNotFound();
                return View(vdd);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpPost]
        public ActionResult FuelDelete(Fuel_Details_Table vobj)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Fuel_Details_Table vdd = db.Fuel_Details_Table.Find(vobj.id);
                if (vdd == null)
                    return HttpNotFound();
                db.Fuel_Details_Table.Remove(vdd);
                db.SaveChanges();
                TempData["retMsg"] = "Fuel Information Delete Successfully.";
                return RedirectToAction("FuelInfoList", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpGet]
        public ActionResult FuelDetails(int id)
        {
            var result = from i in db.Fuel_Details_Table
                         where i.id == id
                         select new Fuel_DetailsViewModel
                         {

                             RegistrationNo = i.RegistrationNo,
                             FuelDate = i.FuelDate,
                             FuelConsumptionCost = i.FuelConsumptionCost,
                             FuelType = i.FuelType,
                             FuelQuantity = i.FuelQuantity,
                             BillNo = i.BillNo,
                             UsedBy = i.UsedBy,
                             Driver = i.Driver,
                             BrCode = i.BrCode,
                             DriverPF = i.DriverPF,
                             UsedByPF = i.UsedByPF,
                             Degination = i.Degination,
                             Rate = i.Rate,



                         };
            Fuel_DetailsViewModel List = result.FirstOrDefault();
            return View(List);

        }


        [HttpGet]
        public ActionResult approvalFuel(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var inq = db.Fuel_Details_Table.Where(o => o.id == id).SingleOrDefault();
            return View(inq);
        }
        [HttpPost]
        public ActionResult approvalFuel(Fuel_DetailsViewModel i, string btn)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (login != null)
            {

                try
                {
                    if (btn == "Approve" && i.id > 0)
                    {
                        Fuel_Details_Table dd = new Fuel_Details_Table();
                        dd = db.Fuel_Details_Table.Find(i.id);

                        dd.ApprovedStatus = "Y";
                        dd.ApprovedBy = login.UserId;
                        dd.ApprovedOn = System.DateTime.Now;

                        db.SaveChanges();
                        TempData["retMsg"] = "Fuel Information approved Successfully.";

                        return RedirectToAction("FuelInfoList", "Vehicle");

                    }

                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            TempData["errMsg"] = TempData["errMsg"] + " " + "Property: " + validationError.PropertyName
                                + " Error: " + validationError.ErrorMessage;
                            //logger.Error("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }

                    return RedirectToAction("FuelInfoList", "Vehicle", null);
                }
                catch (Exception ex)
                {


                    TempData["errMsg"] = ex.Message;
                    return RedirectToAction("FuelInfoList", "Vehicle", null);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }



        [HttpGet]
        public ActionResult Repair()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                var PFLIST = from q in db.EmployeeInfoes


                             select new
                             {
                                 value = q.PFBN,
                                 description = q.PFBN
                             };
                ViewBag.PFLIST = new SelectList(PFLIST.ToList(), "value", "description");

                var DriverPFList = from q in db.EmployeeInfoes

                                   where q.Designation == "Driver"
                                   select new
                                   {
                                       value = q.PFBN,
                                       description = q.PFBN
                                   };
                ViewBag.DriverPFList = new SelectList(DriverPFList.ToList(), "value", "description");

                var OrganizationList = from q in db.Varities_Table

                                       where q.Flag == 8
                                       select new
                                       {
                                           value = q.Value,
                                           description = q.Value
                                       };
                ViewBag.OrganizationList = new SelectList(OrganizationList.ToList(), "value", "description");
                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");



                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Repair(RepairViewModel obj)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Repair dd = new Repair();


                dd.RegistrationNo = obj.RegistrationNo;
                dd.RepairDate = obj.RepairDate;
                dd.BillNo = obj.BillNo;
                dd.OrganizationName = obj.OrganizationName;
                dd.UsedBy = obj.UsedBy;
                dd.UsedByPF = obj.UsedByPF;
                dd.Driver = obj.Driver;
                dd.DriverPF = obj.DriverPF;
                dd.Designation = obj.Degination;
                dd.BrCode = login.BranchCode;
                dd.CreatedBy = login.UserId;
                dd.CreatedOn = System.DateTime.Now;
                dd.Remarks = obj.Remarks;
                dd.Comments = obj.Comments;
                dd.Flag = obj.Flag;


                var detailscount = obj.RepairParts.Count();
                for (var i = 1; i <= detailscount; i++)
                {
                    RepairDetail rd = new RepairDetail();
                    rd.Repairid = dd.Repairid;
                    rd.RepairParts = obj.RepairParts[i - 1];
                    rd.Quantity = obj.Quantity[i - 1];
                    rd.RepairCost = obj.RepairCost[i - 1];
                    rd.CreatedOn = DateTime.Now;
                    rd.CreatedBy = login.UserId;
                    db.RepairDetails.Add(rd);
                }
                
                db.Repairs.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Repair Information Saved Successfully.";
                return RedirectToAction("Repair", "Vehicle");
            }

            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult RepairsList()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                var RepairList = db.Repairs.Where(o => o.ApprovedStatus != "Y").ToList().OrderBy(o => o.RegistrationNo);

                return View(RepairList);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public ActionResult RepairEdit(int? id)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                try
                {
                    var PFLIST = from q in db.EmployeeInfoes


                                 select new
                                 {
                                     value = q.PFBN,
                                     description = q.PFBN
                                 };
                    ViewBag.PFLIST = new SelectList(PFLIST.ToList(), "value", "description");

                    var DriverPFList = from q in db.EmployeeInfoes

                                       where q.Designation == "Driver"
                                       select new
                                       {
                                           value = q.PFBN,
                                           description = q.PFBN
                                       };
                    ViewBag.DriverPFList = new SelectList(DriverPFList.ToList(), "value", "description");

                    var OrganizationList = from q in db.Varities_Table

                                           where q.Flag == 8
                                           select new
                                           {
                                               value = q.Value,
                                               description = q.Value
                                           };
                    ViewBag.OrganizationList = new SelectList(OrganizationList.ToList(), "value", "description");
                    RepairView mm = new RepairView();
                    mm = getMeetingMasterData(id);

                    RepairDetailsViewModel md = new RepairDetailsViewModel();
                    RepairViewModel mv = new RepairViewModel();
                    mv.rv = mm;
                    mv.rdvm = getUnadjustedLoanInfo(Convert.ToInt32(id));

                    return View(mv);
                }
                catch (Exception ex)
                {
                    TempData["retMsg"] = ex.Message;
                    return RedirectToAction("RepairsList", "Vehicle");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public RepairView getMeetingMasterData(int? id)
        {

            var result = from i in db.Repairs
                         join j in db.RepairDetails on i.Repairid equals j.Repairid

                         where i.Repairid == id
                         select new RepairView
                         {

                             Repairid = i.Repairid,
                             RegistrationNo = i.RegistrationNo,
                             RepairDate = i.RepairDate,
                             BillNo = i.BillNo,
                             OrganizationName = i.OrganizationName,
                             UsedBy = i.UsedBy,
                             UsedByPF = i.UsedByPF,
                             Driver = i.Driver,
                             DriverPF = i.DriverPF,
                             Degination = i.Designation,
                         };
            RepairView m = result.FirstOrDefault();
            return m;
        }

        public List<RepairDetailsViewModel> getUnadjustedLoanInfo(int repairID)
        {
            var result = from i in db.RepairDetails
                         where i.Repairid == repairID
                         select new RepairDetailsViewModel
                         {
                             id = i.id,
                             Repairid = i.Repairid,
                             RepairCost = i.RepairCost,
                             Quantity = i.Quantity,
                             RepairParts = i.RepairParts

                         };
            List<RepairDetailsViewModel> loanList = result.ToList();
            return loanList;
        }

        [HttpPost]

        public ActionResult RepairEdit(RepairViewModel m, RepairDetailsViewModel mdb)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                RepairView mm = new RepairView();
                Repair mmdb = db.Repairs.Find(m.rv.Repairid);
                var transaction = db.Database.BeginTransaction();
                mmdb.RegistrationNo = m.rv.RegistrationNo;
                mmdb.RepairDate = m.rv.RepairDate;
                mmdb.UsedBy = m.rv.UsedBy;
                mmdb.UsedByPF = m.rv.UsedByPF;
                mmdb.Driver = m.rv.Driver;
                mmdb.DriverPF = m.rv.DriverPF;
                mmdb.Designation = m.rv.Degination;
                mmdb.BillNo = m.rv.BillNo;
                mmdb.OrganizationName = m.rv.OrganizationName;
              
              
                db.SaveChanges();

                int j = 0;
                for (int i = 0; i < m.rdvm.Count(); i++)
                {

                    try
                    {
                        RepairDetailsViewModel memo = new RepairDetailsViewModel();
                        RepairDetail md = new RepairDetail();
                        int id = m.rdvm[i].id;
                        md = db.RepairDetails.Where(o => o.id.Equals(id)).FirstOrDefault();

                        md.RepairCost = m.rdvm[i].RepairCost;
                        md.RepairParts = m.rdvm[i].RepairParts;
                        md.Quantity = m.rdvm[i].Quantity;

                        try
                        {
                            //db.Entry(md).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            TempData["errMsg"] = "Error:" + e.Message;
                            return RedirectToAction("RepairEdit", "Vehicle");
                        }
                        j++;

                    }

                    catch (Exception e)
                    {
                        TempData["errMsg"] = "Error:" + e.Message;

                        return RedirectToAction("RepairEdit", "Vehicle");
                    }

                }

                transaction.Commit();
                TempData["retMsg"] = "Repair Information updated Successfully.";
                return RedirectToAction("RepairsList", "Vehicle", new { id = m.rv.Repairid });
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }







        [HttpGet]
        public ActionResult AddMoreInfo(int? id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                RepairView mm = new RepairView();
                mm = getMeetingMasterData(id);

                RepairViewModel mv = new RepairViewModel();
                mv.rv = mm;
                mv.rdvm = mv.rdvm;
                return View(mv);

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult AddMoreInfo(RepairViewModel m)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                //var memserial = db.RepairDetails.Where(o => o.Repairid == m.rv.Repairid).Select(o =>o.id);



                List<RepairDetailsViewModel> memoes = new List<RepairDetailsViewModel>();
                for (int i = 0; i < m.RepairParts.Count(); i++)
                {

                    RepairDetailsViewModel memo = new RepairDetailsViewModel();
                    memo.Repairid = m.rv.Repairid;
                    memo.RepairParts = m.RepairParts.ElementAt(i);
                    memo.RepairCost = m.RepairCost.ElementAt(i);
                    memo.Quantity = m.Quantity.ElementAt(i);

                    memoes.Add(memo);
                }
                if (SaveFileMore(m.rv, memoes))
                {

                    TempData["retMsg"] = "Repair Information Saved Successfully.";
                    return RedirectToAction("RepairsList", "Vehicle", new { id = m.rv.Repairid });

                }


                TempData["retMsg"] = "Repair Information Saved Successfully.";
                return RedirectToAction("Repair", "Vehicle");
            }

            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        private bool SaveFileMore(RepairView m, List<RepairDetailsViewModel> mdm)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            var transaction = db.Database.BeginTransaction();
            try
            {

                foreach (var item in mdm)
                {
                    RepairDetail md = new RepairDetail();
                    md.Repairid = m.Repairid;

                    md.RepairParts = item.RepairParts;
                    md.RepairCost = item.RepairCost;
                    md.Quantity = item.Quantity;
                    md.CreatedOn = DateTime.Now;
                    md.CreatedBy = login.UserId;
                    db.RepairDetails.Add(md);

                }
                db.SaveChanges();

                transaction.Commit();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {

                        TempData["AlertMessage"] = validationError.PropertyName + "--" + validationError.ErrorMessage;
                    }
                }
                transaction.Rollback();
                return false;
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                TempData["AlertMessage"] = ex.Message;
                return false;
            }
        }




        public ActionResult RepairInfoDelete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var repair = db.Repairs.Find(id);

            if (repair == null)
            {
                return HttpNotFound();
            }

            return View(repair);
        }

        [HttpPost, ActionName("RepairInfoDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var repairDetails = db.RepairDetails.Where(o => o.Repairid == id);
            db.RepairDetails.RemoveRange(repairDetails);
            var repair = db.Repairs.Find(id);
            db.Repairs.Remove(repair);
            db.SaveChanges();
            TempData["retMsg"] = "Repair Information Delete Successfully.";
            return RedirectToAction("RepairsList", "Vehicle");
        }


        public ActionResult ViewDetails(int? id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                RepairView mm = new RepairView();
                mm = getMeetingMasterData(id);

                RepairDetailsViewModel md = new RepairDetailsViewModel();
                RepairViewModel mv = new RepairViewModel();
                mv.rv = mm;
                mv.rdvm = getUnadjustedLoanInfo(Convert.ToInt32(id));
                return View(mv);

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

 

        [HttpGet]
        public ActionResult RepairDetails(int id)
        {
            var result = from i in db.Repairs 
                         where i.Repairid == id
                         select new RepairViewModel
                         {

                             RegistrationNo = i.RegistrationNo,
                             RepairDate = i.RepairDate,
                             OrganizationName = i.OrganizationName,
                             UsedBy = i.UsedBy,
                             Driver = i.Driver,
                             Degination = i.Designation,
                             BillNo = i.BillNo,
                             BrCode = i.BrCode,

  
                         };
            RepairViewModel List = result.FirstOrDefault();
            return View(List);

        }




        [HttpGet]
        public ActionResult approvalRepair(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var inq = db.Repairs.Where(o => o.Repairid == id).SingleOrDefault();
            return View(inq);
        }
        [HttpPost]
        public ActionResult approvalRepair(RepairViewModel i, string btn)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (login != null)
            {

                try
                {
                    if (btn == "Approve" && i.id > 0)
                    {
                        Repair dd = new Repair();
                        dd = db.Repairs.Find(i.id);
                        dd.ApprovedStatus = "Y";
                        dd.ApprovedBy = login.UserId;
                        dd.ApprovedOn = System.DateTime.Now;
                        db.SaveChanges();
                        TempData["retMsg"] = "Repair Information approved Successfully.";

                        return RedirectToAction("RepairsList", "Vehicle");

                    }

                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            TempData["errMsg"] = TempData["errMsg"] + " " + "Property: " + validationError.PropertyName
                                + " Error: " + validationError.ErrorMessage;
                            //logger.Error("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }

                    return RedirectToAction("RepairsList", "Vehicle", null);
                }
                catch (Exception ex)
                {


                    TempData["errMsg"] = ex.Message;
                    return RedirectToAction("RepairsList", "Vehicle", null);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }






        [HttpGet]
        public ActionResult MaintenanceInfo()

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                var PFLIST = from q in db.EmployeeInfoes


                             select new
                             {
                                 value = q.PFBN,
                                 description = q.PFBN
                             };
                ViewBag.PFLIST = new SelectList(PFLIST.ToList(), "value", "description");

                var DriverPFList = from q in db.EmployeeInfoes

                                   where q.Designation == "Driver"
                                   select new
                                   {
                                       value = q.PFBN,
                                       description = q.PFBN
                                   };
                ViewBag.DriverPFList = new SelectList(DriverPFList.ToList(), "value", "description");

                var MaintenanceList = from q in db.Varities_Table

                                      where q.Flag == 4
                                      select new
                                      {
                                          value = q.Value,
                                          description = q.Value
                                      };
                ViewBag.MaintenanceList = new SelectList(MaintenanceList.ToList(), "value", "description");

                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");
                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]

        public ActionResult MaintenanceInfo(MaintenanceViewModel rd)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Maintenance dd = new Maintenance();

                dd.RegistrationNo = rd.RegistrationNo;
                dd.UsedBy = rd.UsedBy;
                dd.UsedByPF = rd.UsedByPF;
                dd.Designation = rd.Designation;
                dd.DriverPF = rd.DriverPF;
                dd.Driver = rd.Driver;

                dd.MaintenanceType = "Maintenace";
                dd.MaintenanceParts = rd.MaintenanceParts;  
                dd.MaintenanceDate = rd.MaintenanceDate;
                dd.MaintenanceCost = rd.MaintenanceCost;
                dd.UnitQ= rd.UnitQ;
                
                dd.BillNo = rd.BillNo;
                dd.Remarks = rd.Remarks;
                dd.Comments = rd.Comments;
                dd.BrCode = login.BranchCode;
                dd.Flag = rd.Flag;
                dd.CreatedOn = System.DateTime.Now;
                dd.CreatedBy = login.UserId;

                db.Maintenances.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Maintenance Information Saved Successfully.";
                return RedirectToAction("MaintenanceInfoList", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult MaintenanceInfoList()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
               var maintenanceList = db.Maintenances.Where(o => o.ApprovedStatus != "Y").ToList().OrderBy(o => o.RegistrationNo); 

                return View(maintenanceList);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public ActionResult MaintenanceInfoEdit(int id)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                var PFLIST = from q in db.EmployeeInfoes
                             select new
                             {
                                 value = q.PFBN,
                                 description = q.PFBN
                             };
                ViewBag.PFLIST = new SelectList(PFLIST.ToList(), "value", "description");

                var DriverPFList = from q in db.EmployeeInfoes

                                   where q.Designation == "Driver"
                                   select new
                                   {
                                       value = q.PFBN,
                                       description = q.PFBN
                                   };
                ViewBag.DriverPFList = new SelectList(DriverPFList.ToList(), "value", "description");

                var MaintenanceList = from q in db.Varities_Table

                                      where q.Flag == 4
                                      select new
                                      {
                                          value = q.Value,
                                          description = q.Value
                                      };
                ViewBag.MaintenanceList = new SelectList(MaintenanceList.ToList(), "value", "description");
                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");

                MaintenanceViewModel fd = new MaintenanceViewModel();
                var Maintenfo = db.Maintenances.Find(id);
                fd.RegistrationNo = Maintenfo.RegistrationNo;
                fd.UsedBy = Maintenfo.UsedBy;
                fd.UsedByPF = Maintenfo.UsedByPF;
                fd.Designation = Maintenfo.Designation;
                fd.Driver = Maintenfo.Driver;
                fd.DriverPF = Maintenfo.DriverPF;
                fd.MaintenanceDate = Maintenfo.MaintenanceDate;
                fd.BillNo = Maintenfo.BillNo;

                fd.MaintenanceCost = Maintenfo.MaintenanceCost;
                fd.MaintenanceParts = Maintenfo.MaintenanceParts;
                fd.MaintenanceType = "Maintenace";

                fd.UnitQ = Maintenfo.UnitQ;
          
                return View(fd);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]

        public ActionResult MaintenanceInfoEdit(MaintenanceViewModel fd)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Maintenance dd = new Maintenance();
                dd = db.Maintenances.Find(fd.id);

                dd.RegistrationNo = fd.RegistrationNo;
                dd.UsedBy = fd.UsedBy;
                dd.UsedByPF = fd.UsedByPF;
                dd.Designation = fd.Designation;

                dd.Driver = fd.Driver;
                dd.DriverPF = fd.DriverPF;

             
                dd.MaintenanceDate = fd.MaintenanceDate;

                dd.MaintenanceCost = fd.MaintenanceCost;
                dd.MaintenanceParts = fd.MaintenanceParts;
                dd.MaintenanceType = "Maintenace";

                dd.UnitQ = fd.UnitQ;
                dd.BillNo = fd.BillNo;

                db.SaveChanges();
                TempData["retMsg"] = "Maintenance Information Updated Successfully.";
                return RedirectToAction("MaintenanceInfoList", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult MaintenanceDelete(int? id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Maintenance vdd = db.Maintenances.Find(id);
                if (vdd == null)
                    return HttpNotFound();
                return View(vdd);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpPost]
        public ActionResult MaintenanceDelete(Maintenance vobj)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Maintenance vdd = db.Maintenances.Find(vobj.id);
                if (vdd == null)
                    return HttpNotFound();
                db.Maintenances.Remove(vdd);
                db.SaveChanges();
                TempData["retMsg"] = "Maintenance Information Delete Successfully.";
                return RedirectToAction("MaintenanceInfoList", "Vehicle");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpGet]
        public ActionResult MaintenanceDetails(int id)
        {
            var result = from i in db.Maintenances
                         where i.id == id
                         select new MaintenanceViewModel
                         {

                             RegistrationNo = i.RegistrationNo,
                             MaintenanceDate = i.MaintenanceDate,
                             MaintenanceCost = i.MaintenanceCost,
                             MaintenanceParts = i.MaintenanceParts,
                             UnitQ = i.UnitQ,
                             BillNo = i.BillNo,
                             UsedBy = i.UsedBy,
                             Driver = i.Driver,
                             BrCode = i.BrCode,


                         };
            MaintenanceViewModel List = result.FirstOrDefault();
            return View(List);

        }




        [HttpGet]
        public ActionResult approvalMaintenance(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var inq = db.Maintenances.Where(o => o.id == id).SingleOrDefault();
            return View(inq);
        }
        [HttpPost]
        public ActionResult approvalMaintenance(MaintenanceViewModel i, string btn)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (login != null)
            {

                try
                {
                    if (btn == "Approve" && i.id > 0)
                    {
                        Maintenance dd = new Maintenance();
                        dd = db.Maintenances.Find(i.id);
                        
                        dd.ApprovedStatus = "Y";
                        dd.ApprovedBy = login.UserId;
                        dd.ApprovedOn = System.DateTime.Now;
                        //i.receievedby = sl.getuserNamebyregion(Session["region"].ToString());

                        db.SaveChanges();
                        TempData["retMsg"] = "Maintenance Information approved Successfully.";

                        return RedirectToAction("MaintenanceInfoList", "Vehicle");

                    }

                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            TempData["errMsg"] = TempData["errMsg"] + " " + "Property: " + validationError.PropertyName
                                + " Error: " + validationError.ErrorMessage;
                            //logger.Error("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }

                    return RedirectToAction("MaintenanceInfoList", "Vehicle", null);
                }
                catch (Exception ex)
                {


                    TempData["errMsg"] = ex.Message;
                    return RedirectToAction("MaintenanceInfoList", "Vehicle", null);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }




        public ActionResult Index()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                var MaintenanceDet = from x in db.Varities_Table

                                     where x.Flag == 4
                                     select new MaintenanceViewModel
                                     {
                                         MaintenanceType = x.Value

                                     };
               
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ColorIndex()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                var Color = from x in db.Varities_Table

                            where x.Flag == 2 
                            select new CarInfoViewModel
                            {
                                Color = x.Value

                            };
                return View();
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

                MaintenanceViewModel rd = new MaintenanceViewModel();

                return View(rd);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult Create(MaintenanceViewModel rd)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Varities_Table dd = new Varities_Table();

                dd.Value = rd.MaintenanceType;
              
                dd.Flag = 4;
                db.Varities_Table.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Maintenance Parts Saved Successfully.";
                return RedirectToAction("Index", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult CreateColor()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                CarInfoViewModel ci = new CarInfoViewModel();

                return View(ci);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult CreateColor(CarInfoViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Varities_Table dd = new Varities_Table();

                dd.Value = ci.Color;
           
                dd.Flag = 2;
                db.Varities_Table.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Colors Saved Successfully.";
                return RedirectToAction("ColorIndex", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult CreateUsedBy()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                MaintenanceViewModel ci = new MaintenanceViewModel();

                return View(ci);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult CreateUsedBy(MaintenanceViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Varities_Table dd = new Varities_Table();

                dd.Value = ci.UsedBy;

                dd.Flag = 6;
                db.Varities_Table.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Users Saved Successfully.";
                return RedirectToAction("CreateUsedBy", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult CreateDriver()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                MaintenanceViewModel ci = new MaintenanceViewModel();

                return View(ci);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult CreateDriver(MaintenanceViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Varities_Table dd = new Varities_Table();

                dd.Value = ci.Driver;

                dd.Flag = 7;
                db.Varities_Table.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Drivers Saved Successfully.";
                return RedirectToAction("CreateDriver", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public ActionResult CreateOrganization()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                RepairViewModel ci = new RepairViewModel();

                return View(ci);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult CreateOrganization(RepairViewModel ci)

        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                Varities_Table dd = new Varities_Table();

                dd.Value = ci.OrganizationName;

                dd.Flag = 8;
                db.Varities_Table.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Organization Name Saved Successfully.";
                return RedirectToAction("CreateOrganization", "Vehicle");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult EmployeeEntry()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                 EmployeeInfoViewModel EM = new EmployeeInfoViewModel();
                
                var DesignationList = from q in db.InputLists
                                      where q.Field == "Designation"
                                      select new
                                      {
                                          value = q.Value,
                                          description = q.Value
                                      };
                ViewBag.DesignationList = new SelectList(DesignationList.ToList(), "value", "description");
                var DesignationListBN = from q in db.InputLists
                                        where q.Field == "DesignationBN"
                                        select new
                                        {
                                            value = q.Value,
                                            description = q.Value
                                        };
                ViewBag.DesignationListBN = new SelectList(DesignationListBN.ToList(), "value", "description");
                var BranchList = from q in db.Branches
                                 select new
                                 {
                                     value = q.branch_code,
                                     description = q.branch_name + " " + q.branch_code
                                 };
                ViewBag.BranchList = new SelectList(BranchList.ToList(), "value", "description");
                var StatusList = from q in db.InputLists
                                 where q.Field == "Status"
                                 select new
                                 {
                                     value = q.Value,
                                     description = q.Value
                                 };
                ViewBag.StatusList = new SelectList(StatusList.ToList(), "value", "description");

                return View(EM);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            //return View();
        }
        [HttpPost]
        public ActionResult EmployeeEntry(EmployeeInfoViewModel En)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                EmployeeInfo dd = new EmployeeInfo();
                var checkPF = db.EmployeeInfoes.Where(x => x.PFEN == En.PFEN).Select(x => x.PFEN).SingleOrDefault();
                if (checkPF != null)
                {
                    TempData["retMsg"] = "This PF number is already in OUR System.";

                    return RedirectToAction("EmployeeEntry", "Vehicle");
                }
                dd.PFEN = En.PFEN;
                dd.PFBN = En.PFBN;
                dd.EmpNameEN = En.EmpNameEN;
                dd.EmpNameBN = En.EmpNameBN;
                dd.DOB = En.DOB;
                dd.CWorkPlace = En.CWorkPlace;
                dd.Designation = En.Designation;
                dd.DesignationBN = En.DesignationBN;
                dd.JoiningDate = En.JoiningDate;
                //dd.PRLDate = En.DOB.Value.AddYears(59).AddDays(-1);
                dd.Gender = En.Gender;
                dd.BankAcc = En.BankAcc;
                dd.NID = En.NID;
                dd.MobileNum = En.MobileNum;
                dd.Status = En.Status;
                dd.Remarks = En.Remarks;
                dd.Flag = 1;
                db.EmployeeInfoes.Add(dd);
                db.SaveChanges();
                TempData["retMsg"] = "Employee Information Saved Successfully.";

                return RedirectToAction("Welcome", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpGet]
        public ActionResult EmployeeEntryEdit(int id)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                EmployeeInfoViewModel obj = new EmployeeInfoViewModel();
                var DesignationList = from q in db.InputLists
                                      where q.Field == "Designation"
                                      select new
                                      {
                                          value = q.Value,
                                          description = q.Value
                                      };
                var DesignationListBN = from q in db.InputLists
                                        where q.Field == "DesignationBN"
                                        select new
                                        {
                                            value = q.Value,
                                            description = q.Value
                                        };
                ViewBag.DesignationListBN = new SelectList(DesignationListBN.ToList(), "value", "description");
                var BranchList = from q in db.Branches
                                 select new
                                 {
                                     value = q.branch_code,
                                     description = q.branch_name + " " + q.branch_code
                                 };
                ViewBag.BranchList = new SelectList(BranchList.ToList(), "value", "description");
                var StatusList = from q in db.InputLists
                                 where q.Field == "Status"
                                 select new
                                 {
                                     value = q.Value,
                                     description = q.Value
                                 };
                ViewBag.StatusList = new SelectList(StatusList.ToList(), "value", "description");
                var EmployeeEntryEdit = db.EmployeeInfoes.Find(id);
                var checkEmpInfo = db.EmployeeInfoes.Where(x => x.PFEN == EmployeeEntryEdit.PFEN).SingleOrDefault();
                //var checkRetired = checkEmpInfo.PRLDate.Value.AddYears(1);

                obj.DOB = checkEmpInfo.DOB;
                obj.JoiningDate = checkEmpInfo.JoiningDate;
                obj.PRLDate = checkEmpInfo.PRLDate;

                obj.PFEN = EmployeeEntryEdit.PFEN;
                obj.PFBN = checkEmpInfo.PFBN;
                obj.EmpNameBN = EmployeeEntryEdit.EmpNameBN;
                obj.EmpNameEN = EmployeeEntryEdit.EmpNameEN;
                obj.CWorkPlace = EmployeeEntryEdit.CWorkPlace;
                obj.Designation = EmployeeEntryEdit.Designation;
                obj.DesignationBN = EmployeeEntryEdit.DesignationBN;
                obj.Gender = EmployeeEntryEdit.Gender;
                obj.BankAcc = EmployeeEntryEdit.BankAcc;
                obj.NID = EmployeeEntryEdit.NID;
                obj.MobileNum = EmployeeEntryEdit.MobileNum;
                obj.Status = EmployeeEntryEdit.Status;
                obj.Remarks = EmployeeEntryEdit.Remarks;

                ViewBag.DesignationList = new SelectList(DesignationList.ToList(), "value", "description");
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult EmployeeEntryEdit(EmployeeInfoViewModel ed)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                EmployeeInfo d = new EmployeeInfo();
                d = db.EmployeeInfoes.Find(ed.id);
                d.PFEN = ed.PFEN;
                d.PFBN = ed.PFBN;
                d.EmpNameBN = ed.EmpNameBN;
                d.EmpNameEN = ed.EmpNameEN;
                d.DOB = ed.DOB;
                d.JoiningDate = ed.JoiningDate;
                d.PRLDate = ed.PRLDate;
                d.CWorkPlace = ed.CWorkPlace;
                d.Designation = ed.Designation;
                d.DesignationBN = ed.DesignationBN;
                d.Gender = ed.Gender;
                d.BankAcc = ed.BankAcc;
                d.NID = ed.NID;
                d.MobileNum = ed.MobileNum;
                d.Status = ed.Status;
                d.Remarks = ed.Remarks;
                d.Flag = 1;
                db.SaveChanges();
                TempData["retMsg"] = "Employee Information Updated Successfully.";
                return RedirectToAction("Welcome", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult EmployeeEntryInfo()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                return View(db.EmployeeInfoes.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult CalculateDepreciation()
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                var RegistrationList = from q in db.Car_Info

                                       select new
                                       {
                                           value = q.RegistrationNo,
                                           description = q.RegistrationNo
                                       };
                ViewBag.RegistrationList = new SelectList(RegistrationList.ToList(), "value", "description");
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult CalculateDepreciation(DepreciationViewModel dvm)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                List<DepreciationViewModel> tlist = new List<DepreciationViewModel>();
                DepreciationViewModel dep = new DepreciationViewModel();
                var getValue = db.Car_Details.Where(x => x.RegistrationNo == dvm.RegistrationNo).FirstOrDefault();
                double getAmount = Convert.ToDouble(getValue.BookValue);
                var getDays = Convert.ToDateTime(dvm.DateTo) - getValue.BookValueDate;
                int ParseDay = getDays.Value.Days;
                dep.cBookValue = 0; dep.cDepreciation = getAmount;
                for (int j = 1; j <= ParseDay; j++)
                {
                    dep.cBookValue = Math.Round((dep.cDepreciation - ((dep.cDepreciation * 0.2) / 365)),2);
                    dep.cDepreciation = dep.cBookValue;
                    dep.cDay = j;
                    tlist.Add(dep);
                }
                return View(tlist);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        


    }
}