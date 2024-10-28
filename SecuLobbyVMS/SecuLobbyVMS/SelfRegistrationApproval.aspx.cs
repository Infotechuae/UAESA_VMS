using DAL;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Web.UI;
using QRCoder;
using System.Drawing;
using System.Net.Mime;
using System.Net;

using SecuLobbyVMS.App_Code;
using System.Security.Principal;

namespace SecuLobbyVMS
{
  public partial class SelfRegistrationApproval : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    ResourceManager rm;
    CultureInfo ci;
    public static string stcLocID = "";
    string sApprovedBy = "";
    string sSyncOnline = System.Configuration.ConfigurationManager.AppSettings["Sync_Online"];
    string sDateFmt = System.Configuration.ConfigurationManager.AppSettings["DateFMT"];
    string SMS_Timeout_Msg = System.Configuration.ConfigurationManager.AppSettings["SMS_Checkin_Msg"];
    string SMS_URL = System.Configuration.ConfigurationManager.AppSettings["SMS_URL"];
    bool EnableSMS = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SMS"]);
    string SMS_USER = System.Configuration.ConfigurationManager.AppSettings["SMS_USER"];
    string SMS_PWD = System.Configuration.ConfigurationManager.AppSettings["SMS_PWD"];
    string SMS_CID = System.Configuration.ConfigurationManager.AppSettings["SMS_CID"];
    string FacCode = System.Configuration.ConfigurationManager.AppSettings["FacilityCode"];
    int CardLength = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["CardLength"]);

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        //string sLang = Convert.ToString(Session["Lang"]);
        //string sUserID = Convert.ToString(Session["UserID"]);

       
        BindDropDown();

        string sselfID = Request.QueryString["tabid"];

        string sApprovedBy = Request.QueryString["eid"];
        string att = Request.QueryString["att"];
        if (att==null)
        {
        }
        else
        {

        }

        string sVisitorID = "";
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT SecuLobby_VisitorInfo_Self.Visitor_ID, Name,Company,EmiratesID,Mobile,Email,Exp_Date,Doc_type,Aptment_Dept,Area_Floor,Host_to_Visit,Remarks,Req_Stat,Duration,Visitor_Type,LocationID,Purpose  FROM SecuLobby_VisitingDetails_Self " + Environment.NewLine);
        sb.Append("inner join SecuLobby_VisitorInfo_Self ON SecuLobby_VisitorInfo_Self.Visitor_ID=SecuLobby_VisitingDetails_Self.Visitor_ID" + Environment.NewLine);
        sb.Append("where SecuLobby_VisitingDetails_Self.Ref_No='" + sselfID + "'" + Environment.NewLine);

        DataTable dt = ocon.GetTable(sb.ToString(), new DataSet());

        if (dt.Rows.Count > 0)
        {
          visitorID.Value = Convert.ToString(dt.Rows[0]["Visitor_ID"]);
          txtFullname.Text = Convert.ToString(dt.Rows[0]["Name"]);
          drpIDType.SelectedValue = Convert.ToString(dt.Rows[0]["Doc_type"]);
          txtIDNumber.Text = Convert.ToString(dt.Rows[0]["EmiratesID"]);

          DateTime dtexpdate = Convert.ToDateTime(dt.Rows[0]["Exp_Date"]);
          string sexpdate = dtexpdate.ToString("MM/dd/yyyy");
          if (!string.IsNullOrEmpty(sexpdate))
          txtExpiryDate.Text = sexpdate;

       
          txtRemarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);
          txtCompanyName.Text = Convert.ToString(dt.Rows[0]["Company"]);
          txtMobile.Text = Convert.ToString(dt.Rows[0]["Mobile"]);
          txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
          drpPurpose.SelectedValue = Convert.ToString(dt.Rows[0]["Purpose"]);

          drpDepartment.SelectedValue = Convert.ToString(dt.Rows[0]["Aptment_Dept"]);
          drpFloor.SelectedValue = Convert.ToString(dt.Rows[0]["Area_Floor"]);
          drpHost.SelectedValue = Convert.ToString(dt.Rows[0]["Host_to_Visit"]);

          drpLocation.SelectedValue = Convert.ToString(dt.Rows[0]["LocationID"]);
          drpVisitorType.SelectedValue = Convert.ToString(dt.Rows[0]["Visitor_Type"]);
          drpDuration.SelectedValue = Convert.ToString(dt.Rows[0]["Duration"]);


          string sReqStat = Convert.ToString(dt.Rows[0]["Req_Stat"]);

          if (sReqStat == "Approved")
          {
            tabapprej.Style.Add("Display", "none");
           
          }
          else if (sReqStat == "Rejected")
          {
            tabapprej.Style.Add("Display", "none");

          }
          else
          {
            tabapprej.Style.Add("Display", "block");

          }
        }

        DataTable dtImage = ocon.GetTable("select isnull(Image,'') as Image from Visitor_Image_Self where id = '" + visitorID.Value + "'", new DataSet());
        if (dtImage!=null && dtImage.Rows.Count > 0)
        {
          string sIage = Convert.ToString(dtImage.Rows[0]["Image"]);

          if (!string.IsNullOrEmpty(sIage))
          {
            string base64String = Convert.ToBase64String((byte[])dtImage.Rows[0]["Image"]);
            string imageUrl = "data:image/png;base64," + base64String;
            img_PhotoBase64.ImageUrl = imageUrl;
          }
          else
          {
            img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";
          }
        }
        else
        {
          img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";
        }
      }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
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
      //string userName = WindowsIdentity.GetCurrent().Name.ToString();
      //string sApproverID = userName.Substring(userName.LastIndexOf("\\") + 1);

      string sApproverName = "";
      //DataTable dtApp = ocon.GetTable("select pl_Value from PickList_tran where pl_head_id=18 and Other_Data3='" + sApproverID + "' ", new DataSet());
      //if (dtApp.Rows.Count > 0)
      //{
      //  sApproverName = dtApp.Rows[0]["pl_Value"].ToString();
      //}
      if (sCheck == "Pending")
      {
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

        string sHostame = drpHost.SelectedItem.Text;

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



        string sQRCode = sPersonID;
        string sVisitor_Type = drpVisitorType.SelectedItem.Text;
        string sLocationID = drpLocation.SelectedItem.Text;
        StringBuilder sb = new StringBuilder();

        sb.Append("INSERT INTO Scheduling (cId,UserId,Status,Subject,Description,Label,StartTime,EndTime,Location,AllDay,ContactInfo,EntryID,QRCode,Organizer,Loc_ID,HostName,VisPhone,VisCompany,IDType,IDNumber,FloorName,Duration,Visitor_Type,LocationID,Purpose)" + Environment.NewLine);
        sb.Append("VALUES ('" + iCid + "',1,1,'" + txtFullname.Text.Trim() + "','Meeting',1,'" + dtStartTime + "','" + dtEndTime + "','" + drpDepartment.SelectedItem.Text + "',0,'" + txtEmail.Text.Trim() + "','" + iCid + "','" + sQRCode + "','" + sHostEmail + "',1,'" + drpHost.SelectedItem.Text + "','" + txtMobile.Text + "','" + txtCompanyName.Text + "','" + drpIDType.SelectedItem.Text + "','" + txtIDNumber.Text + "','" + drpFloor.SelectedItem.Text + "',"+drpDuration.SelectedValue+",'" + sVisitor_Type + "','" + sLocationID + "','" + drpPurpose.SelectedItem.Text + "')" + Environment.NewLine);

        ocon.Execute(sb.ToString());

        string sSql = "update Scheduling set Image=(select Image from Visitor_Image_Self where ID='" + visitorID.Value + "') where cId='" + iCid + "'";
        ocon.Execute(sSql);

        SendApproverEmail(txtFullname.Text, txtEmail.Text, "Approved", sQRCode, drpHost.SelectedItem.Text, drpDepartment.SelectedItem.Text, txtCompanyName.Text.Trim(), sApproverName);
        //SendApproverEmail(txtFullname.Text, txtEmail.Text, "Approved", sQRCode);

        string sDestURL = string.Format("\"{0}\"", "SelfRegistrationApproval.aspx?tabid=" + sselfID);
        string smessage = string.Format("\"{0}\"", "Visit Request Approved");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        //ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.open('close.html', '_self', null);", true);
        // btnLoad.Attributes.Add("onclick", " CloseWindow();");
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

    private void SendRejectionEmail(string sName, string sEmail, string sStatus, string sQRCode, string sHostName, string sDepartment, string sCompany)
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

      string sApproverName = "";
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
      strHTML = strHTML + "</p>" + sCopanyName + "<p>";


      sendEmail.SendEmails(sName, sEmail, "3434", strHTML, EmailSubject, null, null);

      
    }

    private void SendApproverEmail(string sName, string sEmail, string sStatus, string sQRCode, string sHostName, string sDepartment, string sCompany, string sApprovedBy)
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

      strHTML = strHTML + "<p>Your Meeting request is approved.</p>";
      strHTML = strHTML + "<p>Please visit the nearest reception area.</p>";


      strHTML = strHTML + "</p><p>      Visitor Name : " + sName;
      strHTML = strHTML + "</p><p>      Company : " + sCompany;
      strHTML = strHTML + "</p><p>      Email : " + sEmail;
      strHTML = strHTML + "</p><p>      DateTime : " + DateTime.Now.ToString();

      strHTML = strHTML + "</p><p>      Host Name : " + sHostName;
      strHTML = strHTML + "</p><p>      Purpose : " + drpPurpose.SelectedItem.Text;


      if (sApprovedBy != "")
      {
        strHTML = strHTML + "</p><p>      Approved By : " + sApprovedBy;
      }


      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";

      strHTML = strHTML + @"<img src='cid:" + res.ContentId + @"' width=200 height=200 />";


      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";


      strHTML = strHTML + "</p>Best Regards, <p>";
      strHTML = strHTML + "</p>"+ sCopanyName + "<p>";


      sendEmail.SendEmails(sName, sEmail, "3434", strHTML, EmailSubject, byteImageVis, res);



      #region Host



      #endregion


      #region Cordinator

      

      string SelfAdminEmail = System.Configuration.ConfigurationManager.AppSettings["SelfAdminEmail"];
      //////////////////////Host Email///////////////////////////////////////
      if (SelfAdminEmail == "True")
      {


        string sSqlAdminEmail = "select pl_data,isnull(Other_Data2,'') as Name  from PickList_tran where pl_head_id=4 and pl_value='" + sDepartment + "'";
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



            if (sApprovedBy != "")
            {
              strHTML1 = strHTML1 + "<p>The Below visit request is approved by " + sApprovedBy + "</p>";
            }
            else
            {
              strHTML1 = strHTML1 + "<p>The Below visit request is approved</p>";
            }

            strHTML1 = strHTML1 + "</p><p>      Visitor Name : " + sName;
            strHTML1 = strHTML1 + "</p><p>      Company : " + sCompany;
            strHTML1 = strHTML1 + "</p><p>      Email : " + sEmail;
            strHTML1 = strHTML1 + "</p><p>      Purpose : " + drpPurpose.SelectedItem.Text;

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

    public byte[] ImageControlToByteArray(System.Web.UI.WebControls.Image image)
    {
      try
      {
        // Attempt to find the Image that the control points to
        // and read all of it's bytes
        return File.ReadAllBytes(image.ImageUrl);
      }
      catch (Exception)
      {
        // Uh oh, there was a problem reading the file
        return new byte[0];
      }
    }



    private void BindDropDown()
    {
      #region company


      DataTable dtcompany = ocon.GetTable("SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=23 order by pl_Value", new DataSet());
      if (dtcompany.Rows.Count > 0)
      {
        drpLocation.DataSource = dtcompany;
        drpLocation.DataTextField = "pl_value";
        drpLocation.DataValueField = "pl_value";
        drpLocation.DataBind();
        drpLocation.Items.Insert(0, "Select");
      }
      else
      {
        drpLocation.DataSource = null;
        drpLocation.DataBind();
        drpLocation.Items.Insert(0, "Select");
      }
      #endregion

      #region Department

      string sSqlDepartment = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=4 order by pl_Value";

      DataTable dtDepartment = ocon.GetTable(sSqlDepartment, new DataSet());
      if (dtDepartment.Rows.Count > 0)
      {
        drpDepartment.DataSource = dtDepartment;
        drpDepartment.DataTextField = "pl_value";
        drpDepartment.DataValueField = "pl_value";
        drpDepartment.DataBind();
        drpDepartment.Items.Insert(0, "Select");
      }
      else
      {
        drpDepartment.DataSource = null;
        drpDepartment.DataBind();
        drpDepartment.Items.Insert(0, "Select");
      }
      #endregion

      #region Floor

      string sSqlFloor = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=9 order by pl_Value";

      DataTable dtFloor = ocon.GetTable(sSqlFloor, new DataSet());
      if (dtFloor.Rows.Count > 0)
      {
        drpFloor.DataSource = dtFloor;
        drpFloor.DataTextField = "pl_value";
        drpFloor.DataValueField = "pl_value";
        drpFloor.DataBind();
        drpFloor.Items.Insert(0, "Select");
      }
      else
      {
        drpFloor.DataSource = null;
        drpFloor.DataBind();
        drpFloor.Items.Insert(0, "Select");
      }
      #endregion

      #region Host

      string sSqlHost = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=18 order by pl_Value";

      DataTable dtHost = ocon.GetTable(sSqlHost, new DataSet());
      if (dtHost.Rows.Count > 0)
      {
        drpHost.DataSource = dtHost;
        drpHost.DataTextField = "pl_value";
        drpHost.DataValueField = "Pl_id";
        drpHost.DataBind();
        drpHost.Items.Insert(0, "Select");
      }
      else
      {
        drpHost.DataSource = null;
        drpHost.DataBind();
        drpHost.Items.Insert(0, "Select");
      }
      #endregion

      #region VisitorType

      string sSqlVisitorType = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=1 order by pl_Value";

      DataTable dtVisitorType = ocon.GetTable(sSqlVisitorType, new DataSet());
      if (dtVisitorType.Rows.Count > 0)
      {
        drpVisitorType.DataSource = dtVisitorType;
        drpVisitorType.DataTextField = "pl_value";
        drpVisitorType.DataValueField = "pl_value";
        drpVisitorType.DataBind();
        drpVisitorType.Items.Insert(0, "Select");
      }
      else
      {
        drpVisitorType.DataSource = null;
        drpVisitorType.DataBind();
        drpVisitorType.Items.Insert(0, "Select");
      }
      #endregion

      #region Duration

      string sSqlDuration = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=6 order by pl_Value";

      DataTable dtDuration = ocon.GetTable(sSqlDuration, new DataSet());
      if (dtDuration.Rows.Count > 0)
      {
        drpDuration.DataSource = dtDuration;
        drpDuration.DataTextField = "pl_value";
        drpDuration.DataValueField = "Pl_id";
        drpDuration.DataBind();
        drpDuration.Items.Insert(0, "Select");
      }
      else
      {
        drpDuration.DataSource = null;
        drpDuration.DataBind();
        drpDuration.Items.Insert(0, "Select");
      }
      #endregion

      #region Purpose

      string sSqlPurpose = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=10 order by pl_Value";

      DataTable dtPurpose = ocon.GetTable(sSqlPurpose, new DataSet());
      if (dtPurpose.Rows.Count > 0)
      {
        drpPurpose.DataSource = dtPurpose;
        drpPurpose.DataTextField = "pl_value";
        drpPurpose.DataValueField = "pl_value";
        drpPurpose.DataBind();
        drpPurpose.Items.Insert(0, "Select");
      }
      else
      {
        drpPurpose.DataSource = null;
        drpPurpose.DataBind();
        drpPurpose.Items.Insert(0, "Select");
      }
      #endregion
    }

    protected void btnWatchlist_Click(object sender, EventArgs e)
    {

      string sselfID = "";
      sselfID = Request.QueryString["tabid"];
      string sApprovedBy = Request.QueryString["eid"];



      string sCheck = "";


      DataTable dtCheck = ocon.GetTable("SELECT Req_Stat from SecuLobby_VisitingDetails_Self where Ref_No='" + sselfID + "'", new DataSet());
      if (dtCheck.Rows.Count > 0)
      {
        sCheck = dtCheck.Rows[0]["Req_Stat"].ToString();
      }
      if (sCheck == "Pending")
      {
        string sSqlUpdate = "UPDATE SecuLobby_VisitingDetails_Self SET Req_Stat='Rejected',Rej_reason='" + txtWatchlistReason.Text.Trim() + "', Approvedby='" + sApprovedBy + "' where Ref_No='" + sselfID + "'";

        ocon.Execute(sSqlUpdate);

        //SendEmail(txtFullname.Text, txtEmail.Text, "Rejected", "");
        SendRejectionEmail(txtFullname.Text, txtEmail.Text, "Approved", "", drpHost.SelectedItem.Text, drpDepartment.SelectedItem.Text, txtCompanyName.Text.Trim());

        string sDestURL = string.Format("\"{0}\"", "SelfRegistrationApproval.aspx?tabid=" + sselfID);
        string smessage = string.Format("\"{0}\"", "Visit Request Rejected");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
    }

    private string GetEmailBody(string Template)
    {
      string sVisitorname = txtFullname.Text.Trim();
      string sIDType = drpIDType.SelectedItem.Text;
      string sIDnumber = txtIDNumber.Text.Trim();
      string sExpirydate = txtExpiryDate.Text;

      string sVisitorCompany = txtCompanyName.Text.Trim();
      string sVisitorEmail = txtEmail.Text.Trim();
      string sVisitorPhone = txtMobile.Text.Trim();

      string sDepartment = drpDepartment.SelectedItem.Text;
      string sHostName = drpHost.SelectedItem.Text;
      string sPurpose = drpPurpose.SelectedItem.Text;
      string sVisitorType = drpVisitorType.SelectedItem.Text;
      string sDuration = drpDuration.SelectedItem.Text;
    

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
      

      return Template;
    }
  }
}
