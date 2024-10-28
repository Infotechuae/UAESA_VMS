using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class Admin_Master : System.Web.UI.MasterPage
  {
    ResourceManager rm;
    CultureInfo ci;
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    //Change is method for Role based access by swati
    protected void Page_Load(object sender, EventArgs e)
    {

      if (!Page.IsPostBack)
      {
        lnkDashboard.Visible = false;
        lnkSelfReg.Visible = false;
        lnkRegis.Visible = false;
        lnkCheckin.Visible = false;
        lnkCheckOut.Visible = false;
        lnkTimeOut.Visible = false;
        lnkWatchList.Visible = false;
        lnlReports.Visible = false;
        lnkVisMas.Visible = false;
        lnkVisTran.Visible = false;
        lnkAllReqReport.Visible = false;
        lnkpre.Visible = false;
        lnkPrereg.Visible = false;
        lnkSetting.Visible = false;
        lnkLocation.Visible = false;
        liDepart.Visible = false;
        liFloor.Visible = false;
        liEmployee.Visible = false;
        liVisitorType.Visible = false;
        lnkPurpose.Visible = false;
        liUser.Visible = false;
        liMail.Visible = false;
        lnkCard.Visible = false;
        liMailtemplate.Visible = false;
        lnkRequest.Visible = false;
        lnkRqstlistAll.Visible = false;
        lnkRqstlistAccepted.Visible = false;
        lnkRqstlistPending.Visible = false;
        lnkRqstlistRejected.Visible = false;
        li1.Visible = false;
        li2.Visible = false;


        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);
        string sUserGroup = Convert.ToString(Session["UserGroup"]);
        String Roleid = Convert.ToString(Session["RoleID"]);
        List<string> RoleList = Roleid.Split(',').ToList();

        if (RoleList.Contains("1")) { lnkDashboard.Visible = true; }
        if (RoleList.Contains("2")) { lnkSelfReg.Visible = true; }
        if (RoleList.Contains("3")) { lnkRegis.Visible = true; }
        if (RoleList.Contains("4")) { lnkCheckin.Visible = true; }
        if (RoleList.Contains("5")) { lnkCheckOut.Visible = true; }
        if (RoleList.Contains("6")) { lnkTimeOut.Visible = true; }
        if (RoleList.Contains("7")) { lnkWatchList.Visible = true; }
        if (RoleList.Contains("8")) { lnlReports.Visible = true; lnkVisMas.Visible = true; }
        if (RoleList.Contains("9")) { lnlReports.Visible = true; lnkVisTran.Visible = true; }
        if (RoleList.Contains("10")) { lnlReports.Visible = true; lnkAllReqReport.Visible = true; }
        if (RoleList.Contains("11")) { lnkpre.Visible = true; lnkPrereg.Visible = true; }
        if (RoleList.Contains("12")) { lnkRequest.Visible = true; lnkRqstlistAll.Visible = true; }
        if (RoleList.Contains("13")) { lnkRequest.Visible = true; lnkRqstlistAccepted.Visible = true; }
        if (RoleList.Contains("14")) { lnkRequest.Visible = true; lnkRqstlistPending.Visible = true; }
        if (RoleList.Contains("15")) { lnkRequest.Visible = true; lnkRqstlistRejected.Visible = true; }
        if (RoleList.Contains("16")) { lnkSetting.Visible = true; lnkLocation.Visible = true; }
        if (RoleList.Contains("17")) { lnkSetting.Visible = true; liDepart.Visible = true; }
        if (RoleList.Contains("19")) { lnkSetting.Visible = true; liEmployee.Visible = true; }
        if (RoleList.Contains("20")) { lnkSetting.Visible = true; liVisitorType.Visible = true; }
        if (RoleList.Contains("21")) { lnkSetting.Visible = true; lnkPurpose.Visible = true; }
        if (RoleList.Contains("22")) { lnkSetting.Visible = true; liUser.Visible = true; }
        if (RoleList.Contains("23")) { lnkSetting.Visible = true; liMail.Visible = true; }
        if (RoleList.Contains("24")) { lnkSetting.Visible = true; liMailtemplate.Visible = true; }
        if (RoleList.Contains("25")) { lnkSetting.Visible = true; li1.Visible = true; }
        if (RoleList.Contains("27")) { lnkSetting.Visible = true; li2.Visible = true; }
        if (sLang == "ar-SA")
        {

          lblLanguage.Text = "Arabic";
          imgflag.ImageUrl = "~/dist/img/united-arab-emirates.png";
        }
        else
        {
          lblLanguage.Text = "English";
          imgflag.ImageUrl = "~/dist/img/united-kingdom.png";
        }

        string sSql = "SELECT Username FROM USERS where UserID='" + sUserID + "'";
        DataTable dt = ocon.GetTable(sSql, new DataSet());
        if (dt.Rows.Count > 0)
        {
          lblAccountName.Text = dt.Rows[0]["Username"].ToString();
        }

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }

    private void Loadstring(CultureInfo ci)
    {
      
      lblDashboard.Text = rm.GetString("STR_20", ci);
      lblSelfReg.Text = rm.GetString("STR_49", ci);
      lblCheckIn.Text = rm.GetString("STR_18", ci);
      lblCheckOut.Text = rm.GetString("STR_21", ci);
      lblTimeOut.Text = rm.GetString("STR_22", ci);
      lblWatchList.Text = rm.GetString("STR_23", ci);
      lblReports.Text = rm.GetString("STR_24", ci);
      lblVisMasRpt.Text = rm.GetString("STR_25", ci);
      lblVisTrans.Text = rm.GetString("STR_26", ci);
      lblAllReqReport.Text = rm.GetString("STRN_48", ci);

      lblCP.Text = rm.GetString("STR_27", ci);
      lblLog.Text = rm.GetString("STR_28", ci);
      lblReaady.Text = rm.GetString("STR_29", ci);
      lblSelectLog.Text = rm.GetString("STR_30", ci);
      lblCancel.Text = rm.GetString("STR_31", ci);
      lblLogout1.Text = rm.GetString("STR_28", ci);

      lblChange1.Text = rm.GetString("STR_27", ci);
      lblNewPass.Text = rm.GetString("STR_32", ci);
      lblConfPass.Text = rm.GetString("STR_33", ci);
      lblCancel1.Text = rm.GetString("STR_31", ci);
      lblChange.Text = rm.GetString("STR_34", ci);

      lblpreReg.Text = rm.GetString("STR_49", ci);
      lblVisitorInvite.Text = rm.GetString("STR_50", ci);

      //Change by swati -  menu change in arabic
      Label34.Text = rm.GetString("STRN_1", ci);
      Label5.Text = rm.GetString("STRN_2", ci);
      Label6.Text = rm.GetString("STRN_3", ci);
      Label17.Text = rm.GetString("STRN_4", ci);
      Label8.Text = rm.GetString("STRN_5", ci);
      Label19.Text = rm.GetString("STRN_6", ci);
      Label20.Text = rm.GetString("STRN_7", ci);
      Label9.Text = rm.GetString("STRN_8", ci);
      Label18.Text = rm.GetString("STRN_9", ci);
      Label1.Text = rm.GetString("STRN_10", ci);

      Label11.Text = rm.GetString("STRN_41", ci);
      Label12.Text = rm.GetString("STRN_42", ci);
      Label13.Text = rm.GetString("STRN_43", ci);
      Label14.Text = rm.GetString("STRN_44", ci);
      Label15.Text = rm.GetString("STRN_45", ci);
    }


    protected void btnLang_Click(object sender, EventArgs e)
    {
      Session["Lang"] = hdnlang.Value;
      string sUserGroup = Convert.ToString(Session["UserGroup"]);

      //if (sUserGroup == "9")
      //{
      //  Response.Redirect("VisitorInvite.aspx");
      //}
      //else
      //{
      Response.Redirect("MasterDashboard.aspx");
      // }

    }
  }
}
