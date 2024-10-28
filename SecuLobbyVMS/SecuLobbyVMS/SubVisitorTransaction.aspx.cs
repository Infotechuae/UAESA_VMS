using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class SubVisitorTransaction : System.Web.UI.Page
  {
    ResourceManager rm;
    CultureInfo ci;
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);

        string sUserGroup = Convert.ToString(Session["UserGroup"]);

        string sUserName = "";
        string sSqlUSername = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
        DataTable dtUserName = ocon.GetTable(sSqlUSername, new DataSet());
        if (dtUserName.Rows.Count > 0)
        {
          sUserName = dtUserName.Rows[0]["UserName"].ToString();
        }

        //txtFromDate.Text = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month) + "-01").ToString("MM/dd/yyyy");
        txtFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

        VisitorsReport();


        string sFromdate = string.Format("\"{0}\"", txtFromDate.Text);
        string sTodate = string.Format("\"{0}\"", txtToDate.Text);
        string stext = string.Format("\"{0}\"", txtSearch.Text);
        string sUG = string.Format("\"{0}\"", sUserGroup);
        string sHostName = string.Format("\"{0}\"", sUserName);
        string sVar1 = sFromdate + "," + sTodate + "," + stext + "," + sUG + "," + sHostName;

        A1.Attributes.Add("onclick", "showReport(" + sVar1 + ")");

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      lblVisDet.Text = rm.GetString("STRN_15", ci);
      header1.Text = rm.GetString("STR_47", ci);
      lblFromDate.Text = rm.GetString("STR_45", ci);
      lblToDate.Text = rm.GetString("STR_46", ci);
      btnGenerate.Text = rm.GetString("STR_48", ci);
      btnPDF.Text = rm.GetString("STR_42", ci);
      BtnExcel.Text = rm.GetString("STR_43", ci);

    }
    protected string GetHeader(string str)
    {
      string sHeaderName = "";

      string sLang = Convert.ToString(Session["Lang"]);

      Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
      rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
      ci = Thread.CurrentThread.CurrentCulture;

      if (str == "Visitor Name")
        sHeaderName = rm.GetString("STR_39", ci);
      if (str == "Visitor ID")
        sHeaderName = rm.GetString("STR_40", ci);
      if (str == "Mobile")
        sHeaderName = rm.GetString("STR_6", ci);
      if (str == "Email ID")
        sHeaderName = rm.GetString("STR_7", ci);
      if (str == "Nationality")
        sHeaderName = rm.GetString("STR_8", ci);
      if (str == "Gender")
        sHeaderName = rm.GetString("STR_9", ci);
      if (str == "Company Name")
        sHeaderName = rm.GetString("STR_5", ci);
      if (str == "Department")
        sHeaderName = rm.GetString("STR_11", ci);
      if (str == "Floor")
        sHeaderName = rm.GetString("STR_12", ci);
      if (str == "Visitor Type")
        sHeaderName = rm.GetString("STR_14", ci);
      if (str == "Host")
        sHeaderName = rm.GetString("STR_13", ci);
      if (str == "Check In")
        sHeaderName = rm.GetString("STR_18", ci);
      if (str == "Check Out")
        sHeaderName = rm.GetString("STR_21", ci);
      if (str == "Duration")
        sHeaderName = rm.GetString("STR_15", ci);
      if (str == "Purpose")
        sHeaderName = rm.GetString("STR_61", ci);

      return sHeaderName;
    }



    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (txtFromDate.Text != "" && txtToDate.Text != "")
      {
        if (Convert.ToDateTime(txtToDate.Text) >= Convert.ToDateTime(txtFromDate.Text))
        {

          VisitorsReport();

          string sFromdate = string.Format("\"{0}\"", txtFromDate.Text);
          string sTodate = string.Format("\"{0}\"", txtToDate.Text);
          string stext = string.Format("\"{0}\"", txtSearch.Text);
          string sVar1 = sFromdate + "," + sTodate + "," + stext;

          A1.Attributes.Add("onclick", "showReport(" + sVar1 + ")");
        }
      }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
      string sUserGroup = Convert.ToString(Session["UserGroup"]);
      string sUserID = Convert.ToString(Session["UserID"]);
      string sUserName = "";
      string sSqlUSername = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = ocon.GetTable(sSqlUSername, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      if (txtFromDate.Text != "" && txtToDate.Text != "")
      {
        if (Convert.ToDateTime(txtToDate.Text) >= Convert.ToDateTime(txtFromDate.Text))
        {
          VisitorsReport();

          string sFromdate = string.Format("\"{0}\"", txtFromDate.Text);
          string sTodate = string.Format("\"{0}\"", txtToDate.Text);
          string stext = string.Format("\"{0}\"", txtSearch.Text);
          string sUG = string.Format("\"{0}\"", sUserGroup);
          string sHostName = string.Format("\"{0}\"", sUserName);
          string sVar1 = sFromdate + "," + sTodate + "," + stext + "," + sUG + "," + sHostName;

          A1.Attributes.Add("onclick", "showReport(" + sVar1 + ")");

        }
      }

    }

    public void VisitorsReport()
    {
      string sUserID = Convert.ToString(Session["UserID"]);
      string sUserGroup = Convert.ToString(Session["UserGroup"]);

      string sUserName = "";
      string sSqlUSername = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = ocon.GetTable(sSqlUSername, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }
      //sDateFmt = "dd-MMM-yyyy";
      string[,] infoArray = new string[5, 2];
      infoArray[0, 0] = "FromDate";
      infoArray[0, 1] = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
      infoArray[1, 0] = "ToDate";
      infoArray[1, 1] = Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd");
      infoArray[2, 0] = "search";
      infoArray[2, 1] = txtSearch.Text;
      infoArray[3, 0] = "UserGroup";
      infoArray[3, 1] = sUserGroup;
      infoArray[4, 0] = "HostName";
      infoArray[4, 1] = sUserName;


      DBSQL db = new DBSQL();//declaring class
      DataSet dsReport = db.GetDataset("Report_getVisitorDetail_New", infoArray, MyConnection.ReadConStr("Local"));
      db = null;

      if (dsReport.Tables[0].Rows.Count > 0)
      {
        grdVisDetails.DataSource = dsReport.Tables[0];
        grdVisDetails.DataBind();
      }
      else
      {
        grdVisDetails.DataSource = null;
        grdVisDetails.DataBind();
      }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
      //required to avoid the runtime error "  
      //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    private void ExportGridToExcel()
    {
      Response.Clear();
      Response.Buffer = true;
      Response.ClearContent();
      Response.ClearHeaders();
      Response.Charset = "";
      string FileName = "Visitor_reports_" + DateTime.Now + ".xls";
      StringWriter strwritter = new StringWriter();
      HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.ContentType = "application/vnd.ms-excel";
      Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
      grdVisDetails.GridLines = GridLines.Both;
      grdVisDetails.HeaderStyle.Font.Bold = true;
      grdVisDetails.RenderControl(htmltextwrtter);
      Response.Write(strwritter.ToString());
      Response.End();

    }

    protected void BtnExcel_Click(object sender, EventArgs e)
    {
      ExportGridToExcel();
    }
  }
}


