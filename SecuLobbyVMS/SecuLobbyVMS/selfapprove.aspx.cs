using DAL;
using QRCoder;
using SecuLobbyVMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class selfapprove : System.Web.UI.Page
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

          //string userName = WindowsIdentity.GetCurrent().Name.ToString();
          //string sApproverID = userName.Substring(userName.LastIndexOf("\\") + 1);

          string sApproverName = "";
          //DataTable dtApp = ocon.GetTable("select pl_Value from PickList_tran where pl_head_id=18 and Other_Data3='" + sApproverID + "' ", new DataSet());
          //if (dtApp.Rows.Count > 0)
          //{
          //  sApproverName = dtApp.Rows[0]["pl_Value"].ToString();
          //}

          string sHost = Request.QueryString["Host"];

          string sSqlUpdate = "UPDATE SecuLobby_VisitingDetails_Self SET Req_Stat='Approved', Approvedby='" + ApproveremailID + "' where Ref_No='" + sselfID + "'";

          ocon.Execute(sSqlUpdate);

          int iCid = 0;
          DataTable dtCid = ocon.GetTable("SELECT isnull(max(cId),0) as cId FROM Scheduling", new DataSet());
          if (dtCid.Rows.Count > 0)
          {
            iCid = Convert.ToInt32(dtCid.Rows[0]["cId"]);
          }
          iCid = iCid + 1;

          DateTime dtStartTime = DateTime.Now;
          DateTime dtEndTime = dtStartTime.AddMinutes(60);

          string sHostame = sHost;

          string sHostEmail = "";
          string sSqlHostEmail = "select pl_data from PickList_tran where pl_head_id=18 and pl_value='" + sHostame + "'";


          DataTable dtHostEmail = ocon.GetTable(sSqlHostEmail, new DataSet());
          if (dtHostEmail.Rows.Count > 0)
          {
            sHostEmail = Convert.ToString(dtHostEmail.Rows[0]["pl_data"]);
          }

          Random rnd = new Random();
          int myRandomNo = rnd.Next(1000000, 9999999);
          string sPersonID = myRandomNo.ToString();


          string sDept = "";
          int iDeptID = 0;

          string sHostnm = "";
          int iHostID = 0;

          string sFloor = "";
          string sVisitor_Type = "";
          string sLocationName = "";
          int LocationID = 0;
          string sVistorID = "";
          string sDuration = "";

          string sVisitorName = "";

          string sEmail = "";
          string sMobile = "";
          string sCompany = "";
          string sIdType = "";
          string sIdNumber = "";
          string sPurpose = "";
          int iCompID = 0;

          string sSqlVisitorTran = "SELECT * FROM SecuLobby_VisitingDetails_Self WHERE Ref_No='" + sselfID + "'";
          DataTable dtVisitirTran = ocon.GetTable(sSqlVisitorTran, new DataSet());

          if (dtVisitirTran.Rows.Count > 0)
          {
            sDept = dtVisitirTran.Rows[0]["Aptment_Dept"].ToString();
            iHostID = Convert.ToInt32(dtVisitirTran.Rows[0]["Host_to_Visit"]);
            sFloor = dtVisitirTran.Rows[0]["Area_Floor"].ToString();
            sVisitor_Type = dtVisitirTran.Rows[0]["Visitor_Type"].ToString();
            sLocationName = dtVisitirTran.Rows[0]["LocationID"].ToString();
            sDuration = dtVisitirTran.Rows[0]["Duration"].ToString();
            sPurpose = dtVisitirTran.Rows[0]["Purpose"].ToString();

            string sLocationID = "select * from PickList_tran where pl_head_id=4 and pl_Value='" + sLocationName + "'";
            DataTable dtLocationID = ocon.GetTable(sLocationID, new DataSet());
            if (dtLocationID.Rows.Count > 0)
            {
              LocationID = Convert.ToInt32(dtLocationID.Rows[0]["pl_id"]);

            }

            sVistorID = dtVisitirTran.Rows[0]["Visitor_ID"].ToString();

            //string sdeptID = "select * from PickList_tran where pl_head_id=4 and pl_Value='" + sDept + "'";
            //DataTable dtDeptID = ocon.GetTable(sdeptID, new DataSet());
            //if (dtDeptID.Rows.Count > 0)
            //{
            //  iDeptID = Convert.ToInt32(dtDeptID.Rows[0]["pl_id"]);

            //}

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

          string sQRCode = sPersonID;
          //string sVisitor_Type = drpVisitorType.SelectedValue;
          //string sLocationID = drpCompany.SelectedValue;
          StringBuilder sb = new StringBuilder();
          sb.Append("INSERT INTO Scheduling (cId,UserId,Status,Subject,Description,Label,StartTime,EndTime,Location,AllDay,ContactInfo,EntryID,QRCode,Organizer,Loc_ID,HostName,VisPhone,VisCompany,IDType,IDNumber,FloorName,Duration,Visitor_Type,LocationID,Purpose)" + Environment.NewLine);
          sb.Append("VALUES ('" + iCid + "',1,1,'" + sVisitorName + "','Meeting',1,'" + dtStartTime + "','" + dtEndTime + "','" + sDept + "',0,'" + sEmail + "','" + iCid + "','" + sQRCode + "','" + sHostEmail + "',1,'" + sHostnm + "','" + sMobile + "','" + sCompany + "','" + sIdType + "','" + sIdNumber + "','" + sFloor + "','" + sDuration + "','" + sVisitor_Type + "','" + sLocationName + "','" + sPurpose + "')" + Environment.NewLine);

          ocon.Execute(sb.ToString());

          string sSql = "update Scheduling set Image=(select Image from Visitor_Image_Self where ID='" + sVistorID + "') where cId='"+ iCid + "'";
          ocon.Execute(sSql);


          SendEmail(sVisitorName, sEmail, "Approved", sQRCode, sHostnm, sDept, sCompany, ApproveremailID, sPurpose);


          string smessage = string.Format("\"{0}\"", "Visit Request Approved");

          string sVar = smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

          //string script = "window.open('', '_self').close();";
          //ClientScript.RegisterStartupScript(GetType(), "CloseWindowScript", script, true);
        }
        else
        {
          string smessage = string.Format("\"{0}\"", "Visit Request is already " + sCheck);

          string sVar = smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }


      }
    }

    public byte[] imgToByteArray(System.Drawing.Image img)
    {
      using (MemoryStream mStream = new MemoryStream())
      {
        img.Save(mStream, img.RawFormat);
        return mStream.ToArray();
      }
    }

    private void SendEmail(string sName, string sEmail, string sStatus, string sQRCode, string sHostName, string sDepartment, string sCompany, string sApproverName,string sPurpose)
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
      string EmailSubject = "Visit Request Approved";

      EmailSubject = "Visit Request Approved";

      QRCodeGenerator qrGenerator1 = new QRCodeGenerator();
      byte[] byteImageVis = null;

      QRCodeGenerator.QRCode qrCodeVis = qrGenerator1.CreateQrCode(sQRCode, QRCodeGenerator.ECCLevel.Q);
      System.Web.UI.WebControls.Image imgBarCodeVis = new System.Web.UI.WebControls.Image();
      imgBarCodeVis.Height = 100;
      imgBarCodeVis.Width = 150; //150
      using (Bitmap bitMap = qrCodeVis.GetGraphic(20))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
          byteImageVis = ms.ToArray();

        }


        //ASPxImage1.Controls.Add(imgBarCode);
      }

      MemoryStream ms1 = new MemoryStream(byteImageVis);
      LinkedResource res = new LinkedResource(ms1, "image/jpeg");
      res.ContentId = Guid.NewGuid().ToString();

      string strHTML = "<p> Dear " + sName + ",</p>";

      strHTML = strHTML + "<p>Your visit request is approved.</p>";
      strHTML = strHTML + "<p>Please visit the nearest reception area.</p>";


      strHTML = strHTML + "</p><p>      Visitor Name : " + sName;
      strHTML = strHTML + "</p><p>      Company : " + sCompany;
      strHTML = strHTML + "</p><p>      Email : " + sEmail;
      strHTML = strHTML + "</p><p>      DateTime : " + DateTime.Now.ToString();

      strHTML = strHTML + "</p><p>      Host Name : " + sHostName;
      strHTML = strHTML + "</p><p>      Purpose : " + sPurpose;

      //if (sApproverName != "")
      //{
      //  strHTML = strHTML + "</p><p>      Approved By : " + sApproverName;
      //}

      strHTML = strHTML + @"<img src='cid:" + res.ContentId + @"' width=200 height=200 />";

      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";

      strHTML = strHTML + "</p>Best Regards, <p>";
      strHTML = strHTML + "</p>" + sCopanyName + "<p>";



      sendEmail.SendEmails(sName, sEmail, "3434", strHTML, EmailSubject, byteImageVis, res);



      #region Host

  

      #endregion


      #region Cordinator

 

      string SelfAdminEmail = System.Configuration.ConfigurationManager.AppSettings["SelfAdminEmail"];
      //////////////////////Host Email///////////////////////////////////////
      if (SelfAdminEmail == "True")
      {

      
        string sSqlAdminEmail = "select pl_data,isnull(Other_Data2,'') as Name from PickList_tran where pl_head_id=4 and pl_value='" + sDepartment + "'";
        DataTable dtAdminEmail = ocon.GetTable(sSqlAdminEmail, new DataSet());
        if (dtAdminEmail.Rows.Count > 0)
        {
          string sAdminEmail = Convert.ToString(dtAdminEmail.Rows[0]["pl_data"]);
          string sAdminEmailName = Convert.ToString(dtAdminEmail.Rows[0]["Name"]);


          string sHead = "Dear Sir,";

          if (!string.IsNullOrEmpty(sAdminEmail))
          {
            if (!string.IsNullOrEmpty(sAdminEmailName))
            {
              sHead = "Dear " + sAdminEmailName + ",";
            }
            string strHTML1 = "<p> " + sHead + "</p>";


            strHTML1 = strHTML1 + "<p>The Below visit request is approved</p>";

            strHTML1 = strHTML1 + "</p><p>      Visitor Name : " + sName;
            strHTML1 = strHTML1 + "</p><p>      Company : " + sCompany;
            strHTML1 = strHTML1 + "</p><p>      Email : " + sEmail;
            strHTML1 = strHTML1 + "</p><p>      Purpose : " + sPurpose;

            strHTML1 = strHTML1 + "</p><p>";
            strHTML1 = strHTML1 + "</p><p>";
            strHTML1 = strHTML1 + "</p><p>";
            strHTML1 = strHTML1 + "</p><p>";


            strHTML1 = strHTML1 + "</p>Best Regards, <p>";
            strHTML1 = strHTML1 + "</p>" + sCopanyName + "<p>";


            sendEmail.SendEmails(sDepartment, sAdminEmail, "3434", strHTML1, EmailSubject, null, null);
          }
        }
      }

      #endregion


    }

    private void Update()
    {
     


    }
  }
}
