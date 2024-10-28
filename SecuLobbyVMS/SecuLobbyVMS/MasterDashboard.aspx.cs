using DAL;
using SecuLobbyVMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class MasterDashboard : System.Web.UI.Page
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
        string sUserGroup = Convert.ToString(Session["UserGroup"]);

        string sLocName = Convert.ToString(Session["Loc_name"]);



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

        lnkDashboard.Attributes.Add("class", "nav-item menu-open");
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
        lnkLocation.Attributes.Add("class", "nav-item");
        liDepart.Attributes.Add("class", "nav-item");
        liFloor.Attributes.Add("class", "nav-item");
        liEmployee.Attributes.Add("class", "nav-item");
        liVisitorType.Attributes.Add("class", "nav-item");
        lnkCard.Attributes.Add("class", "nav-item");
        liUser.Attributes.Add("class", "nav-item");
        liMail.Attributes.Add("class", "nav-item");
        lnkRqstlistAll.Attributes.Add("class", "nav-item");
        lnkRqstlistAccepted.Attributes.Add("class", "nav-item");
        lnkRqstlistPending.Attributes.Add("class", "nav-item");
        lnkRqstlistRejected.Attributes.Add("class", "nav-item");

        aDash.Attributes.Add("class", "nav-link active");
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
        aLocation.Attributes.Add("class", "nav-link");
        adepart.Attributes.Add("class", "nav-link");
        aFloor.Attributes.Add("class", "nav-link");
        aEmployee.Attributes.Add("class", "nav-link");
        aVisitorType.Attributes.Add("class", "nav-link");
        aCard.Attributes.Add("class", "nav-link");
        aUser.Attributes.Add("class", "nav-link");
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

        DataSet cmbDS2 = new DataSet();
        DataTable DT = new DataTable();


        string sUserName = "";
        string sSql = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
        DataTable dtUserName = ocon.GetTable(sSql, new DataSet());
        if (dtUserName.Rows.Count > 0)
        {
          sUserName = dtUserName.Rows[0]["UserName"].ToString();
        }

        if (sUserGroup == "1")
        {
          dvheader.Attributes.Add("style", "display:block");
        }
        else
        {
          dvheader.Attributes.Add("style", "display:none");
        }

        try
        {
          if (sUserGroup == "1")
          {
            cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Session["Loc_ID"].ToString(), "GetVisitorCountData", "GetVisitorCountData_Admin", MyConnection.ReadConStr("Local"));

          }
          else if (sUserGroup == "2")
          {
            cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Session["Loc_ID"].ToString(), "GetVisitorCountData", "GetVisitorCountData_Admin", MyConnection.ReadConStr("Local"));

          }
          else if (sUserGroup == "3")
          {
            cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Session["Loc_ID"].ToString(), sUserName, "GetVisitorCountData_user", MyConnection.ReadConStr("Local"));

          }

          else
          {
            cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Session["Loc_ID"].ToString(), "GetVisitorCountData", "GetVisitorCountData_Admin", MyConnection.ReadConStr("Local"));


          }
          DT = cmbDS2.Tables[0];

          if (DT.Rows.Count > 0)
          {
            foreach (DataRow row in DT.Rows)
            {
              lblCheckOutValue.Text = row["Checkout"].ToString();
              lblcheckinValue.Text = row["Checkin"].ToString();
              lblEmergencyValue.Text = row["Checkin"].ToString();

              lblAppointmentValue.Text = row["Appointment"].ToString();
            }
          }

          string sSqlTotReq = "SELECT COUNT(Ref_No) AS TotalRequest FROM SecuLobby_VisitingDetails_Self inner join PickList_tran host on host.pl_id = SecuLobby_VisitingDetails_Self.Host_to_Visit and host.pl_head_id = 18  WHERE convert(date,[Checkin_Time])= convert(date,getdate())";
          if (sUserGroup == "3")
          {
            sSqlTotReq += " and host.pl_Value='" + sUserName + "'";
          }

          DataTable dtTotReq = ocon.GetTable(sSqlTotReq, new DataSet());
          if (dtTotReq != null)
          {
            if (dtTotReq.Rows.Count > 0)
            {
              lblWatchlistValue.Text = dtTotReq.Rows[0]["TotalRequest"].ToString();
            }
            else
            {
              lblWatchlistValue.Text = "0";
            }

          }
          else
          {
            lblWatchlistValue.Text = "0";
          }

        }
        catch (Exception er)
        {

        }

        try
        {
          if (sUserGroup == "3")
          {
            cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Session["Loc_ID"].ToString(), sUserName, "GetTotalVisitorCountData_USER", MyConnection.ReadConStr("Local"));
          }
          else
          {
            cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Session["Loc_ID"].ToString(), "GetTotalVisitorCountData", "GetTotalVisitorCountData", MyConnection.ReadConStr("Local"));
          }
          DT = cmbDS2.Tables[0];

          if (DT.Rows.Count > 0)
          {
            foreach (DataRow row in DT.Rows)
            {

              lblTotalCheckin.Text = row["Checkin"].ToString();
              lblTotalCheckout.Text = row["Checkout"].ToString();
              lblTotalWatchlist.Text = row["WatchList"].ToString();
              lblTotalAppointment.Text = row["Appointment"].ToString();
            }
          }
        }
        catch (Exception er)
        {

        }


        string sSqlVistDetail = "select SecuLobby_VisitorInfo.Name,SecuLobby_VisitorInfo.EmiratesID,SecuLobby_VisitorInfo.Mobile,  "
                              + " case when Checkin_Status = 'N' and CheckOut_Time is not null then 'Check out' when Checkin_Status = 'Y'  and[CheckOut_Time] is null and datediff(MINUTE, Meeting_End_Time, isnull(CheckOut_Time, getdate()) )  > 2 then 'Time Out' else 'Checked In' end as CheckInOutStatus, "
                              + " case when Checkin_Status = 'N' and CheckOut_Time is not null then 'badge badge-danger' when Checkin_Status = 'Y'  and[CheckOut_Time] is null and datediff(MINUTE, Meeting_End_Time, isnull(CheckOut_Time, getdate()) )  > 2 then 'badge badge-warning' else 'badge badge-success' end as CheckInOutColur "
                              + " from SecuLobby_VisitingDetails "
                              + " inner join SecuLobby_VisitorInfo on SecuLobby_VisitorInfo.Visitor_ID = SecuLobby_VisitingDetails.Visitor_ID "
                              + " where Checkin_Time >= convert(datetime, '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 101)";


        DataTable dtDet = ocon.GetTable(sSqlVistDetail, new DataSet());

        if (dtDet.Rows.Count > 0)
        {
          //grdVisDeta.DataSource = dtDet;
          //grdVisDeta.DataBind();
        }

        //int iCurrentMonth = DateTime.Now.Month;
        //int iCurrentYear = DateTime.Now.Year;
        //DateTime dtFromDate = dtFromDate = Convert.ToDateTime(iCurrentYear.ToString() + "-" + (iCurrentMonth-2).ToString() + "-01");

        DateTime iCurrentDate = DateTime.Now;

        DateTime dtFromDate = iCurrentDate.AddDays(-14);

        DateTime dtToDate = iCurrentDate;

        lblVisitorrange.Text = "Visitor : " + dtFromDate.ToString("dd/MMM/yyyy") + " to " + dtToDate.ToString("dd/MMM/yyyy");


        if (sUserGroup == "3")
        {

          divVisHis.Attributes.Add("style", "display:none");
        }
        else
        {
          divVisHis.Attributes.Add("style", "display:block");
        }

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);

      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_20", ci);
      checkin.Text = rm.GetString("STR_18", ci);
      lblCheckOut.Text = rm.GetString("STR_21", ci);

      lblAppointment.Text = rm.GetString("STR_35", ci);

      lblMVR.Text = rm.GetString("STR_36", ci);


      lblVisstatis.Text = rm.GetString("STR_38", ci);

      lblEmergency.Text = rm.GetString("STRN_11", ci);
      lblWatchlist.Text = rm.GetString("STRN_12", ci);
      lblVisstatis.Text = rm.GetString("STRN_13", ci);
      Label2.Text = rm.GetString("STRN_14", ci);
      Label3.Text = rm.GetString("STRN_1", ci);
      Label4.Text = rm.GetString("STR_56", ci);
      lblStatistics.Text = rm.GetString("STR_37", ci);
    }
    [WebMethod]
    public static void UpdateUser(string Password)
    {


      string UserTabID = (string)HttpContext.Current.Session["UserID"];

      string UserCompany = (string)HttpContext.Current.Session["User_Company"];
      DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));

      SqlCommand cmdOrl = new SqlCommand("USER_PASSWORD_CHANGE");
      cmdOrl.CommandType = CommandType.StoredProcedure;
      cmdOrl.Parameters.AddWithValue("@USERID", Convert.ToInt32(UserTabID));
      cmdOrl.Parameters.AddWithValue("@USERPASSWORD", EncryptDecryptHelper.Encrypt(Password));


      DBSQL dbs1 = new DBSQL();

      string rval = dbs1.ExecuteStoredProcedure(cmdOrl, MyConnection.ReadConStr("Local"));
    }


    [WebMethod]
    //public static string GetMonthlyVisitorReport()
    //{


    //  DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));

    //  List<TotalVisiotrReport> RCFA = new List<TotalVisiotrReport>();

    //  DataTable dtPes = new DataTable();
    //  DataRow drPes;
    //  DataColumn dcPes;
    //  dcPes = new DataColumn("MonthName", System.Type.GetType("System.String"));
    //  dtPes.Columns.Add(dcPes);
    //  dcPes = new DataColumn("TotalCount", System.Type.GetType("System.String"));
    //  dtPes.Columns.Add(dcPes);



    //  int iCurrentMonth = DateTime.Now.Month;
    //  int iCurrentYear = DateTime.Now.Year;

    //  for (int i = iCurrentMonth - 2; i <= iCurrentMonth; i++)
    //  {
    //    DateTime Firstdt = DateTime.Now;

    //    if (i.ToString().Length == 1)
    //      Firstdt = Convert.ToDateTime(iCurrentYear.ToString() + "-0" + i.ToString() + "-01");
    //    else
    //      Firstdt = Convert.ToDateTime(iCurrentYear.ToString() + "-" + i.ToString() + "-01");

    //    string sMontName = Firstdt.ToString("MMMM");

    //    drPes = dtPes.NewRow();
    //    drPes["MonthName"] = sMontName;

    //    int iCunt = 0;
    //    string sSqlCount = "SELECT count(Ref_No) as Total FROM SecuLobby_VisitingDetails WHERE month(Checkin_Time)= " + i + " and year(Checkin_Time)= " + iCurrentYear + "";
    //    DataTable dtCount = oconn.GetTable(sSqlCount, new DataSet());
    //    if (dtCount.Rows.Count > 0)
    //    {
    //      iCunt = Convert.ToInt32(dtCount.Rows[0]["Total"]);
    //    }
    //    drPes["TotalCount"] = iCunt.ToString();

    //    dtPes.Rows.Add(drPes);
    //    dtPes.AcceptChanges();
    //  }




    //  if (dtPes.Rows.Count > 0)
    //  {
    //    foreach (DataRow dr in dtPes.Rows)
    //    {
    //      TotalVisiotrReport objValues = new TotalVisiotrReport();
    //      objValues.MonthName = dr["MonthName"].ToString();
    //      objValues.TotalVisitor = Convert.ToInt32(dr["TotalCount"]);

    //      RCFA.Add(objValues);
    //    }


    //  }

    //  JavaScriptSerializer js = new JavaScriptSerializer();

    //  return js.Serialize(RCFA);
    //}
    public static string GetMonthlyVisitorReport()
    {
      DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));

      List<TotalVisiotrReport> RCFA = new List<TotalVisiotrReport>();




      string sSqlCount = "select Datename(weekday, convert(date, Checkin_Time))   as DayName,  count(Ref_no) as Total from dbo.SecuLobby_VisitingDetails where Checkin_Status in('Y', 'N') "
                        + " and convert(date, [Checkin_Time])>= convert(date, getdate() - 60) "
                        + " group by  convert(date, Checkin_Time),Datename(weekday, convert(date, Checkin_Time)) order by Datename(DAY, convert(date, Checkin_Time))";
      DataTable dt = oconn.GetTable(sSqlCount, new DataSet());


      if (dt.Rows.Count > 0)
      {
        foreach (DataRow dr in dt.Rows)
        {
          TotalVisiotrReport objValues = new TotalVisiotrReport();
          objValues.MonthName = dr["DayName"].ToString();
          objValues.TotalVisitor = Convert.ToInt32(dr["Total"]);

          RCFA.Add(objValues);
        }
      }

      JavaScriptSerializer js = new JavaScriptSerializer();

      return js.Serialize(RCFA);


    }

    [WebMethod]
    public static string GetVistorTypeReport()
    {
      string sUserID = (string)HttpContext.Current.Session["UserID"];
      string sUserGroup = (string)HttpContext.Current.Session["UserGroup"];

      DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));

      string sUserName = "";
      string sSqlUserName = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = oconn.GetTable(sSqlUserName, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      List<VisiotrTypeReport> RCFA = new List<VisiotrTypeReport>();


      string sSql = "select Visitor_Type,count(Ref_No) as Total from SecuLobby_VisitingDetails  ";
      if (sUserGroup == "3")
      {
        sSql += " WHERE Host_to_Visit='" + sUserName + "'";

      }
      sSql += "group by Visitor_Type order by Visitor_Type";

      DataTable dt = oconn.GetTable(sSql, new DataSet());


      if (dt.Rows.Count > 0)
      {
        foreach (DataRow dr in dt.Rows)
        {
          VisiotrTypeReport objValues = new VisiotrTypeReport();
          objValues.VisitorType = dr["Visitor_Type"].ToString();
          objValues.TotalVisitor = Convert.ToInt32(dr["Total"]);

          RCFA.Add(objValues);
        }
      }

      JavaScriptSerializer js = new JavaScriptSerializer();

      return js.Serialize(RCFA);
    }

    [WebMethod]
    public static string GetDepartmentReport()
    {
      string sUserID = (string)HttpContext.Current.Session["UserID"];
      string sUserGroup = (string)HttpContext.Current.Session["UserGroup"];

      DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));


      string sUserName = "";
      string sSqlUserName = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = oconn.GetTable(sSqlUserName, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      List<NationalityReport> RCFA = new List<NationalityReport>();

      string sSql = "select Aptment_Dept,COUNT(Ref_No) AS Total from SecuLobby_VisitingDetails INNER JOIN SecuLobby_VisitorInfo ON SecuLobby_VisitorInfo.Visitor_ID=SecuLobby_VisitingDetails.Visitor_ID ";


      if (sUserGroup == "3")
      {
        sSql += " WHERE Host_to_Visit='" + sUserName + "'";

      }
      sSql += "group by Aptment_Dept order by COUNT(Ref_No) desc";

      DataTable dt = oconn.GetTable(sSql, new DataSet());


      if (dt.Rows.Count > 0)
      {
        foreach (DataRow dr in dt.Rows)
        {
          NationalityReport objValues = new NationalityReport();
          objValues.Nationality = dr["Aptment_Dept"].ToString();
          objValues.TotalVisitor = Convert.ToInt32(dr["Total"]);

          RCFA.Add(objValues);
        }
      }

      JavaScriptSerializer js = new JavaScriptSerializer();

      return js.Serialize(RCFA);
    }

    [WebMethod]
    public static string GetLocationReport()
    {

      string sUserID = (string)HttpContext.Current.Session["UserID"];
      string sUserGroup = (string)HttpContext.Current.Session["UserGroup"];

      DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));

      string sUserName = "";
      string sSqlUserName = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = oconn.GetTable(sSqlUserName, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      List<GenderReport> RCFA = new List<GenderReport>();

      string sSql = "select LocationID,COUNT(Ref_No) AS Total from SecuLobby_VisitingDetails INNER JOIN SecuLobby_VisitorInfo ON SecuLobby_VisitorInfo.Visitor_ID=SecuLobby_VisitingDetails.Visitor_ID ";


      if (sUserGroup == "3")
      {
        sSql += " WHERE Host_to_Visit='" + sUserName + "'";

      }
      sSql += "group by LocationID order by LocationID";
      DataTable dt = oconn.GetTable(sSql, new DataSet());


      if (dt.Rows.Count > 0)
      {
        foreach (DataRow dr in dt.Rows)
        {
          GenderReport objValues = new GenderReport();
          objValues.Gender = dr["LocationID"].ToString();
          objValues.TotalVisitor = Convert.ToInt32(dr["Total"]);

          RCFA.Add(objValues);
        }
      }

      JavaScriptSerializer js = new JavaScriptSerializer();

      return js.Serialize(RCFA);
    }

    class TotalVisiotrReport
    {
      public int TotalVisitor;
      public string MonthName;
    }

    class VisiotrTypeReport
    {
      public int TotalVisitor;
      public string VisitorType;
    }
    class NationalityReport
    {
      public int TotalVisitor;
      public string Nationality;
    }
    class GenderReport
    {
      public int TotalVisitor;
      public string Gender;
    }

    protected void btnEmail_Click(object sender, EventArgs e)
    {
      string sUserID = Convert.ToString(Session["UserID"]);
      string sUserGroup = Convert.ToString(Session["UserGroup"]);

      string sUserName = "";
      string sSqlUserName = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = ocon.GetTable(sSqlUserName, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      string sSql = "SELECT DISTINCT SecuLobby_VisitorInfo.Name,SecuLobby_VisitorInfo.Email FROM SecuLobby_VisitingDetails "
                  + " INNER JOIN SecuLobby_VisitorInfo ON SecuLobby_VisitorInfo.Visitor_ID = SecuLobby_VisitingDetails.Visitor_ID "
                  + " WHERE Checkin_Status = 'Y'";
      if (sUserGroup == "3")
      {
        sSql += " and Host_to_Visit='" + sUserName + "'";

      }
      DataTable dt = ocon.GetTable(sSql, new DataSet());


      if (dt.Rows.Count > 0)
      {

        foreach (DataRow dr in dt.Rows)
        {
          string sName = dr["Name"].ToString();
          string sEmail = dr["Email"].ToString();

          if (!string.IsNullOrEmpty(sEmail))
          {
            string EmailSubject = "Emergency Message";
            string EmailID = "";
            string EmailPassword = "";
            string EmailSMTP = "";
            string EmailPort = "";
            string iSSl = "False";
            string EmailAccount = "";
            string AccountName = "";

            string sSqlMail = "SELECT ID, EmailID, EmailPWD, SMTPServer, EmailPort,EmailAccount,AccountName,isnull(IsSSl,'False') as IsSSl FROM MailSettings";
            DataTable dtMailSetting = ocon.GetTable(sSqlMail, new DataSet());
            if (dtMailSetting.Rows.Count > 0)
            {

              EmailID = Convert.ToString(dtMailSetting.Rows[0]["EmailID"]);
              EmailPassword = Convert.ToString(dtMailSetting.Rows[0]["EmailPWD"]);
              EmailPassword = EncryptDecryptHelper.Decrypt(EmailPassword);
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
              strHTML = strHTML + "<p>  " + txtWatchlistReason.Text.Trim() + "</p>";

              msg.IsBodyHtml = true;

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

    protected void btnSMS_Click(object sender, EventArgs e)
    {

    }
  }
}
