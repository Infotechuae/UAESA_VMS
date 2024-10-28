using DAL;
using DAL.ADConnectors;
using SecuLobbyVMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class SubMailSettings : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    ResourceManager rm;
    CultureInfo ci;
    string EmailPassword = "";
    string Accountpass = "";
    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        header1.Text = rm.GetString("STRN_8", ci);
        lblEmailType.Text = rm.GetString("STRN_22", ci);
        lblSMTP.Text = rm.GetString("STRN_23", ci);
        lblEmail.Text = rm.GetString("STR_7", ci);
        lblPassword.Text = rm.GetString("STRN_24", ci);
        lblPort.Text = rm.GetString("STRN_25", ci);
        lblSSl.Text = rm.GetString("STRN_26", ci);
        lblEmailName.Text = rm.GetString("STRN_27", ci);
        Label4.Text = rm.GetString("STRN_28", ci);
        Label1.Text = rm.GetString("STRN_29", ci);
        Label2.Text = rm.GetString("STRN_30", ci);
        Label5.Text = rm.GetString("STRN_31", ci);

        btnSave.Text = rm.GetString("STRN_21", ci);
        btnAD.Text = rm.GetString("STRN_35", ci);

        btnReset.Text = rm.GetString("STR_19", ci);

        string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl,[ADServiceAccountName],[ADDomainPath],[ADOUS],ADServiceAccountPassword FROM MailSettings";
        DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());
        if (dtMailSetting.Rows.Count > 0)
        {
          txtEmail.Text = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
          EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
          //txtPassword.Text = EncryptDecryptHelper.Decrypt(EmailPassword);
          txtPassword.Text = DecryptQRCODE(EmailPassword);
          txtSMTP.Text = Convert.ToString(dtMailSetting.Rows[0]["SMTPServer"]);
          txtPort.Text = Convert.ToString(dtMailSetting.Rows[0]["EmailPort"]);
          drpEmailType.SelectedValue = Convert.ToString(dtMailSetting.Rows[0]["EmailAccount"]);
          txtEmailName.Text = Convert.ToString(dtMailSetting.Rows[0]["AccountName"]);
          drpSsl.SelectedValue = Convert.ToString(dtMailSetting.Rows[0]["IsSSl"]);
          txtServiceAccountName.Text = Convert.ToString(dtMailSetting.Rows[0]["ADServiceAccountName"]);
          txtDomainPath.Text = Convert.ToString(dtMailSetting.Rows[0]["ADDomainPath"]);
          textOUS.Text = Convert.ToString(dtMailSetting.Rows[0]["ADOUS"]);
          Accountpass = Convert.ToString(dtMailSetting.Rows[0]["ADServiceAccountPassword"]);
          //txtAccountPass.Text = EncryptDecryptHelper.Decrypt(Accountpass);
          txtAccountPass.Text = DecryptQRCODE(Accountpass);
        }
        else
        {
          txtEmail.Text = "";
          txtPassword.Text = "";
          txtSMTP.Text = "";
          txtPort.Text = "";
          txtEmailName.Text = "";
          txtServiceAccountName.Text = "";
          txtDomainPath.Text = "";
          textOUS.Text = "";
          txtAccountPass.Text = "";
        }


      }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
      Response.Redirect("MailSettings.aspx");
    }

    protected void btnGetAD_Click(object sender, EventArgs e)
    {
      var ServiceAccountName = txtServiceAccountName.Text.Trim();

      var DomainPath = txtDomainPath.Text.Trim();
      string strOUS = textOUS.Text.Trim();
      var OUS = new List<string>();

      string[] OUSLines = strOUS.Split(';');


      foreach (var OUSLine in OUSLines)
      {
        OUS.Add(OUSLine);
      }

      string sSql = "SELECT ID, ADServiceAccountPassword  FROM MailSettings";
      DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());
      string ServiceAccountPassword = "";
      string sUpdate = string.Empty;
      if (dtMailSetting.Rows.Count > 0)
      {
        string sId = Convert.ToString(dtMailSetting.Rows[0]["ID"]);

        //ServiceAccountPassword = EncryptDecryptHelper.Decrypt(Convert.ToString(dtMailSetting.Rows[0]["ADServiceAccountPassword"]));
        ServiceAccountPassword = EncryptQRCODE(Convert.ToString(dtMailSetting.Rows[0]["ADServiceAccountPassword"]));
      }


      var Records = ActiveDirectoryHelpers.GetRecords(ServiceAccountName, ServiceAccountPassword, DomainPath, OUS);

      int i = 0;

      string sSqlMax = "select isnull(max(pl_id),0) as pl_id from PickList_tran where pl_head_id=18";
      DataTable dt = ocon.GetTable(sSqlMax, new DataSet());
      if (dt.Rows.Count > 0)
      {
        i = Convert.ToInt32(dt.Rows[0]["pl_id"]);
      }

      i = i + 1;


      int j = 0;

      string sSqlMaxDepartment = "select isnull(max(pl_id),0) as pl_id from PickList_tran where pl_head_id=4";
      DataTable dtMaxDepartment = ocon.GetTable(sSqlMaxDepartment, new DataSet());
      if (dtMaxDepartment.Rows.Count > 0)
      {
        j = Convert.ToInt32(dtMaxDepartment.Rows[0]["pl_id"]);
      }

      j = j + 1;

      foreach (var Record in Records)
      {
        int iDeptId = 0;
        #region Department

        string sSqlDeptExists = "select * from PickList_tran where pl_head_id=4 and pl_Value='" + Record.Department.Trim() + "'";
        DataTable dtDept = ocon.GetTable(sSqlDeptExists, new DataSet());
        if (dtDept.Rows.Count > 0)
        {
          iDeptId = Convert.ToInt32(dtDept.Rows[0]["pl_id"]);
        }
        else
        {
          string sInsert = "insert into PickList_tran  (pl_head_id,pl_id,pl_Value)"
                      + " values (4,'" + j + "','" + Record.Department.Trim() + "' )";

          ocon.Execute(sInsert);


          iDeptId = j;
          j++;
        }

        #endregion

        #region Employee

        string sSqlEmpExists = "select * from PickList_tran where pl_head_id=18 and pl_Data='" + Record.Email.Trim() + "'";
        DataTable dtEmp = ocon.GetTable(sSqlEmpExists, new DataSet());
        if (dtEmp.Rows.Count > 0)
        { }
        else
        {
          try
          {
            string sInsert = "insert into PickList_tran  (pl_head_id,pl_id,pl_Value,pl_Data,Other_Data1,Other_Data2)"
                                 + " values (18,'" + i + "','" + Record.FullName.Trim() + "', '" + Record.Email.Trim() + "', '" + Record.Department.Trim() + "', '" + Record.ManagerName.Trim() + "')";

            ocon.Execute(sInsert);
          }
          catch (Exception ex)
          {
            continue;
          }

          try
          {
           // string sPassword = EncryptDecryptHelper.Encrypt("P@ssw0rd123");

            string sPassword = EncryptQRCODE("P@ssw0rd123");

            string sInsertUser = "insert into [dbo].[Users] ([UserCode],[UserName],  [Phone],  [Pwd],  [UserActive],  [UserApprove],   [UserEmail],  UserGroup,Loc_Name,Location_ID)  "
                            + " values('" + Record.Email.Trim() + "', '" + Record.FullName.Trim() + "', '" + Record.Phone + "', '" + sPassword + "', 1, 1,'" + Record.Email.Trim() + "','3', 'Location 1',1) ";
            ocon.Execute(sInsertUser);
          }
          catch (Exception ex)
          {
            continue;
          }

          i++;
        }
        #endregion

      }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
      if (drpEmailType.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please Select Email Type');", true);
        return;
      }
      if (drpSsl.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please select SSl Requied');", true);
        return;
      }

      string sSql = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl,ADServiceAccountPassword FROM MailSettings";
      DataTable dtMailSetting = ocon.GetTable(sSql, new DataSet());

      string sUpdate = string.Empty;
      if (dtMailSetting.Rows.Count > 0)
      {
        string sId = Convert.ToString(dtMailSetting.Rows[0]["ID"]);
        EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
        //EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
        EmailPassword= EncryptQRCODE(EmailPassword);
        Accountpass = Convert.ToString(dtMailSetting.Rows[0]["ADServiceAccountPassword"]);
        //Accountpass = EncryptDecryptHelper.Decrypt(Accountpass);
        Accountpass = EncryptQRCODE(Accountpass);

        if (txtPassword.Text != "")
        {
          if (EmailPassword != txtPassword.Text.Trim())
          {

            //string sNewPassword = EncryptDecryptHelper.Encrypt(txtPassword.Text.Trim());
            string sNewPassword = EncryptQRCODE(txtPassword.Text.Trim());

            sUpdate = "update dbo.MailSettings set EmailID='" + txtEmail.Text.Trim() + "',SMTPServer='" + txtSMTP.Text.Trim() + "',EmailPort='" + txtPort.Text.Trim() + "',EmailPWD='" + sNewPassword + "',EmailAccount='" + drpEmailType.SelectedItem.Text.Trim() + "',AccountName='" + txtEmailName.Text.Trim() + "',IsSSl='" + drpSsl.SelectedItem.Text.Trim() + "',ADServiceAccountName ='" + txtServiceAccountName.Text.Trim() + "',ADDomainPath='" + txtDomainPath.Text.Trim() + "',ADOUS='" + textOUS.Text.Trim() + "'  where ID='" + sId + "'";

          }
          else
          {
            sUpdate = "update dbo.MailSettings set EmailID='" + txtEmail.Text.Trim() + "',SMTPServer='" + txtSMTP.Text.Trim() + "',EmailPort='" + txtPort.Text.Trim() + "',EmailAccount='" + drpEmailType.SelectedItem.Text.Trim() + "',AccountName='" + txtEmailName.Text.Trim() + "',IsSSl='" + drpSsl.SelectedItem.Text.Trim() + "',ADServiceAccountName ='" + txtServiceAccountName.Text.Trim() + "',ADDomainPath='" + txtDomainPath.Text.Trim() + "',ADOUS='" + textOUS.Text.Trim() + "'  where ID='" + sId + "'";

          }
        }
        else
        {
          sUpdate = "update dbo.MailSettings set EmailID='" + txtEmail.Text.Trim() + "',SMTPServer='" + txtSMTP.Text.Trim() + "',EmailPort='" + txtPort.Text.Trim() + "',EmailAccount='" + drpEmailType.SelectedItem.Text.Trim() + "',AccountName='" + txtEmailName.Text.Trim() + "',IsSSl='" + drpSsl.SelectedItem.Text.Trim() + "',ADServiceAccountName ='" + txtServiceAccountName.Text.Trim() + "',ADDomainPath='" + txtDomainPath.Text.Trim() + "',ADOUS='" + textOUS.Text.Trim() + "'  where ID='" + sId + "'";

        }

        ocon.Execute(sUpdate);


        if (txtAccountPass.Text != "")
        {
          if (Accountpass != txtAccountPass.Text.Trim())
          {
            //string sNewAccountPassword = EncryptDecryptHelper.Encrypt(txtAccountPass.Text.Trim());

            string sNewAccountPassword = EncryptQRCODE(txtAccountPass.Text.Trim());

            string sUdate1 = "update dbo.MailSettings set ADServiceAccountPassword='" + sNewAccountPassword + "' where ID='" + sId + "'";

            ocon.Execute(sUdate1);

          }
        }

        string sDestURL = string.Format("\"{0}\"", "MailSettings.aspx");
        string smessage = string.Format("\"{0}\"", "Mail setting updated successfully");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
      else
      {
        if (txtPassword.Text == "")
        {
          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter password');", true);
          return;
        }

        //string sNewPassword = EncryptDecryptHelper.Encrypt(txtPassword.Text.Trim());
        //string sNewAccountPassword = EncryptDecryptHelper.Encrypt(txtAccountPass.Text.Trim());

        string sNewPassword = EncryptQRCODE(txtPassword.Text.Trim());
        string sNewAccountPassword = EncryptQRCODE(txtAccountPass.Text.Trim());

        string sInsert = "insert into dbo.MailSettings  (EmailID, EmailPWD, SMTPServer, EmailPort, IsSSl, EmailAccount, AccountName,ADServiceAccountName,ADDomainPath,ADOUS,ADServiceAccountPassword)"
                      + " values ('" + txtEmail.Text.Trim() + "', '" + sNewPassword + "', '" + txtSMTP.Text.Trim() + "', '" + txtPort.Text.Trim() + "', '" + drpSsl.SelectedItem.Text.Trim() + "', '" + drpEmailType.SelectedItem.Text.Trim() + "', '" + txtEmailName.Text.Trim() + "', '" + txtServiceAccountName.Text.Trim() + "', '" + txtDomainPath.Text.Trim() + "', '" + textOUS.Text.Trim() + "','" + sNewAccountPassword + "')";

        ocon.Execute(sInsert);

        string sDestURL = string.Format("\"{0}\"", "MailSettings.aspx");
        string smessage = string.Format("\"{0}\"", "Mail setting added successfully");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
      string strHTML1 = "<p> Dear,</p>";
      strHTML1 = strHTML1 + "<p>This test email.</p>";
      try
      {
        sendEmail.SendEmails("Test Email", txtTestemail.Text, "3434", strHTML1, "Test Email", null, null);
        string sDestURL = string.Format("\"{0}\"", "MailSettings.aspx");
        string smessage = string.Format("\"{0}\"", "Email sent successfully");
        string sVar = sDestURL + "," + smessage;
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
      catch (Exception ex)
      {
        string sDestURL = string.Format("\"{0}\"", "MailSettings.aspx");
        string smessage = string.Format("\"{0}\"", "Email not sent <br/>" + ex.ToString());
        string sVar = sDestURL + "," + smessage;
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
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
