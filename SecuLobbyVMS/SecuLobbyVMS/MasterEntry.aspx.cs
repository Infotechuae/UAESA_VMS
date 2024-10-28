using DAL;
using System;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class MasterEntry : System.Web.UI.Page
  {
    ResourceManager rm;
    CultureInfo ci;
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);


        int iTabId = Convert.ToInt32(Request.QueryString["ID"]);
        int iPlHeadID = Convert.ToInt32(Request.QueryString["PlHeadID"]);


        if (iPlHeadID == 4)
          header1.Text = "Department";
        if (iPlHeadID == 9)
          header1.Text = "Floor";
        if (iPlHeadID == 18)
          header1.Text = "Employee / Host";
        if (iPlHeadID == 1)
          header1.Text = "Visitor Type";
        if (iPlHeadID == 5)
          header1.Text = "Card";
        if (iPlHeadID == 23)
          header1.Text = "Location";
        if (iPlHeadID == 10)
          header1.Text = "Purpose";

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);


        #region Department

        string sSqlDepartment = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=4 order by pl_Value";

        DataTable dtDepartment = ocon.GetTable(sSqlDepartment, new DataSet());
        if (dtDepartment.Rows.Count > 0)
        {
          drpDepartment.DataSource = dtDepartment;
          drpDepartment.DataTextField = "pl_value";
          drpDepartment.DataValueField = "pl_value";
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


        string sSql = "SELECT ID,pl_id,pl_Value,pl_Data,Other_Data1,Other_Data2,Other_Data3,isnull(Card_next_ID,0) as Card_next_ID FROM PickList_tran WHERE ID='" + iTabId + "'";
        DataTable dt = ocon.GetTable(sSql, new DataSet());
        if (dt.Rows.Count > 0)
        {
          txtID.Text = Convert.ToString(dt.Rows[0]["pl_id"]);
          txtName.Text = Convert.ToString(dt.Rows[0]["pl_Value"]);
          txtemail.Text = Convert.ToString(dt.Rows[0]["pl_Data"]);

          if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Other_Data1"])))
          {
            drpDepartment.SelectedValue = Convert.ToString(dt.Rows[0]["Other_Data1"]);
          }
          txtManName.Text = Convert.ToString(dt.Rows[0]["Other_Data2"]);
          txtManEmail.Text = Convert.ToString(dt.Rows[0]["Other_Data3"]);

          int iCard_next_ID = Convert.ToInt32(dt.Rows[0]["Card_next_ID"]);


          //if (iCard_next_ID>0)
          //  drpCompany.SelectedValue = iCard_next_ID.ToString();
        }
        else
        {
          txtID.Text = "";
          txtName.Text = "";
          txtemail.Text = "";
          txtManName.Text = "";
          txtManEmail.Text = "";

          int iCid = 0;
          DataTable dtCid = ocon.GetTable("SELECT isnull(max(pl_id),0) as cId FROM PickList_tran where pl_head_id='" + iPlHeadID + "'", new DataSet());
          if (dtCid.Rows.Count > 0)
          {
            iCid = Convert.ToInt32(dtCid.Rows[0]["cId"]);
          }
          iCid = iCid + 1;

          txtID.Text = iCid.ToString();
        }

        if (iPlHeadID == 18)
        {

          divEmail.Attributes.Add("style", "display:block");
          divDepartment.Attributes.Add("style", "display:block");
          divManagerName.Attributes.Add("style", "display:block");
          divManagerEmail.Attributes.Add("style", "display:block");
        }

        else
        {

          divEmail.Attributes.Add("style", "display:none");
          divDepartment.Attributes.Add("style", "display:none");
          divManagerName.Attributes.Add("style", "display:none");
          divManagerEmail.Attributes.Add("style", "display:none");
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
        System.Web.UI.HtmlControls.HtmlGenericControl lnkPurpose = Master.FindControl("lnkPurpose") as System.Web.UI.HtmlControls.HtmlGenericControl;
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
        System.Web.UI.HtmlControls.HtmlAnchor aPurpose = Master.FindControl("aPurpose") as System.Web.UI.HtmlControls.HtmlAnchor;
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
        lnkPurpose.Attributes.Remove("class");
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
        aPurpose.Attributes.Remove("class");
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


        if (iPlHeadID == 4)
        {
          liDepart.Attributes.Add("class", "nav-item menu-open");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (iPlHeadID == 9)
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item menu-open");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (iPlHeadID == 18)
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item menu-open");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (iPlHeadID == 1)
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item menu-open");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (iPlHeadID == 10)
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item menu-open");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (iPlHeadID == 5)
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item menu-open");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (iPlHeadID == 23)
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item menu-open");
        }
        else
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }


        liUser.Attributes.Add("class", "nav-item");
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

        if (iPlHeadID == 4)
        {
          adepart.Attributes.Add("class", "nav-link active");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (iPlHeadID == 9)
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link active");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (iPlHeadID == 18)
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link active");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (iPlHeadID == 1)
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link active");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (iPlHeadID == 10)
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link active");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (iPlHeadID == 5)
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link active");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (iPlHeadID == 23)
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link active");
        }
        else
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }


        aUser.Attributes.Add("class", "nav-link");
        aMail.Attributes.Add("class", "nav-link");
        aAccepted.Attributes.Add("class", "nav-link");
        aPending.Attributes.Add("class", "nav-link");
        aRejected.Attributes.Add("class", "nav-link");

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
      if (header1.Text == "Department")
        header1.Text = rm.GetString("STR_11", ci);
      if (header1.Text == "Floor")
        header1.Text = rm.GetString("STR_12", ci);
      if (header1.Text == "Employee / Host")
        header1.Text = rm.GetString("STRN_50", ci);
      if (header1.Text == "Visitor Type")
        header1.Text = rm.GetString("STR_14", ci);
      if (header1.Text == "Card")
        header1.Text = rm.GetString("STRN_49", ci);
      if (header1.Text == "Location")
        header1.Text = rm.GetString("STR_56", ci);
      if (header1.Text == "Purpose")
        header1.Text = rm.GetString("STR_61", ci);


      lblID.Text = rm.GetString("STRN_36", ci);
      lblName.Text = rm.GetString("STRN_37", ci);

      lblEmail.Text = rm.GetString("STR_7", ci);
      lblDepartent.Text = rm.GetString("STRN_1", ci);
      lblManName.Text = rm.GetString("STRN_53", ci);
      lblManEmail.Text = rm.GetString("STRN_54", ci);
      btnSave.Text = rm.GetString("STRN_21", ci);
      btnReset.Text = rm.GetString("STR_19", ci);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      int iTabId = Convert.ToInt32(Request.QueryString["ID"]);
      int iPlHeadID = Convert.ToInt32(Request.QueryString["PlHeadID"]);

      string sHead = "";
      if (iPlHeadID == 4)
        sHead = "Department";
      if (iPlHeadID == 9)
        sHead = "Floor";
      if (iPlHeadID == 18)
        sHead = "Employee";
      if (iPlHeadID == 1)
        sHead = "VisitorType";
      if (iPlHeadID == 5)
        sHead = "Card";
      if (iPlHeadID == 23)
        sHead = "Location";
      if (iPlHeadID == 10)
        sHead = "Purpose";

      int iCid = 0;
      DataTable dtCid = ocon.GetTable("SELECT isnull(max(pl_id),0) as cId FROM PickList_tran where pl_head_id='" + iPlHeadID + "'", new DataSet());
      if (dtCid.Rows.Count > 0)
      {
        iCid = Convert.ToInt32(dtCid.Rows[0]["cId"]);
      }
      iCid = iCid + 1;

      string sSql = "SELECT ID,pl_id,pl_Value,pl_Data,Other_Data1,Other_Data2 FROM PickList_tran WHERE ID='" + iTabId + "'";
      DataTable dt = ocon.GetTable(sSql, new DataSet());
      if (dt.Rows.Count > 0)
      {
        string sSqlExist = "SELECT * FROM PickList_tran WHERE pl_id='" + txtID.Text.Trim() + "' AND pl_head_id='" + iPlHeadID + "' AND ID!='" + iTabId + "'";
        DataTable dtExist = ocon.GetTable(sSqlExist, new DataSet());

        if (dtExist.Rows.Count > 0)
        {
          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('ID is already exists');", true);
          return;
        }
        else
        {

          if (iPlHeadID == 18)
          {
            if (txtemail.Text.Trim() != "")
            {
              string sSqlEmailExist = "SELECT * FROM PickList_tran WHERE pl_Data='" + txtemail.Text.Trim() + "' AND pl_head_id='" + iPlHeadID + "' AND ID!='" + iTabId + "'";
              DataTable dtEmailExist = ocon.GetTable(sSqlEmailExist, new DataSet());

              if (dtEmailExist.Rows.Count > 0)
              {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Email is already exists');", true);
                return;
              }
            }
          }

          int inextid = 0;
          
          string sUpdate = "UPDATE PickList_tran SET pl_id='" + txtID.Text.Trim() + "',pl_Value='" + txtName.Text.Trim() + "',pl_Data='" + txtemail.Text.Trim() + "',Other_Data1='" + drpDepartment.SelectedItem.Text.Trim() + "',Other_Data2='" + txtManName.Text.Trim() + "',Other_Data3='" + txtManEmail.Text.Trim() + "' WHERE ID='" + iTabId + "'";
          ocon.Execute(sUpdate);

          string sDestURL = string.Format("\"{0}\"", "Masters.aspx?masid=" + sHead);
          string smessage = string.Format("\"{0}\"", sHead + " updated successfully");

          string sVar = sDestURL + "," + smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
      }
      else
      {
        string sSqlExist = "SELECT * FROM PickList_tran WHERE pl_id='" + txtID.Text.Trim() + "' AND pl_head_id='" + iPlHeadID + "'";
        DataTable dtExist = ocon.GetTable(sSqlExist, new DataSet());

        if (dtExist.Rows.Count > 0)
        {
          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('ID is already exists');", true);
          return;
        }
        else
        {
          if (iPlHeadID == 18)
          {
            if (txtemail.Text.Trim() != "")
            {
              string sSqlEmailExist = "SELECT * FROM PickList_tran WHERE pl_Data='" + txtemail.Text.Trim() + "' AND pl_head_id='" + iPlHeadID + "'";
              DataTable dtEmailExist = ocon.GetTable(sSqlEmailExist, new DataSet());

              if (dtEmailExist.Rows.Count > 0)
              {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Email is already exists');", true);
                return;
              }
            }
          }


          int inextid = 0;
        
          string sInsert = "INSERT INTO PickList_tran (pl_head_id,pl_id,pl_Value,pl_Data,Other_Data1,Other_Data2,Other_Data3,Status,isValid,Loc_ID) "
                        + " VALUES('" + iPlHeadID + "', '" + iCid + "', '" + txtName.Text.Trim() + "', '" + txtemail.Text.Trim() + "', '" + drpDepartment.SelectedItem.Text.Trim() + "','" + txtManName.Text.Trim() + "','" + txtManEmail.Text.Trim() + "', 'ACTIVE', 1, 1)";
          ocon.Execute(sInsert);

          string sDestURL = string.Format("\"{0}\"", "Masters.aspx?masid=" + sHead);
          string smessage = string.Format("\"{0}\"", sHead + " added successfully");

          string sVar = sDestURL + "," + smessage;


          ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
        }
      }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
      int iPlHeadID = Convert.ToInt32(Request.QueryString["PlHeadID"]);


      if (iPlHeadID == 4)
        Response.Redirect("Masters.aspx?masid=Department");
      if (iPlHeadID == 9)
        Response.Redirect("Masters.aspx?masid=Floor");
      if (iPlHeadID == 18)
        Response.Redirect("Masters.aspx?masid=Employee");
      if (iPlHeadID == 1)
        Response.Redirect("Masters.aspx?masid=VisitorType");
      if (iPlHeadID == 5)
        Response.Redirect("Masters.aspx?masid=Card");
      if (iPlHeadID == 23)
        Response.Redirect("Masters.aspx?masid=Location");
      if (iPlHeadID == 10)
        Response.Redirect("Masters.aspx?masid=Purpose");

    }
  }
}
