using DAL;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Native.Data;
using DevExpress.Web.ASPxHtmlEditor.Internal;
using DevExpress.XtraReports.UI;
using SecuLobbyVMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Charting.Styles;


namespace SecuLobbyVMS
{
  public partial class ApprovalSetting : System.Web.UI.Page
  {
    ResourceManager rm;
    CultureInfo ci;

    public string sUserID;
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        checkin_A.CheckedChanged += new EventHandler(this.Check_Clicked);

        int iTabId = Convert.ToInt32(Request.QueryString["ID"]);
        string sSql = "SELECT * FROM ApprovalSettings WHERE ID='" + iTabId + "'";
        System.Data.DataTable dt = ocon.GetTable(sSql, new DataSet());
        if (dt.Rows.Count > 0)
        {
          txtID.Text = Convert.ToString(dt.Rows[0]["ServiceLevel"]);
          dd1.SelectedValue = Convert.ToString(dt.Rows[0]["MenuID"]);
          drpVisitorType.SelectedValue = Convert.ToString(dt.Rows[0]["VisitorType"]);
          checkin_A.Checked = Convert.ToBoolean(dt.Rows[0]["ApprovalRequired"]);
          checkin_L.Checked = Convert.ToBoolean(dt.Rows[0]["LineManagerApproval"]);
          checkin_H.Checked = Convert.ToBoolean(dt.Rows[0]["HostApproval"]);
          if (checkin_A.Checked == true)
          {
            checkin_H.Disabled = false;
            checkin_L.Disabled = false;
          }

        }


        string Sql = "SELECT a.[ID],a.MenuID,a.[ApprovalRequired],a.[HostApproval],a.[LineManagerApproval],a.VisitorType,a.ServiceLevel,m.MenuName  FROM ApprovalSettings a  inner join tbl_menu m on m.ID = a.MenuID";

        System.Data.DataTable dtt = ocon.GetTable(Sql, new DataSet());

        if (dtt.Rows.Count > 0)
        {
          grdDetails.DataSource = dtt;
          grdDetails.DataBind();

        }
        else
        {
          grdDetails.DataSource = null;
          grdDetails.DataBind();
        }



        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);

        //Change by swati -   value change in arabic
        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        btnSaveSettings.Text = rm.GetString("STRN_18", ci);
        header1.Text = rm.GetString("STRN_7", ci);

        System.Web.UI.HtmlControls.HtmlGenericControl lnkDashboard = Master.FindControl("lnkDashboard") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkSelfReg = Master.FindControl("lnkSelfReg") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkRegis = Master.FindControl("lnkRegis") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCheckin = Master.FindControl("lnkCheckin") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCheckOut = Master.FindControl("lnkCheckOut") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkTimeOut = Master.FindControl("lnkTimeOut") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkWatchList = Master.FindControl("lnkWatchList") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkVisMas = Master.FindControl("lnkVisMas") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkVisTran = Master.FindControl("lnkVisTran") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkPrereg = Master.FindControl("lnkPrereg") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liDepart = Master.FindControl("liDepart") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liFloor = Master.FindControl("liFloor") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liEmployee = Master.FindControl("liEmployee") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liVisitorType = Master.FindControl("liVisitorType") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCard = Master.FindControl("lnkCard") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liUser = Master.FindControl("liUser") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liMail = Master.FindControl("liMail") as System.Web.UI.HtmlControls.HtmlGenericControl;


        System.Web.UI.HtmlControls.HtmlAnchor aDash = Master.FindControl("aDash") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aSelfRegis = Master.FindControl("aSelfRegis") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aRegis = Master.FindControl("aRegis") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCheckIN = Master.FindControl("aCheckIN") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCheckOut = Master.FindControl("aCheckOut") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aTimeOut = Master.FindControl("aTimeOut") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aWatchList = Master.FindControl("aWatchList") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisMas = Master.FindControl("aVisMas") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisTrans = Master.FindControl("aVisTrans") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aPrereg = Master.FindControl("aPrereg") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor adepart = Master.FindControl("adepart") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aFloor = Master.FindControl("aFloor") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aEmployee = Master.FindControl("aEmployee") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisitorType = Master.FindControl("aVisitorType") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCard = Master.FindControl("aCard") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aUser = Master.FindControl("aUser") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aMail = Master.FindControl("aMail") as System.Web.UI.HtmlControls.HtmlAnchor;


