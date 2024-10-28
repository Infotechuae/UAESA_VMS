using DAL;
using QRCoder;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class Requestlist : System.Web.UI.Page
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

        btnApproved.Visible = false;
        btnReject.Visible = false;

        string sUserName = "";
        string sSqlUserName = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
        DataTable dtUserName = ocon.GetTable(sSqlUserName, new DataSet());
        if (dtUserName.Rows.Count > 0)
        {
          sUserName = dtUserName.Rows[0]["UserName"].ToString();
        }

        string sStat = Request.QueryString["Stat"];


        if (sStat == "All")
          header1.Text = "All Request";
        if (sStat == "Accepted")
          header1.Text = "Accepted Request";
        if (sStat == "Pending")
          header1.Text = "Pending Request";
        if (sStat == "Rejected")
          header1.Text = "Rejected Request";

        //if (sStat == "Pending")
        //{
        //  btnApproved.Visible = true;
        //  btnReject.Visible = true;
        //}
        //else
        //{
        //  btnApproved.Visible = false;
        //  btnReject.Visible = false;
        //}
       

        txtFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

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
        lnkLocation.Attributes.Add("class", "nav-item");
        liDepart.Attributes.Add("class", "nav-item");
        liFloor.Attributes.Add("class", "nav-item");
        liEmployee.Attributes.Add("class", "nav-item");
        liVisitorType.Attributes.Add("class", "nav-item");
        lnkCard.Attributes.Add("class", "nav-item");
        liUser.Attributes.Add("class", "nav-item");
        liMail.Attributes.Add("class", "nav-item");

        if (sStat == "All")
        {
          lnkRqstlistAll.Attributes.Add("class", "nav-item menu-open");
          lnkRqstlistAccepted.Attributes.Add("class", "nav-item");
          lnkRqstlistPending.Attributes.Add("class", "nav-item");
          lnkRqstlistRejected.Attributes.Add("class", "nav-item");
        }
        else if (sStat == "Accepted")
        {

          lnkRqstlistAll.Attributes.Add("class", "nav-item");
          lnkRqstlistAccepted.Attributes.Add("class", "nav-item menu-open");
          lnkRqstlistPending.Attributes.Add("class", "nav-item");
          lnkRqstlistRejected.Attributes.Add("class", "nav-item");
        }
        else if (sStat == "Pending")
        {
          lnkRqstlistAll.Attributes.Add("class", "nav-item");
          lnkRqstlistAccepted.Attributes.Add("class", "nav-item");
          lnkRqstlistPending.Attributes.Add("class", "nav-item menu-open");
          lnkRqstlistRejected.Attributes.Add("class", "nav-item");
        }
        else if (sStat == "Rejected")
        {
          lnkRqstlistAll.Attributes.Add("class", "nav-item");
          lnkRqstlistAccepted.Attributes.Add("class", "nav-item");
          lnkRqstlistPending.Attributes.Add("class", "nav-item");
          lnkRqstlistRejected.Attributes.Add("class", "nav-item menu-open");
        }
        else
        {
          lnkRqstlistAll.Attributes.Add("class", "nav-item");
          lnkRqstlistAccepted.Attributes.Add("class", "nav-item");
          lnkRqstlistPending.Attributes.Add("class", "nav-item");
          lnkRqstlistRejected.Attributes.Add("class", "nav-item");
        }





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
        aLocation.Attributes.Add("class", "nav-link");
        adepart.Attributes.Add("class", "nav-link");
        aFloor.Attributes.Add("class", "nav-link");
        aEmployee.Attributes.Add("class", "nav-link");
        aVisitorType.Attributes.Add("class", "nav-link");
        aCard.Attributes.Add("class", "nav-link");
        aUser.Attributes.Add("class", "nav-link");
        aMail.Attributes.Add("class", "nav-link");

        if (sStat == "All")
        {
          aAll.Attributes.Add("class", "nav-link active");
          aAccepted.Attributes.Add("class", "nav-link");
          aPending.Attributes.Add("class", "nav-link");
          aRejected.Attributes.Add("class", "nav-link");

        }
        else if (sStat == "Accepted")
        {
          aAll.Attributes.Add("class", "nav-link");
          aAccepted.Attributes.Add("class", "nav-link active");
          aPending.Attributes.Add("class", "nav-link");
          aRejected.Attributes.Add("class", "nav-link");
        }
        else if (sStat == "Pending")
        {
          aAll.Attributes.Add("class", "nav-link");
          aAccepted.Attributes.Add("class", "nav-link");
          aPending.Attributes.Add("class", "nav-link active");
          aRejected.Attributes.Add("class", "nav-link");
        }
        else if (sStat == "Rejected")
        {
          aAll.Attributes.Add("class", "nav-link");
          aAccepted.Attributes.Add("class", "nav-link");
          aPending.Attributes.Add("class", "nav-link");
          aRejected.Attributes.Add("class", "nav-link active");
        }
        else
        {
          aAll.Attributes.Add("class", "nav-link");
          aAccepted.Attributes.Add("class", "nav-link");
          aPending.Attributes.Add("class", "nav-link");
          aRejected.Attributes.Add("class", "nav-link");
        }


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
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      gridbind();
    }


    public void gridbind()
    {
      string sStat = Request.QueryString["Stat"];

      string sUserID = Convert.ToString(Session["UserID"]);
      string sUserGroup = Convert.ToString(Session["UserGroup"]);

      string sUserName = "";
      string sSqlUserName = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = ocon.GetTable(sSqlUserName, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }


      string sSql = "select SecuLobby_VisitingDetails_Self.Ref_No,SecuLobby_VisitorInfo_Self.Visitor_ID, Name,Company,Doc_type,EmiratesID,Mobile,Email,LocationID,Aptment_Dept,host.pl_Value as Host, host.pl_data as HostEmail,Visitor_Type,Dur.pl_Value as Duration,Dur.pl_id as DurID, "
                  + " Purpose,Area_Floor,Checkin_Time,Req_Stat "
                  + " from SecuLobby_VisitingDetails_Self "
                  + " inner join SecuLobby_VisitorInfo_Self on SecuLobby_VisitorInfo_Self.Visitor_ID = SecuLobby_VisitingDetails_Self.Visitor_ID "
                  + " inner join PickList_tran host on host.pl_id = SecuLobby_VisitingDetails_Self.Host_to_Visit and host.pl_head_id = 18 "
                  + " inner join PickList_tran Dur on Dur.pl_id = SecuLobby_VisitingDetails_Self.Duration and Dur.pl_head_id = 6 "
                  + " where convert(date, Checkin_Time)>= '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and convert(date, Checkin_Time)<= '" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'";


      if (sStat == "Accepted")
      {
        sSql += " and Req_Stat='Approved'";
      }
      else if (sStat == "Pending")
      {
        sSql += " and Req_Stat='Pending'";
      }
      else if (sStat == "Rejected")
      {
        sSql += " and Req_Stat='Rejected'";
      }

      if (sUserGroup == "3")
      {
        sSql += " and host.pl_Value='" + sUserName + "'";
      }

      if (txtSearch.Text != "")
      {
        sSql += " and (Name like '%" + txtSearch.Text.Trim() + "%' or Company like '%" + txtSearch.Text.Trim() + "%' or EmiratesID like '%" + txtSearch.Text.Trim() + "%' or Mobile like '%" + txtSearch.Text.Trim() + "%' or Email like '%" + txtSearch.Text.Trim() + "%' or LocationID like '%" + txtSearch.Text.Trim() + "%' or Aptment_Dept like '%" + txtSearch.Text.Trim() + "%' or host.pl_Value like '%" + txtSearch.Text.Trim() + "%' or Dur.pl_Value like '%" + txtSearch.Text.Trim() + "%' or Area_Floor like '%" + txtSearch.Text.Trim() + "%')";
      }
      sSql += " order by Checkin_Time desc";

      DataTable dt = ocon.GetTable(sSql, new DataSet());

      if (dt.Rows.Count > 0)
      {
        grdDetails.DataSource = dt;
        grdDetails.DataBind();
        if (sStat == "Pending")
        {
          btnApproved.Visible = true;
          btnReject.Visible = true;
        }
      }
      else
      {
        grdDetails.DataSource = null;
        grdDetails.DataBind();
        if (sStat != "Pending")
        {
          btnApproved.Visible = false;
          btnReject.Visible = false;
        }
      }

      if (sStat == "Pending")
      {
        if (grdDetails.Rows.Count > 0)
        {
          grdDetails.Columns[0].Visible = true;
        }
        else
        {
          grdDetails.Columns[0].Visible = false;
        }
      }
      else
      {
        grdDetails.Columns[0].Visible = false;
      }
    }

    protected void btnApproved_Click(object sender, EventArgs e)
    {
      int i = 0;
      foreach (GridViewRow grgr in grdDetails.Rows)
      {
        Label lblid = (Label)(grgr.FindControl("lblid"));
        Label lblHostEmail = (Label)(grgr.FindControl("lblHostEmail"));

        Label lblName = (Label)(grgr.FindControl("lblName"));
        Label lblHostName = (Label)(grgr.FindControl("lblHostName"));
        Label lblEmail = (Label)(grgr.FindControl("lblEmail"));
        Label lblCompany = (Label)(grgr.FindControl("lblCompany"));
        Label lblDepartment = (Label)(grgr.FindControl("lblDepartment"));
        Label lblPurpose = (Label)(grgr.FindControl("lblPurpose"));
        Label lblMobile = (Label)(grgr.FindControl("lblMobile"));
        Label lblEmiratesID = (Label)(grgr.FindControl("lblEmiratesID"));
        Label lblVisitorType = (Label)(grgr.FindControl("lblVisitorType"));
        Label lblLocationID = (Label)(grgr.FindControl("lblLocationID"));
        Label lblEmiratesIDType = (Label)(grgr.FindControl("lblEmiratesIDType"));
        Label lblFloor = (Label)(grgr.FindControl("lblFloor"));
        Label lblDurID = (Label)(grgr.FindControl("lblDurID"));

        Label lblVisitorID = (Label)(grgr.FindControl("lblVisitorID"));


        CheckBox check = (CheckBox)(grgr.FindControl("check"));
        if (check.Checked)
        {
          string sCheck = "";
          string sApproverName = "";

          DataTable dtCheck = ocon.GetTable("SELECT Req_Stat from SecuLobby_VisitingDetails_Self where Ref_No='" + lblid.Text + "'", new DataSet());
          if (dtCheck.Rows.Count > 0)
          {
            sCheck = dtCheck.Rows[0]["Req_Stat"].ToString();
          }

          if (sCheck == "Pending")
          {
            string sSqlUpdate = "UPDATE SecuLobby_VisitingDetails_Self SET Req_Stat='Approved', Approvedby='" + lblHostEmail.Text + "' where Ref_No='" + lblid.Text + "'";

            ocon.Execute(sSqlUpdate);

            int iCid = 0;
            DataTable dtCid = ocon.GetTable("SELECT isnull(max(cId),0) as cId FROM Scheduling", new DataSet());
            if (dtCid.Rows.Count > 0)
            {
              iCid = Convert.ToInt32(dtCid.Rows[0]["cId"]);
            }
            iCid = iCid + 1;

            Random rnd = new Random();
            int myRandomNo = rnd.Next(1000000, 9999999);
            string sPersonID = myRandomNo.ToString();
            string sQRCode = sPersonID;

            DateTime dtStartTime = DateTime.Now;
            DateTime dtEndTime = dtStartTime.AddMinutes(60);

            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO Scheduling (cId,UserId,Status,Subject,Description,Label,StartTime,EndTime,Location,AllDay,ContactInfo,EntryID,QRCode,Organizer,Loc_ID,HostName,VisPhone,VisCompany,IDType,IDNumber,FloorName,Duration,Visitor_Type,LocationID,Purpose)" + Environment.NewLine);
            sb.Append("VALUES ('" + iCid + "',1,1,'" + lblName.Text.Trim() + "','Meeting',1,'" + dtStartTime + "','" + dtEndTime + "','" + lblDepartment.Text + "',0,'" + lblEmail.Text.Trim() + "','" + iCid + "','" + sQRCode + "','" + lblHostEmail.Text + "',1,'" + lblHostName.Text + "','" + lblMobile.Text + "','" + lblCompany.Text + "','" + lblEmiratesIDType.Text + "','" + lblEmiratesID.Text + "','" + lblFloor.Text + "'," + lblDurID.Text + ",'" + lblVisitorType.Text + "','" + lblLocationID.Text + "','" + lblPurpose.Text + "')" + Environment.NewLine);

            ocon.Execute(sb.ToString());

            string sSql = "update Scheduling set Image=(select Image from Visitor_Image_Self where ID='" + lblVisitorID.Text + "') where cId='" + iCid + "'";
            ocon.Execute(sSql);

            i++;

            SendApproverEmail(lblName.Text, lblEmail.Text, "Approved", sQRCode, lblHostName.Text, lblDepartment.Text, lblCompany.Text.Trim(), sApproverName, lblPurpose.Text);

          }

        }

      }
      if (i > 0)
      {
        string sDestURL = string.Format("\"{0}\"", "Requestlist.aspx?Stat=Pending");
        string smessage = string.Format("\"{0}\"", "Visit Request Approved");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
    }


    protected void Search_Click(object sender, EventArgs e)
    {
      if (txtFromDate.Text != "" && txtToDate.Text != "")
      {
        if (Convert.ToDateTime(txtToDate.Text) >= Convert.ToDateTime(txtFromDate.Text))
        {
          gridbind();

        }
      }
    }



    protected void btnReject_Click(object sender, EventArgs e)
    {
      int i = 0;
      foreach (GridViewRow grgr in grdDetails.Rows)
      {
        Label lblid = (Label)(grgr.FindControl("lblid"));
        Label lblHostEmail = (Label)(grgr.FindControl("lblHostEmail"));

        Label lblName = (Label)(grgr.FindControl("lblName"));
        Label lblHostName = (Label)(grgr.FindControl("lblHostName"));
        Label lblEmail = (Label)(grgr.FindControl("lblEmail"));
        Label lblCompany = (Label)(grgr.FindControl("lblCompany"));
        Label lblDepartment = (Label)(grgr.FindControl("lblDepartment"));
        CheckBox check = (CheckBox)(grgr.FindControl("check"));
        if (check.Checked)
        {
          string sCheck = "";

          DataTable dtCheck = ocon.GetTable("SELECT Req_Stat from SecuLobby_VisitingDetails_Self where Ref_No='" + lblid.Text + "'", new DataSet());
          if (dtCheck.Rows.Count > 0)
          {
            sCheck = dtCheck.Rows[0]["Req_Stat"].ToString();
          }

          if (sCheck == "Pending")
          {
            string sSqlUpdate = "UPDATE SecuLobby_VisitingDetails_Self SET Req_Stat='Rejected', Approvedby='" + lblHostEmail.Text + "' where Ref_No='" + lblid.Text + "'";

            ocon.Execute(sSqlUpdate);

            i++;
            SendRejectionEmail(lblName.Text, lblEmail.Text, "Approved", "", lblHostName.Text, lblDepartment.Text, lblCompany.Text.Trim());


          }

        }

      }
      if (i > 0)
      {
        string sDestURL = string.Format("\"{0}\"", "Requestlist.aspx?Stat=Pending");
        string smessage = string.Format("\"{0}\"", "Visit Request Rejected");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
    }

    private void SendRejectionEmail(string sName, string sEmail, string sStatus, string sQRCode, string sHostName, string sDepartment, string sCompany)
    {
      DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));

      string sCopanyName = "UAE Space Agency";

      string sStringConfig = "SELECT ISNULL(Config_value,'') AS Config_value  FROM Tbl_General_Configuration WHERE Config_name='Company name'";
      DataTable dtConfig = ocon.GetTable(sStringConfig, new DataSet());

      if (dtConfig.Rows.Count > 0)
        sCopanyName = dtConfig.Rows[0]["Config_value"].ToString();

      string EmailMask = System.Configuration.ConfigurationManager.AppSettings["EmailMask"];
      string EmailEnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"];
      string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
      string EmailSubject = "Visit Request Rejected";

      EmailSubject = "Visit Request Rejected";


     

      string sApproverName = "";
   


      string strHTML = "<p> Dear " + sName + ",</p>";

      strHTML = strHTML + "<p>Your visit request is Rejected</p>";
      strHTML = strHTML + "</p><p>      Company : " + sCompany;
      strHTML = strHTML + "</p><p>      Email : " + sEmail;
      strHTML = strHTML + "</p><p>      DateTime : " + DateTime.Now.ToString();

      strHTML = strHTML + "</p><p>      Host Name : " + sHostName;


      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";

      strHTML = strHTML + "</p>Best Regards, <p>";
      strHTML = strHTML + "</p>" + sCopanyName + "<p>";


      sendEmail.SendEmails(sName, sEmail, "3434", strHTML, EmailSubject, null, null);


    }

    private void SendApproverEmail(string sName, string sEmail, string sStatus, string sQRCode, string sHostName, string sDepartment, string sCompany, string sApprovedBy,string sPurpose)
    {


      DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));


      string sCopanyName = "UAE Space Agency";

      string sStringConfig = "SELECT ISNULL(Config_value,'') AS Config_value  FROM Tbl_General_Configuration WHERE Config_name='Company name'";
      DataTable dtConfig = ocon.GetTable(sStringConfig, new DataSet());

      if (dtConfig.Rows.Count > 0)
        sCopanyName = dtConfig.Rows[0]["Config_value"].ToString();

      string EmailMask = System.Configuration.ConfigurationManager.AppSettings["EmailMask"];
      string EmailEnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"];
      string DisableEmailWS = System.Configuration.ConfigurationManager.AppSettings["DisableEmailWS"];
      string EmailSubject = "Visit Request Approved";

      EmailSubject = "Visit Request Approved";

      QRCodeGenerator qrGenerator1 = new QRCodeGenerator();
      byte[] byteImageVis = null;

      QRCodeGenerator.QRCode qrCodeVis = qrGenerator1.CreateQrCode(sQRCode, QRCodeGenerator.ECCLevel.Q);
      System.Web.UI.WebControls.Image imgBarCodeVis = new System.Web.UI.WebControls.Image();
      imgBarCodeVis.Height = 100;
      imgBarCodeVis.Width = 150; //150
      using (Bitmap bitMap = qrCodeVis.GetGraphic(20))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
          byteImageVis = ms.ToArray();

        }


        //ASPxImage1.Controls.Add(imgBarCode);
      }

      MemoryStream ms1 = new MemoryStream(byteImageVis);
      LinkedResource res = new LinkedResource(ms1, "image/jpeg");
      res.ContentId = Guid.NewGuid().ToString();

      string strHTML = "<p> Dear " + sName + ",</p>";

      strHTML = strHTML + "<p>Your Meeting request is approved.</p>";
      strHTML = strHTML + "<p>Please visit the nearest reception area.</p>";


      strHTML = strHTML + "</p><p>      Visitor Name : " + sName;
      strHTML = strHTML + "</p><p>      Company : " + sCompany;
      strHTML = strHTML + "</p><p>      Email : " + sEmail;
      strHTML = strHTML + "</p><p>      DateTime : " + DateTime.Now.ToString();

      strHTML = strHTML + "</p><p>      Host Name : " + sHostName;
      strHTML = strHTML + "</p><p>      Purpose : " + sPurpose;


      if (sApprovedBy != "")
      {
        strHTML = strHTML + "</p><p>      Approved By : " + sApprovedBy;
      }
      strHTML = strHTML + @"<img src='cid:" + res.ContentId + @"' width=200 height=200 />";


      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";
      strHTML = strHTML + "</p><p>";


      strHTML = strHTML + "</p>Best Regards, <p>";
      strHTML = strHTML + "</p>" + sCopanyName + "<p>";


      sendEmail.SendEmails(sName, sEmail, "3434", strHTML, EmailSubject, byteImageVis, res);



      #region Host



      #endregion


      #region Cordinator



      string SelfAdminEmail = System.Configuration.ConfigurationManager.AppSettings["SelfAdminEmail"];
      //////////////////////Host Email///////////////////////////////////////
      if (SelfAdminEmail == "True")
      {


        string sSqlAdminEmail = "select pl_data,isnull(Other_Data2,'') as Name  from PickList_tran where pl_head_id=4 and pl_value='" + sDepartment + "'";
        DataTable dtAdminEmail = ocon.GetTable(sSqlAdminEmail, new DataSet());
        if (dtAdminEmail.Rows.Count > 0)
        {
          string sAdminEmail = Convert.ToString(dtAdminEmail.Rows[0]["pl_data"]);
          string sAdminEmailName = Convert.ToString(dtAdminEmail.Rows[0]["Name"]);


          string sHead = "Dear Sir,";

          if (!string.IsNullOrEmpty(sAdminEmail))
          {
            if (!string.IsNullOrEmpty(sAdminEmailName))
            {
              sHead = "Dear " + sAdminEmailName + ",";
            }
            string strHTML1 = "<p> " + sHead + "</p>";



            if (sApprovedBy != "")
            {
              strHTML1 = strHTML1 + "<p>The Below visit request is approved by " + sApprovedBy + "</p>";
            }
            else
            {
              strHTML1 = strHTML1 + "<p>The Below visit request is approved</p>";
            }

            strHTML1 = strHTML1 + "</p><p >      Visitor Name : " + sName;
            strHTML1 = strHTML1 + "</p><p>      Company : " + sCompany;
            strHTML1 = strHTML1 + "</p><p>      Email : " + sEmail;
            strHTML1 = strHTML1 + "</p><p>      Purpose : " + sPurpose;

            strHTML1 = strHTML1 + "</p><p>";
            strHTML1 = strHTML1 + "</p><p>";
            strHTML1 = strHTML1 + "</p><p>";
            strHTML1 = strHTML1 + "</p><p>";


            strHTML1 = strHTML1 + "</p>Best Regards, <p>";
            strHTML1 = strHTML1 + "</p>" + sCopanyName + "<p>";


            sendEmail.SendEmails(sDepartment, sAdminEmail, "3434", strHTML1, EmailSubject, null, null);
          }
        }
      }

      #endregion


    }

  }
}
