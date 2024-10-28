using DAL;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

//using SecuLobby.App_Code;
namespace SecuLobbyVMS.App_Code
{
  public class Utils
  {
    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";

    public static Random a = new Random(DateTime.Now.Ticks.GetHashCode());
    static HttpContext Context { get { return HttpContext.Current; } }

    //public static string EmailMask = System.Configuration.ConfigurationManager.AppSettings["EmailMask"];
    //public static string EmailID = System.Configuration.ConfigurationManager.AppSettings["EmailUserName"];
    //public static string EmailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];
    //public static string EmailSMTP = System.Configuration.ConfigurationManager.AppSettings["EmailSMTP"];
    //public static string EmailSubject = System.Configuration.ConfigurationManager.AppSettings["EmailSubject"];
    //public static string EmailPort = System.Configuration.ConfigurationManager.AppSettings["EmailPort"];
    //public static string EmailEnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"];
    //public static string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
    //Fetching Records having ID and Prcedure Name
    const string
        CurrentDemoKey = "DXCurrentDemo",
        CurrentThemeCookieKeyPrefix = "DXCurrentTheme",
        DefaultTheme = "Metropolis",
        BogusDemoTitle = "Delivered!";

    //   static readonly Dictionary<DemoModel, IEnumerable<SourceCodePage>> sourceCodeCache = new Dictionary<DemoModel, IEnumerable<SourceCodePage>>();

    const string PROC_NAME = "GENERAL_SP_READ";
    static HttpRequest Request
    {
      get
      {
        return Context.Request;
      }
    }
    public static string CurrentThemeCookieKey
    {
      get
      {
        return CurrentThemeCookieKeyPrefix;
      }
    }
    public static void setAppConfig(string key, string Val)
    {
      try
      {
        System.Configuration.Configuration config =
         System.Configuration.ConfigurationManager.OpenExeConfiguration
                               (System.Configuration.ConfigurationUserLevel.None);

        config.AppSettings.Settings.Add(key, Val);
        config.Save();
        // System.Configuration.ConfigurationManager.RefreshSection(sectionName);
      }
      catch (Exception e)
      {
        Console.WriteLine("[CreateAppSettings: {0}]",
                          e.ToString());

      }
    }
    public static void ApplyTheme(Page page)
    {
      var themeName = CurrentTheme;
      if (string.IsNullOrEmpty(themeName))
        themeName = "Default";
      page.Theme = themeName;
    }

    public static string getTheme()
    {
      var themeName = CurrentTheme;
      if (string.IsNullOrEmpty(themeName))
        themeName = "Default";
      return themeName;
    }

    public static bool IsDarkTheme
    {
      get
      {
        var theme = CurrentTheme;
        return theme == "Office2010Black" || theme == "PlasticBlue" || theme == "RedWine" || theme == "BlackGlass";
      }
    }

    public static string CurrentPageName
    {
      get
      {
        var key = "CE1167E3-A068-4E7C-8BFD-4A7D308BEF43";
        if (Context.Items[key] == null)
          Context.Items[key] = GetCurrentPageName();
        return Context.Items[key].ToString();
      }
    }

