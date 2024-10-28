using AjaxControlToolkit;
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
  public partial class SubAllRequestReport : System.Web.UI.Page
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




        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_47", ci);
      lblFromDate.Text = rm.GetString("STR_45", ci);
      lblToDate.Text = rm.GetString("STR_46", ci);
      btnGenerate.Text = rm.GetString("STR_48", ci);

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

      return sHeaderName;
    }



    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (txtFromDate.Text != "" && txtToDate.Text != "")
      {
        if (Convert.ToDateTime(txtToDate.Text) >= Convert.ToDateTime(txtFromDate.Text))
        {

          VisitorsReport();


        }
      }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
      string sUserGroup = Convert.ToString(Session["UserGroup"]);
      string sUserID = Convert.ToString(Session["UserID"]);
      //string sUserName = "";
      //string sSqlUSername = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      //DataTable dtUserName = ocon.GetTable(sSqlUSername, new DataSet());
      //if (dtUserName.Rows.Count > 0)
      //{
      //  sUserName = dtUserName.Rows[0]["UserName"].ToString();
      //}

      if (txtFromDate.Text != "" && txtToDate.Text != "")
      {
        if (Convert.ToDateTime(txtToDate.Text) >= Convert.ToDateTime(txtFromDate.Text))
        {
          VisitorsReport();



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


      string sSql = "select SecuLobby_VisitingDetails_Self.Ref_No,SecuLobby_VisitorInfo_Self.Visitor_ID, Name,Company,Doc_type,EmiratesID,Mobile,Email,LocationID,Aptment_Dept,host.pl_Value as Host, host.pl_data as HostEmail,Visitor_Type,Dur.pl_Value as Duration,Dur.pl_id as DurID, "
                  + " Purpose,Area_Floor,Checkin_Time,Req_Stat "
                  + " from SecuLobby_VisitingDetails_Self "
                  + " inner join SecuLobby_VisitorInfo_Self on SecuLobby_VisitorInfo_Self.Visitor_ID = SecuLobby_VisitingDetails_Self.Visitor_ID "
                  + " inner join PickList_tran host on host.pl_id = SecuLobby_VisitingDetails_Self.Host_to_Visit and host.pl_head_id = 18 "
                  + " inner join PickList_tran Dur on Dur.pl_id = SecuLobby_VisitingDetails_Self.Duration and Dur.pl_head_id = 6 "
                  + " where convert(date, Checkin_Time)>= '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and convert(date, Checkin_Time)<= '" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'";

      if (txtSearch.Text != "")
      {
        sSql += " and (Name like '%" + txtSearch.Text.Trim() + "%' or Company like '%" + txtSearch.Text.Trim() + "%' or EmiratesID like '%" + txtSearch.Text.Trim() + "%' or Mobile like '%" + txtSearch.Text.Trim() + "%' or Email like '%" + txtSearch.Text.Trim() + "%' or LocationID like '%" + txtSearch.Text.Trim() + "%' or Aptment_Dept like '%" + txtSearch.Text.Trim() + "%' or host.pl_Value like '%" + txtSearch.Text.Trim() + "%' or Dur.pl_Value like '%" + txtSearch.Text.Trim() + "%' or Area_Floor like '%" + txtSearch.Text.Trim() + "%' or Req_Stat like '%" + txtSearch.Text.Trim() + "%' or Visitor_Type like '%" + txtSearch.Text.Trim() + "%')";
      }
      if (sUserGroup == "3")
      {
        sSql += " AND host.pl_Value='" + sUserName + "'";
      }
      sSql += " order by Checkin_Time desc";

      DataTable dt = ocon.GetTable(sSql, new DataSet());

      if (dt.Rows.Count > 0)
      {
        grdDetails.DataSource = dt;
        grdDetails.DataBind();

      }
      else
      {
        grdDetails.DataSource = null;
        grdDetails.DataBind();
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
      grdDetails.GridLines = GridLines.Both;
      grdDetails.HeaderStyle.Font.Bold = true;
      grdDetails.RenderControl(htmltextwrtter);
      Response.Write(strwritter.ToString());
      Response.End();

    }

    protected void BtnExcel_Click(object sender, EventArgs e)
    {
      ExportGridToExcel();
    }
  }
}


