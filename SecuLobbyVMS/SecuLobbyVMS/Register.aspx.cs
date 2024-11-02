using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class Register : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    DataSet rDS = null;
    string tableName = "Users";
    string Sp_name = "UsersMast_Update";

    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";

    protected void Page_Load(object sender, EventArgs e)
    {

      if (!IsPostBack && !IsCallback)
      {

        BindDropDown();
      }
    }

    protected void btnSignup_Click(object sender, EventArgs e)
    {
      if (chkTerms.Checked)
      {
        try
        {
          Random rnd = new Random();
          int myRandomNo = rnd.Next(1000000, 9999999);
          string sOTP = myRandomNo.ToString();


          SqlCommand cmdOrl = new SqlCommand("GetNewRegistation_New");
          cmdOrl.CommandType = CommandType.StoredProcedure;
          cmdOrl.Parameters.AddWithValue("@UserCode", txtUsername.Text);
          //cmdOrl.Parameters.AddWithValue("@Password", EncryptDecryptHelper.Encrypt(txtpassword.Text.Trim()));
          cmdOrl.Parameters.AddWithValue("@Password", EncryptQRCODE(txtpassword.Text.Trim()));
          cmdOrl.Parameters.AddWithValue("@UserFullName", txtFullName.Text);
          cmdOrl.Parameters.AddWithValue("@Email", txtEmail.Text);
          cmdOrl.Parameters.AddWithValue("@Phone", txtMobile.Text);
          cmdOrl.Parameters.AddWithValue("@Company", txtcompany.Text);
          cmdOrl.Parameters.AddWithValue("@Location_ID", drpLocation.SelectedIndex);
          cmdOrl.Parameters.AddWithValue("@LoginOTP", sOTP);


          DAL.DALSQL dbs1 = new DAL.DALSQL();
          string rval = dbs1.ExecuteStoredProcedurereturn(cmdOrl, MyConnection.ReadConStr("Local"));

          SendEmail(txtFullName.Text, txtEmail.Text, sOTP);

          string sDestURL = string.Format("\"{0}\"", "VerifyOTP.aspx?LogID=" + rval);
          string smessage = string.Format("\"{0}\"", "Registration Success." + "<br>" + "<br>" + "<h6>" + "Thank you.We have sent you an email with OTP." + "<br>" + "Please verify that OTP in next step to activate your account." + "</h6>");

          string sVar = sDestURL + "," + smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
          //}
          //else
          //{

          //  ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Already registered with the system');", true);
          //  return;
          //}

        }
        catch (Exception ex)
        {
          throw ex;
        }

      }
      else
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please accept the Terms and Conditions');", true);
        return;
      }
    }

    private void BindDropDown()
    {

      #region Location


      DataTable dtloc = ocon.GetTable("SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=23 order by pl_Value", new DataSet());
      if (dtloc.Rows.Count > 0)
      {
        drpLocation.DataSource = dtloc;
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

    }
    private void SendEmail(string sName, string sEmail, string sOTP)
    {
      CreateLog("Mail Started");


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
        EmailPassword = DecryptQRCODE(EmailPassword);
        EmailSMTP = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
        EmailPort = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
        EmailAccount = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
        AccountName = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
        iSSl = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);


        CreateLog("Mail setting :" + EmailID + " , " + EmailPassword + " , " + EmailSMTP + " , " + EmailPort + " , " + EmailAccount + " , " + iSSl);

        MailMessage msg = new MailMessage();
        msg.To.Add(new MailAddress(Convert.ToString(sEmail)));

        CreateLog("Mail To: " + sEmail);

        msg.From = new MailAddress(EmailID, "Visitor Management System");
        msg.Subject = EmailSubject;
        string strHTML = "<p> Dear " + sName + ",</p>";
        strHTML = strHTML + "<p>  You have successfully registered as visitor. Your OTP for registration is " + sOTP + ". Use this OTP to validate your registration. </p>";

        msg.IsBodyHtml = true;

        CreateLog("Mail Body :" + strHTML);

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
          ServicePointManager.ServerCertificateValidationCallback = (sender1, cert, chain, sslPolicyErrors) => true;
          System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
          System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

          //commented by shafeeq
          SmtpClient client = new SmtpClient();
          client.UseDefaultCredentials = false;
          client.Port = Convert.ToInt16(EmailPort); // Usually 587 for STARTTLS
          client.Host = EmailSMTP;

          NetworkCredential basicCredential = new NetworkCredential(AccountName, EmailPassword);
          client.Credentials = basicCredential;
          client.Timeout = (60 * 5 * 1000);
          client.DeliveryMethod = SmtpDeliveryMethod.Network;
          client.EnableSsl = true;

          //added by shafeeq
          //SmtpClient client = new SmtpClient();
          //client.Credentials = new System.Net.NetworkCredential(EmailID, EmailPassword);
          //client.Port = Convert.ToInt16(EmailPort);
          //client.Host = EmailSMTP;
          //client.DeliveryMethod = SmtpDeliveryMethod.Network;
          //client.EnableSsl = Convert.ToBoolean(iSSl);

          CreateLog("Step 3");

          try
          {
            client.Send(msg);

            CreateLog("Success");
          }
          catch (Exception ex)
          {
            CreateLog(ex.ToString());
          }
        }

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

    public static void CreateLog(string sMessage)
    {

      //System.Data.DataRow drUser = ((System.Data.DataRow)System.Web.HttpContext.Current.Session["UserInfoRow"]);
      string sUserID = "System";
      string sError = Environment.NewLine + "Date and Time : " + DateTime.Now + " ; Message : " + sMessage;
      string InitialPath = System.Web.HttpContext.Current.Server.MapPath("~/images/Temp/Log");
      if (!System.IO.Directory.Exists(InitialPath)) //Log Folder Checking 
      {
        System.IO.Directory.CreateDirectory(InitialPath);
      }
      string SubFolderRelativePath = Convert.ToString(DateTime.Today.ToString("dd-MM-yyyy"));//First SubFolder Name

      string subfolderPath = System.IO.Path.Combine(InitialPath, SubFolderRelativePath);//First Sub Folder Path 
      string subOfsubFolder = System.IO.Path.Combine(subfolderPath, Convert.ToString(sUserID));//Second SubFolder Path 
      string tempFilePath = System.IO.Path.Combine(subOfsubFolder, Convert.ToString(DateTime.Today.ToString("dd-MM-yyyy")));// Text File Path

      System.IO.DirectoryInfo tempFolder = new System.IO.DirectoryInfo(InitialPath); //Initial Path
      System.IO.DirectoryInfo newTempPath = new System.IO.DirectoryInfo(subfolderPath); //First Sub Folder Path in DirectoryInfo

      string[] sErr = { sError };

      if (!System.IO.Directory.Exists(subfolderPath)) // First Sub Folder Check
      {
        System.IO.DirectoryInfo subFolder = tempFolder.CreateSubdirectory(SubFolderRelativePath); // First SubFolder Create Using Date(Folder Name)


        #region CreateSubFolderOfSubFolder
        System.IO.DirectoryInfo subfolderOfSub = subFolder.CreateSubdirectory(Convert.ToString(sUserID));// Second SubFolder Create Using UserID(Folder Name)
        #endregion

        System.IO.File.WriteAllLines(tempFilePath, sErr); // File Creation
      }
      else
      {
        #region CreateSubFolder
        if (!System.IO.Directory.Exists(subOfsubFolder))  // Second Sub Folder Check
        {
          #region CreateSubFolderOfSubFolder
          System.IO.DirectoryInfo subfolderOfSub = newTempPath.CreateSubdirectory(Convert.ToString(sUserID));// Second SubFolder Create Using UserID(Folder Name)
          System.IO.File.WriteAllLines(tempFilePath, sErr);
          #endregion
        }
        else
        {
          System.IO.File.AppendAllText(tempFilePath, sError);
        }

        #endregion
      }

    }

  }
}