        lnkDashboard.Attributes.Remove("class");
        lnkSelfReg.Attributes.Remove("class");
        lnkRegis.Attributes.Remove("class");
        lnkCheckin.Attributes.Remove("class");
        lnkCheckOut.Attributes.Remove("class");
        lnkTimeOut.Attributes.Remove("class");
        lnkWatchList.Attributes.Remove("class");
        lnkVisMas.Attributes.Remove("class");
        lnkVisTran.Attributes.Remove("class");
        lnkPrereg.Attributes.Remove("class");
        liDepart.Attributes.Remove("class");
        liFloor.Attributes.Remove("class");
        liEmployee.Attributes.Remove("class");
        liVisitorType.Attributes.Remove("class");
        lnkCard.Attributes.Remove("class");
        liUser.Attributes.Remove("class");
        liMail.Attributes.Remove("class");

        aDash.Attributes.Remove("class");
        aSelfRegis.Attributes.Remove("class");
        aRegis.Attributes.Remove("class");
        aCheckIN.Attributes.Remove("class");
        aCheckOut.Attributes.Remove("class");
        aTimeOut.Attributes.Remove("class");
        aWatchList.Attributes.Remove("class");
        aVisMas.Attributes.Remove("class");
        aVisTrans.Attributes.Remove("class");
        aPrereg.Attributes.Remove("class");
        adepart.Attributes.Remove("class");
        aFloor.Attributes.Remove("class");
        aEmployee.Attributes.Remove("class");
        aVisitorType.Attributes.Remove("class");
        aCard.Attributes.Remove("class");
        aUser.Attributes.Remove("class");
        aMail.Attributes.Remove("class");

        lnkDashboard.Attributes.Add("class", "nav-item");
        lnkSelfReg.Attributes.Add("class", "nav-item");
        lnkRegis.Attributes.Add("class", "nav-item");
        lnkCheckin.Attributes.Add("class", "nav-item");
        lnkCheckOut.Attributes.Add("class", "nav-item");
        lnkTimeOut.Attributes.Add("class", "nav-item");
        lnkWatchList.Attributes.Add("class", "nav-item");
        lnkVisMas.Attributes.Add("class", "nav-item");
        lnkVisTran.Attributes.Add("class", "nav-item");
        lnkPrereg.Attributes.Add("class", "nav-item");
        liDepart.Attributes.Add("class", "nav-item");
        liFloor.Attributes.Add("class", "nav-item");
        liEmployee.Attributes.Add("class", "nav-item");
        liVisitorType.Attributes.Add("class", "nav-item");
        lnkCard.Attributes.Add("class", "nav-item");
        liUser.Attributes.Add("class", "nav-item");
        liMail.Attributes.Add("class", "nav-item");


        aDash.Attributes.Add("class", "nav-link");
        aSelfRegis.Attributes.Add("class", "nav-link");
        aRegis.Attributes.Add("class", "nav-link");
        aCheckIN.Attributes.Add("class", "nav-link");
        aCheckOut.Attributes.Add("class", "nav-link");
        aTimeOut.Attributes.Add("class", "nav-link");
        aWatchList.Attributes.Add("class", "nav-link");
        aVisMas.Attributes.Add("class", "nav-link");
        aVisTrans.Attributes.Add("class", "nav-link");
        aPrereg.Attributes.Add("class", "nav-link");
        adepart.Attributes.Add("class", "nav-link");
        aFloor.Attributes.Add("class", "nav-link");
        aEmployee.Attributes.Add("class", "nav-link");
        aVisitorType.Attributes.Add("class", "nav-link");
        aCard.Attributes.Add("class", "nav-link");
        aUser.Attributes.Add("class", "nav-link");
        liMail.Attributes.Add("class", "nav-link");

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
        li2.Attributes.Add("class", "nav-item menu-open");
        a2.Attributes.Add("class", "nav-link active");

        /////////////////////////
        ///

        #region VisitorType

        string sSqlVisitorType = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=1 order by pl_Value";

        System.Data.DataTable dtVisitorType = ocon.GetTable(sSqlVisitorType, new DataSet());
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


        /////////////////////////////////////////
        //       string sSql1= "SELECT * FROM ApprovalSettings inner join Tbl_Menu on ApprovalSettings.MenuID=Tbl_Menu.ID";
        //       DataTable dt1 = ocon.GetTable(sSql1, new DataSet());
        //if (dt1.Rows.Count > 0)
        //{

