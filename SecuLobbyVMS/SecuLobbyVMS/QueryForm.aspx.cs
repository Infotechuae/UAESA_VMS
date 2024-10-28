using DAL;
using DevExpress.CodeParser;
using SecuLobbyVMS.App_Code;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class QueryForm : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    ResourceManager rm;
    CultureInfo ci;
    public static string stcLocID = "";
    string sSyncOnline = System.Configuration.ConfigurationManager.AppSettings["Sync_Online"];
    string sDateFmt = System.Configuration.ConfigurationManager.AppSettings["DateFMT"];
    string SMS_Timeout_Msg = System.Configuration.ConfigurationManager.AppSettings["SMS_Checkin_Msg"];
    string SMS_URL = System.Configuration.ConfigurationManager.AppSettings["SMS_URL"];
    bool EnableSMS = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SMS"]);
    string SMS_USER = System.Configuration.ConfigurationManager.AppSettings["SMS_USER"];
    string SMS_PWD = System.Configuration.ConfigurationManager.AppSettings["SMS_PWD"];
    string SMS_CID = System.Configuration.ConfigurationManager.AppSettings["SMS_CID"];
    string FacCode = System.Configuration.ConfigurationManager.AppSettings["FacilityCode"];
    int CardLength = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["CardLength"]);
    string constr = ConfigurationManager.ConnectionStrings["SecuLobbyConnectionStringLocal"].ConnectionString;

    protected void Page_Init(object sender, EventArgs e)
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
      Response.Cache.SetNoStore();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
       
      }
    }



    protected void btnsave_Click(object sender, EventArgs e)
    {

      if (multitxt.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Query');", true);
        return;
      }

      if (multitxt.Text.Contains("select"))
      {

        SqlDataAdapter da;
        DataSet ds = new DataSet();
        StringBuilder htmlTable = new StringBuilder();
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["SecuLobbyConnectionStringLocal"].ConnectionString;
        SqlCommand cmd = new SqlCommand(multitxt.Text, con);
        da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        htmlTable.Append("<table border='1'>");


        // htmlTable.Append("<tr style='background-color:green; color: White;'><th>Customer ID.</th><th>Name</th><th>Address</th><th>Contact No</th></tr>");
        htmlTable.Append("<tr>");
        for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
        {
       
          htmlTable.Append("<th>" + ds.Tables[0].Columns[k] + "</th>");
        
        }
        htmlTable.Append("</tr>");
        if (!object.Equals(ds.Tables[0], null))
        {
          if (ds.Tables[0].Rows.Count > 0)
          {
           

              for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
              {
              htmlTable.Append("<tr>");
              for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
              {
                htmlTable.Append("<td>" + ds.Tables[0].Rows[i][ds.Tables[0].Columns[j]] + "</td>");
              }
              htmlTable.Append("</tr>");
            }
          

          }

        }

        htmlTable.Append("</table>");
          DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
        

      }


      else
      {
        string Query = multitxt.Text.Trim();
        ocon.Execute(Query);
        string sDestURL = string.Format("\"{0}\"", "QueryForm.aspx");
        string smessage = string.Format("\"{0}\"", "Query Run Sucessfully");
        string sVar = sDestURL + "," + smessage;
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }


    }








    //protected void btnsave_Click(object sender, EventArgs e)
    //{

    //  if (multitxt.Text == "")
    //  {
    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Query');", true);
    //    return;
    //  }

    //  if (multitxt.Text.Contains("select"))
    //  {

    //    SqlDataAdapter da;
    //    DataSet ds = new DataSet();
    //    StringBuilder htmlTable = new StringBuilder();
    //    SqlConnection con = new SqlConnection();
    //    con.ConnectionString = ConfigurationManager.ConnectionStrings["SecuLobbyConnectionStringLocal"].ConnectionString;
    //    SqlCommand cmd = new SqlCommand(multitxt.Text, con);
    //    da = new SqlDataAdapter(cmd);
    //    da.Fill(ds);
    //    con.Open();
    //    cmd.ExecuteNonQuery();
    //    con.Close();

    //    htmlTable.Append("<table border='1'>");
    //    int j = 0;

    //    // htmlTable.Append("<tr style='background-color:green; color: White;'><th>Customer ID.</th><th>Name</th><th>Address</th><th>Contact No</th></tr>");


    //      if (!object.Equals(ds.Tables[0], null))
    //      {

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //          for (j = 0; j < ds.Tables[0].Columns.Count; j++)
    //          {
    //            htmlTable.Append("<tr><th>" + ds.Tables[0].Columns[j] + "</th></tr>");


    //          for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //          {
    //            htmlTable.Append("<tr><td>" + ds.Tables[0].Rows[i][ds.Tables[0].Columns[j]] + "</td>");

    //            htmlTable.Append("</tr>");
    //          }
    //          }

    //        }
    //        htmlTable.Append("</table>");
    //        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    //      }


    //      //else
    //      //{
    //      //  htmlTable.Append("<tr>");
    //      //  htmlTable.Append("<td align='center' colspan='4'>There is no Record.</td>");
    //      //  htmlTable.Append("</tr>");
    //      //}

    //  }


    //  else
    //  {
    //    string Query = multitxt.Text.Trim();
    //    ocon.Execute(Query);
    //    string sDestURL = string.Format("\"{0}\"", "QueryForm.aspx");
    //    string smessage = string.Format("\"{0}\"", "Query Run Sucessfully");
    //    string sVar = sDestURL + "," + smessage;
    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
    //  }


    //}


  }

}
