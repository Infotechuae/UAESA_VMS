using DAL;
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
//using Microsoft.Office; 
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.iCalendar;
using SecuLobby.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class frmSchedule : System.Web.UI.Page
  {
    ResourceManager rm;
    CultureInfo ci;
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    ASPxSchedulerStorage Storage { get { return ASPxScheduler1.Storage; } }

    protected override void OnInitComplete(EventArgs e)
    {
      base.OnInitComplete(e);

      base.OnInitComplete(e);

      SetupStatuses(ASPxScheduler1);
      ASPxScheduler1.InitNewAppointment += new AppointmentEventHandler(ASPxScheduler1_InitNewAppointment);
      ASPxScheduler1.OptionsBehavior.ShowDetailedErrorInfo = false;
      DataHelper.EnsureOnlineVersionModificationLock(ASPxScheduler1);
      ApplyUserRestrictions();

      if (Request.QueryString.Count == 1 && Request.QueryString[0] == "SeculobbyCalendar.ics")
        PostCalendarFile();
    }
    protected void SPxScheduler1_OnBeforeExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e)
    {
      if (e.CommandId == "EXPORTAPT")
      {
        e.Command = new ExportAppointmentCallbackCommand((ASPxScheduler)sender);
      }
      //if (e.CommandId == "CREATAPTWR")
      //  e.Command = new CreateAppointmentWithReminderCallbackCommand(ASPxScheduler1);
    }
    TimeOfDayInterval[] workTimes = new TimeOfDayInterval[] {
            new TimeOfDayInterval(TimeSpan.FromHours(0), TimeSpan.FromHours(16)),
            new TimeOfDayInterval(TimeSpan.FromHours(10), TimeSpan.FromHours(20)),
            null,
            new TimeOfDayInterval(TimeSpan.FromHours(7), TimeSpan.FromHours(15)),
            new TimeOfDayInterval(TimeSpan.FromHours(16), TimeSpan.FromHours(24)),
        };

    void PostCalendarFile()
    {
      iCalendarExporter exporter = new iCalendarExporter(ASPxScheduler1.Storage);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        exporter.Export(memoryStream);
        Stream responseStream = Response.OutputStream;
        memoryStream.WriteTo(responseStream);
        responseStream.Flush();
      }
      Response.ContentType = "text/calendar";
      Response.AddHeader("Content-Disposition", "attachment; filename=SeculobbyCalendar.ics");
      Response.End();
    }
    protected void ASPxScheduler1_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
    {
      ASPxSchedulerPopupMenu menu = e.Menu;
      if (menu.MenuId.Equals(SchedulerMenuItemId.AppointmentMenu))
      {
        DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem("Export Appointment", "ExportAppointment");
        menu.Items.Insert(1, item);
        menu.ClientSideEvents.ItemClick = "function(s, e) { OnMenuClick(s,e); }";
      }
    }

    void ApplyOptions()
    {
      ASPxSchedulerOptionsForms options = ASPxScheduler1.OptionsForms;
      options.BeginUpdate();
      try
      {
        SchedulerFormVisibility value = (SchedulerFormVisibility)0;
        options.AppointmentFormVisibility = value;
        options.GotoDateFormVisibility = value;
      }
      finally
      {
        options.EndUpdate();
      }
    }

    protected void ASPxScheduler1_QueryWorkTime(object sender, QueryWorkTimeEventArgs e)
    {
      //if (!chkCustomWorkTime.Checked)
      //    return;

      if (ASPxScheduler1.Storage.Resources == null)
        return;

      int resourceIndex = ASPxScheduler1.Storage.Resources.Items.IndexOf(e.Resource);
      if (resourceIndex >= 0)
      {
        if (resourceIndex == 0)
        {
          if ((e.Interval.Start.Day % 2) == 0)
            e.WorkTime = workTimes[resourceIndex % workTimes.Length];
          else
            e.WorkTime = TimeOfDayInterval.Empty;
        }
        else
        {
          if (ASPxScheduler1.WorkDays.IsWorkDay(e.Interval.Start.Date))
            e.WorkTime = workTimes[resourceIndex % workTimes.Length];
        }
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);


        AppointmentCalenderTran("", "GridAppoinment");

        ASPxScheduler1.GoToToday();

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }

    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_35", ci);
      header2.Text = rm.GetString("STR_62", ci);

    }
    protected string GetHeader(string str)
    {
      string sHeaderName = "";

      string sLang = Convert.ToString(Session["Lang"]);

      Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
      rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
      ci = Thread.CurrentThread.CurrentCulture;

      if (str == "Host")
        sHeaderName = rm.GetString("STR_13", ci);
      if (str == "Visitor")
        sHeaderName = rm.GetString("STR_39", ci);
      if (str == "Visitor Email")
        sHeaderName = rm.GetString("STR_7", ci);
      if (str == "Department")
        sHeaderName = rm.GetString("STR_11", ci);
      if (str == "Start Date")
        sHeaderName = rm.GetString("STR_59", ci);
      if (str == "End Date")
        sHeaderName = rm.GetString("STR_60", ci);
      if (str == "Duration")
        sHeaderName = rm.GetString("STR_15", ci);
      if (str == "Purpose")
        sHeaderName = rm.GetString("STR_61", ci);
      if (str == "Checkin")
        sHeaderName = rm.GetString("STR_18", ci);

      return sHeaderName;
    }

    void SetupStatuses(ASPxScheduler control)
    {
      control.Storage.Appointments.Statuses.Clear();
      using (var context = new ShedDbContext())
      {
        foreach (var usType in context.UsageTypes.ToList())
        {
          string name = usType.Name;
          Color color = GetStatusColor(usType.Color);
          control.Storage.Appointments.Statuses.Add(GetStatusColor(color), name, name);
        }
      }
    }
    Color GetStatusColor(object cl)
    {
      if (cl == DBNull.Value)
        return Color.FromArgb(0xFFFFFF);
      if (cl is Color)
        return (Color)cl;
      int statusColor = Convert.ToInt32(cl);
      return Color.FromArgb(statusColor);
    }
    void ASPxScheduler1_InitNewAppointment(object sender, AppointmentEventArgs e)
    {
      e.Appointment.StatusKey = 0;
    }

    void ApplyUserRestrictions()
    {
      ASPxScheduler1.BeginUpdate();
      try
      {
        SchedulerOptionsCustomization options = ASPxScheduler1.OptionsCustomization;
        options.AllowAppointmentConflicts = true ? AppointmentConflictsMode.Allowed : AppointmentConflictsMode.Forbidden;

        options.AllowAppointmentMultiSelect = true;

      }
      finally
      {
        ASPxScheduler1.EndUpdate();
      }
      ASPxScheduler1.ApplyChanges(ASPxSchedulerChangeAction.RenderViewMenu);
    }

    UsedAppointmentType ToUsedAppointmentType(ASPxCheckBox chk)
    {
      return chk.Checked ? UsedAppointmentType.All : UsedAppointmentType.None;
    }


    void View_ControlsCreated(object sender, EventArgs e)
    {


      ASPxScheduler1.ResourceNavigator.EnableFirstLast = true;
      ASPxScheduler1.ResourceNavigator.EnableIncreaseDecrease = true;
      ASPxScheduler1.ResourceNavigator.EnablePrevNext = true;
      ASPxScheduler1.ResourceNavigator.EnablePrevNextPage = true;

      ASPxScheduler1.Views.DayView.VisibleTime = new TimeOfDayInterval(new TimeSpan(8, 0, 0), new TimeSpan(22, 0, 0));

      ASPxScheduler1.OptionsCustomization.AllowAppointmentDrag = UsedAppointmentType.None;
      ASPxScheduler1.OptionsCustomization.AllowAppointmentDragBetweenResources = UsedAppointmentType.None;
      ASPxScheduler1.OptionsCustomization.AllowAppointmentResize = UsedAppointmentType.None;
      ASPxScheduler1.OptionsCustomization.AllowAppointmentMultiSelect = false;

      ASPxScheduler1.TimelineView.Enabled = false;
      ASPxScheduler1.WeekView.Enabled = false;
      ASPxScheduler1.WorkWeekView.Enabled = false;
      ASPxScheduler1.MonthView.Enabled = false;
      ASPxScheduler1.DayView.Enabled = true;


    }


    public void AppointmentCalenderTran(string Value, string module)
    {
      GridBind();
      //DataSet cmbDS2 = new DataSet();
      //string loc_ID = Session["Loc_ID"].ToString();
      //cmbDS2 = DAL.Utils.fetchDSQueryRecordsSP(Value, loc_ID, module, "GetAppointmentCalenderNew", MyConnection.ReadConStr("Local"));

      //if (cmbDS2.Tables[0].Rows.Count > 0)
      //{
      //  grdDetails.DataSource = cmbDS2.Tables[0];
      //  grdDetails.DataBind();
      //}
      //else
      //{
      //  grdDetails.DataSource = null;
      //  grdDetails.DataBind();
      //}

      Storage.Appointments.ResourceSharing = true;
      SetUpMapping();


      string sUserID = Convert.ToString(Session["UserID"]);
      string sUserGroup = Convert.ToString(Session["UserGroup"]);

      string sUserName = "";
      string sSqlUSername = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = ocon.GetTable(sSqlUSername, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      string selectSQL = "select ID,Subject as UserId,Subject as Label,Status as Status,StartTime as StartTime,  EndTime,[Organizer] as Subject,Description as Description,[Location] as Location,0 as wTime,0 as WMins,EventType, AllDay,RecurrenceInfo,ReminderInfo,EntryID FROM Scheduling   WHERE StartTime between  @StartTime AND dateadd(d,1,@EndTime) ";
      if (sUserGroup == "3")
      {
        selectSQL += " AND HostName='" + sUserName + "'";
      }
      selectSQL += "order by  StartTime";

     SqlDataAdapter SqlScheduler = new SqlDataAdapter();
      SqlConnection sConn = new SqlConnection(MyConnection.ReadConStr("Local"));

      SqlCommand selectCMD = new SqlCommand(selectSQL, sConn);
      SqlScheduler.SelectCommand = selectCMD;
      selectCMD.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = DateTime.Now.AddDays(-10);
      selectCMD.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = DateTime.Now.AddDays(10);
      DataSet sDS = new DataSet();
      SqlScheduler.Fill(sDS, "rScheduler");

      ASPxScheduler1.AppointmentDataSource = sDS.Tables[0];


      ASPxScheduler1.DataBind();
    }
    private void SetUpMapping()
    {

      Storage.BeginUpdate();
      try
      {

        ASPxResourceMappingInfo resMappings = Storage.Resources.Mappings;
        resMappings.Caption = "Model";
        resMappings.ResourceId = "ID";

        ASPxAppointmentMappingInfo aptMappings = Storage.Appointments.Mappings;
        aptMappings.AppointmentId = "ID";
        aptMappings.Status = "Status";
        aptMappings.Subject = "Subject";
        aptMappings.Description = "Description";
        aptMappings.Label = "Label";
        aptMappings.Start = "StartTime";
        aptMappings.End = "EndTime";
        aptMappings.Location = "Location";
        aptMappings.AllDay = "AllDay";
        aptMappings.Type = "EventType";
        aptMappings.RecurrenceInfo = "RecurrenceInfo";
        aptMappings.ReminderInfo = "ReminderInfo";
        aptMappings.ResourceId = "EntryID";

      }
      finally
      {
        Storage.EndUpdate();
      }
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      //if (txtSearch.Text != "")
      //{
      //  AppointmentCalenderTran(txtSearch.Text, "GridAppoinment");
      //}
      //else
      //{
      //  AppointmentCalenderTran("GridAppoinment", "GridAppoinment");
      //}
      GridBind();
    }
    private void GridBind()
    {
      string sUserID = Convert.ToString(Session["UserID"]);
      string sUserGroup = Convert.ToString(Session["UserGroup"]);

      string sUserName = "";
      string sSqlUSername = "SELECT isnull(UserName,'') as UserName FROM Users WHERE UserID='" + sUserID + "'";
      DataTable dtUserName = ocon.GetTable(sSqlUSername, new DataSet());
      if (dtUserName.Rows.Count > 0)
      {
        sUserName = dtUserName.Rows[0]["UserName"].ToString();
      }

      string sSql = "SELECT ID, isnull(HostName,'') as Organizer, [Subject] as VisitorName,[ContactInfo] as  Visitor_Email,[Location],[StartTime], [EndTime],left([Description],50) as Body,QRCode as QRCode,'Checkin' as Checkin "
                  + " FROM Scheduling where[StartTime] between getdate()-1  and getdate()+2 and isnull(Checkedin,0)=0  ";

      if (sUserGroup == "3")
      {
        sSql += " AND HostName='" + sUserName + "'";
      }

      if (txtSearch.Text != "")
      {
        sSql += " AND (QRCode ='" + txtSearch.Text.Trim() + "' or Subject like '%" + txtSearch.Text.Trim() + "%' or   ContactInfo like '%" + txtSearch.Text.Trim() + "%' or   HostName like '%" + txtSearch.Text.Trim() + "%' or   Location like '%" + txtSearch.Text.Trim() + "%' or   left([Description],50) like '%" + txtSearch.Text.Trim() + "%')";
      }

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
        if (sUserGroup == "3")
        {
          grdDetails.Columns[8].Visible = false;
        }
        else
        {
          grdDetails.Columns[8].Visible = true;
        }
      }
    }


  }

  public class ExportAppointmentCallbackCommand : SchedulerCallbackCommand
  {
    public ExportAppointmentCallbackCommand(ASPxScheduler control)
        : base(control)
    {
    }

    public override string Id { get { return "EXPORTAPT"; } }
    protected override void ParseParameters(string parameters)
    {
    }
    protected override void ExecuteCore()
    {
      PostCalendarFile(Control.SelectedAppointments);
    }
    void PostCalendarFile(AppointmentBaseCollection appointments)
    {
      iCalendarExporter exporter = new iCalendarExporter(Control.Storage, appointments);
      if (appointments.Count == 0)
        return;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        exporter.Export(memoryStream);
        Stream outputStream = Control.Page.Response.OutputStream;
        memoryStream.WriteTo(outputStream);
        outputStream.Flush();
      }
      Control.Page.Response.ContentType = "text/calendar";
      Control.Page.Response.AddHeader("Content-Disposition", "attachment; filename=appointment.ics");
      Control.Page.Response.End();
    }

  }
}
