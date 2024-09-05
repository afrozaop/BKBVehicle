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
    public partial class FuelReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fuelrpt.ProcessingMode = ProcessingMode.Local;
                DataTable result;
                string RegistrationNo = Request.QueryString["RegistrationNo"].ToString();
                string ReportType = Request.QueryString["ReportType"].ToString();
                string dateFrom = Request.QueryString["dateFrom"].ToString();
                string dateTo = Request.QueryString["dateTo"].ToString();
                //ReportParameter p1 = new ReportParameter("ReportType", ReportType);
                if (ReportType == "gas")
                {
                    Fuelrpt.LocalReport.ReportPath = Server.MapPath("~/Report/RDLC/FuelReport.rdlc");
                    result = GetData("EXEC dbo.SP_FuelDetailsList @RegistrationNo='" + RegistrationNo + "',@ReportType='" + ReportType + "',@dateFrom='" + dateFrom + "',@dateTo='" + dateTo + "'");
                    if (result.Rows.Count > 0)
                    {
                        //this.Fuelrpt.LocalReport.SetParameters(new ReportParameter[] { p1 });
                        ReportDataSource dtReport = new ReportDataSource("DS_Fuel", result);
                        Fuelrpt.LocalReport.DataSources.Clear();
                        Fuelrpt.LocalReport.DataSources.Add(dtReport);
                        Fuelrpt.LocalReport.Refresh();
                    }
                }
                else
                {
                    Fuelrpt.LocalReport.ReportPath = Server.MapPath("~/Report/RDLC/FuelReport.rdlc");
                    result = GetData("EXEC dbo.SP_FuelDetailsList @RegistrationNo='" + RegistrationNo + "',@ReportType='" + ReportType + "',@dateFrom='" + dateFrom + "',@dateTo='" + dateTo + "'"); 
                    if (result.Rows.Count > 0)
                    {
                        //this.Fuelrpt.LocalReport.SetParameters(new ReportParameter[] { p1 });
                        ReportDataSource dtReport = new ReportDataSource("DS_Fuel", result);
                        Fuelrpt.LocalReport.DataSources.Clear();
                        Fuelrpt.LocalReport.DataSources.Add(dtReport);
                        Fuelrpt.LocalReport.Refresh();
                    }
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