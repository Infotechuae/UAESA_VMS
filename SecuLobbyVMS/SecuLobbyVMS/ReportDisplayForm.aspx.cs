using DAL;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class ReportDisplayForm : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sReportName = Request.QueryString["Rpt_Name"].ToString();

        string sReportPath = sReportName;
        if (System.IO.File.Exists(sReportPath)) { System.IO.File.Delete(sReportPath); }
        sReportPath = Server.MapPath("images\\Reports\\" + sReportPath);

        C1WebReport1.ReportSource.FileName = sReportPath;
        string fileName = C1WebReport1.ReportSource.FileName;
        string[] names = C1WebReport1.Report.GetReportInfo(fileName);
        C1WebReport1.ReportSource.ReportName = names[0];
        C1.C1Report.C1Report rpt = C1WebReport1.Report;
        string sConn = string.Empty;
        if (names.Length > 1)
        {
          String sServerName = @"192.168.91.64,5492";
          String sDatabaseName = "Seculobby2023";
          String sUser = "taadmin";
          String sPass = "Fm5erv1ce";



          sConn = "Provider=SQLOLEDB.1;Password ='"+ sPass + "';Persist Security Info=True;" + "User ID='"+ sUser + "';Initial Catalog='"+ sDatabaseName + "';Data Source='"+ sServerName + "'";
          rpt.DataSource.ConnectionString = sConn;
        }

        InitSQLReportDataSource(rpt, names, sConn);

        rpt.Layout.Orientation = C1.C1Report.OrientationEnum.Auto;
        C1WebReport1.Report = rpt;
        //C1WebReport1.Report.ReportDefinition = "AssetLibrary";
        C1WebReport1.ImageRenderMethod = C1.Web.C1WebReport.ImageRenderMethodEnum.HttpHandler;

        C1WebReport1.ShowPDF(true);


        //C1WebReport1.ExportRenderMethod = C1.Web.C1WebReport.ExportRenderMethodEnum.HttpHandler;
        //C1WebReport1.ImageRenderMethod = C1.Web.C1WebReport.ImageRenderMethodEnum.HttpHandler;
        //C1WebReport1.Export(C1.Web.C1WebReport.ExportFormatsEnum.RTF);
      }

    }

    private void InitSQLReportDataSource(C1.C1Report.C1Report rpt, string[] names, string sConn)
    {
      string sReportName = Request.QueryString["Rpt_Name"].ToString();

      string sSTR = rpt.DataSource.RecordSource;

      if (sReportName == "VisitorMasterReport.xml")
      {
       
        string sText = Request.QueryString["SearchText"].ToString();

  
        sSTR = sSTR.Replace("?text", sText.ToString());
      }
      else if (sReportName == "VisitorTransacReport.xml")
      {
        string sFromDate = Request.QueryString["FromDate"].ToString();
        string sToDate = Request.QueryString["ToDate"].ToString();

        string sText = Request.QueryString["Search"].ToString();
        string sUG = Request.QueryString["UserGroup"].ToString();
        string sHN = Request.QueryString["HostName"].ToString();

        sSTR = sSTR.Replace("?FromDate", sFromDate.ToString());
        sSTR = sSTR.Replace("?ToDate", sToDate.ToString());
        sSTR = sSTR.Replace("?text", sText.ToString());
        sSTR = sSTR.Replace("?UserGroup", sUG.ToString());
        sSTR = sSTR.Replace("?HostName", sHN.ToString());
      }

      DataTable oTable1 = ocon.GetTable(sSTR, new DataSet());
      rpt.DataSource.Recordset = oTable1;

      if (names.Length > 1)
      {
        foreach (C1.C1Report.Field fld in rpt.Fields)
        {
          oTable1 = null;
          sSTR = "";
          if (fld.Subreport != null)
          {
            fld.Subreport.DataSource.ConnectionString = sConn;
            sSTR = fld.Subreport.DataSource.RecordSource;
            if (sReportName == "VisitorMasterReport.xml")
            {

              string sText = Request.QueryString["SearchText"].ToString();


              sSTR = sSTR.Replace("?text", sText.ToString());
            }
            else if (sReportName == "VisitorTransacReport.xml")
            {
              string sFromDate = Request.QueryString["FromDate"].ToString();
              string sToDate = Request.QueryString["ToDate"].ToString();

              string sText = Request.QueryString["Search"].ToString();

              sSTR = sSTR.Replace("?FromDate", sFromDate.ToString());
              sSTR = sSTR.Replace("?ToDate", sToDate.ToString());
              sSTR = sSTR.Replace("?text", sText.ToString());
            }

            oTable1 = ocon.GetTable(sSTR, new DataSet());
            fld.Subreport.DataSource.Recordset = oTable1;
          }
        }
      }
    }
  }
}
