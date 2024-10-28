using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class SubWatchlist : System.Web.UI.Page
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


        VisitorsTran("", "WatchListNew");

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_23", ci);
      btncheckOut.Text = rm.GetString("STR_41", ci);

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
      if (str == "Company Name")
        sHeaderName = rm.GetString("STR_5", ci);
      if (str == "Visitor Type")
        sHeaderName = rm.GetString("STR_14", ci);
      if (str == "Host")
        sHeaderName = rm.GetString("STR_13", ci);
      if (str == "Check In")
        sHeaderName = rm.GetString("STR_18", ci);
      if (str == "Duration")
        sHeaderName = rm.GetString("STR_15", ci);

      return sHeaderName;
    }

      protected void btncheckOut_Click(object sender, EventArgs e)
    {
      foreach (GridViewRow grgr in grdVisDetails.Rows)
      {
        Label lblRefNo = (Label)(grgr.FindControl("lblRefNo"));
        Label lblVisitorID = (Label)(grgr.FindControl("lblVisitorID"));

        CheckBox check = (CheckBox)(grgr.FindControl("check"));
        if (check.Checked)
          Visitor_CheckOut(lblRefNo.Text, lblVisitorID.Text, "");

        string sDestURL = string.Format("\"{0}\"", "Watchlist.aspx");
        string smessage = string.Format("\"{0}\"", "Watchlist removed Successfully");

        string sVar = sDestURL + "," + smessage;

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

      }
    }

    public void VisitorsTran(string Value, string module)
    {
      string sSpName = "VisitorTransaction";
      if (Value != "")
        sSpName = "VisitorTransaction_WithSearch";



      DataSet cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Value, Session["Loc_ID"].ToString(), module, sSpName, MyConnection.ReadConStr("Local"));

      if (cmbDS2.Tables[0].Rows.Count > 0)
      {
        grdVisDetails.DataSource = cmbDS2.Tables[0];
        grdVisDetails.DataBind();
      }
      else
      {
        grdVisDetails.DataSource = null;
        grdVisDetails.DataBind();
      }

    }

    public void Visitor_CheckOut(string Ref_No, string Visitor_ID, string Remarks)
    {
      try
      {

        string sUpdate = "UPDATE SecuLobby_VisitingDetails set Add_To_WatchList='N' WHERE Visitor_ID='" + Visitor_ID + "' and Ref_No='" + Ref_No + "' ";
        ocon.Execute(sUpdate);

        string sDelete = "DELETE Watch_List WHERE Visitor_ID='" + Visitor_ID + "'";
        ocon.Execute(sDelete);


      

      }
      catch (Exception ex)
      {

      }
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (txtSearch.Text != "")
      {
        VisitorsTran(txtSearch.Text, "WatchListNew");
      }
      else
      {
        VisitorsTran("", "WatchListNew");
      }
    }
  }
}
