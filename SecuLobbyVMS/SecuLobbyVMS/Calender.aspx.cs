using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class Calender : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string GetCalenderScheduleReport()
    {


      DBConnection oconn = new DBConnection(MyConnection.ReadConStr("Local"));

      List<CalenderSchedule> RCFA = new List<CalenderSchedule>();

      string sSql = "SELECT Description,StartTime FROM Scheduling  ";
              
      DataTable dt = oconn.GetTable(sSql, new DataSet());


      if (dt.Rows.Count > 0)
      {
        foreach (DataRow dr in dt.Rows)
        {
          CalenderSchedule objValues = new CalenderSchedule();
          objValues.MeetingName = dr["Description"].ToString();

          DateTime dtMeetingTime = Convert.ToDateTime(dr["StartTime"]);

          int Year = dtMeetingTime.Year;
          int month = dtMeetingTime.Month;
          int day = dtMeetingTime.Day;
          int hour = dtMeetingTime.Hour;
          int min = dtMeetingTime.Minute;

          objValues.MeetingTime = Year.ToString() + "," + month.ToString() + "," + day.ToString() + "," + hour.ToString() + "," + min.ToString();

          RCFA.Add(objValues);
        }
      }

      JavaScriptSerializer js = new JavaScriptSerializer();

      return js.Serialize(RCFA);
    }

    class CalenderSchedule
    {
      public string MeetingTime;
      public string MeetingName;
    }

  }
}
