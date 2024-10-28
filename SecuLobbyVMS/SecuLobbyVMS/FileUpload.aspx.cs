using DAL;
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
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class FileUpload : System.Web.UI.Page
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
        
        int iTabId = Convert.ToInt32(Request.QueryString["ID"]);
      
      
        string Sql = "SELECT *  FROM [tblFiles]";

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

      }
    }


    protected void btnsave_Click(object sender, EventArgs e)
    {
      if (FileUpload1.PostedFile.FileName == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please select File');", true);
        return;
      }
      string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
        string contentType = FileUpload1.PostedFile.ContentType;
        using (Stream fs = FileUpload1.PostedFile.InputStream)
        {
          using (BinaryReader br = new BinaryReader(fs))
          {
            byte[] bytes = br.ReadBytes((Int32)fs.Length);
           using (SqlConnection con = new SqlConnection(constr))
            {
              string query = "insert into tblFiles values (@Name, @ContentType, @Data ,@Date_Time)";
              using (SqlCommand cmd = new SqlCommand(query))
              {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Name", filename);
                cmd.Parameters.AddWithValue("@ContentType", contentType);
                cmd.Parameters.AddWithValue("@Data", bytes);
                cmd.Parameters.AddWithValue("@Date_Time", DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
              }
            }
          }
        }
        Response.Redirect(Request.Url.AbsoluteUri);
      }

    protected void DownloadFile(object sender, EventArgs e)
    {
      int id = int.Parse((sender as LinkButton).CommandArgument);
      byte[] bytes;
      string fileName, contentType;
      using (SqlConnection con = new SqlConnection(constr))
      {
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandText = "select Name, Data, ContentType from tblFiles where Id=@Id";
          cmd.Parameters.AddWithValue("@Id", id);
          cmd.Connection = con;
          con.Open();
          using (SqlDataReader sdr = cmd.ExecuteReader())
          {
            sdr.Read();
            bytes = (byte[])sdr["Data"];
            contentType = sdr["ContentType"].ToString();
            fileName = sdr["Name"].ToString();
          }
          con.Close();
        }
      }
      Response.Clear();
      Response.Buffer = true;
      Response.Charset = "";
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.ContentType = contentType;
      Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
      Response.BinaryWrite(bytes);
      Response.Flush();
      Response.End();
    }

    protected void grdDetails_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
      System.Web.UI.WebControls.Label lblid = grdDetails.Rows[e.RowIndex].Controls[0].FindControl("lblid") as System.Web.UI.WebControls.Label;

      string sSqlDelete = "DELETE tblFiles WHERE id='" + lblid.Text + "'";
      ocon.Execute(sSqlDelete);


      string sDestURL = string.Format("\"{0}\"", "FileUpload.aspx");
      string smessage = string.Format("\"{0}\"", "File deleted successfully");

      string sVar = sDestURL + "," + smessage;

      ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);


      //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Approval deleted successfully');", true);

      grdDetails.EditIndex = -1;

      // gridbind();
    }


  }

}
