using DAL;
using System;
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
  public partial class User : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    string sUserType = "2";
    ResourceManager rm;
    CultureInfo ci;
    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";
    protected void Page_Load(object sender, EventArgs e)
    {

      if (!Page.IsPostBack)
      {
        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);
        BindDropDown();


        int iTabId = Convert.ToInt32(Request.QueryString["ID"]);

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);

        string sSql = "select UserID,UserCode,UserName,UserEmail,Phone,Pwd,UserGroup,Location_ID from Users where UserID ='" + iTabId + "'";
        DataTable dt = ocon.GetTable(sSql, new DataSet());
        //case when UserGroup = 1 then 'Admin' else 'User' end as
        if (dt.Rows.Count > 0)
        {
          txtID.Text = Convert.ToString(dt.Rows[0]["UserCode"]);
          txtName.Text = Convert.ToString(dt.Rows[0]["UserName"]);
          string sPwd = Convert.ToString(dt.Rows[0]["Pwd"]);
          //txtPassword.Text = EncryptDecryptHelper.Decrypt(sPwd);
          txtPassword.Text = DecryptQRCODE(sPwd);


          //txtPassword.Attributes["value"] = EncryptDecryptHelper.Decrypt(sPwd);
          txtPassword.Attributes["value"] = DecryptQRCODE(sPwd);
          txtemail.Text = Convert.ToString(dt.Rows[0]["UserEmail"]);
          txtPhone.Text = Convert.ToString(dt.Rows[0]["Phone"]);
          drpType.SelectedValue = Convert.ToString(dt.Rows[0]["UserGroup"]);
          drpLoc.SelectedValue = Convert.ToString(dt.Rows[0]["Location_ID"]);
          txtID.ReadOnly = false;
        }
        else
        {
          txtID.Text = "";
          txtName.Text = "";
          txtemail.Text = "";
          txtPhone.Text = "";
          txtPassword.Text = "";

          txtID.ReadOnly = false;
        }



        System.Web.UI.HtmlControls.HtmlGenericControl lnkDashboard = Master.FindControl("lnkDashboard") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkSelfReg = Master.FindControl("lnkSelfReg") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkRegis = Master.FindControl("lnkRegis") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCheckin = Master.FindControl("lnkCheckin") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCheckOut = Master.FindControl("lnkCheckOut") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkTimeOut = Master.FindControl("lnkTimeOut") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkWatchList = Master.FindControl("lnkWatchList") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkVisMas = Master.FindControl("lnkVisMas") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkVisTran = Master.FindControl("lnkVisTran") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkAllReqReport = Master.FindControl("lnkAllReqReport") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkPrereg = Master.FindControl("lnkPrereg") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkLocation = Master.FindControl("lnkLocation") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liDepart = Master.FindControl("liDepart") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liFloor = Master.FindControl("liFloor") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liEmployee = Master.FindControl("liEmployee") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liVisitorType = Master.FindControl("liVisitorType") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCard = Master.FindControl("lnkCard") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liUser = Master.FindControl("liUser") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liMail = Master.FindControl("liMail") as System.Web.UI.HtmlControls.HtmlGenericControl;

        System.Web.UI.HtmlControls.HtmlGenericControl lnkRqstlistAll = Master.FindControl("lnkRqstlistAll") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkRqstlistAccepted = Master.FindControl("lnkRqstlistAccepted") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkRqstlistPending = Master.FindControl("lnkRqstlistPending") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkRqstlistRejected = Master.FindControl("lnkRqstlistRejected") as System.Web.UI.HtmlControls.HtmlGenericControl;



        System.Web.UI.HtmlControls.HtmlAnchor aDash = Master.FindControl("aDash") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aSelfRegis = Master.FindControl("aSelfRegis") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aRegis = Master.FindControl("aRegis") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCheckIN = Master.FindControl("aCheckIN") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCheckOut = Master.FindControl("aCheckOut") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aTimeOut = Master.FindControl("aTimeOut") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aWatchList = Master.FindControl("aWatchList") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisMas = Master.FindControl("aVisMas") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisTrans = Master.FindControl("aVisTrans") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aAllReqReport = Master.FindControl("aAllReqReport") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aPrereg = Master.FindControl("aPrereg") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor adepart = Master.FindControl("adepart") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aLocation = Master.FindControl("aLocation") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aFloor = Master.FindControl("aFloor") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aEmployee = Master.FindControl("aEmployee") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisitorType = Master.FindControl("aVisitorType") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCard = Master.FindControl("aCard") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aUser = Master.FindControl("aUser") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aMail = Master.FindControl("aMail") as System.Web.UI.HtmlControls.HtmlAnchor;

        System.Web.UI.HtmlControls.HtmlAnchor aAll = Master.FindControl("aAll") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aAccepted = Master.FindControl("aAccepted") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aPending = Master.FindControl("aPending") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aRejected = Master.FindControl("aRejected") as System.Web.UI.HtmlControls.HtmlAnchor;


        lnkDashboard.Attributes.Remove("class");
        lnkSelfReg.Attributes.Remove("class");
        lnkRegis.Attributes.Remove("class");
        lnkCheckin.Attributes.Remove("class");
        lnkCheckOut.Attributes.Remove("class");
        lnkTimeOut.Attributes.Remove("class");
        lnkWatchList.Attributes.Remove("class");
        lnkVisMas.Attributes.Remove("class");
        lnkVisTran.Attributes.Remove("class");
        lnkAllReqReport.Attributes.Remove("class");
        lnkPrereg.Attributes.Remove("class");
        lnkLocation.Attributes.Remove("class");
        liDepart.Attributes.Remove("class");
        liFloor.Attributes.Remove("class");
        liEmployee.Attributes.Remove("class");
        liVisitorType.Attributes.Remove("class");
        lnkCard.Attributes.Remove("class");
        liUser.Attributes.Remove("class");
        liMail.Attributes.Remove("class");
        lnkRqstlistAll.Attributes.Remove("class");
        lnkRqstlistAccepted.Attributes.Remove("class");
        lnkRqstlistPending.Attributes.Remove("class");
        lnkRqstlistRejected.Attributes.Remove("class");

        aDash.Attributes.Remove("class");
        aSelfRegis.Attributes.Remove("class");
        aRegis.Attributes.Remove("class");
        aCheckIN.Attributes.Remove("class");
        aCheckOut.Attributes.Remove("class");
        aTimeOut.Attributes.Remove("class");
        aWatchList.Attributes.Remove("class");
        aVisMas.Attributes.Remove("class");
        aVisTrans.Attributes.Remove("class");
        aAllReqReport.Attributes.Remove("class");
        aPrereg.Attributes.Remove("class");
        aLocation.Attributes.Remove("class");
        adepart.Attributes.Remove("class");
        aFloor.Attributes.Remove("class");
        aEmployee.Attributes.Remove("class");
        aVisitorType.Attributes.Remove("class");
        aCard.Attributes.Remove("class");
        aUser.Attributes.Remove("class");
        aMail.Attributes.Remove("class");
        aAll.Attributes.Remove("class");
        aAccepted.Attributes.Remove("class");
        aPending.Attributes.Remove("class");
        aRejected.Attributes.Remove("class");

        lnkDashboard.Attributes.Add("class", "nav-item");
        lnkSelfReg.Attributes.Add("class", "nav-item");
        lnkRegis.Attributes.Add("class", "nav-item");
        lnkCheckin.Attributes.Add("class", "nav-item");
        lnkCheckOut.Attributes.Add("class", "nav-item");
        lnkTimeOut.Attributes.Add("class", "nav-item");
        lnkWatchList.Attributes.Add("class", "nav-item");
        lnkVisMas.Attributes.Add("class", "nav-item");
        lnkVisTran.Attributes.Add("class", "nav-item");
        lnkAllReqReport.Attributes.Add("class", "nav-item");
        lnkPrereg.Attributes.Add("class", "nav-item");
        liDepart.Attributes.Add("class", "nav-item");
        liFloor.Attributes.Add("class", "nav-item");
        liEmployee.Attributes.Add("class", "nav-item");
        liVisitorType.Attributes.Add("class", "nav-item");
        lnkCard.Attributes.Add("class", "nav-item");
        liUser.Attributes.Add("class", "nav-item menu-open");
        liMail.Attributes.Add("class", "nav-item");
        lnkRqstlistAll.Attributes.Add("class", "nav-item");
        lnkRqstlistAccepted.Attributes.Add("class", "nav-item");
        lnkRqstlistPending.Attributes.Add("class", "nav-item");
        lnkRqstlistRejected.Attributes.Add("class", "nav-item");

        aDash.Attributes.Add("class", "nav-link");
        aSelfRegis.Attributes.Add("class", "nav-link");
        aRegis.Attributes.Add("class", "nav-link");
        aCheckIN.Attributes.Add("class", "nav-link");
        aCheckOut.Attributes.Add("class", "nav-link");
        aTimeOut.Attributes.Add("class", "nav-link");
        aWatchList.Attributes.Add("class", "nav-link");
        aVisMas.Attributes.Add("class", "nav-link");
        aVisTrans.Attributes.Add("class", "nav-link");
        aAllReqReport.Attributes.Add("class", "nav-link");
        aPrereg.Attributes.Add("class", "nav-link");
        adepart.Attributes.Add("class", "nav-link");
        aFloor.Attributes.Add("class", "nav-link");
        aEmployee.Attributes.Add("class", "nav-link");
        aVisitorType.Attributes.Add("class", "nav-link");
        aCard.Attributes.Add("class", "nav-link");
        aUser.Attributes.Add("class", "nav-link active");
        aMail.Attributes.Add("class", "nav-link");
        aAll.Attributes.Add("class", "nav-link");
        aAccepted.Attributes.Add("class", "nav-link");
        aPending.Attributes.Add("class", "nav-link");
        aRejected.Attributes.Add("class", "nav-link");

        System.Web.UI.HtmlControls.HtmlGenericControl lnkPurpose = Master.FindControl("lnkPurpose") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor aPurpose = Master.FindControl("aPurpose") as System.Web.UI.HtmlControls.HtmlAnchor;
        lnkPurpose.Attributes.Remove("class");
        aPurpose.Attributes.Remove("class");
        lnkPurpose.Attributes.Add("class", "nav-item");
        aPurpose.Attributes.Add("class", "nav-link");

        System.Web.UI.HtmlControls.HtmlGenericControl liMailtemplate = Master.FindControl("liMailtemplate") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor aMailtemplate = Master.FindControl("aMailtemplate") as System.Web.UI.HtmlControls.HtmlAnchor;
        liMailtemplate.Attributes.Remove("class");
        aMailtemplate.Attributes.Remove("class");
        liMailtemplate.Attributes.Add("class", "nav-item");
        aMailtemplate.Attributes.Add("class", "nav-link");

        System.Web.UI.HtmlControls.HtmlGenericControl li1 = Master.FindControl("li1") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor a1 = Master.FindControl("a1") as System.Web.UI.HtmlControls.HtmlAnchor;
        li1.Attributes.Remove("class");
        a1.Attributes.Remove("class");
        li1.Attributes.Add("class", "nav-item");
        a1.Attributes.Add("class", "nav-link");


        System.Web.UI.HtmlControls.HtmlGenericControl li2 = Master.FindControl("li2") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor a2 = Master.FindControl("a2") as System.Web.UI.HtmlControls.HtmlAnchor;
        li2.Attributes.Remove("class");
        a2.Attributes.Remove("class");
        li2.Attributes.Add("class", "nav-item");
        a2.Attributes.Add("class", "nav-link");
      }
    }

    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STRN_5", ci);
      btnSave.Text = rm.GetString("STRN_21", ci);
      btnReset.Text = rm.GetString("STR_19", ci);

      Label4.Text = rm.GetString("STRN_39", ci);
      lblName.Text = rm.GetString("STRN_37", ci);
      Label1.Text = rm.GetString("STRN_24", ci);
      lblEmail.Text = rm.GetString("STR_7", ci);
      lblPhone.Text = rm.GetString("STRN_38", ci);
      Label2.Text = rm.GetString("STRN_20", ci);
      Label3.Text = rm.GetString("STR_56", ci);
    }
    protected void btnusersearch_Click(object sender, EventArgs e)
    {

      string suSql = "SELECT pl_id, pl_Value, pl_Data, Card_next_ID, ar_pl_Value, Other_Data1, Other_Data2 FROM PickList_tran WHERE    (pl_head_id = 18) and ( pl_Value like '" + txtID.Text.Trim() + "%' OR pl_Data like '" + txtID.Text.Trim() + "%' )  ";
      DataTable udt = ocon.GetTable(suSql, new DataSet());
      if (udt.Rows.Count > 0)
      {
        txtID.Text = Convert.ToString(udt.Rows[0]["pl_Data"]);
        txtName.Text = Convert.ToString(udt.Rows[0]["pl_Value"]);

        txtemail.Text = Convert.ToString(udt.Rows[0]["pl_Data"]);
        txtPhone.Text = "";
      }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
      int iTabId = Convert.ToInt32(Request.QueryString["ID"]);


      string sSql = "SELECT * FROM Users WHERE UserID='" + iTabId + "'";
      DataTable dt = ocon.GetTable(sSql, new DataSet());
      if (dt.Rows.Count > 0)
      {
        string sSqlExist = "SELECT * FROM Users WHERE UserCode='" + txtID.Text.Trim() + "' AND UserID!='" + iTabId + "'";
        DataTable dtExist = ocon.GetTable(sSqlExist, new DataSet());

        if (dtExist.Rows.Count > 0)
        {
          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('User ID is already exists');", true);
          return;
        }
        else
        {
          if (txtemail.Text.Trim() != "")
          {
            string sSqlEmailExist = "SELECT * FROM Users WHERE UserEmail='" + txtemail.Text.Trim() + "' AND UserID!='" + iTabId + "'";
            DataTable dtEmailExist = ocon.GetTable(sSqlEmailExist, new DataSet());

            if (dtEmailExist.Rows.Count > 0)
            {
              ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Email is already exists');", true);
              return;
            }
          }
          //string sPassword = EncryptDecryptHelper.Encrypt(txtPassword.Text.Trim());
          string sPassword = EncryptQRCODE(txtPassword.Text.Trim());


          //if(drpType.SelectedItem.Text=="Admin")
          sUserType = drpType.SelectedItem.Value;

          string sUpdate = "UPDATE Users SET UserCode='"+txtID.Text.Trim()+"', UserName='" + txtName.Text.Trim() + "',Location_ID='" + drpLoc.SelectedItem.Value + "',UserGroup='" + sUserType.Trim() + "',UserEmail='" + txtemail.Text.Trim() + "',Phone='" + txtPhone.Text.Trim() + "',Pwd='" + sPassword + "' WHERE UserID ='" + iTabId + "'";
          ocon.Execute(sUpdate);

          string sDestURL = string.Format("\"{0}\"", "Userlists.aspx");
          string smessage = string.Format("\"{0}\"", "User updated successfully");

          string sVar = sDestURL + "," + smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
      }
      else
      {
        string sUserType = "2";
        //if (drpType.SelectedItem.Text == "Admin")
        sUserType = drpType.SelectedValue.ToString();

        string sSqlExist = "SELECT * FROM Users WHERE UserCode='" + txtID.Text.Trim() + "'";
        DataTable dtExist = ocon.GetTable(sSqlExist, new DataSet());

        if (dtExist.Rows.Count > 0)
        {
          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('User ID is already exists');", true);
          return;
        }
        else
        {

          if (txtemail.Text.Trim() != "")
          {
            string sSqlEmailExist = "SELECT * FROM Users WHERE UserEmail='" + txtemail.Text.Trim() + "'";
            DataTable dtEmailExist = ocon.GetTable(sSqlEmailExist, new DataSet());

            if (dtEmailExist.Rows.Count > 0)
            {
              ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Email is already exists');", true);
              return;
            }
          }
         // string sPassword = EncryptDecryptHelper.Encrypt(txtPassword.Text.Trim());
          string sPassword = EncryptQRCODE(txtPassword.Text.Trim());

          string sInsert = "insert into [dbo].[Users] ([UserCode],[UserName],  [Phone],  [Pwd],  [UserActive],  [UserApprove],   [UserEmail],  UserGroup,Loc_Name,Location_ID)  "
                          + " values('" + txtID.Text.Trim() + "', '" + txtName.Text.Trim() + "', '" + txtPhone.Text.Trim() + "', '" + sPassword + "', 1, 1,'" + txtemail.Text.Trim() + "','" + sUserType + "', 'Location 1','" + drpLoc.SelectedItem.Value + "') ";
          ocon.Execute(sInsert);

          string sDestURL = string.Format("\"{0}\"", "Userlists.aspx");
          string smessage = string.Format("\"{0}\"", "User added successfully");

          string sVar = sDestURL + "," + smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
      }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
      Response.Redirect("Userlists.aspx");



    }
    private void BindDropDown()
    {

      #region type

      //new change by swati
      DataTable dttype = ocon.GetTable("select ID,UsertypeName,RoleID from UserBasedMenu", new DataSet());
      if (dttype.Rows.Count > 0)
      {
        drpType.DataSource = dttype;
        drpType.DataTextField = "UsertypeName";
        drpType.DataValueField = "ID";
        drpType.DataBind();
        drpType.Items.Insert(0, "Select");
      }
      else
      {
        drpType.DataSource = null;
        drpType.DataBind();
        drpType.Items.Insert(0, "Select");
      }
      #endregion

      #region Location



      DataTable dtloation = ocon.GetTable("SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=23 order by pl_Value", new DataSet());
      if (dtloation.Rows.Count > 0)
      {
        drpLoc.DataSource = dtloation;
        drpLoc.DataTextField = "pl_value";
        drpLoc.DataValueField = "pl_id";
        drpLoc.DataBind();
        drpLoc.Items.Insert(0, "Select");
      }
      else
      {
        drpLoc.DataSource = null;
        drpLoc.DataBind();
        drpLoc.Items.Insert(0, "Select");
      }
      #endregion

      //#region AD User

      //DataTable dtUser = ocon.GetTable("select pl_Value,pl_Data,Other_Data3 from PickList_tran where pl_head_id = 18 ", new DataSet());
      //if (dttype.Rows.Count > 0)
      //{
      //  drpType.DataSource = dttype;
      //  drpType.DataTextField = "USERGROUP";
      //  drpType.DataValueField = "ID";
      //  drpType.DataBind();
      //  drpType.Items.Insert(0, "Select");
      //}
      //else
      //{
      //  drpType.DataSource = null;
      //  drpType.DataBind();
      //  drpType.Items.Insert(0, "Select");
      //}

      //#endregion
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
