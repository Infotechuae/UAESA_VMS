using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class SelfRegistration : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    ResourceManager rm;
    CultureInfo ci;
    public static string stcLocID = "";
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
    protected void Page_Init(object sender, EventArgs e)
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
      Response.Cache.SetNoStore();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      //Response.Cache.SetCacheability(HttpCacheability.NoCache);
      //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
      //Response.Cache.SetNoStore();

      //Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
      //Response.Expires = -1500;
      //Response.CacheControl = "no-cache";
      Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      if (!Page.IsPostBack)
      {
        if (Session["START"] == null)
        {
          Session.Clear();
          Session.Abandon();
          Response.Redirect("LoginSelfRegistration.aspx");
        }
        else
        {

          string sLang = Convert.ToString(Session["Lang"]);
          //string sUserID = Convert.ToString(Session["UserID"]);

          img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";
          BindDropDown();
          //Session["ImgByte"] = null;
          //string sselfID = "";
          //sselfID = Convert.ToString(Session["selfid"]);


          btnCheckin.Visible = true;
          //btnWatchlist.Visible = false;

          //Session["ImgByte"] = null;
          //Session["FrontSide_img"] = null;
          //Session["BackSide_img"] = null;


          Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
          rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
          ci = Thread.CurrentThread.CurrentCulture;
          Loadstring(ci);
        }
      }
    }
    private void Loadstring(CultureInfo ci)
    {

      header1.Text = rm.GetString("STRN_15", ci);
      btnLoad.Text = rm.GetString("STR_17", ci);
      btnCheckin.Text = rm.GetString("STR_18", ci);
      btnReset.Text = rm.GetString("STR_19", ci);

      lblFullName.Text = rm.GetString("STR_2", ci);
      lblIDNumber.Text = rm.GetString("STR_3", ci);
      lblExpiryDate.Text = rm.GetString("STR_4", ci);

      lblCompanyName.Text = rm.GetString("STR_5", ci);
      lblMobileNumber.Text = rm.GetString("STR_6", ci);
      lblEmail.Text = rm.GetString("STR_7", ci);

      //lblNationality.Text = rm.GetString("STR_8", ci);
      //lblGender.Text = rm.GetString("STR_9", ci);



      lblDepartment.Text = rm.GetString("STR_11", ci);
      lblFloor.Text = rm.GetString("STR_12", ci);
      lblHost.Text = rm.GetString("STR_13", ci);

      lblVisitorType.Text = rm.GetString("STR_14", ci);
      lblDuration.Text = rm.GetString("STR_15", ci);
      lblRemarks.Text = rm.GetString("STR_16", ci);
      Label15.Text = rm.GetString("STR_61", ci);
    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
      Response.Redirect("MasterDashboard.aspx");
    }
    protected void btnLoad_Click(object sender, EventArgs e)
    {
      btnCheckin.Enabled = true;
      //string sLocation_ID = Session["Loc_ID"].ToString();
      string sLocation_ID = Session["Loc_ID"].ToString();

      string sSql = "SELECT [ID],[FullName] ,[DOB],[DOC_Number],[Nationality],[Photo],[FrontSide_img],[BackSide_img],[Doc_Type],[Sex],[Arabic_Name],[Company],[Designation],[Mobile], emailID, Exp_Date FROM[dbo].[Doc_Data] where[Location] ='" + sLocation_ID + "'";

      DataTable DT = ocon.GetTable(sSql, new DataSet());

      if (DT.Rows.Count > 0)
      {

        txtFullname.Text = DT.Rows[0]["FullName"].ToString().Replace(",", " ");
        drpIDType.SelectedValue = "Emirates ID";
        txtIDNumber.Text = DT.Rows[0]["DOC_Number"].ToString();

        if (DT.Rows[0]["Exp_Date"].ToString() != "")
          txtExpiryDate.Text = Convert.ToDateTime(DT.Rows[0]["Exp_Date"]).ToString("MM/dd/yyyy");
        else
          txtExpiryDate.Text = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");


        txtCompanyName.Text = DT.Rows[0]["Company"].ToString();
        txtMobile.Text = DT.Rows[0]["Mobile"].ToString();
        txtEmail.Text = DT.Rows[0]["EmailID"].ToString();

        txtCompanyName.Text = DT.Rows[0]["Company"].ToString();
        txtMobile.Text = DT.Rows[0]["Mobile"].ToString();
        txtEmail.Text = DT.Rows[0]["EmailID"].ToString();





        if (DT.Rows[0]["Photo"].ToString().Length == 0)
        {
          img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";
        }
        else
        {
          string base64String = Convert.ToBase64String((byte[])DT.Rows[0]["Photo"]);
          string imageUrl = "data:image/png;base64," + base64String;
          img_PhotoBase64.ImageUrl = imageUrl;
          Session["ImgByte"] = (byte[])DT.Rows[0]["Photo"];

        }
      

        try
        {
          string sSqlDelete = "DELETE Doc_Data WHERE DOC_Number='" + DT.Rows[0]["DOC_Number"].ToString() + "'";
          ocon.Execute(sSqlDelete);

          string sSqlDelete1 = "DELETE Doc_Flag";
          ocon.Execute(sSqlDelete1);
        }
        catch(Exception ex)
        { }


        if (txtIDNumber.Text.Length > 2)
        {

          GetVisitor_details("", "", "", txtIDNumber.Text, "EID");
          Session["Ref_No"] = txtIDNumber.Text;
        }
      }
      else
      {
        txtFullname.Text = "";
        txtIDNumber.Text = "";
        txtExpiryDate.Text = "";
        txtCompanyName.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";
        BindDropDown();
        Session["ImgByte"] = null;
        Session["FrontSide_img"] = null;
        Session["BackSide_img"] = null;
      }
    }
    public string GetNationMasterData(string nationCode)
    {
      string NationName = DAL.Utils.fetchString("pl_Value", "PickList_tran", "pl_head_id=2 and (pl_Data='" + nationCode + "' or pl_value='" + nationCode + "')", MyConnection.ReadConStr("Local"));

      if (NationName.Length == 0)
        NationName = nationCode;
      return NationName;
    }
    protected void btnCheckin_Click(object sender, EventArgs e)
    {


      if (txtFullname.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Name');", true);
        return;
      }
      if (drpIDType.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select ID Type');", true);
        return;
      }
      if (txtIDNumber.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter ID Number');", true);
        return;
      }
      if (txtExpiryDate.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Expiry Date');", true);
        return;
      }
      if (txtCompanyName.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Company Name');", true);
        return;
      }

      if (txtMobile.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Mobile Number');", true);
        return;
      }
      if (txtEmail.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Email');", true);
        return;
      }
      //added location by swati in sept24
      if (drpLocation.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Location');", true);
        return;
      }
      if (drpDepartment.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Department');", true);
        return;
      }
      //if (drpFloor.SelectedItem.Text == "Select")
      //{
      //  ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Floor');", true);
      //  return;
      //}
      if (drpHost.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Host');", true);
        return;
      }
      if (drpVisitorType.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Visitor Type');", true);
        return;
      }
      if (drpDuration.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Duration');", true);
        return;
      }
      if (txtIDNumber.Text.Length > 2)
      {
        DateTime dtow = DateTime.Now;
        if (Convert.ToDateTime(txtExpiryDate.Text.Trim()) < dtow)
        {
          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Your ID is Expired');", true);
          return;
        }
        else
        {

          SaveVisitorData();

          try
          {
            string sSqlDelete = "DELETE Doc_Data WHERE DOC_Number='" + txtIDNumber.Text.Trim() + "'";
            ocon.Execute(sSqlDelete);

            string sSqlDelete1 = "DELETE Doc_Flag";
            ocon.Execute(sSqlDelete1);
          }
          catch (Exception ex)
          { }


          #region Email

          //////////////////////Host Email///////////////////////////////////////
          ///
          string ServiceLevel = "";
          bool ApprovalRequired = false;
          bool HostApproval = false;
          bool LineManagerApproval = false;
          string sHostEmail = "";
          string sManagerName = "";
          string sManagerEmail = "";

          string sSql = "select isnull(ApprovalRequired,0) as ApprovalRequired,isnull(HostApproval,0) as HostApproval,isnull(LineManagerApproval,0) as LineManagerApproval,isnull(ServiceLevel,'') as ServiceLevel from ApprovalSettings inner join PickList_tran pt on pt.pl_Value = '" + drpVisitorType.SelectedItem.Text + "' where MenuID = 26";
          DataTable dt = ocon.GetTable(sSql, new DataSet());
          if (dt.Rows.Count > 0)
          {
            ApprovalRequired = (bool)dt.Rows[0]["ApprovalRequired"];
            HostApproval = (bool)dt.Rows[0]["HostApproval"];
            LineManagerApproval = (bool)dt.Rows[0]["LineManagerApproval"];
            ServiceLevel = Convert.ToString(dt.Rows[0]["ServiceLevel"]);
          }

          string sHostame = drpHost.SelectedItem.Text;
          string sSqlHostEmail = "select isnull(pl_data,'') as pl_data,isnull(Other_Data2,'') as Other_Data2 from PickList_tran where pl_head_id=18 and pl_value='" + sHostame + "'";
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
            string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Pre Registration' and Email_For='Request With Approval'";

            DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());

            if (dtEmailtemp.Rows.Count > 0)
            {
              foreach (DataRow dr in dtEmailtemp.Rows)
              {
                string sEmailStage = dr["EmailStage"].ToString();
                string EmailSubject = dr["Email_Sub"].ToString();
                string strHTML = dr["Email_Body"].ToString();
                strHTML = GetEmailBody(strHTML);

                if (sEmailStage == "Requester")
                {
                  if (!string.IsNullOrEmpty(txtEmail.Text))
                  {
                    sendEmail.SendEmails(txtFullname.Text, txtEmail.Text, "02", strHTML, EmailSubject, null, null);
                  }
                }
                if (HostApproval == true)
                {
                  if (sEmailStage == "Host")
                  {
                    if (!string.IsNullOrEmpty(sHostEmail))
                    {
                      sendEmail.SendEmails(sHostame, sHostEmail, "02", strHTML, EmailSubject, null, null);
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
            string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Pre Registration' and Email_For='Request Without Approval'";

            DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());

            if (dtEmailtemp.Rows.Count > 0)
            {
              foreach (DataRow dr in dtEmailtemp.Rows)
              {
                string sEmailStage = dr["EmailStage"].ToString();
                string EmailSubject = dr["Email_Sub"].ToString();
                string strHTML = dr["Email_Body"].ToString();
                strHTML = GetEmailBody(strHTML);
                if (sEmailStage == "Requester")
                {
                  if (!string.IsNullOrEmpty(txtEmail.Text))
                  {
                    sendEmail.SendEmails(txtFullname.Text, txtEmail.Text, "02", strHTML, EmailSubject, null, null);
                  }
                }

                if (sEmailStage == "Host")
                {
                  if (!string.IsNullOrEmpty(sHostEmail))
                  {
                    sendEmail.SendEmails(sHostame, sHostEmail, "02", strHTML, EmailSubject, null, null);
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


          #endregion


          string sDestURL = string.Format("\"{0}\"", "SelfRegistration.aspx");
          string smessage = string.Format("\"{0}\"", "Registration Successful.<br/>Please wait for the approval");

          string sVar = sDestURL + "," + smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
      }
      else
      {

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Proper ID Number');", true);
        return;
      }
    }

    public void SaveVisitorData()
    {

      string badgeNo = "";
      btnCheckin.Enabled = false;
      try
      {
        SqlCommand cmdOrl = new SqlCommand("Insert_VisitorInfo_Self");
        cmdOrl.CommandType = CommandType.StoredProcedure;
        cmdOrl.Parameters.AddWithValue("@Name", txtFullname.Text);
        cmdOrl.Parameters.AddWithValue("@Company", txtCompanyName.Text);
        cmdOrl.Parameters.AddWithValue("@Designation", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@Nationality", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@EmiratesID", txtIDNumber.Text);
        cmdOrl.Parameters.AddWithValue("@Gender", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@Mobile", txtMobile.Text);
        cmdOrl.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmdOrl.Parameters.AddWithValue("@Aptment_Dept", drpDepartment.SelectedItem.Text);
        cmdOrl.Parameters.AddWithValue("@Visitor_Type", drpVisitorType.SelectedItem.Text);
        cmdOrl.Parameters.AddWithValue("@Access_Card_No", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@Host_to_Visit", drpHost.SelectedValue);
        cmdOrl.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        cmdOrl.Parameters.AddWithValue("@Purpose", drpPurpose.SelectedItem.Text);
        cmdOrl.Parameters.AddWithValue("@Duration", drpDuration.SelectedItem.Value);
        cmdOrl.Parameters.AddWithValue("@Area_Floor", drpFloor.SelectedItem.Text);
        cmdOrl.Parameters.AddWithValue("@Card_Code", badgeNo);
        cmdOrl.Parameters.AddWithValue("@Belonging", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@Services", "");
        cmdOrl.Parameters.AddWithValue("@Vehicle_Code", "");
        cmdOrl.Parameters.AddWithValue("@Vehicle_Type", "");
        cmdOrl.Parameters.AddWithValue("@Vehicle_No", "");
        cmdOrl.Parameters.AddWithValue("@LocationID", drpLocation.SelectedItem.Text);
        DateTime creationDate;
        if (txtExpiryDate.Text == null)
        {
          txtExpiryDate.Text = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
        }
        cmdOrl.Parameters.AddWithValue("@Exp_date", Convert.ToDateTime(txtExpiryDate.Text).ToString("yyyy-MM-dd"));


        cmdOrl.Parameters.AddWithValue("@DOB", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@DOC_Type", drpIDType.SelectedValue);



        cmdOrl.Parameters.AddWithValue("@Meeting_Start_Time", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@Meeting_End_Time", DBNull.Value);


        cmdOrl.Parameters.AddWithValue("@Checkin_Time", System.DateTime.Now);
        cmdOrl.Parameters.AddWithValue("@CheckOut_Time", DBNull.Value);
        cmdOrl.Parameters.AddWithValue("@Status", "CIN");
        cmdOrl.Parameters.AddWithValue("@Loc_ID", "1");

        //cmdOrl.Parameters.AddWithValue("@Add_To_WatchList", txt_Nationality.Text);
        //cmdOrl.Parameters.AddWithValue("@Reason_To_Add_Watchlist", txt_EID.Text);
        byte[] img_Photoblob = null;
        byte[] FrontSide_Photoblob = null;
        byte[] BackSide_Photoblob = null;
        ImageConverter imgConverter = new ImageConverter();
        try
        {
          if (Session["ImgByte"] == null)
          {
            img_Photoblob = null;
          }
          else
          {
            img_Photoblob = (System.Byte[])Session["ImgByte"];
          }

          FrontSide_Photoblob = null;
          BackSide_Photoblob = null;


        }
        catch (Exception)
        {
          img_Photoblob = null;
        }
        SqlParameter imageParameter = new SqlParameter("@Image", SqlDbType.Image);
        imageParameter.Value = (object)img_Photoblob;
        cmdOrl.Parameters.Add(imageParameter);


        SqlParameter FrontSideimageParameter = new SqlParameter("@FrontSideImage", SqlDbType.Image);
        FrontSideimageParameter.Value = (object)FrontSide_Photoblob;
        cmdOrl.Parameters.Add(FrontSideimageParameter);

        SqlParameter BackSideimageParameter = new SqlParameter("@BackSideImage", SqlDbType.Image);
        BackSideimageParameter.Value = (object)BackSide_Photoblob;
        cmdOrl.Parameters.Add(BackSideimageParameter);

        DAL.DALSQL dbs1 = new DAL.DALSQL();
        string rval = dbs1.ExecuteStoredProcedurereturn(cmdOrl, MyConnection.ReadConStr("Local"));

       



      }
      catch (Exception ex)
      {
        throw ex;
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

    //private void SendEmail(string sName, string sEmail,string sID,string Stype)
    //{
    //  DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

    //  string EmailMask = System.Configuration.ConfigurationManager.AppSettings["EmailMask"];
    //  string EmailEnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"];
    //  string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
    //  string EmailSubject = "Visitor Meeting Request";

    //  string EmailID = "";
    //  string EmailPassword = "";
    //  string EmailSMTP = "";
    //  string EmailPort = "";
    //  string AccountName = "";
    //  string _FooterText = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(0, HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf("/") + 1) + "SelfRegistrationApproval.aspx?tabid=" + sID;


    //  string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,AccountName FROM MailSettings";
    //  DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());
    //  if (dtMailSetting.Rows.Count > 0)
    //  {
    //    AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
    //    EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
    //    EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
    //    EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
    //    EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
    //    EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);


    //    MailMessage msg = new MailMessage();
    //    msg.To.Add(new MailAddress(Convert.ToString(sEmail)));

    //    msg.From = new MailAddress(EmailID, EmailMask);
    //    msg.Subject = EmailSubject;
    //    string strHTML = "<p> Dear " + sName + ",</p>";
    //    if (Stype=="Host")
    //    strHTML = strHTML + "<p> Visitor wants to meet you .</p>";
    //    else
    //      strHTML = strHTML + "<p> Visitor wants to meet your department person .</p>";

    //    strHTML = strHTML + "</p><p>      <em>Visitor Name    :</em>" + txtFullname.Text;
    //    strHTML = strHTML + "</p><p>      <em>Company    :</em>" + txtCompanyName.Text;
    //    strHTML = strHTML + "</p><p>      <em>Email    :</em>" + txtEmail.Text;
    //    strHTML = strHTML + "</p><p>      <em>Phone No    :</em>" + txtMobile.Text;

    //    strHTML = strHTML + "</p><p>";



    //     strHTML = strHTML + "<p>Please approve or reject this meeting through the below link link.</p>";
    //    strHTML = strHTML + "<a href='" + _FooterText + "' target = '_blank' ><b> Click Here </b></a>";

    //    strHTML = strHTML + " <table cellspacing=" + "0" + " cellpadding=" + "0" + "><tr><td align=" + "center" + " width=" + "300" + " height=" + "40" + " bgcolor=" + "#000091" + " style=" + "-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;" + ">   </span></a></td></tr></table>";


    //    msg.IsBodyHtml = true;
    //    ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
    //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
    //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

    //    if (DisableEmailWS == "True")
    //    {
    //      msg.Body = strHTML;
    //      msg.IsBodyHtml = true;

    //      SmtpClient client = new SmtpClient();
    //      client.Port = Convert.ToInt16(EmailPort);
    //      client.Host = EmailSMTP;
    //      NetworkCredential basicCredential = new NetworkCredential(AccountName, EmailPassword);
    //      client.Credentials = basicCredential;
    //      client.Timeout = (60 * 5 * 1000);

    //      client.DeliveryMethod = SmtpDeliveryMethod.Network;
    //      client.EnableSsl = Convert.ToBoolean(EmailEnableSsl);
    //      try
    //      {
    //        client.Send(msg);
    //      }
    //      catch (Exception ex)
    //      { }

    //      //ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
    //      //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
    //      //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

    //      //SmtpClient client = new SmtpClient();
    //      //client.UseDefaultCredentials = false;
    //      //client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
    //      //client.Port = Convert.ToInt16(EmailPort);
    //      //client.Host = EmailSMTP;
    //      //client.DeliveryMethod = SmtpDeliveryMethod.Network;
    //      //client.EnableSsl = Convert.ToBoolean(EmailEnableSsl);
    //      ////   client.UseDefaultCredentials = false;
    //      //// You can use Port 25 if 587 is blocked (mine is!)
    //      //client.TargetName = "STARTTLS/smtp.office365.com";
    //      //client.Timeout = 600000;


    //    }
    //  }
    //}

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

    public void GetVisitor_details(string VisitorID, string Name, string PhoneNo, string EID, string Type)
    {

      DataSet dsVistor = new DataSet();
      DBSQL db = new DBSQL();//declaring class
      DataTable DT = new DataTable();

      DataTable DIOT = new DataTable();
      DataTable DTimage = new DataTable();

      string[,] infoArray = new string[5, 2];
      infoArray[0, 0] = "Name";
      infoArray[0, 1] = Name.ToString();
      infoArray[1, 0] = "PhoneNo";
      infoArray[1, 1] = PhoneNo.ToString();
      infoArray[2, 0] = "EID";
      infoArray[2, 1] = EID.ToString();
      infoArray[3, 0] = "Loc_ID";
      infoArray[3, 1] = "1";
      infoArray[4, 0] = "VisitorID";
      infoArray[4, 1] = VisitorID;


      dsVistor = db.GetDataset("getVisitorDetails", infoArray, MyConnection.ReadConStr("Local"));
      DT = dsVistor.Tables[0];
      DataTable DDT = dsVistor.Tables[1];
      if (dsVistor.Tables.Count > 3)
        DIOT = dsVistor.Tables[2];
      if (dsVistor.Tables.Count > 3)
        DTimage = dsVistor.Tables[3];

      if (DT.Rows.Count > 0)
      {
        //  if (Type == "EID")
        //  {

        //  }
        //  else
        //  {
        foreach (DataRow row in DT.Rows)
        {
          //  Session["Ref_No"] = row["Ref_No"].ToString();
          //hdn_RefNo.Value = row["Ref_No"].ToString();

          txtFullname.Text = row["Name"].ToString().Replace(",", " ");
          txtCompanyName.Text = row["Company"].ToString();




          txtIDNumber.Text = row["EmiratesID"].ToString();
          Session["Ref_No"] = txtIDNumber.Text;
          txtEmail.Text = row["Email"].ToString();
          txtMobile.Text = row["Mobile"].ToString();




          Session["DOB"] = row["DOB"].ToString();
          drpIDType.SelectedValue = row["Doc_type"].ToString();

          DateTime readdate;
          bool Changedata = false;
          DateTime.TryParseExact(txtExpiryDate.Text, sDateFmt,
                                   CultureInfo.InvariantCulture, DateTimeStyles.None,
                                   out readdate);
          if (readdate < DateTime.Now)
          {
            Changedata = true;
          }

          //DateTime creationDate;
          //if (DateTime.TryParseExact(row["Exp_Date"].ToString(), sDateFmt,
          //                           CultureInfo.InvariantCulture, DateTimeStyles.None,
          //                           out creationDate))
          //{
          //  txtExpiryDate.Text = creationDate.ToString();
          //}
          //else
          //{
          //  string expdate = Convert.ToDateTime(row["Exp_Date"]).ToString("MM/dd/yyyy");

          //  txtExpiryDate.Text = expdate;
          //}





          // string imageUrl = null;
          //if (DTimage.Rows.Count > 0)
          //{
          //  Session["ImgByte"] = null;
          //  foreach (DataRow imgrow in DTimage.Rows)
          //  {


          //    if (imgrow["iType"].ToString() == "1")
          //    {

          //      if (Session["ImgByte"] != null  )
          //      {

          //      }
          //      else
          //      {
          //        if (imgrow["Image"].ToString().Length == 0)
          //        {
          //          img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";

          //        }
          //        else
          //        {
          //          string base64String = Convert.ToBase64String((byte[])imgrow["Image"]);
          //          string imageUrl = "data:image/png;base64," + base64String;
          //          img_PhotoBase64.ImageUrl = imageUrl;
          //          Session["ImgByte"] = (byte[])imgrow["Image"];

          //        }
          //      }
          //    }



          //    if (imgrow["iType"].ToString() == "2")
          //    {
          //      if (imgrow["Image"].ToString().Length == 0)
          //      {
          //        Session["FrontSide_img"] = null;

          //      }
          //      else
          //      {
          //        Session["FrontSide_img"] = (byte[])imgrow["Image"];

          //      }
          //    }
          //    if (imgrow["iType"].ToString() == "3")
          //    {
          //      if (imgrow["Image"].ToString().Length == 0)
          //      {


          //        Session["BackSide_img"] = null;
          //      }
          //      else
          //      {


          //        Session["BackSide_img"] = (byte[])imgrow["Image"];
          //      }
          //    }
          //  }
          //}

        }

        //}

        if (DDT.Rows.Count > 0)
        {
          Session["VisitorID"] = VisitorID;

        }

        btnCheckin.Visible = true;

        if (DIOT.Rows.Count > 0)
        {
          foreach (DataRow IOrow in DIOT.Rows)
          {
            string status = IOrow["Checkin_Status"].ToString();

            btnCheckin.Visible = true;

          }
        }
        else
        {
          btnCheckin.Visible = true;

        }

        DataSet dtWatchlistCheckin;
        dtWatchlistCheckin = DAL.Utils.fetchDSQueryRecordsSP(txtIDNumber.Text, "1", "EID", "ValidateCheckinVisitor", MyConnection.ReadConStr("Local"));


        if (dtWatchlistCheckin.Tables[1].Rows.Count > 0)
        {
          foreach (DataRow brow in dtWatchlistCheckin.Tables[1].Rows)
          {
            string sMessage = "Visitor Comapny in Watch List, Company Name: " + brow["Company"].ToString();

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('" + sMessage + "');", true);
            return;




          }
          //ClearCardData();
          //return;
        }
        if (dtWatchlistCheckin.Tables[0].Rows.Count > 0)
        {
          foreach (DataRow brow in dtWatchlistCheckin.Tables[0].Rows)
          {
            string sMessage = "Visitor in Watch List, Reason: " + brow["Reason_To_Add_Watchlist"].ToString();

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('" + sMessage + "');", true);
            return;

          }
          //ClearCardData();
          //return;
        }
      }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
      Response.Redirect("SelfRegistration.aspx");
      btnCheckin.Enabled = true;
    }

    protected void txtIDNumber_TextChanged(object sender, EventArgs e)
    {
      GetVisitor_details("", "", "", txtIDNumber.Text.Trim(), "EID");
      Session["Ref_No"] = txtIDNumber.Text.Trim();
    }

    private void BindDropDown()
    {

      #region Location


      DataTable dtcompany = ocon.GetTable("SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=23 order by pl_Value", new DataSet());
      if (dtcompany.Rows.Count > 0)
      {
        drpLocation.DataSource = dtcompany;
        drpLocation.DataTextField = "pl_value";
        drpLocation.DataValueField = "pl_id";
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

      string sSqlLocation = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=4 order by pl_Value";

      DataTable dtLocation = ocon.GetTable(sSqlLocation, new DataSet());
      if (dtLocation.Rows.Count > 0)
      {
        drpDepartment.DataSource = dtLocation;
        drpDepartment.DataTextField = "pl_value";
        drpDepartment.DataValueField = "pl_id";
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

    protected void CompanySelectedChange(object sender, EventArgs e)
    {
      FillDeptCombo(drpLocation.SelectedItem.Value);
    }
    protected void FillDeptCombo(string CompanyName)
    {
      if (string.IsNullOrEmpty(CompanyName)) return;
      BindCombosArea(drpDepartment, "Department", CompanyName);
      BindCombosArea(drpFloor, "Area", CompanyName);
    }
    //protected void DepartmentSelectedChange(object sender, EventArgs e)
    //{
    //  FillFloorCombo(drpDepartment.SelectedItem.Value);
    //}
    //protected void FillFloorCombo(string DeptName)
    //{
    //  if (string.IsNullOrEmpty(DeptName)) return;
    //  BindCombosArea(drpFloor, "Area", DeptName);

    //}
    private void BindCombosArea(DropDownList combos, string CmbName, string Value1)
    {
      string selectSQL = "";
      try
      {

        selectSQL = "SELECT  Pl_id as Pl_id ,pl_Value pl_value FROM dbo.Picklist_hd INNER JOIN dbo.PickList_tran ON dbo.Picklist_hd.ID = dbo.PickList_tran.pl_head_id where   PL_Header='" + CmbName + "'  and  Card_next_ID='" + Value1 + "'    ";

        DataTable dt = ocon.GetTable(selectSQL, new DataSet());

        //DataSet cmbDS;

        //SqlDataAdapter Sqlcmb = new SqlDataAdapter();
        //SqlConnection sConncmb = new SqlConnection(MyConnection.ReadConStr("Local"));
        //SqlCommand selectcmb = new SqlCommand(selectSQL, sConncmb);
        //Sqlcmb.SelectCommand = selectcmb;

        //cmbDS = new DataSet();
        //Sqlcmb.Fill(cmbDS, CmbName);


        if (dt.Rows.Count > 0)
        {
          combos.DataSource = dt;
          combos.DataBind();
          combos.DataValueField = "Pl_id";
          combos.DataTextField = "pl_value";
          combos.Items.Insert(0, "Select");


        }
        else
        {

          combos.DataSource = null;
          combos.DataBind();
          combos.Items.Insert(0, "Select");
        }
        //Sqlcmb = null;
        //sConncmb = null;
        //selectcmb = null;
      }

      catch (Exception ex)
      {
        throw ex;
      }
    }

    protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (drpDepartment.SelectedItem.Text != "Select")
      {
        string sSql = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=18 AND Other_Data1='" + drpDepartment.SelectedItem.Text + "' order by pl_Value";

        DataTable dtHost = ocon.GetTable(sSql, new DataSet());

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
      string sRemarks = txtRemarks.Text;


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

      return Template;
    }
  }
}
