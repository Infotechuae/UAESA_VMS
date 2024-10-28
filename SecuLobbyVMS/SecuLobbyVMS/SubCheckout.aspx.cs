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
  public partial class SubCheckout : System.Web.UI.Page
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


        VisitorsTran("", "TodayIN");

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_21", ci);
      btncheckOut.Text = rm.GetString("STR_21", ci);
      lblwach.Text = rm.GetString("STR_23", ci);

      lblVisDet.Text = rm.GetString("STRN_15", ci);
      lblReas.Text = rm.GetString("STRN_47", ci);
      lblClose.Text = rm.GetString("STRN_46", ci);
      btnWatchlist.Text = rm.GetString("STRN_21", ci);
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
        sHeaderName = rm.GetString("STR_11", ci);
      if (str == "Purpose")
        sHeaderName = rm.GetString("STR_61", ci);

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

        string sDestURL = string.Format("\"{0}\"", "Checkout.aspx");
        string smessage = string.Format("\"{0}\"", "Successfully Checkout");

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


        #region Email

        string ServiceLevel = "";
        bool ApprovalRequired = false;
        bool HostApproval = false;
        bool LineManagerApproval = false;
        string sHostEmail = "";
        string sManagerName = "";
        string sManagerEmail = "";
        string sVisitorType = "";

        string sqlstr = "SELECT dbo.SecuLobby_VisitorInfo.Visitor_ID, dbo.SecuLobby_VisitingDetails.Ref_No,SecuLobby_VisitorInfo.Company, dbo.SecuLobby_VisitorInfo.Name, dbo.SecuLobby_VisitorInfo.Email, dbo.SecuLobby_VisitorInfo.Mobile, dbo.SecuLobby_VisitingDetails.Aptment_Dept,dbo.SecuLobby_VisitingDetails.Host_to_Visit,dbo.SecuLobby_VisitingDetails.Visitor_Type FROM     dbo.SecuLobby_VisitorInfo INNER JOIN dbo.SecuLobby_VisitingDetails ON dbo.SecuLobby_VisitorInfo.Visitor_ID = dbo.SecuLobby_VisitingDetails.Visitor_ID where Ref_No ='" + Ref_No + "'";
        DataTable dtVisitor = ocon.GetTable(sqlstr, new DataSet());

        if (dtVisitor.Rows.Count > 0)
        {
          sHost = Convert.ToString(dtVisitor.Rows[0]["Host_to_Visit"]);
          sVisitorType = Convert.ToString(dtVisitor.Rows[0]["Visitor_Type"]);
          sFullname = Convert.ToString(dtVisitor.Rows[0]["Name"]);
          sEmail = Convert.ToString(dtVisitor.Rows[0]["Email"]);
        }

        string sSql = "select isnull(ApprovalRequired,0) as ApprovalRequired,isnull(HostApproval,0) as HostApproval,isnull(LineManagerApproval,0) as LineManagerApproval,isnull(ServiceLevel,'') as ServiceLevel from ApprovalSettings inner join PickList_tran pt on pt.pl_Value = '" + sVisitorType + "' where MenuID = 5 ";
        DataTable dt = ocon.GetTable(sSql, new DataSet());
        if (dt.Rows.Count > 0)
        {
          ApprovalRequired = (bool)dt.Rows[0]["ApprovalRequired"];
          HostApproval = (bool)dt.Rows[0]["HostApproval"];
          LineManagerApproval = (bool)dt.Rows[0]["LineManagerApproval"];
          ServiceLevel = Convert.ToString(dt.Rows[0]["ServiceLevel"]);
        }

        #endregion





        //////////////////////Host Email///////////////////////////////////////

        string sSqlHostEmail = "select isnull(pl_data,'') as pl_data,isnull(Other_Data2,'') as Other_Data2 from PickList_tran where pl_head_id=18 and pl_value='" + sHost + "'";
        DataTable dtHostEmail = ocon.GetTable(sSqlHostEmail, new DataSet());



        if (dtHostEmail.Rows.Count > 0)
        {
          sHostEmail = Convert.ToString(dtHostEmail.Rows[0]["pl_data"]);
          sManagerName = Convert.ToString(dtHostEmail.Rows[0]["Other_Data2"]);

          string sSqlManEmail = "select isnull(pl_data,'') as pl_data from PickList_tran where pl_head_id=18 and pl_value='" + sManagerName + "'";
          DataTable dtManEmail = ocon.GetTable(sSqlManEmail, new DataSet());
          if (dtHostEmail.Rows.Count > 0)
          {
            sManagerEmail = Convert.ToString(dtHostEmail.Rows[0]["pl_data"]);
          }
        }

        if (ApprovalRequired == true)
        {
          string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Check Out' and Email_For='Request With Approval'";
          DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());

          if (dtEmailtemp.Rows.Count > 0)
          {
            foreach (DataRow dr in dtEmailtemp.Rows)
            {
              string sEmailStage = dr["EmailStage"].ToString();
              string EmailSubject = dr["Email_Sub"].ToString();
              string strHTML = dr["Email_Body"].ToString();
              strHTML = GetEmailBody(strHTML, Ref_No);

              if (sEmailStage == "Requester")
              {
                if (!string.IsNullOrEmpty(sEmail))
                {
                  sendEmail.SendEmails(sFullname, sEmail, "02", strHTML, EmailSubject, null, null);
                }
              }
              if (HostApproval == true)
              {
                if (sEmailStage == "Host")
                {
                  if (!string.IsNullOrEmpty(sHostEmail))
                  {
                    sendEmail.SendEmails(sHost, sHostEmail, "02", strHTML, EmailSubject, null, null);
                  }
                }
              }
              if (LineManagerApproval == true)
              {
                if (sEmailStage == "Line manager")
                {
                  if (!string.IsNullOrEmpty(sManagerEmail))
                  {
                    sendEmail.SendEmails(sManagerName, sManagerEmail, "02", strHTML, EmailSubject, null, null);
                  }
                }
              }
              if (ServiceLevel != "")
              {
                if (sEmailStage == "Service level")
                {
                  if (!string.IsNullOrEmpty(ServiceLevel))
                  {
                    sendEmail.SendEmails(sManagerName, ServiceLevel, "02", strHTML, EmailSubject, null, null);
                  }
                }
              }

            }
          }
        }
        else
        {
          string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Check Out' and Email_For='Request Without Approval'";

          DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());

          if (dtEmailtemp.Rows.Count > 0)
          {
            foreach (DataRow dr in dtEmailtemp.Rows)
            {
              string sEmailStage = dr["EmailStage"].ToString();
              string EmailSubject = dr["Email_Sub"].ToString();
              string strHTML = dr["Email_Body"].ToString();
              strHTML = GetEmailBody(strHTML, Ref_No);

              if (sEmailStage == "Requester")
              {
                if (!string.IsNullOrEmpty(sEmail))
                {
                  sendEmail.SendEmails(sFullname, sEmail, "02", strHTML, EmailSubject, null, null);
                }
              }

              if (sEmailStage == "Host")
              {
                if (!string.IsNullOrEmpty(sHostEmail))
                {
                  sendEmail.SendEmails(sHost, sHostEmail, "02", strHTML, EmailSubject, null, null);
                }
              }


              if (sEmailStage == "Line manager")
              {
                if (!string.IsNullOrEmpty(sManagerEmail))
                {
                  sendEmail.SendEmails(sManagerName, sManagerEmail, "02", strHTML, EmailSubject, null, null);
                }
              }


              if (sEmailStage == "Service level")
              {
                if (!string.IsNullOrEmpty(ServiceLevel))
                {
                  sendEmail.SendEmails(sManagerName, ServiceLevel, "02", strHTML, EmailSubject, null, null);
                }
              }

            }
          }

        }







        /////////////////////////////////////////////////////////////






        VisitorsTran("", "TodayIN");

      }
      catch (Exception ex)
      {

      }
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (txtSearch.Text != "")
      {
        VisitorsTran(txtSearch.Text, "TodayIN");
      }
      else
      {
        VisitorsTran("", "TodayIN");
      }
    }

    protected void btnWatchlist_Click(object sender, EventArgs e)
    {
      foreach (GridViewRow grgr in grdVisDetails.Rows)
      {
        Label lblRefNo = (Label)(grgr.FindControl("lblEmiratesID"));
        Label lblVisitorID = (Label)(grgr.FindControl("lblVisitorID"));

        CheckBox check = (CheckBox)(grgr.FindControl("check"));
        if (check.Checked)
        {
          Visitor_CheckOut(lblRefNo.Text, lblVisitorID.Text, "");

          SqlCommand cmdOrl = new SqlCommand("UpdateVisitorWatchList");
          cmdOrl.CommandType = CommandType.StoredProcedure;
          cmdOrl.Parameters.AddWithValue("@Ref_No", lblRefNo.Text);
          cmdOrl.Parameters.AddWithValue("@Loc_ID", Session["Loc_ID"]);
          cmdOrl.Parameters.AddWithValue("@Reason_To_Add_Watchlist", txtWatchlistReason.Text);

          DBSQL dbs1 = new DBSQL();
          dbs1.ExecuteStoredProcedure(cmdOrl, MyConnection.ReadConStr("Local"));

        }
        string sDestURL = string.Format("\"{0}\"", "Checkout.aspx");
        string smessage = string.Format("\"{0}\"", "Watchlist Updated Successful");

        string sVar = sDestURL + "," + smessage;

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

      }
    }


    private string GetEmailBody(string Template, string sRefNo)
    {
      string sqlstr = "SELECT dbo.SecuLobby_VisitorInfo.Visitor_ID, dbo.SecuLobby_VisitingDetails.Ref_No,SecuLobby_VisitorInfo.Company, dbo.SecuLobby_VisitorInfo.Name, dbo.SecuLobby_VisitorInfo.Email, dbo.SecuLobby_VisitorInfo.Mobile, dbo.SecuLobby_VisitingDetails.Aptment_Dept,dbo.SecuLobby_VisitingDetails.Host_to_Visit FROM     dbo.SecuLobby_VisitorInfo INNER JOIN dbo.SecuLobby_VisitingDetails ON dbo.SecuLobby_VisitorInfo.Visitor_ID = dbo.SecuLobby_VisitingDetails.Visitor_ID where Ref_No ='" + sRefNo + "'";
      DataTable dtVisitor = ocon.GetTable(sqlstr, new DataSet());

      if (dtVisitor.Rows.Count > 0)
      {

        string sHostName = Convert.ToString(dtVisitor.Rows[0]["Host_to_Visit"]);

        string sVisitorname = Convert.ToString(dtVisitor.Rows[0]["Name"]);
        string sVisitorCompany = Convert.ToString(dtVisitor.Rows[0]["Company"]);
        string sVisitorEmail = Convert.ToString(dtVisitor.Rows[0]["Email"]);
        string sVisitorPhone = Convert.ToString(dtVisitor.Rows[0]["Mobile"]);

        string sDepartment = Convert.ToString(dtVisitor.Rows[0]["Aptment_Dept"]);


        string sIDType = "";
        string sIDnumber = "";
        string sExpirydate = "";


        string sPurpose = "";
        string sVisitorType = "";
        string sDuration = "";
        string sRemarks = "";


        Template = Template.Replace("[VisitorName]", sVisitorname);
        Template = Template.Replace("[IDType]", sIDType);
        Template = Template.Replace("[IDNumber]", sIDnumber);
        Template = Template.Replace("[VisitorCompany]", sVisitorCompany);
        Template = Template.Replace("[VisitorEmail]", sVisitorEmail);
        Template = Template.Replace("[VisitorPhone]", sVisitorPhone);

        Template = Template.Replace("[Visitdate]", DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
        Template = Template.Replace("[Department]", sDepartment);
        Template = Template.Replace("[HostName]", sHostName);
        Template = Template.Replace("[VisitorPurpose]", sPurpose);
        Template = Template.Replace("[VisitorType]", sVisitorType);
        Template = Template.Replace("[Duration]", sDuration);
        Template = Template.Replace("[Remarks]", sRemarks);
      }
      return Template;
    }


  }
}