        //}
        //if (Convert.ToBoolean(dt1.Rows[0]["HostApproval"]) != true)
        //  checkin_H.Disabled = true;
        //if (Convert.ToBoolean(dt1.Rows[0]["LineManagerApproval"]) != true)
        //  checkin_L.Disabled = true;



      }
    }
    protected void btnSaveSettings_Click(object sender, EventArgs e)
    {
      //Button btndetails = (Button)sender;
      //GridViewRow gvrow = (GridViewRow)btndetails.Parent.Parent;
      //int rowindex = gvrow.RowIndex;
      //Label lblUserMasterId = grdDetails.Rows[rowindex].Controls[0].FindControl("lblUserMasterId") as Label;

      int iTabId = Convert.ToInt32(Request.QueryString["ID"]);
      string sSql = "SELECT * FROM ApprovalSettings WHERE ID='" + iTabId + "'";
      System.Data.DataTable dt = ocon.GetTable(sSql, new DataSet());
      if (dt.Rows.Count > 0)
      {
        string sUpdate = "UPDATE ApprovalSettings SET MenuID='" + dd1.SelectedItem.Value + "',ServiceLevel='" + txtID.Text.Trim() +
          "',VisitorType='" + drpVisitorType.SelectedItem.Text + "', ApprovalRequired='" + checkin_A.Checked + "', HostApproval='" +
          checkin_H.Checked + "',LineManagerApproval='" + checkin_L.Checked + "' WHERE ID ='" + iTabId + "'";
        ocon.Execute(sUpdate);
        string sDestURL = string.Format("\"{0}\"", "ApprovalSetting.aspx");
        string smessage = string.Format("\"{0}\"", "Approval Settings updated successfully");

        string sVar = sDestURL + "," + smessage;
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }

      else if (ValidateData())
      {

        string sInsert = "insert into [dbo].[ApprovalSettings]  ([MenuID], [ApprovalRequired], [HostApproval], [LineManagerApproval], [VisitorType], [ServiceLevel])"
                        + " values('" + dd1.SelectedItem.Value + "', '" + checkin_A.Checked + "', '" + checkin_H.Checked + "', '" + checkin_L.Checked + "','" + drpVisitorType.SelectedItem.Text + "','" + txtID.Text.Trim() + "') ";
        ocon.Execute(sInsert);

        string sDestURL = string.Format("\"{0}\"", "ApprovalSetting.aspx");
        string smessage = string.Format("\"{0}\"", "Approval Settings added successfully");

        string sVar = sDestURL + "," + smessage;

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
      else
      {
        string sDestURL = string.Format("\"{0}\"", "ApprovalSetting.aspx");
        string smessage = string.Format("\"{0}\"", "Please select Host or Line Manager Approval for all selected Approvals");

        string sVar = sDestURL + "," + smessage;

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
    }

    public bool ValidateData()
    {
      bool valid = true;
      if (checkin_A.Checked == true)
      {
        if (checkin_H.Checked == false && checkin_L.Checked == false)
        {
          valid = false;
        }
      }

      return valid;
    }

    protected string GetHeader(string str)
    {
      string sHeaderName = "";

      string sLang = Convert.ToString(Session["Lang"]);

      Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
      rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
      ci = Thread.CurrentThread.CurrentCulture;

      if (str == "User ID")
        sHeaderName = rm.GetString("STRN_39", ci);
      if (str == "Name")
        sHeaderName = rm.GetString("STRN_37", ci);
      if (str == "Email")
        sHeaderName = rm.GetString("STR_7", ci);
      if (str == "Location")
        sHeaderName = rm.GetString("STR_56", ci);
      if (str == "Phone")
        sHeaderName = rm.GetString("STRN_38", ci);
      if (str == "User Type")
        sHeaderName = rm.GetString("STRN_20", ci);
      if (str == "Edit")
        sHeaderName = rm.GetString("STRN_51", ci);
      if (str == "Delete")
        sHeaderName = rm.GetString("STRN_52", ci);

      return sHeaderName;
    }

    protected void Check_Clicked(Object sender, EventArgs e)
    {

      checkin_A.CheckedChanged += new EventHandler(this.Check_Clicked);
      if (checkin_A.Checked == true)
      {
        checkin_H.Disabled = false;
        checkin_L.Disabled = false;

      }
      if (checkin_A.Checked == false)
      {

        checkin_H.Checked = false;
        checkin_L.Checked = false;
        checkin_H.Disabled = true;
        checkin_L.Disabled = true;

      }
    }

    protected void grdDetails_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
      System.Web.UI.WebControls.Label lblid = grdDetails.Rows[e.RowIndex].Controls[0].FindControl("lblid") as System.Web.UI.WebControls.Label;

      string sSqlDelete = "DELETE ApprovalSettings WHERE ID='" + lblid.Text + "'";
      ocon.Execute(sSqlDelete);


      string sDestURL = string.Format("\"{0}\"", "ApprovalSetting.aspx");
      string smessage = string.Format("\"{0}\"", "Approval deleted successfully");

      string sVar = sDestURL + "," + smessage;

      ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);


      //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Approval deleted successfully');", true);

      grdDetails.EditIndex = -1;

      // gridbind();
    }


  }
}
