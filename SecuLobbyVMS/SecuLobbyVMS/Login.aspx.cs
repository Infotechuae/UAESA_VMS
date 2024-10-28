using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class Login : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    string userName = "";
    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";
    protected void Page_Load(object sender, EventArgs e)
    {
      System.Security.Principal.WindowsPrincipal user;
      user = new WindowsPrincipal(this.Request.LogonUserIdentity);
      this.Request.LogonUserIdentity.Impersonate();
      if (!Page.IsPostBack)
      {
        Session.Clear();


        Session["isLoginSucess"] = "TRUE";
        Session["UserID"] = null;

        Session["UserFullName"] = null;
        Session["UserGroup"] = null;
        Session["UserLevel"] = null;
        Session["UserAccess"] = null;
        Session["UserProcess"] = null;
        Session["UserApprove"] = null;
        Session["UserUnProcess"] = null;
        Session["Email"] = null;
        Session["UserAccessLevel"] = null;
        Session["PAccessLevel"] = null;
        Session["IDAccessLevel"] = null;
        Session["UserGrpName"] = null;
        Session["UserGrpGraph"] = null;
        Session["User"] = null;
        Session["Phone"] = null;
        Session["Company"] = null;
        Session["Loc_ID"] = null;
        Session["User_Type"] = null;
        Session["Lang"] = null;
        Session["selfid"] = null;
        Session["Loc_name"] = null;
        Session["RoleID"] = null;
        

        userName = WindowsIdentity.GetCurrent().Name.ToString();
        txtusername.Text = userName.Substring(userName.LastIndexOf("\\") + 1);

        try
        {
          string sSql = "SELECT * FROM Tbl_Login_status WHERE UserName='" + txtusername.Text + "'";
          DataTable dtcheck = ocon.GetTable(sSql, new DataSet());

          if (dtcheck.Rows.Count > 0)
          {

          }
          else
          {
            string sSqlInsert = "INSERT INTO Tbl_Login_status (UserName,logintime,Loginsuccess) VALUES ('" + txtusername.Text + "','" + DateTime.Now + "',1)";
            ocon.Execute(sSqlInsert);
          }
        }
        catch (Exception ex)
        {
        }


        DataSet dsLogin = new DataSet();
        DataTable DT = new DataTable();
        string UserGroup = "";
        string str;
        string UserID = "";
        string UserFullName = "";
        string UserEmail = "";
        string UserLevel = "";
        string UserAccess = "";
        string UserProcess = "";
        string UserApprove = "";
        string UserUnProcess = "";
        string UserAccessLevel = "";
        string IDAccessLevel = "";
        string PAccessLevel = "";
        string UserGrpName = "";
        string UserGrpGraph = "";
        string Loc_name = "";
        string RoleID = "";
        string User_Type;

        string[,] infoArray = new string[3, 2];
        infoArray[0, 0] = "usercode";
        infoArray[0, 1] = txtusername.Text.ToString();
        infoArray[1, 0] = "Password";
        //infoArray[1, 1] = EncryptDecryptHelper.Encrypt(txtpassword.Text.Trim());
        infoArray[1, 1] = EncryptQRCODE(txtpassword.Text.Trim());
        infoArray[2, 0] = "MecID";
        infoArray[2, 1] = "WINAUTH";
        //   DAL.Utils.fetchRecordsDs("select userID, UserName,FullName,Pwd, Def_Loc_ID, Usr_group from User_Master where UserName='" + txtusername.Value + "'  and Pwd= '" + txtpassword.Value + "'", MyConnection.ReadConStr("Local"));
        dsLogin = DAL.Utils.fetchDSRecordsSP("User_Read", infoArray, "User_Read", MyConnection.ReadConStr("Local"));
        DT = dsLogin.Tables[0];

        if (DT.Rows.Count > 0)
        {
          if (DT.Select()[0].ItemArray[4].ToString() != "0")
          {
            UserID = DT.Select()[0].ItemArray[0].ToString();
            UserFullName = DT.Select()[0].ItemArray[2].ToString();
            UserEmail = DT.Select()[0].ItemArray[3].ToString();
            UserGroup = DT.Select()[0].ItemArray[5].ToString();

            UserLevel = DT.Select()[0].ItemArray[6].ToString();
            UserAccess = DT.Select()[0].ItemArray[7].ToString();
            UserProcess = DT.Select()[0].ItemArray[8].ToString();
            UserApprove = DT.Select()[0].ItemArray[9].ToString();
            UserUnProcess = DT.Select()[0].ItemArray[10].ToString();

            UserAccessLevel = DT.Select()[0].ItemArray[11].ToString();
            PAccessLevel = DT.Select()[0].ItemArray[12].ToString();
            IDAccessLevel = DT.Select()[0].ItemArray[13].ToString();
            UserGrpName = DT.Select()[0].ItemArray[14].ToString();
            UserGrpGraph = DT.Select()[0].ItemArray[15].ToString();
            User_Type = DT.Select()[0].ItemArray[16].ToString();
            Loc_name = DT.Select()[0].ItemArray[17].ToString();
            RoleID = DT.Select()[0].ItemArray[18].ToString();

            Session["isLoginSucess"] = "TRUE";
            Session["UserID"] = UserID;
            //Session["User"] = txtLogin.Value.ToString();
            Session["UserFullName"] = UserFullName;
            Session["UserGroup"] = UserGroup;
            Session["UserLevel"] = UserLevel;
            Session["UserAccess"] = UserAccess;
            Session["UserProcess"] = UserProcess;
            Session["UserApprove"] = UserApprove;
            Session["UserUnProcess"] = UserUnProcess;
            Session["Email"] = UserEmail;
            Session["UserAccessLevel"] = UserAccessLevel;
            Session["PAccessLevel"] = PAccessLevel;
            Session["IDAccessLevel"] = IDAccessLevel;
            Session["UserGrpName"] = UserGrpName;
            Session["UserGrpGraph"] = UserGrpGraph;
            Session["User"] = UserID;
            Session["Phone"] = DT.Select()[0].ItemArray[15].ToString();
            Session["Company"] = DT.Select()[0].ItemArray[16].ToString();
            Session["Loc_ID"] = DT.Select()[0].ItemArray[14].ToString();
            Session["User_Type"] = User_Type;
            Session["Loc_name"] = Loc_name;
            Session["RoleID"] = RoleID;
            Session["Lang"] = "en-GB";
            if (UserID.Length > 0)
            {
                Response.Redirect("MasterDashboard.aspx");

            }

          }
        }

      }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
      DBSQL db = new DBSQL();//declaring class
                             //   clsLogin lg = new clsLogin();
      DataSet dsLogin = new DataSet();
      DataTable DT = new DataTable();
      string UserGroup = "";
      string str;
      string UserID = "";
      string UserFullName = "";
      string UserEmail = "";
      string UserLevel = "";
      string UserAccess = "";
      string UserProcess = "";
      string UserApprove = "";
      string UserUnProcess = "";
      string UserAccessLevel = "";
      string IDAccessLevel = "";
      string PAccessLevel = "";
      string UserGrpName = "";
      string UserGrpGraph = "";
      string Loc_name = "";
      string RoleID = "";
      string User_Type;
      userName = WindowsIdentity.GetCurrent().Name.ToString();
      if (txtusername.Text.Equals(userName.Substring(userName.LastIndexOf("\\") + 1)))
      {
        string[,] infoArray = new string[3, 2];
        infoArray[0, 0] = "usercode";
        infoArray[0, 1] = txtusername.Text.ToString();
        infoArray[1, 0] = "Password";
        //infoArray[1, 1] = EncryptDecryptHelper.Encrypt(txtpassword.Text.Trim());
        infoArray[1, 1] = EncryptQRCODE(txtpassword.Text.Trim());
        infoArray[2, 0] = "MecID";
        infoArray[2, 1] = "WINAUTH";
        //   DAL.Utils.fetchRecordsDs("select userID, UserName,FullName,Pwd, Def_Loc_ID, Usr_group from User_Master where UserName='" + txtusername.Value + "'  and Pwd= '" + txtpassword.Value + "'", MyConnection.ReadConStr("Local"));
        dsLogin = DAL.Utils.fetchDSRecordsSP("User_Read", infoArray, "User_Read", MyConnection.ReadConStr("Local"));
        DT = dsLogin.Tables[0];

        if (DT.Rows.Count > 0)
        {
          if (DT.Select()[0].ItemArray[4].ToString() != "0")
          {
            //change by swati - ItemArray binding correction for all
            UserID = DT.Select()[0].ItemArray[0].ToString();
            UserFullName = DT.Select()[0].ItemArray[2].ToString();
            UserEmail = DT.Select()[0].ItemArray[3].ToString();
            UserGroup = DT.Select()[0].ItemArray[5].ToString();
            UserLevel = DT.Select()[0].ItemArray[6].ToString();
            UserAccess = DT.Select()[0].ItemArray[7].ToString();
            UserProcess = DT.Select()[0].ItemArray[8].ToString();
            UserApprove = DT.Select()[0].ItemArray[9].ToString();
            UserUnProcess = DT.Select()[0].ItemArray[10].ToString();
            UserAccessLevel = DT.Select()[0].ItemArray[11].ToString();
            PAccessLevel = DT.Select()[0].ItemArray[12].ToString();
            IDAccessLevel = DT.Select()[0].ItemArray[13].ToString();
            UserGrpName = DT.Select()[0].ItemArray[14].ToString();
            UserGrpGraph = DT.Select()[0].ItemArray[15].ToString();
            User_Type = DT.Select()[0].ItemArray[16].ToString();
            Loc_name = DT.Select()[0].ItemArray[17].ToString();
            RoleID = DT.Select()[0].ItemArray[18].ToString();

            if (chkRemember.Checked == true)
            {
              Response.Cookies["userid"].Value = txtusername.Text;
              Response.Cookies["pwd"].Value = txtpassword.Text;
              Response.Cookies["userid"].Expires = DateTime.Now.AddDays(15);
              Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(15);
            }
            else
            {
              Response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);
              Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);
            }


            Session["isLoginSucess"] = "TRUE";
            Session["UserID"] = UserID;
            //Session["User"] = txtLogin.Value.ToString();
            Session["UserFullName"] = UserFullName;
            Session["UserGroup"] = UserGroup;
            Session["UserLevel"] = UserLevel;
            Session["UserAccess"] = UserAccess;
            Session["UserProcess"] = UserProcess;
            Session["UserApprove"] = UserApprove;
            Session["UserUnProcess"] = UserUnProcess;
            Session["Email"] = UserEmail;
            Session["UserAccessLevel"] = UserAccessLevel;
            Session["PAccessLevel"] = PAccessLevel;
            Session["IDAccessLevel"] = IDAccessLevel;
            Session["UserGrpName"] = UserGrpName;
            Session["UserGrpGraph"] = UserGrpGraph;
            Session["User"] = UserID;
            Session["Phone"] = DT.Select()[0].ItemArray[15].ToString();
            Session["Company"] = DT.Select()[0].ItemArray[16].ToString();
            Session["Loc_ID"] = DT.Select()[0].ItemArray[14].ToString();
            Session["User_Type"] = User_Type;
            Session["Loc_name"] = Loc_name;
            Session["RoleID"] = RoleID;
            Session["Lang"] = "en-GB";
           

            if (UserID.Length > 0)
            {
                Response.Redirect("SelfRegistrationApproval.aspx?tabid=0&id=");
            }
            else
            {
              Session["isLoginSucess"] = "FALSE";
              Session["EMP"] = "0";

              ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt');", true);
              return;
            }


          }
          else
          {
            Session["isLoginSucess"] = "FALSE";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt');", true);
            return;

          }
        }


      }
      else
      {
        if (txtpassword.Text.Trim() == "" || txtusername.Text.Trim() == "")
        {
          if (txtpassword.Text.Trim() == "")
          {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt,Blank Password not Allowed');", true);
            return;
          }
          else
          {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt,Blank Username not Allowed');", true);
            return;

          }
        }
        else
        {
          string[,] infoArray = new string[3, 2];
          infoArray[0, 0] = "usercode";
          infoArray[0, 1] = txtusername.Text.ToString();
          infoArray[1, 0] = "Password";
          //infoArray[1, 1] = EncryptDecryptHelper.Encrypt(txtpassword.Text.Trim());
          infoArray[1, 1] = EncryptQRCODE(txtpassword.Text.Trim());
          infoArray[2, 0] = "MecID";
          infoArray[2, 1] = "";
          //   DAL.Utils.fetchRecordsDs("select userID, UserName,FullName,Pwd, Def_Loc_ID, Usr_group from User_Master where UserName='" + txtusername.Value + "'  and Pwd= '" + txtpassword.Value + "'", MyConnection.ReadConStr("Local"));
          dsLogin = DAL.Utils.fetchDSRecordsSP("User_Read", infoArray, "User_Read", MyConnection.ReadConStr("Local"));
          DT = dsLogin.Tables[0];

          if (DT.Rows.Count > 0)
          {
            if (DT.Select()[0].ItemArray[4].ToString() != "0")
            {
              UserID = DT.Select()[0].ItemArray[0].ToString();
              UserFullName = DT.Select()[0].ItemArray[2].ToString();
              UserEmail = DT.Select()[0].ItemArray[3].ToString();
              UserGroup = DT.Select()[0].ItemArray[5].ToString();

              UserLevel = DT.Select()[0].ItemArray[6].ToString();
              UserAccess = DT.Select()[0].ItemArray[7].ToString();
              UserProcess = DT.Select()[0].ItemArray[8].ToString();
              UserApprove = DT.Select()[0].ItemArray[9].ToString();
              UserUnProcess = DT.Select()[0].ItemArray[10].ToString();

              UserAccessLevel = DT.Select()[0].ItemArray[11].ToString();
              PAccessLevel = DT.Select()[0].ItemArray[12].ToString();
              IDAccessLevel = DT.Select()[0].ItemArray[13].ToString();
              UserGrpName = DT.Select()[0].ItemArray[14].ToString();
              UserGrpGraph = DT.Select()[0].ItemArray[15].ToString();
              User_Type = DT.Select()[0].ItemArray[16].ToString();
              Loc_name = DT.Select()[0].ItemArray[17].ToString();
              RoleID = DT.Select()[0].ItemArray[18].ToString();

              if (chkRemember.Checked == true)
              {
                Response.Cookies["userid"].Value = txtusername.Text;
                Response.Cookies["pwd"].Value = txtpassword.Text;
                Response.Cookies["userid"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(15);
              }
              else
              {
                Response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);
              }


              Session["isLoginSucess"] = "TRUE";
              Session["UserID"] = UserID;
              //Session["User"] = txtLogin.Value.ToString();
              Session["UserFullName"] = UserFullName;
              Session["UserGroup"] = UserGroup;
              Session["UserLevel"] = UserLevel;
              Session["UserAccess"] = UserAccess;
              Session["UserProcess"] = UserProcess;
              Session["UserApprove"] = UserApprove;
              Session["UserUnProcess"] = UserUnProcess;
              Session["Email"] = UserEmail;
              Session["UserAccessLevel"] = UserAccessLevel;
              Session["PAccessLevel"] = PAccessLevel;
              Session["IDAccessLevel"] = IDAccessLevel;
              Session["UserGrpName"] = UserGrpName;
              Session["UserGrpGraph"] = UserGrpGraph;
              Session["User"] = UserID;
              Session["Phone"] = DT.Select()[0].ItemArray[15].ToString();
              Session["Company"] = DT.Select()[0].ItemArray[16].ToString();
              Session["Loc_ID"] = DT.Select()[0].ItemArray[14].ToString();
              Session["User_Type"] = User_Type;
              Session["Loc_name"] = Loc_name;
              Session["RoleID"] = RoleID;
              Session["Lang"] = "en-GB";
             

              if (UserID.Length > 0)
              {

                int retval = 0;

                //Response.Redirect("MasterDashboard.aspx");

                //if (UserGroup == "1")
                //{
                //  Response.Redirect("MasterDashboard.aspx");
                //}
                //else if (UserGroup == "2")
                //{
                //  Response.Redirect("MasterDashboard.aspx");

                //}
                //if (UserGroup == "9")
                //{
                //  Response.Redirect("VisitorInvite.aspx");
                //}
                //else if (UserGroup == "10")//Kiosk
                //{
                //  Response.Redirect("SelfRegistration.aspx");
                //}
                //else if (UserGroup == "11")//Reception
                //{
                //  Response.Redirect("MasterDashboard.aspx");
                //}
                //else if (UserGroup == "1")//Admin
                //{
                //  Response.Redirect("MasterDashboard.aspx");
                //}
                //else if (UserGroup == "2")//Report
                //{
                //  Response.Redirect("VisitorTransaction.aspx");
                //}
                //else if (UserGroup == "3")//User
                //{
                //  Response.Redirect("MasterDashboard.aspx");
                //}
                //else
                //{
                //  Response.Redirect("SelfRegistrationApproval.aspx?tabid=0&id=");
                //}

                //Change by swati - for Role based access //
                Response.Redirect("MasterDashboard.aspx");

              }
              else
              {
                Session["isLoginSucess"] = "FALSE";
                Session["EMP"] = "0";

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt');", true);
                return;
              }


            }
            else
            {
              Session["isLoginSucess"] = "FALSE";
              ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt');", true);
              return;

            }
          }
          else
          {


            Session["isLoginSucess"] = "FALSE";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Invalid login attempt');", true);
            return;


          }

          db = null;
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
  }
}
