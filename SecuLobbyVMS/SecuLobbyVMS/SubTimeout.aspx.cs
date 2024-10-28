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
  public partial class SubTimeout : System.Web.UI.Page
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


        VisitorsTran("", "Overstay");

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_22", ci);
      btncheckOut.Text = rm.GetString("STR_21", ci);
      lblVisDet.Text = rm.GetString("STRN_15", ci);
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

      if (str == "Department")
        sHeaderName = rm.GetString("STRN_1", ci);

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

        string sDestURL = string.Format("\"{0}\"", "Timeout.aspx");
        string smessage = string.Format("\"{0}\"", "CheckOut Successful");

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

        SqlCommand cmdOrl = new SqlCommand("UpdateVisitorCheckOut");
        cmdOrl.CommandType = CommandType.StoredProcedure;
        cmdOrl.Parameters.AddWithValue("@Ref_No", Ref_No);
        cmdOrl.Parameters.AddWithValue("@Visitor_ID", Visitor_ID);
        cmdOrl.Parameters.AddWithValue("@EmiratesID", "");
        cmdOrl.Parameters.AddWithValue("@MobileNO", "");
        cmdOrl.Parameters.AddWithValue("@Remarks", Remarks);
        cmdOrl.Parameters.AddWithValue("@Loc_ID", Session["Loc_ID"].ToString());

        DBSQL dbs1 = new DBSQL();
        dbs1.ExecuteStoredProcedure(cmdOrl, MyConnection.ReadConStr("Local"));

        string sHost = "";
        string sFullname = "";
        string sCompanyName = "";
        string sEmail = "";
        string sMobile = "";
        string sDept = "";

        string sqlstr = "SELECT dbo.SecuLobby_VisitorInfo.Visitor_ID, dbo.SecuLobby_VisitingDetails.Ref_No,SecuLobby_VisitorInfo.Company, dbo.SecuLobby_VisitorInfo.Name, dbo.SecuLobby_VisitorInfo.Email, dbo.SecuLobby_VisitorInfo.Mobile, dbo.SecuLobby_VisitingDetails.Aptment_Dept,dbo.SecuLobby_VisitingDetails.Host_to_Visit FROM     dbo.SecuLobby_VisitorInfo INNER JOIN dbo.SecuLobby_VisitingDetails ON dbo.SecuLobby_VisitorInfo.Visitor_ID = dbo.SecuLobby_VisitingDetails.Visitor_ID where Ref_No ='" + Ref_No + "'";
        DataTable dtVisitor = ocon.GetTable(sqlstr, new DataSet());

        if (dtVisitor.Rows.Count > 0)
        {
          sHost = Convert.ToString(dtVisitor.Rows[0]["Host_to_Visit"]);

          sFullname = Convert.ToString(dtVisitor.Rows[0]["Name"]);
          sCompanyName = Convert.ToString(dtVisitor.Rows[0]["Company"]);
          sEmail = Convert.ToString(dtVisitor.Rows[0]["Email"]);
          sMobile = Convert.ToString(dtVisitor.Rows[0]["Mobile"]);

          sDept = Convert.ToString(dtVisitor.Rows[0]["Aptment_Dept"]);

        }

        string sCopanyName = "UAE Space Agency";

        string sStringConfig = "SELECT ISNULL(Config_value,'') AS Config_value  FROM Tbl_General_Configuration WHERE Config_name='Company name'";
        DataTable dtConfig = ocon.GetTable(sStringConfig, new DataSet());

        if (dtConfig.Rows.Count > 0)
          sCopanyName = dtConfig.Rows[0]["Config_value"].ToString();


        string SelfHostEmail = System.Configuration.ConfigurationManager.AppSettings["SelfHostEmail"];
        // string _FooterText = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(0, HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf("/") + 1) + "SelfRegistrationApproval.aspx?tabid=" + sRefNo + "?tabid="+;

        if (SelfHostEmail == "True")
        {
          //////////////////////Host Email///////////////////////////////////////

          string sSqlHostEmail = "select pl_data from PickList_tran where pl_head_id=18 and pl_value='" + sHost + "'";
          DataTable dtHostEmail = ocon.GetTable(sSqlHostEmail, new DataSet());

          if (dtHostEmail.Rows.Count > 0)
          {
            string sHostEmail = Convert.ToString(dtHostEmail.Rows[0]["pl_data"]);
            string EmailSubject = "Visitor Checked Out";
            if (!string.IsNullOrEmpty(sHostEmail))
            {
              string strHTML = "<p> Dear " + sHost + ",</p>";

              strHTML = strHTML + "<p>The below Visitor is Checked Out .</p>";
              //else
              //  strHTML = strHTML + "<p> Visitor wants to meet your department person .</p>";

              strHTML = strHTML + "</p><p>      Visitor Name : " + sFullname;
              strHTML = strHTML + "</p><p>      Company : " + sCompanyName;
              strHTML = strHTML + "</p><p>      Email  : " + sEmail;
              strHTML = strHTML + "</p><p>      Phone No : " + sMobile;

              strHTML = strHTML + "</p><p>";
              strHTML = strHTML + "</p><p>";
              strHTML = strHTML + "</p><p>";
              strHTML = strHTML + "</p><p>";


              strHTML = strHTML + "</p>Best Regards, <p>";
              strHTML = strHTML + "</p>" + sCopanyName + "<p>";

              // strHTML = strHTML + "<p>Please approve or reject this meeting through the below link link.</p>";


              sendEmail.SendEmails(sHost, sHostEmail, "24", strHTML, EmailSubject, null, null);

            }
          }
        }
        /////////////////////////////////////////////////////////////




        string SelfAdminEmail = System.Configuration.ConfigurationManager.AppSettings["SelfAdminEmail"];
        //////////////////////dept admin Email///////////////////////////////////////
        if (SelfAdminEmail == "True")
        {
         
          string sSqlAdminEmail = "select pl_data,isnull(Other_Data2,'') as Name from PickList_tran where pl_head_id=4 and pl_value='" + sDept + "'";
          DataTable dtAdminEmail = ocon.GetTable(sSqlAdminEmail, new DataSet());
          if (dtAdminEmail.Rows.Count > 0)
          {
            string sAdminEmail = Convert.ToString(dtAdminEmail.Rows[0]["pl_data"]);
            string sAdminEmailName = Convert.ToString(dtAdminEmail.Rows[0]["Name"]);

            string EmailSubject = "Visitor Checked Out";

            string sHead = "Dear Sir,";

            if (!string.IsNullOrEmpty(sAdminEmail))
            {
              if (!string.IsNullOrEmpty(sAdminEmailName))
              {
                sHead = "Dear " + sAdminEmailName + ",";
              }
              string strHTML = "<p> " + sHead + "</p>";

              strHTML = strHTML + "<p>The below Visitor is Checked Out .</p>";


              strHTML = strHTML + "</p><p>      Visitor Name : " + sFullname;
              strHTML = strHTML + "</p><p>      Company : " + sCompanyName;
              strHTML = strHTML + "</p><p>      Email  : " + sEmail;
              strHTML = strHTML + "</p><p>      Phone No : " + sMobile;

              strHTML = strHTML + "</p><p>";
              strHTML = strHTML + "</p><p>";
              strHTML = strHTML + "</p><p>";
              strHTML = strHTML + "</p><p>";


              strHTML = strHTML + "</p>Best Regards, <p>";
              strHTML = strHTML + "</p>Visitor Management Systems<p>";



              //  strHTML = strHTML + "<p>Please approve or reject this meeting through the below link link.</p>";

              sendEmail.SendEmails(sDept, sAdminEmail, "23", strHTML, EmailSubject, null, null);

              string sSqlSecEmail = "select pl_data from PickList_tran where pl_head_id=4 and pl_value='Security_Notification'";
              DataTable dtSecEmail = ocon.GetTable(sSqlSecEmail, new DataSet());
              if (dtSecEmail.Rows.Count > 0)
              {
                string sSecEmail = Convert.ToString(dtSecEmail.Rows[0]["pl_data"]);

                if (!string.IsNullOrEmpty(sSecEmail))
                {
                  string strHTML2 = "<p> Dear Tawazun Security,</p>";

                  strHTML2 = strHTML2 + "<p> Visitor Checked IN </p>";
                  //else
                  //  strHTML = strHTML + "<p> Visitor wants to meet your department person .</p>";

                  strHTML2 = strHTML2 + "</p><p>      Visitor Name : " + sFullname;
                  strHTML2 = strHTML2 + "</p><p>      Company : " + sCompanyName;
                  strHTML2 = strHTML2 + "</p><p>      Email : " + sEmail;
                  strHTML2 = strHTML2 + "</p><p>      Phone No : " + sMobile;

                  strHTML2 = strHTML2 + "</p><p>";
                  strHTML2 = strHTML2 + "</p><p>";
                  strHTML2 = strHTML2 + "</p><p>";
                  strHTML2 = strHTML2 + "</p><p>";

                  strHTML2 = strHTML2 + "</p>Best Regards, <p>";
                  strHTML2 = strHTML2 + "</p>" + sCopanyName + "<p>";

                  sendEmail.SendEmails(sDept, sSecEmail, "23", strHTML2, EmailSubject, null, null);
                }
              }
            }
          }
        }

        string CheckOutMail = System.Configuration.ConfigurationManager.AppSettings["CheckOutMail"];
        //////////////////////dept admin Email///////////////////////////////////////
        if (CheckOutMail == "True")
        {



          string EmailSubject = "Checked Out";
          if (!string.IsNullOrEmpty(sEmail))
          {
            string strHTML = "<p> Dear " + sFullname + ",</p>";

            strHTML = strHTML + "<p>You successfully checked out.Thank You for visiting .</p>";



            strHTML = strHTML + "</p><p>";
            strHTML = strHTML + "</p><p>";
            strHTML = strHTML + "</p><p>";
            strHTML = strHTML + "</p><p>";


            strHTML = strHTML + "</p>Best Regards, <p>";
            strHTML = strHTML + "</p>" + sCopanyName + "<p>";


            sendEmail.SendEmails(sFullname, sEmail, "24", strHTML, EmailSubject, null, null);

          }
        }


        VisitorsTran("", "Overstay");

      }
      catch (Exception ex)
      {

      }
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (txtSearch.Text != "")
      {
        VisitorsTran(txtSearch.Text, "Overstay");
      }
      else
      {
        VisitorsTran("", "Overstay");
      }
    }
  }
}
