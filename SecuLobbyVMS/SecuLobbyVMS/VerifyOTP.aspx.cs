using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class VerifyOTP : System.Web.UI.Page
  {

    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

    protected void Page_Load(object sender, EventArgs e)
    {

      if (!IsPostBack && !IsCallback)
      {
        txtOtp.Text = "";
      }
    }

    protected void btnSignup_Click(object sender, EventArgs e)
    {
      string sLogID = Request.QueryString["LogID"];

      if (txtverification.Text == txt1.Value)
      {
        try
        {
          
          if (!string.IsNullOrEmpty(sLogID))
          {
            string sSql = "SELECT *  FROM Users where UserID='" + sLogID + "' and LoginOTP='" + txtOtp.Text.Trim() + "'";
            DataTable dt = ocon.GetTable(sSql, new DataSet());

            if (dt.Rows.Count > 0)
            {

              string sUpdate = "UPDATE Users SET UserActive=1 WHERE UserID=" + sLogID;
              ocon.Execute(sUpdate);

              string sDestURL = string.Format("\"{0}\"", "LoginSelfRegistration.aspx");
              string smessage = string.Format("\"{0}\"", "Registration completed successfully.");

              string sVar = sDestURL + "," + smessage;


              ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

            }
            else
            {
              string sDestURL = string.Format("\"{0}\"", "VerifyOTP.aspx?LogID=" + sLogID);
              string smessage = string.Format("\"{0}\"", "Invalid OTP.");

              string sVar = sDestURL + "," + smessage;


              ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

            }
          }
          else
          {
            string sDestURL = string.Format("\"{0}\"", "VerifyOTP.aspx?LogID=" + sLogID);
            string smessage = string.Format("\"{0}\"", "Enter OTP.");

            string sVar = sDestURL + "," + smessage;


            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

          }



        }
        catch (Exception ex)
        {
          throw ex;
        }

      }
      else
      {
        string sDestURL = string.Format("\"{0}\"", "VerifyOTP.aspx?LogID=" + sLogID);
        string smessage = string.Format("\"{0}\"", "Invalid Captcha.");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);

      }
    }

    private void SendEmail(string sName, string sEmail, string sOTP)
    {



      DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));


      string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
      string EmailSubject = "Registration Successful";

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
       // EmailPassword = DecryptQRCODE(EmailPassword);
        EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
        EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
        EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
        AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
        iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);


        MailMessage msg = new MailMessage();
        msg.To.Add(new MailAddress(Convert.ToString(sEmail)));

        msg.From = new MailAddress(EmailID, AccountName);
        msg.Subject = EmailSubject;
        string strHTML = "<p> Dear " + sName + ",</p>";
        strHTML = strHTML + "<p>  You have successfully register the portal . Please USe the OTp to complete the registration " + sOTP + "</p>";

        msg.IsBodyHtml = true;


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
}
