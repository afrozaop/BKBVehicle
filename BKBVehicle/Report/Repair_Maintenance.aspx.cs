using BoldReports.ReportDesignerEnums;
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
    public partial class Repair_Maintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                Re_Main_rpt.ProcessingMode = ProcessingMode.Local;
                DataTable result;
                string RegistrationNo = Request.QueryString["RegistrationNo"].ToString();
                string dateFrom = Request.QueryString["dateFrom"].ToString();
                string dateTo = Request.QueryString["dateTo"].ToString();
               
                //ReportParameter p1 = new ReportParameter("ReportType", ReportType);

                //if (ReportType == "repair")
                //{
                //    Re_Main_rpt.LocalReport.ReportPath = Server.MapPath("~/Report/RDLC/Repair_Maintenance.rdlc");
                //   // result = GetData("EXEC dbo.SP_RepairDetailsList @RegistrationNo='" + RegistrationNo + "',@dateFrom='" + dateFrom + "',@dateTo='" + dateTo + "'", @ReportType = '" + ReportType + "'"); 
                //    if (result.Rows.Count > 0)
                //    {
                //        this.Re_Main_rpt.LocalReport.SetParameters(new ReportParameter[] { p1 });
                //        ReportDataSource dtReport = new ReportDataSource("DS_Repair_Maintenance", result);
                //        Re_Main_rpt.LocalReport.DataSources.Clear();
                //        Re_Main_rpt.LocalReport.DataSources.Add(dtReport);
                //        Re_Main_rpt.LocalReport.Refresh();
                //    }

                //}
                //else 
                //{
                //    Re_Main_rpt.LocalReport.ReportPath = Server.MapPath("~/Report/RDLC/Repair_Maintenance.rdlc");
                //    //result = GetData("EXEC dbo.SP_RepairDetailsList @RegistrationNo='" + RegistrationNo + "',@dateFrom='" + dateFrom + "',@dateTo='" + dateTo + "'", @ReportType = '" + ReportType + "'");
                //    if (result.Rows.Count > 0)
                //    {
                //        this.Re_Main_rpt.LocalReport.SetParameters(new ReportParameter[] { p1 });
                //        ReportDataSource dtReport = new ReportDataSource("DS_Repair_Maintenance", result);
                //        Re_Main_rpt.LocalReport.DataSources.Clear();
                //        Re_Main_rpt.LocalReport.DataSources.Add(dtReport);
                //        Re_Main_rpt.LocalReport.Refresh();
                //    }
                //}
            
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
