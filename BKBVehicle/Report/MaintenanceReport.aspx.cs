using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKBVehicle.Report
{
    public partial class MaintenanceReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Maintenancerpt.ProcessingMode = ProcessingMode.Local;
                DataTable result;
                string RegistrationNo = Request.QueryString["RegistrationNo"].ToString();
                string dateFrom = Request.QueryString["dateFrom"].ToString();
                string dateTo = Request.QueryString["dateTo"].ToString();
                Maintenancerpt.LocalReport.ReportPath = Server.MapPath("~/Report/RDLC/MaintenanceReport.rdlc");
                result = GetData("EXEC dbo.SP_Maintenance @RegistrationNo='" + RegistrationNo + "',@dateFrom='" + dateFrom + "',@dateTo='" + dateTo + "'");


                if (result.Rows.Count > 0)
                {
                    // this.MissingRpt.LocalReport.SetParameters(new ReportParameter[] { p1});
                    ReportDataSource dtReport = new ReportDataSource("DS_Maintenance", result);
                    Maintenancerpt.LocalReport.DataSources.Clear();
                    Maintenancerpt.LocalReport.DataSources.Add(dtReport);
                    // Report data source code...
                    Maintenancerpt.LocalReport.Refresh();
                }

            }
        }
        private DataTable GetData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["BKBVehicleConnectionString"].ConnectionString;
            //string conString = ReadConfig.GetAppSettingUsingConfigurationManager("ElectronicsDB");
            SqlCommand cmd = new SqlCommand(query);

            using (SqlConnection con = new SqlConnection(conString))
            {
                DataTable dt = new DataTable();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    return dt;
                }
            }

        }
    }
}
