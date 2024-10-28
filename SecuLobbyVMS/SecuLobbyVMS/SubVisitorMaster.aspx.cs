using DAL;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class SubVisitorMaster : System.Web.UI.Page
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


        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);

        VisitorMasterReport("");

        string stext = string.Format("\"{0}\"", txtSearch.Text);
       
        string sVar1 = stext;


        A1.Attributes.Add("onclick", "showReport(" + sVar1 + ")");
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_25", ci);
      btnPDF.Text = rm.GetString("STR_42", ci);
     
     
    }
    private void VisitorMasterReport(string sValue)
    {
      string sLang = Convert.ToString(Session["Lang"]);

      Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
      rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
      ci = Thread.CurrentThread.CurrentCulture;



      string sSql = "select Visitor_ID,Name,EmiratesID,Company,Nationality,Gender,Mobile,Email,Visitor_Image.Image as Picture "
                  + " from SecuLobby_VisitorInfo "
                  + " LEFT OUTER JOIN Visitor_Image ON dbo.SecuLobby_VisitorInfo.Visitor_ID = Visitor_Image.ID and[Type] = 1 ";

      if (sValue != "")
      {
        sSql += " WHERE ( Name LIKE '%" + sValue + "%'  OR EmiratesID LIKE '%" + sValue + "%' OR Company LIKE '%" + sValue + "%' OR Nationality LIKE '%" + sValue + "%' OR Gender LIKE '%" + sValue + "%' OR Mobile LIKE '%" + sValue + "%' OR Email LIKE '%" + sValue + "%') ";
      }

      sSql += " ORDER BY Name";

      DataTable dt = ocon.GetTable(sSql, new DataSet());

      if (dt.Rows.Count > 0)
      {
        StringBuilder sb = new StringBuilder();

        sb.Append("<table class='table table-bordered table-hover'>");

        sb.Append("<thead>");
        sb.Append("<tr>");
        sb.Append("<th>" + rm.GetString("STR_44", ci)+"</th>");
        sb.Append("<th>" + rm.GetString("STR_39", ci) + "</th>");
        sb.Append("<th>" + rm.GetString("STR_40", ci) + "</th>");
        sb.Append("<th>" + rm.GetString("STR_5", ci) + "</th>");
        sb.Append("<th>" + rm.GetString("STR_6", ci) + "</th>");
        sb.Append("<th>" + rm.GetString("STR_7", ci) + "</th>");
        sb.Append("</tr>");
        sb.Append("</thead>");

        sb.Append("<tbody>");

        foreach (DataRow dr in dt.Rows)
        {
          sb.Append("<tr data-widget='expandable-table' aria-expanded='false'>");

          if (dr["Picture"].ToString().Length == 0)
          {
            sb.Append("<td><img src='dist/img/NoImage-Avatar-PNG-Image.png' height='85' width='80' /></td>");
          }
          else
          {
            string base64String = Convert.ToBase64String((byte[])dr["Picture"]);
            string imageUrl = "data:image/png;base64," + base64String;
            sb.Append("<td><img src='" + imageUrl + "' height='85' width='80'  /></td>");


          }

          sb.Append("<td>" + dr["Name"].ToString() + "</td>");
          sb.Append("<td>" + dr["EmiratesID"].ToString() + "</td>");
          sb.Append("<td>" + dr["Company"].ToString() + "</td>");
          sb.Append("<td>" + dr["Mobile"].ToString() + "</td>");
          sb.Append("<td>" + dr["Email"].ToString() + "</td>");

          sb.Append("</tr>");

          string sSql1 = "SELECT LocationID, Aptment_Dept as Department,Area_Floor,Visitor_Type,SecuLobby_VisitingDetails.Host_to_Visit as HostName,access_card_no,Checkin_Time,CheckOut_Time, TblDuration.pl_Value AS Duration,Purpose "
                        + " FROM SecuLobby_VisitingDetails "
                        + " INNER JOIN dbo.PickList_tran as TblDuration ON dbo.SecuLobby_VisitingDetails.Duration = TblDuration.pl_id and TblDuration.pl_head_id = 6 "
                        + " WHERE Visitor_ID =" + dr["Visitor_ID"].ToString() + " "
                        + " ORDER BY Checkin_Time DESC";

          DataTable dt1 = ocon.GetTable(sSql1, new DataSet());

          if (dt1.Rows.Count > 0)
          {
            sb.Append("<tr class='expandable-body'>");
            sb.Append("<td colspan ='8'>");

            sb.Append("<table style='width:100%;'>");


            sb.Append("<tr>");
            sb.Append("<td>#</td>");

            sb.Append("<th>" + rm.GetString("STR_11", ci) + "</th>");
            sb.Append("<th>" + rm.GetString("STR_14", ci) + "</th>");
            sb.Append("<th>" + rm.GetString("STR_13", ci) + "</th>");
            sb.Append("<th>" + rm.GetString("STR_61", ci) + "</th>");
            sb.Append("<th>" + rm.GetString("STR_18", ci) + "</th>");
            sb.Append("<th>" + rm.GetString("STR_21", ci) + "</th>");
            sb.Append("<th>" + rm.GetString("STR_15", ci) + "</th>");
            

            sb.Append("</tr>");

            int i = 1;

            foreach (DataRow dr1 in dt1.Rows)
            {
              sb.Append("<tr>");

              sb.Append("<td>" + i + "</td>");
             
              sb.Append("<td>" + dr1["Department"].ToString() + "</td>");
              sb.Append("<td>" + dr1["Visitor_Type"].ToString() + "</td>");
              sb.Append("<td>" + dr1["HostName"].ToString() + "</td>");
              sb.Append("<td>" + dr1["Purpose"].ToString() + "</td>");
              sb.Append("<td>" + dr1["Checkin_Time"].ToString() + "</td>");
              sb.Append("<td>" + dr1["CheckOut_Time"].ToString() + "</td>");
              sb.Append("<td>" + dr1["Duration"].ToString() + "</td>");

              sb.Append("</tr>");

              i++;
            }
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

          }
        }
        sb.Append("</tbody>");

        sb.Append("</table>");
        ltrtable.Text = sb.ToString();
      }
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (txtSearch.Text != "")
        VisitorMasterReport(txtSearch.Text.Trim());
      else
        VisitorMasterReport("");

      string stext = string.Format("\"{0}\"", txtSearch.Text);
     
      string sVar1 = stext;
     

      A1.Attributes.Add("onclick", "showReport(" + sVar1 + ")");
     
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
      //required to avoid the runtime error "  
      //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    private void ExportGridToExcel()
    {
      Response.Clear();
      Response.Buffer = true;
      Response.ClearContent();
      Response.ClearHeaders();
      Response.Charset = "";
      string FileName = "Visitor_Master_" + DateTime.Now + ".xls";
      StringWriter strwritter = new StringWriter();
      HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.ContentType = "application/vnd.ms-excel";
      Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
      //ltrtable.GridLines = GridLines.Both;
      //ltrtable.HeaderStyle.Font.Bold = true;
      pnlgrid.RenderControl(htmltextwrtter);
      Response.Write(strwritter.ToString());
      Response.End();

    }

    protected void BtnExcel_Click(object sender, EventArgs e)
    {

    }
  }
}
