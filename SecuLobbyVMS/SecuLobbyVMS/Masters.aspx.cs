using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class Masters : System.Web.UI.Page
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


        string sMasterID = Request.QueryString["masid"];


        if (sMasterID == "Department")
          header1.Text = "Department";
        if (sMasterID == "Floor")
          header1.Text = "Floor";
        if (sMasterID == "Employee")
          header1.Text = "Employee / Host";
        if (sMasterID == "VisitorType")
          header1.Text = "Visitor Type";
        if (sMasterID == "Card")
          header1.Text = "Card";
        if (sMasterID == "Location")
          header1.Text = "Location";
        if (sMasterID == "Purpose")
          header1.Text = "Purpose";


        gridbind();

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
        if (sMasterID == "Department")
        {
          liDepart.Attributes.Add("class", "nav-item menu-open");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (sMasterID == "Floor")
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item menu-open");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (sMasterID == "Employee")
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item menu-open");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (sMasterID == "VisitorType")
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item menu-open");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (sMasterID == "Purpose")
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item menu-open");
          lnkCard.Attributes.Add("class", "nav-item");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (sMasterID == "Card")
        {
          liDepart.Attributes.Add("class", "nav-item");
          liFloor.Attributes.Add("class", "nav-item");
          liEmployee.Attributes.Add("class", "nav-item");
          liVisitorType.Attributes.Add("class", "nav-item");
          lnkPurpose.Attributes.Add("class", "nav-item");
          lnkCard.Attributes.Add("class", "nav-item menu-open");
          lnkLocation.Attributes.Add("class", "nav-item");
        }
        else if (sMasterID == "Location")
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

        if (sMasterID == "Department")
        {
          adepart.Attributes.Add("class", "nav-link active");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (sMasterID == "Floor")
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link active");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (sMasterID == "Employee")
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link active");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (sMasterID == "VisitorType")
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link active");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (sMasterID == "Purpose")
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link active");
          aCard.Attributes.Add("class", "nav-link");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (sMasterID == "Card")
        {
          adepart.Attributes.Add("class", "nav-link");
          aFloor.Attributes.Add("class", "nav-link");
          aEmployee.Attributes.Add("class", "nav-link");
          aVisitorType.Attributes.Add("class", "nav-link");
          aPurpose.Attributes.Add("class", "nav-link");
          aCard.Attributes.Add("class", "nav-link active");
          aLocation.Attributes.Add("class", "nav-link");
        }
        else if (sMasterID == "Location")
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

        aAll.Attributes.Add("class", "nav-link");
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

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
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

      btnAdd.Text = rm.GetString("STRN_17", ci);



    }

    protected string GetHeader(string str)
    {
      string sHeaderName = "";

      string sLang = Convert.ToString(Session["Lang"]);

      Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
      rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
      ci = Thread.CurrentThread.CurrentCulture;

      if (str == "ID")
        sHeaderName = rm.GetString("STRN_36", ci);

      if (str == "Name")
        sHeaderName = rm.GetString("STRN_37", ci);

      if (str == "Email")
        sHeaderName = rm.GetString("STR_7", ci);

      if (str == "Department")
        sHeaderName = rm.GetString("STRN_1", ci);

      if (str == "Edit")
        sHeaderName = rm.GetString("STRN_51", ci);

      if (str == "Delete")
        sHeaderName = rm.GetString("STRN_52", ci);


      return sHeaderName;
    }
      protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      gridbind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
      string sMasterID = Request.QueryString["masid"];

      int iPlHeadID = 0;
      if (sMasterID == "Department")
        iPlHeadID = 4;
      if (sMasterID == "Floor")
        iPlHeadID = 9;
      if (sMasterID == "Employee")
        iPlHeadID = 18;
      if (sMasterID == "VisitorType")
        iPlHeadID = 1;
      if (sMasterID == "Card")
        iPlHeadID = 5;
      if (sMasterID == "Location")
        iPlHeadID = 23;
      if (sMasterID == "Purpose")
        iPlHeadID = 10;

      Response.Redirect("MasterEntry.aspx?PlHeadID="+ iPlHeadID);
    }
    public void gridbind()
    {
      string sMasterID = Request.QueryString["masid"];

      int iPlHeadID = 0;
      if (sMasterID == "Department")
        iPlHeadID = 4;
      if (sMasterID == "Floor")
        iPlHeadID = 9;
      if (sMasterID == "Employee")
        iPlHeadID = 18;
      if (sMasterID == "VisitorType")
        iPlHeadID = 1;
      if (sMasterID == "Card")
        iPlHeadID = 5;
      if (sMasterID == "Location")
        iPlHeadID = 23;
      if (sMasterID == "Purpose")
        iPlHeadID = 10;

      string sSql = "select ID,pl_head_id,pl_id,pl_Value,pl_Data,Card_next_ID,Other_Data1,Other_Data2,Loc_ID from PickList_tran where pl_head_id='" + iPlHeadID + "'";
      if (txtSearch.Text != "")
      {
        sSql += " and (pl_id like '%" + txtSearch.Text.Trim() + "%' or pl_Value like '%" + txtSearch.Text.Trim() + "%' or pl_Data like '%" + txtSearch.Text.Trim() + "%' or Other_Data1 like '%" + txtSearch.Text.Trim() + "%' or Other_Data2 like '%" + txtSearch.Text.Trim() + "%')";


      }

      sSql += " order by pl_id";

      DataTable dt = ocon.GetTable(sSql, new DataSet());

      if (dt.Rows.Count > 0)
      {
        grdDetails.DataSource = dt;
        grdDetails.DataBind();

      }
      else
      {
        grdDetails.DataSource = null;
        grdDetails.DataBind();
      }

      if (grdDetails.Rows.Count > 0)
      {
        if (sMasterID == "Employee")
        {
          grdDetails.Columns[3].Visible = true;
          grdDetails.Columns[4].Visible = true;

        }
        else
        {
          grdDetails.Columns[3].Visible = false;
          grdDetails.Columns[4].Visible = false;
        }
      }

    }

    protected void grdDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      System.Web.UI.WebControls.Label lblid = grdDetails.Rows[e.RowIndex].Controls[0].FindControl("lblid") as System.Web.UI.WebControls.Label;

      string sSqlDelete = "DELETE PickList_tran WHERE id='" + lblid.Text + "'";
     
      ocon.Execute(sSqlDelete);

      string sMasterID = Request.QueryString["masid"];


      string sMessage = sMasterID + " deleted successfully";

      ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert( '" + sMessage + "');", true);

      grdDetails.EditIndex = -1;

      gridbind();
    }
  }
}
