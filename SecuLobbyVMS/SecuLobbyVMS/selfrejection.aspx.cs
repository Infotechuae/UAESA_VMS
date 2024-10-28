using DAL;
using QRCoder;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class selfrejection : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sselfID = "";
        sselfID = Request.QueryString["tabid"];

        string ApproveremailID = Request.QueryString["eid"];
        string att = Request.QueryString["att"];
        if (att == null)
        {
        }
        else
        {

        }

        string sCheck = "";

        DataTable dtCheck = ocon.GetTable("SELECT Req_Stat from SecuLobby_VisitingDetails_Self where Ref_No='" + sselfID + "'", new DataSet());
        if (dtCheck.Rows.Count > 0)
        {
          sCheck = dtCheck.Rows[0]["Req_Stat"].ToString();
        }

        if (sCheck == "Pending")
        {
          string sHost = Request.QueryString["Host"];

       
          string sApproverName = "";
          //DataTable dtApp = ocon.GetTable("select pl_Value from PickList_tran where pl_head_id=18 and Other_Data3='" + sApproverID + "' ", new DataSet());
          //if (dtApp.Rows.Count > 0)
          //{
          //  sApproverName = dtApp.Rows[0]["pl_Value"].ToString();
          //}

          string sSqlUpdate = "UPDATE SecuLobby_VisitingDetails_Self SET Req_Stat='Rejected', Approvedby='" + ApproveremailID + "' where Ref_No='" + sselfID + "'";

          ocon.Execute(sSqlUpdate);

          string sDept = "";
          int iDeptID = 0;

          string sHostnm = "";
          int iHostID = 0;

          string sFloor = "";
          string sVisitor_Type = "";
          string sLocationID = "";
          string sVistorID = "";

          string sVisitorName = "";

          string sEmail = "";
          string sMobile = "";
          string sCompany = "";
          string sIdType = "";
          string sIdNumber = "";

          string sSqlVisitorTran = "SELECT * FROM SecuLobby_VisitingDetails_Self WHERE Ref_No='" + sselfID + "'";
          DataTable dtVisitirTran = ocon.GetTable(sSqlVisitorTran, new DataSet());

          if (dtVisitirTran.Rows.Count > 0)
          {
            sDept = dtVisitirTran.Rows[0]["Aptment_Dept"].ToString();
            iHostID = Convert.ToInt32(dtVisitirTran.Rows[0]["Host_to_Visit"]);
            sFloor = dtVisitirTran.Rows[0]["Area_Floor"].ToString();
            sVisitor_Type = dtVisitirTran.Rows[0]["Visitor_Type"].ToString();
            sLocationID = dtVisitirTran.Rows[0]["LocationID"].ToString();

            sVistorID = dtVisitirTran.Rows[0]["Visitor_ID"].ToString();

            string sdeptID = "select * from PickList_tran where pl_head_id=4 and pl_Value='" + sDept + "'";
            DataTable dtDeptID = ocon.GetTable(sdeptID, new DataSet());
            if (dtDeptID.Rows.Count > 0)
            {
              iDeptID = Convert.ToInt32(dtDeptID.Rows[0]["pl_id"]);

            }

            string sHostname = "select * from PickList_tran where pl_head_id=18 and pl_id='" + iHostID + "'";
            DataTable dtHostname = ocon.GetTable(sHostname, new DataSet());
            if (dtHostname.Rows.Count > 0)
            {
              sHostnm = Convert.ToString(dtHostname.Rows[0]["pl_Value"]);

            }

            string sVisname = "select * from SecuLobby_VisitorInfo_Self where  Visitor_ID='" + sVistorID + "'";
            DataTable dtVisname = ocon.GetTable(sVisname, new DataSet());
            if (dtVisname.Rows.Count > 0)
            {
              sVisitorName = Convert.ToString(dtVisname.Rows[0]["Name"]);
              sEmail = Convert.ToString(dtVisname.Rows[0]["Email"]);
              sMobile = Convert.ToString(dtVisname.Rows[0]["Mobile"]);
              sCompany = Convert.ToString(dtVisname.Rows[0]["Company"]);
              sIdType = Convert.ToString(dtVisname.Rows[0]["Doc_type"]);
              sIdNumber = Convert.ToString(dtVisname.Rows[0]["EmiratesID"]);
            }
          }

          SendEmail(sVisitorName, sEmail, "Approved", "", sHostnm, sDept, sCompany);

          //string script = "window.open('', '_self').close();";
          //ClientScript.RegisterStartupScript(GetType(), "CloseWindowScript", script, true);
          string smessage = string.Format("\"{0}\"", "Visit Request Rejected");

          string sVar = smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
        else
        {
          string smessage = string.Format("\"{0}\"", "Visit Request is already " + sCheck);

          string sVar = smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
      }
    }
    private void SendEmail(string sName, string sEmail, string sStatus, string sQRCode, string sHostName, string sDepartment, string sCompany)
    {
      DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

      string sCopanyName = "UAE Space Agency";

      string sStringConfig = "SELECT ISNULL(Config_value,'') AS Config_value  FROM Tbl_General_Configuration WHERE Config_name='Company name'";
      DataTable dtConfig = ocon.GetTable(sStringConfig, new DataSet());

      if (dtConfig.Rows.Count > 0)
        sCopanyName = dtConfig.Rows[0]["Config_value"].ToString();

      string EmailMask = System.Configuration.ConfigurationManager.AppSettings["EmailMask"];
      string EmailEnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"];
      string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
      string EmailSubject = "Visit Request Rejected";

      EmailSubject = "Visit Request Rejected";


      //string userName = WindowsIdentity.GetCurrent().Name.ToString();
      //string sApproverID = userName.Substring(userName.LastIndexOf("\\") + 1);

      //string sApproverName = "";
      //DataTable dtApp = ocon.GetTable("select pl_Value from PickList_tran where pl_head_id=18 and Other_Data3='" + sApproverID + "' ", new DataSet());
      //if (dtApp.Rows.Count > 0)
      //{
      //  sApproverName = dtApp.Rows[0]["pl_Value"].ToString();
      //}


      string strHTML = "<p> Dear " + sName + ",</p>";

      strHTML = strHTML + "<p>Your visit request is Rejected</p>";


      strHTML = strHTML + "</p><p>      Company : " + sCompany;
      strHTML = strHTML + "</p><p>      Email : " + sEmail;
      strHTML = strHTML + "</p><p>      DateTime : " + DateTime.Now.ToString();

      strHTML = strHTML + "</p><p>      Host Name : " + sHostName;


      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";

      strHTML = strHTML + "</p>Best Regards, <p>";
      strHTML = strHTML + "</p>"+ sCopanyName + "<p>";


      sendEmail.SendEmails(sName, sEmail, "3434", strHTML, EmailSubject, null, null);






    }
  }
}
