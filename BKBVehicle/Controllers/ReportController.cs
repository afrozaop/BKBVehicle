using BKBVehicle.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BKBVehicle.Models.VehicleAppModel;
using BoldReports.ReportDesignerEnums;
namespace BKBVehicle.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        BKBVehicleEntities db = new BKBVehicleEntities();



        public ActionResult MaintenanceHistory(string RegistrationNo, string dateFrom, string dateTo)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                MaintenanceViewModel test = new MaintenanceViewModel();

                var RegLIST = from q in db.Car_Info
                              select new
                              {
                                  value = q.RegistrationNo,
                                  description = q.RegistrationNo
                              };
                ViewBag.RegLIST = new SelectList(RegLIST.ToList(), "value", "description");


                test.RegistrationNo = RegistrationNo == null ? "" : RegistrationNo;
                test.dateFrom = (dateFrom == null || dateFrom == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd");
                test.dateTo = (dateTo == null || dateTo == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");


                return View(test);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult FuelReportHistory(string RegistrationNo, string ReportType, string dateFrom, string dateTo)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Fuel_DetailsViewModel test = new Fuel_DetailsViewModel();

                var RegLIST = from q in db.Car_Info
                              select new
                              {
                                  value = q.RegistrationNo,
                                  description = q.RegistrationNo
                              };
                ViewBag.RegLIST = new SelectList(RegLIST.ToList(), "value", "description");


                test.RegistrationNo = RegistrationNo == null ? "" : RegistrationNo;
                test.dateFrom = (dateFrom == null || dateFrom == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd");
                test.dateTo = (dateTo == null || dateTo == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");
                test.ReportType = ReportType == null ? "" : ReportType;

                return View(test);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public ActionResult RepairReportHistory(string RegistrationNo, string dateFrom, string dateTo)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                RepairViewModel test = new RepairViewModel();

                var RegLIST = from q in db.Car_Info
                              select new
                              {
                                  value = q.RegistrationNo,
                                  description = q.RegistrationNo
                              };
                ViewBag.RegLIST = new SelectList(RegLIST.ToList(), "value", "description");


                test.RegistrationNo = RegistrationNo == null ? "" : RegistrationNo;
                test.dateFrom = (dateFrom == null || dateFrom == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd");
                test.dateTo = (dateTo == null || dateTo == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");


                return View(test);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult SummaryRepairReport(string dateFrom, string dateTo)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                RepairViewModel test = new RepairViewModel();
                test.dateFrom = (dateFrom == null || dateFrom == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd");
                test.dateTo = (dateTo == null || dateTo == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");


                return View(test);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult SummaryFuelReport(string dateFrom, string dateTo, string ReportType)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                Fuel_DetailsViewModel test = new Fuel_DetailsViewModel();
                test.ReportType = ReportType == null ? "" : ReportType;
                test.dateFrom = (dateFrom == null || dateFrom == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd");
                test.dateTo = (dateTo == null || dateTo == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");


                return View(test);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult SummaryFuelMaintenance(string dateFrom, string dateTo)
        {
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {

                FuelMaintenanceViewModel test = new FuelMaintenanceViewModel();
                test.dateFrom = (dateFrom == null || dateFrom == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd");
                test.dateTo = (dateTo == null || dateTo == "") ? Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");


                return View(test);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
    }

}