    public static string NewRandomNumber(int size)
    {
      Random random = new Random((int)DateTime.Now.Ticks);
      StringBuilder builder = new StringBuilder();
      string s;
      for (int i = 0; i < size; i++)
      {
        s = Convert.ToString(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
        builder.Append(s);
      }


      return ((builder.ToString()));

    }
    public static void BindGridCombos(GridViewDataComboBoxColumn combos, string CmbName, string Loc_ID)
    {
      string selectSQL = "";
      try
      {
        if (CmbName == "Card")
        {
          selectSQL = "SELECT  Pl_id as ID ,pl_Value DataValue FROM dbo.Picklist_hd INNER JOIN dbo.PickList_tran ON dbo.Picklist_hd.ID = dbo.PickList_tran.pl_head_id where Status<>'1' and Loc_ID= '" + Loc_ID + "' and  PL_Header='" + CmbName + "'";

        }
        else if (CmbName == "Host")
        {
          selectSQL = "SELECT  UserID as ID ,UserName DataValue FROM  Users  where  UserGroup=5 and Location_ID= '" + Loc_ID + "'";
        }
        else
        {
          selectSQL = "SELECT  Pl_id as ID ,pl_Value DataValue FROM dbo.Picklist_hd INNER JOIN dbo.PickList_tran ON dbo.Picklist_hd.ID = dbo.PickList_tran.pl_head_id where  PL_Header='" + CmbName + "'";
        }
        DataSet cmbDS;

        SqlDataAdapter Sqlcmb = new SqlDataAdapter();
        SqlConnection sConncmb = new SqlConnection(MyConnection.ReadConStr("Local"));
        SqlCommand selectcmb = new SqlCommand(selectSQL, sConncmb);
        Sqlcmb.SelectCommand = selectcmb;

        cmbDS = new DataSet();
        Sqlcmb.Fill(cmbDS, CmbName);


        combos.PropertiesComboBox.DataSource = cmbDS.Tables[0];
        combos.PropertiesComboBox.ValueField = cmbDS.Tables[0].Columns[0].ColumnName;
        combos.PropertiesComboBox.TextField = cmbDS.Tables[0].Columns[1].ColumnName;

        //   combos.PropertiesComboBox.DataBind();
        Sqlcmb = null;
        sConncmb = null;
        selectcmb = null;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public static void BindCombos(ASPxComboBox combos, string CmbName, string Loc_ID)
    {
      string selectSQL = "";
      try
      {

        DataSet cmbDS = new DataSet();

        cmbDS = DAL.Utils.fetchDSQueryRecordsSP(CmbName, Loc_ID, CmbName, "GetComboData", MyConnection.ReadConStr("Local"));

        if (cmbDS.Tables[0].Rows.Count > 0)
        {
          combos.ValueField = cmbDS.Tables[0].Columns[0].ColumnName;
          combos.TextField = cmbDS.Tables[0].Columns[1].ColumnName;
          combos.DataSource = cmbDS.Tables[0];
          combos.DataBind();
        }



      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    static string GetCurrentPageName()
    {
      var fileName = Path.GetFileName(Context.Request.Path);
      var result = fileName.Substring(0, fileName.Length - 5);
      if (result.ToLower() == "default")
        result = "mail";
      if (result.ToLower().Contains("print"))
        result = "print";
      return result.ToLower();
    }

    public static string CurrentTheme
    {
      get
      {
        if (Request.Cookies[CurrentThemeCookieKey] != null)
          return HttpUtility.UrlDecode(Request.Cookies[CurrentThemeCookieKey].Value);
        return DefaultTheme;
      }
    }

    public static void sendmail(string token, string visPhone, string fromEmail, string visEmail, string PersonName, string Links, string dts, string vmsg, string Location, string Duration)
    {
      try
      {
        DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));


        string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
        string EmailSubject = "Visitor Invite";


        string EmailID = "";
        string EmailPassword = "";
        string EmailSMTP = "";
        string EmailPort = "";
        string iSSl = "False";
        string EmailAccount = "";
        string AccountName = "";


        string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());

        if (dtMailSetting.Rows.Count > 0)
        {

          EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
          EmailPassword = DecryptQRCODE(EmailPassword);
          EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);

          string strHostName = null;
          string strIPAddress = null;
          string TokenNo = "";
          string MobileNo = "";
          if (visEmail.Length > 0)
          {
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            //    strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
            strIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            TokenNo = Convert.ToString(token);
            MobileNo = Convert.ToString(visPhone);
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(Convert.ToString(visEmail)));
            //msg.To.Add(new MailAddress(Convert.ToString(visEmail)));

            msg.From = new MailAddress(EmailID, AccountName);
            msg.Subject = EmailSubject;
            string strHTML = "<p> Dear  Sir </p>";
            strHTML = strHTML + "<p>  Visitor named (" + PersonName + " ) requested for an appointment,Please Accept the request by clicking below link.</p>";


            strHTML = strHTML + "</p><p>      <em>Name    :</em>" + PersonName;
            strHTML = strHTML + "</p><p>      <em>DateTime:</em>" + dts;
            strHTML = strHTML + "</p><p>      <em>Duration:</em>" + Duration;
            strHTML = strHTML + "</p><p>      <em>Remark  :</em>" + vmsg;
            strHTML = strHTML + "</p><p>      <em>Location:</em>" + Location;

            strHTML = strHTML + " <table cellspacing=" + "0" + " cellpadding=" + "0" + "><tr><td align=" + "center" + " width=" + "300" + " height=" + "40" + " bgcolor=" + "#000091" + " style=" + "-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;" + "><a href=" + Links + '_' + TokenNo + " style=" + "font-size:16px; font-weight: bold; font-family: Helvetica, Arial, sans-serif; text-decoration: none; line-height:40px; width:100%; display:inline-block" + "><span style=" + "color: #FFFFFF" + ">Approve</span></a></td></tr></table>";

            msg.Body = strHTML;
            msg.IsBodyHtml = true;



            if (EmailAccount == "Microsoft 365")
            {

              ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

              SmtpClient client = new SmtpClient();
              client.UseDefaultCredentials = false;
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);

              client.TargetName = "STARTTLS/smtp.office365.com";
              client.Timeout = 600000;

              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }
            else
            {
              SmtpClient client = new SmtpClient();
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);

              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);
              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }
          }



        }
      }
      catch (Exception ex)
      {
        throw ex;
      }


    }

    
    public static void sendVisitormail(Byte[] bytes, string token, string visPhone, string fromEmail, string visEmail, string dts, string vmsg, string PersonName)
    {
      try
      {
        DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));


        string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
        string EmailSubject = "Visitor Invite";


        string EmailID = "";
        string EmailPassword = "";
        string EmailSMTP = "";
        string EmailPort = "";
        string iSSl = "False";
        string EmailAccount = "";
        string AccountName = "";


        string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());

        if (dtMailSetting.Rows.Count > 0)
        {

          EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
          EmailPassword = DecryptQRCODE(EmailPassword);
          EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);

          string strHostName = null;
          string strIPAddress = null;
          string TokenNo = "";
          string MobileNo = "";
          strHostName = System.Web.HttpContext.Current.Request.UserHostName;
          strHostName = System.Web.HttpContext.Current.Request.UserHostName;
          //    strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
          strIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

          TokenNo = Convert.ToString(token);
          MobileNo = Convert.ToString(visPhone);
          MailMessage msg = new MailMessage();
          msg.To.Add(new MailAddress(Convert.ToString(visEmail)));
          //msg.To.Add(new MailAddress(Convert.ToString(visEmail)));
          msg.From = new MailAddress(EmailID, AccountName);
          msg.Subject = EmailSubject;

          msg.IsBodyHtml = true;
          msg.AlternateViews.Add(getEmbeddedImage("c:/image.png", bytes));

          string strHTML = "<p> Dear  Visitor </p>";
          strHTML = strHTML + "<p>  Your appointment is approved, </p>";


          strHTML = strHTML + "</p><p>    &gt;&gt; Visitor  <em>Name:</em>" + PersonName;
          strHTML = strHTML + "</p><p>    &gt;&gt; Visitor  <em>DateTime:</em>" + dts;
          strHTML = strHTML + "</p><p>    &gt;&gt; Visitor  <em>Message:</em>" + vmsg;

          msg.Body = strHTML;
          msg.IsBodyHtml = true;



          if (EmailAccount == "Microsoft 365")
          {

            ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
            client.Port = Convert.ToInt16(EmailPort);
            client.Host = EmailSMTP;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(iSSl);

            client.TargetName = "STARTTLS/smtp.office365.com";
            client.Timeout = 600000;

            try
            {
              client.Send(msg);
            }
            catch (Exception ex)
            { }
          }
          else
          {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);

            client.Port = Convert.ToInt16(EmailPort);
            client.Host = EmailSMTP;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(iSSl);
            try
            {
              client.Send(msg);
            }
            catch (Exception ex)
            { }
          }




        }
      }
      catch (Exception ex)
      {
        throw ex;
      }


    }

    public static void sendEmployee_Visitormail(Byte[] bytes, string token, string visPhone, string fromEmail, string visEmail, string PersonName, string Links, string dts, string vmsg, string Location, string Duration, bool blapprove, string HostName, string txtParking, string sCompany)
    {
      try
      {
        DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

        string ServiceLevel = "";
        bool ApprovalRequired = false;
        bool HostApproval = false;
        bool LineManagerApproval = false;
        string sHostEmail = "";
        string sManagerName = "";
        string sManagerEmail = "";

        string sSql = "select isnull(ApprovalRequired,0) as ApprovalRequired,isnull(HostApproval,0) as HostApproval,isnull(LineManagerApproval,0) as LineManagerApproval,isnull(ServiceLevel,'') as ServiceLevel from ApprovalSettings where MenuID = 11 ";
        DataTable dtAS = ocon.GetTable(sSql, new DataSet());
        if (dtAS.Rows.Count > 0)
        {
          ApprovalRequired = (bool)dtAS.Rows[0]["ApprovalRequired"];
          HostApproval = (bool)dtAS.Rows[0]["HostApproval"];
          LineManagerApproval = (bool)dtAS.Rows[0]["LineManagerApproval"];
          ServiceLevel = Convert.ToString(dtAS.Rows[0]["ServiceLevel"]);
        }

        string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
        string EmailSubject = "Visitor Invite";
        string strHTML = "";
        string EmailID = "";
        string EmailPassword = "";
        string EmailSMTP = "";
        string EmailPort = "";
        string iSSl = "False";
        string EmailAccount = "";
        string AccountName = "";

        if (ApprovalRequired == true)
        {
          string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Visitor Invite' and Email_For='Request With Approval' and EmailStage='Requester'";

          DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());
          foreach (DataRow dr in dtEmailtemp.Rows)
          {

            EmailSubject = dr["Email_Sub"].ToString();
            strHTML = dr["Email_Body"].ToString();


          }
        }
        else
        {
          string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Visitor Invite' and Email_For='Request Without Approval' and EmailStage='Requester'";

          DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());
          foreach (DataRow dr in dtEmailtemp.Rows)
          {

            EmailSubject = dr["Email_Sub"].ToString();
            strHTML = dr["Email_Body"].ToString();


          }
        }

        string sSqlEm = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSqlEm, new DataSet());

        if (dtMailSetting.Rows.Count > 0)
        {

          EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
          EmailPassword = DecryptQRCODE(EmailPassword);
          EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);
          string strHostName = null;
          string strIPAddress = null;
          string TokenNo = "";
          string MobileNo = "";
          if (visEmail.Length > 0)
          {
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            //    strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
            strIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            TokenNo = Convert.ToString(token);
            MobileNo = Convert.ToString(visPhone);
            MailMessage msg = new MailMessage();



            MemoryStream ms = new MemoryStream(bytes);
            LinkedResource res = new LinkedResource(ms, "image/jpeg");



            res.ContentId = Guid.NewGuid().ToString();


            msg.To.Add(new MailAddress(Convert.ToString(visEmail)));

            msg.From = new MailAddress(EmailID, "Visitor Management System");
            msg.Subject = EmailSubject;

           
            string sVisitorname = PersonName;
        
          
        
            string sVisitorEmail = visEmail;

            
            string sDepartment = Location;
            string sHostName = HostName;
         
            string sDuration = Duration;
     


            strHTML = strHTML.Replace("[VisitorName]", sVisitorname);
            
            strHTML = strHTML.Replace("[VisitorEmail]", sVisitorEmail);

           
            strHTML = strHTML.Replace("[Visitdate]", dts);
            strHTML = strHTML.Replace("[Department]", sDepartment);
            strHTML = strHTML.Replace("[HostName]", sHostName);
    
            strHTML = strHTML.Replace("[Duration]", sDuration);
         



            strHTML = strHTML + @"<img src='cid:" + res.ContentId + @"' width=100 height=150 />";



            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(strHTML, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);

            msg.IsBodyHtml = true;
            msg.AlternateViews.Add(alternateView);

            msg.Attachments.Add(new Attachment(new MemoryStream(bytes), "QRCode.jpg"));

            if (DisableEmailWS == "True")
            {
              msg.Body = strHTML;
              msg.IsBodyHtml = true;



              if (EmailAccount == "Microsoft 365")
              {

                ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
                client.Port = Convert.ToInt16(EmailPort);
                client.Host = EmailSMTP;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = Convert.ToBoolean(iSSl);

                client.TargetName = "STARTTLS/smtp.office365.com";
                client.Timeout = 600000;

                try
                {
                  client.Send(msg);
                }
                catch (Exception ex)
                { }
              }
              else
              {
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);

                client.Port = Convert.ToInt16(EmailPort);
                client.Host = EmailSMTP;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = Convert.ToBoolean(iSSl);
                try
                {
                  client.Send(msg);
                }
                catch (Exception ex)
                { }
              }
            }
          }

        }
      }
      catch (Exception ex)
      { }

    }
    public static SecuLobbyVMS.smartWS.SmartISS_Service objservice = new SecuLobbyVMS.smartWS.SmartISS_Service();

    public static void EmailWs(bool saveLogs,
    Byte[] bytes,
            string token,
    string visPhone,
    string fromEmail,
    string visEmail,
    string dts,
    string vmsg,
    string PersonName,
    string Duration,
    string txtParking,
    string HostName,
    string Location,
    bool blapprove,
    string Links,
    string EmailID,
    string EmailPassword,
    string EmailMask,
    string EmailSubject,
    string EmailPort,
    string EmailSMTP,
    string EmailEnableSsl)
    {

      DataSet wsDS = new DataSet();
      String ErrCode = "";
      String ErrMsg = "";

      string Webservicelink = System.Configuration.ConfigurationManager.AppSettings["WebService"];
      //  objservice.Url = @"http://185.241.124.28/SeculobbyWS/SmartISS_Service.asmx?";
      if (Webservicelink.Length > 10)
      {
        objservice.Url = Webservicelink;
      }

      try
      {
        string procedure = "";
        string Operation = "Email";
        List<SqlDbParam> paramlist = new List<SqlDbParam>();

        //paramlist.Add(SecuLobby.App_Code.StaticFuncs.DbParam("@code", "2745", SqlDbType.NVarChar));
        //paramlist.Add(SecuLobby.App_Code.StaticFuncs.DbParam("@status", "A", SqlDbType.NVarChar));
        //paramlist.Add(SecuLobby.App_Code.StaticFuncs.DbParam("@Company", "5000", SqlDbType.NVarChar));
        //bool saveLogs = false;
        //Byte[] bytes = null; ;
        //string token = "324324324";
        //string visPhone = "Seculobby";
        //string fromEmail = "moideen@infotechuae.com";
        //string visEmail = "moideen@infotechuae.com";
        //string dts = "01-01-2020 01:00";
        //string vmsg = "Vms msg";
        //string PersonName = "MOideen";
        //string Duration = "1 hours";
        //string txtParking = " Parking ";
        //string HostName = "Seculobby";
        //string Location = "Seculobby";
        //bool blapprove = false;
        //string Links = "";
        //string EmailID = "seculobbyinfo@gmail.com";
        //string EmailPassword = "SecuLobby123@";
        //string EmailMask = "Seculobby";
        //string EmailSubject = "Seculobby";
        //string EmailPort = "587";
        //string EmailSMTP = "smtp.gmail.com";
        //string EmailEnableSsl = "true";
        string errcode = "";
        string message = "";

        //     Int64 rVal = objservice.SetEmailInterface(procedure, Operation, paramlist.ToArray(), saveLogs, bytes,
        //           token,
        //     visPhone,
        //     fromEmail,
        //     visEmail,
        //     dts,
        //      vmsg,
        //     PersonName,
        //     Duration,
        //      txtParking,
        //      HostName,
        //      Location,
        //blapprove,
        //  Links,
        //     EmailID,
        //    EmailPassword,
        //    EmailMask,
        //    EmailSubject,
        //    EmailPort,
        //    EmailSMTP,
        //     EmailEnableSsl,
        //   out errcode,
        //    out message);

        Int64 rVal = 0;

      }
      catch (Exception ed)
      {
        // MessageBox.Show(ed.Message);
      }


    }


    private static AlternateView getEmbeddedImage(String filePath, Byte[] bytes)
    {
      // Byte[] bytes = (Byte[])dt.Rows[0]["Data"];
      MemoryStream ms = new MemoryStream(bytes);
      LinkedResource res = new LinkedResource(ms, "image/jpeg");


      res.ContentId = Guid.NewGuid().ToString();
      string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
      AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
      alternateView.LinkedResources.Add(res);
      return alternateView;
    }
    public static DataSet fetchDSQueryRecordsSP(string strValue, string ModuleCode, string StrSPname, string connectionString)
    {

      string m_TableName = ModuleCode + "DT";
      string m_DataSetName = ModuleCode;
      string[,] _paramArray;


      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[2, 2];

        _paramArray[0, 0] = "StrVal";
        _paramArray[0, 1] = strValue;

        _paramArray[1, 0] = "ModuleCode";
        _paramArray[1, 1] = ModuleCode;

        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

        //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds;
    }

    public static string fetchStringRecordsSP(string strValue, string ModuleCode, string StrSPname, string connectionString)
    {

      string m_TableName = ModuleCode + "DT";
      string m_DataSetName = ModuleCode;
      string[,] _paramArray;
      string strVal = "0";

      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[2, 2];

        _paramArray[0, 0] = "StrVal";
        _paramArray[0, 1] = strValue;

        _paramArray[1, 0] = "ModuleCode";
        _paramArray[1, 1] = ModuleCode;

        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

        if (ds.Tables[0].Rows.Count > 0)
        {

          strVal = ds.Tables[0].Rows[0][0].ToString();
        }

      }
      catch (Exception ex)
      {
        strVal = "0";
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return strVal;
    }
    public static DataSet fetchDSQueryRecordsSP(string strValue, string strValue2, string ModuleCode, string StrSPname, string connectionString)
    {

      string m_TableName = ModuleCode + "DT";
      string m_DataSetName = ModuleCode;
      string[,] _paramArray;


      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[3, 2];

        _paramArray[0, 0] = "StrVal";
        _paramArray[0, 1] = strValue;
        _paramArray[1, 0] = "StrVal2";
        _paramArray[1, 1] = strValue2;

        _paramArray[2, 0] = "ModuleCode";
        _paramArray[2, 1] = ModuleCode;

        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

        //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds;
    }
    public static DataSet fetchRecordsDS(string COLNAME, string TBNAME, string WHERECLAUSE, string connectionString)
    {

      string m_TableName = "GeneralDT";
      string m_DataSetName = "GeneralDS";
      string[,] _paramArray;


      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[3, 2];

        _paramArray[0, 0] = "v_ColumnName";
        _paramArray[0, 1] = COLNAME;

        _paramArray[1, 0] = "v_tbName";
        _paramArray[1, 1] = TBNAME;

        _paramArray[2, 0] = "v_WhereClause";
        _paramArray[2, 1] = WHERECLAUSE;

        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, PROC_NAME, _paramArray, connectionString);
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds;
    }

    //public static int Update(string tbname, string cols, string whereclause)
    //{
    //    int[] valuesReturned;
    //    int newId = 0;
    //    try
    //    {

    //        string[,] _param;

    //        _param = new string[3, 2];

    //        //tbname = "member_mst";
    //        //cols = "member_pin=123";
    //        //whereclause = "member_id=3276";
    //        _param[0, 0] = "v_tbname";
    //        _param[0, 1] = tbname;

    //        _param[1, 0] = "v_colnames";
    //        _param[1, 1] = cols;

    //        _param[2, 0] = "v_whereclause";
    //        _param[2, 1] = whereclause;

    //        valuesReturned = DAL.DALSQL.executeSP("UPDATEBYCOLUMN", _param);
    //        newId = valuesReturned[0];
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //    }
    //    return newId;
    //}
    public static DataTable fetchRecords(string COLNAME, string TBNAME, string WHERECLAUSE, string connectionString)
    {

      string m_TableName = "GeneralDT";
      string m_DataSetName = "GeneralDS";
      string[,] _paramArray;


      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[3, 2];

        _paramArray[0, 0] = "v_ColumnName";
        _paramArray[0, 1] = COLNAME;

        _paramArray[1, 0] = "v_tbName";
        _paramArray[1, 1] = TBNAME;

        _paramArray[2, 0] = "v_WhereClause";
        _paramArray[2, 1] = WHERECLAUSE;

        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, PROC_NAME, _paramArray, connectionString);
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds.Tables[0];
    }
    public static bool UpdDataDs(string connectionString, string Tablename, DataSet DBSds)
    {
      // Save or Update  one table  in Access database
      SqlConnection conn = new SqlConnection();



      SqlCommandBuilder SqlCB;
      SqlTransaction trans = null;
      SqlDataAdapter da;
      try
      {
        conn = DAL.DALSQL.getConnection(connectionString);
        trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
        //    for (int i = 0; i <= Tab_Select.GetUpperBound(0); i++)
        //  {

        //     if (Tab_Select[i] != null)
        //     {
        //da = new OleDbDataAdapter();
        //da.SelectCommand = new OleDbCommand (Tab_Select[i], conn, trans);
        //custCB = new OleDbCommandBuilder(da);
        //da.Update(DBSds, DBSds.Tables[i].TableName);
        da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand("select * from " + Tablename, conn, trans);

        SqlCB = new SqlCommandBuilder(da);

        da.Update(DBSds, Tablename);

        trans.Commit();
        conn.Close();
        conn.Dispose();
        return true;
      }

      catch (SqlException ex)
      {
        //  Console.WriteLine(ex.Message + "   Transaction Rollback, Try again ");
        trans.Rollback();
        throw ex;


        // return false;
      }

    }

    public static DataTable fetchQueryRecords(string strQury, string connectionString)
    {

      string m_TableName = "GeneralQDT";
      string m_DataSetName = "GeneralQDS";
      string[,] _paramArray;


      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[1, 2];

        _paramArray[0, 0] = "v_QUERY";
        _paramArray[0, 1] = strQury;


        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, "GENERAL_QUERY_READ", _paramArray, connectionString);
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds.Tables[0];
    }

    public static DataTable fetchQueryRecordsSP(string strValue, string StrSPname, string connectionString)
    {

      string m_TableName = StrSPname + "DT";
      string m_DataSetName = StrSPname + "DS";
      string[,] _paramArray;


      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {
        _paramArray = new string[1, 2];

        _paramArray[0, 0] = "StrVal";
        _paramArray[0, 1] = strValue;


        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds.Tables[0];
    }
    public static DataTable fetchQuerySP(string[,] _paramArray, string StrSPname, string connectionString)
    {


      string m_TableName = StrSPname + "DT";
      string m_DataSetName = StrSPname + "DS";



      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {


        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds.Tables[0];
    }
    public static DataSet fetchDSQuerySP(string[,] _paramArray, string StrSPname, string connectionString)
    {


      string m_TableName = StrSPname + "DT";
      string m_DataSetName = StrSPname + "DS";



      DAL.DALSQL objDal;
      DataSet ds = null;
      try
      {


        objDal = new DAL.DALSQL();
        ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {
        objDal = null;
      }
      return ds;
    }
    public static int fetchValues(string COLNAME, string TBNAME, string WHERECLAUSE, string connectionString)
    {

      DataTable dt = null;
      int result = 0;
      try
      {
        dt = new DataTable();
        dt = fetchRecords(COLNAME, TBNAME, WHERECLAUSE, connectionString);
        if (dt.Rows.Count > 0)
          if (!(dt.Select()[0].ItemArray[0].Equals(System.DBNull.Value)))
            result = Convert.ToInt32(dt.Select()[0].ItemArray[0]);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        dt = null;
      }
      return result;
    }
    public static string fetchString(string COLNAME, string TBNAME, string WHERECLAUSE, string connectionString)
    {

      DataTable dt = null;
      string result = "";
      try
      {
        dt = new DataTable();
        dt = fetchRecords(COLNAME, TBNAME, WHERECLAUSE, connectionString);
        if (dt.Rows.Count > 0)
          result = dt.Select()[0].ItemArray[0].ToString();

      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        dt = null;
      }
      return result;

    }

    public static DataTable fetchRecords(String _query, string connectionString)
    //this is a newly added methods which takes only a query as param
    //this is mainly used for lists
    {
      SqlConnection _conn = new SqlConnection();

      _conn = DAL.DALSQL.getConnection(connectionString);
      SqlCommand _command = new SqlCommand();
      _command.Connection = _conn;
      _command.CommandText = _query;
      SqlDataAdapter _adapter = new SqlDataAdapter(_command);
      DataTable _dataTable = null;
      _dataTable = new DataTable("_Tab1");

      try
      {
        _adapter.Fill(_dataTable);
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        _command.Dispose();
        if (_conn != null && _conn.State == ConnectionState.Open)
        {
          _conn.Close();
          _conn.Dispose();

        }
      }
      return _dataTable;
    }

    public static DataSet fetchRecordsDs(String _query, string connectionString)
    //this is a newly added methods which takes only a query as param
    //this is mainly used for lists
    {
      SqlConnection _conn = new SqlConnection();
      DataSet _dataSet = null;
      try
      {
        _conn = DAL.DALSQL.getConnection(connectionString);
        SqlCommand _command = new SqlCommand();
        _command.Connection = _conn;
        _command.CommandText = _query;
        SqlDataAdapter _adapter = new SqlDataAdapter(_command);
        _dataSet = new DataSet("_DSet");
        _adapter.Fill(_dataSet);
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {

        // _command.Dispose();
        _conn.Close();
        _conn.Dispose();

      }
      return _dataSet;

    }


    public static int fetchValues(String _query, string connectionString)
    {
      int Val = 1;
      SqlConnection _conn = new SqlConnection();
      try
      {
        _conn = DAL.DALSQL.getConnection(connectionString);
        SqlCommand orCmd = new SqlCommand(_query, _conn);
        Val = Convert.ToInt32(orCmd.ExecuteScalar());

      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {

        // orCmd.Dispose();
        _conn.Close();
        _conn.Dispose();
      }
      return Val;
    }









    //public static string CurrentThemeTitle
    //{
    //    get
    //    {
    //        var theme = CurrentTheme;
    //        //var themeModel =  ThemesModel.Current.Groups.SelectMany(g => g.Themes).FirstOrDefault(t => t.Name == theme);
    //        //return themeModel != null ? themeModel.Title : theme;
    //        return theme;
    //    }
    //}

    public static void RegisterCssLink(Page page, string url)
    {
      RegisterCssLink(page, url, null);
    }
    public static void RegisterCssLink(Page page, string url, string clientId)
    {

      HtmlLink link = new HtmlLink();
      page.Header.Controls.Add(link);
      link.EnableViewState = false;
      link.Attributes["type"] = "text/css";
      link.Attributes["rel"] = "stylesheet";
      if (!string.IsNullOrEmpty(clientId))
        link.Attributes["id"] = clientId;
      link.Href = url;
    }
    private static string MeetingRequestString(string from, List<string> toUsers, string subject, string desc, string location, DateTime startTime, DateTime endTime, int? eventID = null, bool isCancel = false)
    {
      StringBuilder str = new StringBuilder();

      str.AppendLine("BEGIN:VCALENDAR");
      str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
      str.AppendLine("VERSION:2.0");
      str.AppendLine(string.Format("METHOD:{0}", (isCancel ? "CANCEL" : "REQUEST")));
      str.AppendLine("BEGIN:VEVENT");

      str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime.ToUniversalTime()));
      str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.Now));
      str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime.ToUniversalTime()));
      str.AppendLine(string.Format("LOCATION: {0}", location));
      str.AppendLine(string.Format("UID:{0}", (eventID.HasValue ? "blablabla" + eventID : Guid.NewGuid().ToString())));
      str.AppendLine(string.Format("DESCRIPTION:{0}", desc.Replace("\n", "<br>")));
      str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", desc.Replace("\n", "<br>")));
      str.AppendLine(string.Format("SUMMARY:{0}", subject));

      str.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":MAILTO:{1}", from, from));
      str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", string.Join(",", toUsers), string.Join(",", toUsers)));

      str.AppendLine("BEGIN:VALARM");
      str.AppendLine("TRIGGER:-PT15M");
      str.AppendLine("ACTION:DISPLAY");
      str.AppendLine("DESCRIPTION:Reminder");
      str.AppendLine("END:VALARM");
      str.AppendLine("END:VEVENT");
      str.AppendLine("END:VCALENDAR");

      return str.ToString();
    }
    //    static async Task SendGridAsync()
    //    {
    //        var client = new SendGridClient("your-api-key");

    //        var msg = new SendGridMessage()
    //        {
    //            From = new EmailAddress("{sender-email}", "{sender-name}"),
    //            Subject = "Hello World from the SendGrid CSharp SDK!",
    //            HtmlContent = "<strong>Hello, Email using HTML!</strong>"
    //        };
    //        var recipients = new List<EmailAddress>
    //{
    //    new EmailAddress("{recipient-email}", "{recipient-name}")
    //};
    //        msg.AddTos(recipients);

    //        string CalendarContent = MeetingRequestString("{ORGANIZER}", new List<string>() { "{ATTENDEE}" }, "{subject}", "{description}", "{location}", DateTime.Now, DateTime.Now.AddDays(2));
    //        byte[] calendarBytes = Encoding.UTF8.GetBytes(CalendarContent.ToString());
    //        SendGrid.Helpers.Mail.Attachment calendarAttachment = new SendGrid.Helpers.Mail.Attachment();
    //        calendarAttachment.Filename = "invite.ics";
    //        //the Base64 encoded content of the attachment.
    //        calendarAttachment.Content = Convert.ToBase64String(calendarBytes);
    //        calendarAttachment.Type = "text/calendar";
    //        msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>() { calendarAttachment };

    //        var response = await client.SendEmailAsync(msg);
    //    }

    public static void sendVisitor_InviteCalendermail(string fromEmail, string PersonName, string dts, string vmsg, string Location, string Duration, List<string> Visitor_list, List<string> Host_list, string Meetingsubject, string Meetinglocation, string Meetingdescription, string MeetingORGANIZER, string HostName, List<string> Visitor_Name)
    {
      try
      {
        DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

        string ServiceLevel = "";
        bool ApprovalRequired = false;
        bool HostApproval = false;
        bool LineManagerApproval = false;
        string sHostEmail = "";
        string sManagerName = "";
        string sManagerEmail = "";

        string sSql = "select isnull(ApprovalRequired,0) as ApprovalRequired,isnull(HostApproval,0) as HostApproval,isnull(LineManagerApproval,0) as LineManagerApproval,isnull(ServiceLevel,'') as ServiceLevel from ApprovalSettings where MenuID = 11 ";
        DataTable dtAS = ocon.GetTable(sSql, new DataSet());
        if (dtAS.Rows.Count > 0)
        {
          ApprovalRequired = (bool)dtAS.Rows[0]["ApprovalRequired"];
          HostApproval = (bool)dtAS.Rows[0]["HostApproval"];
          LineManagerApproval = (bool)dtAS.Rows[0]["LineManagerApproval"];
          ServiceLevel = Convert.ToString(dtAS.Rows[0]["ServiceLevel"]);
        }

        string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
        string EmailSubject = "Visitor Invite";

        string EmailID = "";
        string EmailPassword = "";
        string EmailSMTP = "";
        string EmailPort = "";
        string iSSl = "False";
        string EmailAccount = "";
        string AccountName = "";
        string strHTML = "";

        if (ApprovalRequired == true)
        {
          string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Visitor Invite' and Email_For='Request With Approval' and EmailStage!='Requester'";

          DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());
          foreach (DataRow dr in dtEmailtemp.Rows)
          {

            EmailSubject = dr["Email_Sub"].ToString();
            strHTML = dr["Email_Body"].ToString();


          }
        }
        else
        {
          string sSqlEmailTemp = "SELECT id,Email_For,Email_Body,Email_Sub,EmailStage FROM Tbl_EmailTemplate WHERE EmailType='Visitor Invite' and Email_For='Request Without Approval' and EmailStage!='Requester'";

          DataTable dtEmailtemp = ocon.GetTable(sSqlEmailTemp, new DataSet());
          foreach (DataRow dr in dtEmailtemp.Rows)
          {

            EmailSubject = dr["Email_Sub"].ToString();
            strHTML = dr["Email_Body"].ToString();


          }
        }


        string sSqlMS = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSqlMS, new DataSet());

        if (dtMailSetting.Rows.Count > 0)
        {

          EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
          EmailPassword = DecryptQRCODE(EmailPassword);
          EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);

          string strHostName = null;
          string strIPAddress = null;
          string TokenNo = "";
          string MobileNo = "";
          if (fromEmail.Length > 0)
          {
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            //    strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
            strIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            //TokenNo = Convert.ToString(token);
            //MobileNo = Convert.ToString(visPhone);
            MailMessage msg = new MailMessage();



            //MemoryStream ms = new MemoryStream(bytes);
            //LinkedResource res = new LinkedResource(ms, "image/jpeg");


            //   res.ContentId = Guid.NewGuid().ToString();

            foreach (string value in Visitor_list)
            {
              msg.To.Add(new MailAddress(Convert.ToString(value)));
            }
            foreach (string hostemail in Host_list)
            {
              msg.To.Add(new MailAddress(Convert.ToString(hostemail)));
            }
            string sVisName = "";
            foreach (string visname in Visitor_Name)
            {
              if (sVisName != "")
                sVisName = sVisName + "," + Convert.ToString(visname);
              else
                sVisName = Convert.ToString(visname);
            }


            msg.From = new MailAddress(EmailID, "Visitor Management System");
            msg.Subject = EmailSubject;
            string sVisitorname = PersonName;

            string sDepartment = Location;
            string sHostName = HostName;

            string sDuration = Duration;



            strHTML = strHTML.Replace("[VisitorName]", sVisitorname);

            

            strHTML = strHTML.Replace("[Visitdate]", dts);
            strHTML = strHTML.Replace("[Department]", sDepartment);
            strHTML = strHTML.Replace("[HostName]", sHostName);

            strHTML = strHTML.Replace("[Duration]", sDuration);


            strHTML = strHTML + " <table cellspacing=" + "0" + " cellpadding=" + "0" + "><tr><td align=" + "center" + " width=" + "300" + " height=" + "40" + " bgcolor=" + "#000091" + " style=" + "-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;" + ">   </span></a></td></tr></table>";





            //new List<string>() { "thanumoideen@gmail.com","moideenmm@yahoo.com" }
            string CalendarContent = MeetingRequestString(MeetingORGANIZER, Visitor_list, Meetingsubject, Meetingdescription, Meetinglocation, Convert.ToDateTime(dts), Convert.ToDateTime(dts).AddMinutes(60));
            byte[] calendarBytes = Encoding.UTF8.GetBytes(CalendarContent.ToString());
            Stream calendarStream = new MemoryStream(calendarBytes);
            Attachment attachment = new Attachment(calendarStream, "SeculobbyMeeting.ics", "text/calendar");

            msg.Attachments.Add(attachment);

            msg.Body = strHTML;
            msg.IsBodyHtml = true;


            //  msg.AlternateViews.Add(alternateView);



            if (EmailAccount == "Microsoft 365")
            {

              ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

              SmtpClient client = new SmtpClient();
              client.UseDefaultCredentials = false;
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);

              client.TargetName = "STARTTLS/smtp.office365.com";
              client.Timeout = 600000;

              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }
            else
            {
              SmtpClient client = new SmtpClient();
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);

              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);
              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }


          }


        }
      }
      catch (Exception ex)
      {

        //     EmailWs(false, bytes, token, visPhone, fromEmail, visEmail, dts, vmsg, PersonName, Duration, txtParking, HostName, Location, blapprove, Links, EmailID,
        //EmailPassword, EmailMask, EmailSubject, EmailPort, EmailSMTP, EmailEnableSsl);


      }


    }

    public static void sendVisitor_InviteApprovemail(string fromEmail, string PersonName, string dts, string vmsg, string Location, string Duration, List<string> Visitor_list, List<string> Host_list, string Meetingsubject, string Meetinglocation, string Meetingdescription, string MeetingORGANIZER, string HostName, List<string> Visitor_Name, string TabID)
    {
      try
      {
        DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));


        string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
        string EmailSubject = "Visitor Invite";

        string EmailID = "";
        string EmailPassword = "";
        string EmailSMTP = "";
        string EmailPort = "";
        string iSSl = "False";
        string EmailAccount = "";
        string AccountName = "";


        string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());

        if (dtMailSetting.Rows.Count > 0)
        {

          EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
          EmailPassword = DecryptQRCODE(EmailPassword);
          EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);

          string strHostName = null;
          string strIPAddress = null;
          string TokenNo = "";
          string MobileNo = "";
          if (fromEmail.Length > 0)
          {
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;
            strHostName = System.Web.HttpContext.Current.Request.UserHostName;

            strIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            MailMessage msg = new MailMessage();


            foreach (string hostemail in Host_list)
            {
              msg.To.Add(new MailAddress(Convert.ToString(hostemail)));
            }
            string sVisName = "";
            foreach (string visname in Visitor_Name)
            {
              if (sVisName != "")
                sVisName = sVisName + "," + Convert.ToString(visname);
              else
                sVisName = Convert.ToString(visname);
            }

            string _FooterText = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(0, HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf("/") + 1) + "action.aspx?tabid=" + TabID;


            msg.From = new MailAddress(EmailID, AccountName);
            msg.Subject = EmailSubject;
            string strHTML = "<p> Dear  Sir </p>";
            strHTML = strHTML + "<p>The below person want to meet you. Please check the below details.</p>";

            strHTML = strHTML + "</p><p>      <em>Meeting name  :</em>" + Meetingsubject;
            strHTML = strHTML + "</p><p>      <em>Visitor Name    :</em>" + sVisName;

            strHTML = strHTML + "</p><p>      <em>DateTime:</em>" + dts;
            strHTML = strHTML + "</p><p>      <em>Duration:</em>" + Duration;
            strHTML = strHTML + "</p><p>      <em>Host Name    :</em>" + HostName;

            strHTML = strHTML + "</p><p>      <em>Location:</em>" + Location;


            strHTML = strHTML + "<p>Please approve this meeting through the below link link.</p>";
            strHTML = strHTML + "<a href='" + _FooterText + "' target = '_blank' ><b> Approve </b></a>";

            strHTML = strHTML + " <table cellspacing=" + "0" + " cellpadding=" + "0" + "><tr><td align=" + "center" + " width=" + "300" + " height=" + "40" + " bgcolor=" + "#000091" + " style=" + "-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;" + ">   </span></a></td></tr></table>";




            msg.Body = strHTML;
            msg.IsBodyHtml = true;




            if (EmailAccount == "Microsoft 365")
            {

              ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

              SmtpClient client = new SmtpClient();
              client.UseDefaultCredentials = false;
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);

              client.TargetName = "STARTTLS/smtp.office365.com";
              client.Timeout = 600000;

              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }
            else
            {
              SmtpClient client = new SmtpClient();
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);

              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);
              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }


          }


        }
      }
      catch (Exception ex)
      {

        //     EmailWs(false, bytes, token, visPhone, fromEmail, visEmail, dts, vmsg, PersonName, Duration, txtParking, HostName, Location, blapprove, Links, EmailID,
        //EmailPassword, EmailMask, EmailSubject, EmailPort, EmailSMTP, EmailEnableSsl);


      }


    }


    public static void sendHostNotifymail(string HostID, string VisitorName, string Company, string Remarks, string Msg)
    {
      try
      {
        DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));


        string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
        string EmailSubject = "Visitor Invite";

        string EmailID = "";
        string EmailPassword = "";
        string EmailSMTP = "";
        string EmailPort = "";
        string iSSl = "False";
        string EmailAccount = "";
        string AccountName = "";


        string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());

        if (dtMailSetting.Rows.Count > 0)
        {

          EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
          EmailPassword = DecryptQRCODE(EmailPassword);
          EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);

          string strHostName = null;
          string strIPAddress = null;
          string TokenNo = "";
          string MobileNo = "";

          strHostName = System.Web.HttpContext.Current.Request.UserHostName;
          strHostName = System.Web.HttpContext.Current.Request.UserHostName;
          //    strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
          strIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;


          MailMessage msg = new MailMessage();




          string HostEmailID = DAL.Utils.fetchString("UserEmail", "Users", "UserID='" + HostID + "'", MyConnection.ReadConStr("Local"));
          if (HostEmailID.Length > 4)
          {
            msg.To.Add(new MailAddress(Convert.ToString(HostEmailID)));
            //}
            string HostNotifyMsg = System.Configuration.ConfigurationManager.AppSettings["HostNotifyMsg"];
            if (HostNotifyMsg.Length < 2)
              HostNotifyMsg = "You got visitor , Please find the below visitor details .  ";

            msg.From = new MailAddress(EmailID, AccountName);
            msg.Subject = "Visitor Notification";
            string strHTML = "<p> Dear  Sir </p>";
            strHTML = strHTML + "<p> " + HostNotifyMsg + " .</p>";
            strHTML = strHTML + "</p><p>      <em>Visitor      :</em>" + VisitorName;
            strHTML = strHTML + "</p><p>      <em>Company Name :</em>" + Company;
            strHTML = strHTML + "</p><p>      <em>Remarks      :</em>" + Remarks;


            strHTML = strHTML + " <table cellspacing=" + "0" + " cellpadding=" + "0" + "><tr><td align=" + "center" + " width=" + "300" + " height=" + "40" + " bgcolor=" + "#000091" + " style=" + "-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;" + ">   </span></a></td></tr></table>";




            msg.Body = strHTML;
            msg.IsBodyHtml = true;




            if (EmailAccount == "Microsoft 365")
            {

              ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
              System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

              SmtpClient client = new SmtpClient();
              client.UseDefaultCredentials = false;
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);

              client.TargetName = "STARTTLS/smtp.office365.com";
              client.Timeout = 600000;

              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }
            else
            {
              SmtpClient client = new SmtpClient();
              client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);

              client.Port = Convert.ToInt16(EmailPort);
              client.Host = EmailSMTP;
              client.DeliveryMethod = SmtpDeliveryMethod.Network;
              client.EnableSsl = Convert.ToBoolean(iSSl);
              try
              {
                client.Send(msg);
              }
              catch (Exception ex)
              { }
            }

          }



        }
      }
      catch (Exception ex)
      {



      }


    }

    public static string EncryptQRCODE(string plainText)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = Encoding.UTF8.GetBytes(iv);

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
          using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
          }
        }
      }
    }

    public static string DecryptQRCODE(string cipherText)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = Encoding.UTF8.GetBytes(iv);

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
        {
          using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader sr = new StreamReader(cs))
            {
              return sr.ReadToEnd();
            }
          }
        }
      }
    }

   
  }
}
