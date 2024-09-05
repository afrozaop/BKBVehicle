<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryFuelMaintenance.aspx.cs" Inherits="BKBVehicle.Report.SummaryFuelMaintenance" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
        <rsweb:reportviewer id="SummaryFuelMaintenancerpt" runat="server" style="" width="100%" height="100%" zoommode="PageWidth" showprintbutton="true"
            sizetoreportcontent="True" font-names="Verdana" font-size="8pt" waitmessagefont-names="Verdana" waitmessagefont-size="14pt">
        </rsweb:reportviewer>
    </div>
    </form>
</body>
</html>
