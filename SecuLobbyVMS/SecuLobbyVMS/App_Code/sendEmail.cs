using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using DAL;
using System.Security.Cryptography;
using System.Text;

namespace SecuLobbyVMS.App_Code
{
  public class sendEmail
  {
    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";

    public sendEmail()
    {
      //
      // TODO: Add constructor logic here
      //
    }
    public static string SendEmails(string sName, string sEmail, string sID, string strHTML, string EmailSubject, byte[] byteImageVis, LinkedResource res)
    {
      //ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
      //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
      //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

      //string EmailMask = System.Configuration.ConfigurationManager.AppSettings["EmailMask"];
      //string EmailEnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"];
      string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];



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


        MailMessage msg = new MailMessage();
        msg.To.Add(new MailAddress(Convert.ToString(sEmail)));

        msg.From = new MailAddress(EmailID, "Visitor Management System");
        msg.Subject = EmailSubject;


        msg.IsBodyHtml = true;
       
        ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        if (DisableEmailWS == "True")
        {
          msg.Body = strHTML;
          msg.IsBodyHtml = true;


          if (byteImageVis != null)
          {
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(strHTML, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);

            msg.IsBodyHtml = true;

            msg.AlternateViews.Add(alternateView);
            msg.Attachments.Add(new Attachment(new MemoryStream(byteImageVis), "QRCode.jpg"));
          }
          else
          {
            msg.IsBodyHtml = true;
          }


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
            client.Timeout = (60 * 5 * 1000);

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
            client.UseDefaultCredentials = false;
            client.Port = Convert.ToInt16(EmailPort); // Usually 587 for STARTTLS
            client.Host = EmailSMTP;
            NetworkCredential basicCredential = new NetworkCredential(AccountName, EmailPassword);
            client.Credentials = basicCredential;
            client.Timeout = (60 * 5 * 1000);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            try
            {

              client.Send(msg);
              return "Send";
            }
            catch (Exception ex)
            {
              return ex.Message;
            }
          }



        }
        return "No Data";
      }
      return "No Data";
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
