using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using System.IO;
using BKBVehicle.Models.ViewModel;
using BKBVehicle.DataModel;
using System.Data.Entity.Validation;
using System.Net;

namespace BKBVehicle.Controllers
{
    public class HomeController : Controller
    {

        public string pageSized = ConfigurationManager.AppSettings["PageSize"];
        public int gressDays = Convert.ToInt32(ConfigurationManager.AppSettings["gressDays"]);

        BKBVehicleEntities db = new BKBVehicleEntities();
        public ActionResult Index()
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

        public ActionResult Welcome()
        {
            LoginModels logIn = new LoginModels();
            var login = (BKBVehicle.Models.ViewModel.LoginModels)Session["LoginCredentials"];
            if (login != null)
            {
                try
                {

                    return View();
                }
                catch (System.Data.Entity.Core.EntityException)
                {
                    return View();
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    return View();
                }
                catch (Exception)
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
