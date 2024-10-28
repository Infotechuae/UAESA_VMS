using DAL;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class subVisitorRequest : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sLang = Convert.ToString(Session["Lang"]);
        string sUserID = Convert.ToString(Session["UserID"]);


        BindDropDown();



        txtReqName.Text = Session["UserFullName"].ToString();
        txtReqCompanyName.Text = Session["Company"].ToString();
        txtReqEmail.Text = Session["Email"].ToString();
        txtVisMobile.Text = Session["Phone"].ToString();

        txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtTime.Text = DateTime.Now.ToString("hh:mm tt");

        img_PhotoBase64.ImageUrl = "~/dist/img/NoImage-Avatar-PNG-Image.png";

        faUpload.Attributes["onchange"] = "UploadFile(this)";

        btnLoad.Attributes.Add("onclick", "document.getElementById('" + faUpload.ClientID + "').click();");

        Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
        rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        ci = Thread.CurrentThread.CurrentCulture;
        Loadstring(ci);
      }
    }
    private void Loadstring(CultureInfo ci)
    {
      header1.Text = rm.GetString("STR_51", ci);
      btnInvite.Text = rm.GetString("STR_52", ci);
      btnReset.Text = rm.GetString("STR_19", ci);
      lblFullName.Text = rm.GetString("STR_53", ci);
      lblCompanyName.Text = rm.GetString("STR_5", ci);
      lblEmail.Text = rm.GetString("STR_7", ci);
      header2.Text = rm.GetString("STR_54", ci);
      lblMeetName.Text = rm.GetString("STR_55", ci);
      lblLocation.Text = rm.GetString("STR_56", ci);
      lblDate.Text = rm.GetString("STR_57", ci);
      lblTime.Text = rm.GetString("STR_58", ci);
      lblDuration.Text = rm.GetString("STR_15", ci);
      lblRemarks.Text = rm.GetString("STR_16", ci);
    }

    protected void Upload(object sender, EventArgs e)
    {
      string sFilePathXML = @"~/images/VisitorImages/";

      FileInfo oFileInfo = new FileInfo(faUpload.FileName);
      if (oFileInfo.Extension.ToLower() == ".gif" || oFileInfo.Extension.ToLower() == ".png" || oFileInfo.Extension.ToLower() == ".jpg" || oFileInfo.Extension.ToLower() == ".jpeg")
      {
        string filename = Path.GetFileName(faUpload.PostedFile.FileName);
        if (System.IO.File.Exists(Server.MapPath(sFilePathXML + Path.GetFileName(faUpload.FileName))))
        {
          System.IO.File.Delete(Server.MapPath(sFilePathXML + Path.GetFileName(faUpload.FileName)));
        }

        faUpload.SaveAs(Server.MapPath(sFilePathXML + Path.GetFileName(faUpload.FileName)));

        string sUploadedImage = Server.MapPath("~/images/VisitorImages/" + filename);

        string base64Image = ConvertImageToBase64(sUploadedImage);

        hdnupload.Value = base64Image;

        img_PhotoBase64.ImageUrl = "data:image/png;base64," + base64Image;

        if (System.IO.File.Exists(Server.MapPath(sFilePathXML + Path.GetFileName(faUpload.FileName))))
        {
          System.IO.File.Delete(Server.MapPath(sFilePathXML + Path.GetFileName(faUpload.FileName)));
        }
      }
    }
    public static string ConvertImageToBase64(string imagePath)
    {

      byte[] imageBytes = File.ReadAllBytes(imagePath);


      string base64String = Convert.ToBase64String(imageBytes);

      return base64String;
    }
    public string GetNationMasterData(string nationCode)
    {
      string NationName = DAL.Utils.fetchString("pl_Value", "PickList_tran", "pl_head_id=2 and (pl_Data='" + nationCode + "' or pl_value='" + nationCode + "')", MyConnection.ReadConStr("Local"));

      if (NationName.Length == 0)
        NationName = nationCode;
      return NationName;
    }

    public byte[] imgToByteArray(System.Drawing.Image img)
    {
      using (MemoryStream mStream = new MemoryStream())
      {
        img.Save(mStream, img.RawFormat);
        return mStream.ToArray();
      }
    }



    public byte[] ImageControlToByteArray(System.Web.UI.WebControls.Image image)
    {
      try
      {
        // Attempt to find the Image that the control points to
        // and read all of it's bytes
        return File.ReadAllBytes(image.ImageUrl);
      }
      catch (Exception)
      {
        // Uh oh, there was a problem reading the file
        return new byte[0];
      }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
      Response.Redirect("VisitorInvite.aspx");
    }


    private void BindDropDown()
    {
      #region Host

      string sSqlHost = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=18 order by pl_Value";

      DataTable dtHost = ocon.GetTable(sSqlHost, new DataSet());
      if (dtHost.Rows.Count > 0)
      {
        drpHostName.DataSource = dtHost;
        drpHostName.DataTextField = "pl_value";
        drpHostName.DataValueField = "Pl_id";
        drpHostName.DataBind();
        drpHostName.Items.Insert(0, "Select");
      }
      else
      {
        drpHostName.DataSource = null;
        drpHostName.DataBind();
        drpHostName.Items.Insert(0, "Select");
      }
      #endregion

      #region Duration

      string sSqlDuration = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=6 order by pl_Value";

      DataTable dtDuration = ocon.GetTable(sSqlDuration, new DataSet());
      if (dtDuration.Rows.Count > 0)
      {
        drpDuration.DataSource = dtDuration;
        drpDuration.DataTextField = "pl_value";
        drpDuration.DataValueField = "Pl_id";
        drpDuration.DataBind();
        drpDuration.Items.Insert(0, "Select");
      }
      else
      {
        drpDuration.DataSource = null;
        drpDuration.DataBind();
        drpDuration.Items.Insert(0, "Select");
      }
      #endregion
    }


    protected void btnInvite_Click(object sender, EventArgs e)
    {
      if (txtMeetname.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter meeting name');", true);
        return;
      }
      if (txtLocation.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter location');", true);
        return;
      }
      if (txtDate.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter meeting date');", true);
        return;
      }
      if (txtTime.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter meeting time');", true);
        return;
      }
      if (drpDuration.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please select meeting duration');", true);
        return;
      }
      if (drpHostName.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please select Host Name');", true);
        return;
      }
      if (txtHostEmail.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter Host Email');", true);
        return;
      }



      try
      {
        string MeetingID = "0";

        string sQRCode = "";

        DateTime dt = DateTime.Parse(txtTime.Text.ToString());
        string mstartTime = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + " " + dt.ToString("HH:mm");

        SqlCommand cmdOrl = new SqlCommand("Visitor_Meeting_Invite");
        cmdOrl.CommandType = CommandType.StoredProcedure;

        cmdOrl.Parameters.Add("IDValue", SqlDbType.NVarChar);
        cmdOrl.Parameters["IDValue"].Value = sQRCode;
        cmdOrl.Parameters.Add("Module", SqlDbType.NVarChar);
        cmdOrl.Parameters["Module"].Value = "Self";
        cmdOrl.Parameters.Add("WebUser", SqlDbType.NVarChar);
        cmdOrl.Parameters["WebUser"].Value = Session["UserID"].ToString();
        cmdOrl.Parameters.Add("Loc_ID", SqlDbType.NVarChar);
        cmdOrl.Parameters["Loc_ID"].Value = Session["Loc_ID"].ToString();
        cmdOrl.Parameters.Add("MeetingDate", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingDate"].Value = mstartTime;
        cmdOrl.Parameters.Add("MeetingTime", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingTime"].Value = txtTime.Text.ToString();

        cmdOrl.Parameters.Add("Duration", SqlDbType.NVarChar);
        cmdOrl.Parameters["Duration"].Value = drpDuration.SelectedValue;
        cmdOrl.Parameters.Add("MeetingName", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingName"].Value = txtMeetname.Text;

        cmdOrl.Parameters.Add("MeetingAgenda", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingAgenda"].Value = txtRemarks.Text;

        cmdOrl.Parameters.Add("MeetingLocation", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingLocation"].Value = txtLocation.Text;




        DAL.DALSQL dbs1 = new DAL.DALSQL();
        string PKval = dbs1.ExecuteStoredProcedurereturn(cmdOrl, MyConnection.ReadConStr("Local"));


        MeetingID = PKval;

        string sVisitorQR = "";
        var Visitorlist = new List<string>() { };
        var VisitorName = new List<string>() { };



        sVisitorQR = "";

        Visitorlist.Add(txtReqEmail.Text);
        VisitorName.Add(txtReqName.Text);




        QRCodeGenerator qrGenerator1 = new QRCodeGenerator();
        byte[] byteImageVis = null;
        byte[] byteImageVis1 = null;
        QRCodeGenerator.QRCode qrCodeVis = qrGenerator1.CreateQrCode(sVisitorQR, QRCodeGenerator.ECCLevel.Q);
        System.Web.UI.WebControls.Image imgBarCodeVis = new System.Web.UI.WebControls.Image();
        imgBarCodeVis.Height = 100;
        imgBarCodeVis.Width = 150; //150
        using (Bitmap bitMap = qrCodeVis.GetGraphic(20))
        {
          using (MemoryStream ms = new MemoryStream())
          {
            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byteImageVis = ms.ToArray();
            byteImageVis1 = (byte[])ms.ToArray();
            imgBarCodeVis.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImageVis);
          }


          //ASPxImage1.Controls.Add(imgBarCode);
        }



        SqlCommand cmdVisitor = new SqlCommand("Visitor_Visitor_Request_PREREGISTRATION");
        cmdVisitor.CommandType = CommandType.StoredProcedure;

        cmdVisitor.Parameters.Add("IDValue", SqlDbType.NVarChar);
        cmdVisitor.Parameters["IDValue"].Value = MeetingID;
        cmdVisitor.Parameters.Add("Module", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Module"].Value = "Self";
        cmdVisitor.Parameters.Add("WebUser", SqlDbType.NVarChar);
        cmdVisitor.Parameters["WebUser"].Value = Session["UserID"].ToString();
        cmdVisitor.Parameters.Add("Loc_ID", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Loc_ID"].Value = Session["Loc_ID"].ToString();

        cmdVisitor.Parameters.Add("MeetingDate", SqlDbType.NVarChar);
        cmdVisitor.Parameters["MeetingDate"].Value = mstartTime;
        cmdVisitor.Parameters.Add("MeetingTime", SqlDbType.NVarChar);
        cmdVisitor.Parameters["MeetingTime"].Value = txtTime.Text.ToString();

        cmdVisitor.Parameters.Add("Duration", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Duration"].Value = drpDuration.SelectedIndex;
        cmdVisitor.Parameters.Add("VisPhone", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisPhone"].Value = txtVisMobile.Text;




        cmdVisitor.Parameters.Add("VisEmail", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisEmail"].Value = txtReqEmail.Text;

        cmdVisitor.Parameters.Add("Phone", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Phone"].Value = txtVisMobile.Text;
        cmdVisitor.Parameters.Add("Email", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Email"].Value = txtHostEmail.Text;
        cmdVisitor.Parameters.Add("VisCompany", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisCompany"].Value = txtReqCompanyName.Text;

        cmdVisitor.Parameters.Add("VisName", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisName"].Value = txtReqName.Text;
        cmdVisitor.Parameters.Add("VisMessage", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisMessage"].Value = txtMeetname.Text;

        cmdVisitor.Parameters.Add("VisID", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisID"].Value = "";
        cmdVisitor.Parameters.Add("Location", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Location"].Value = "0";

        cmdVisitor.Parameters.Add("MeetingLocation", SqlDbType.NVarChar);
        cmdVisitor.Parameters["MeetingLocation"].Value = txtLocation.Text;

        cmdVisitor.Parameters.Add("QRID", SqlDbType.NVarChar);
        cmdVisitor.Parameters["QRID"].Value = sVisitorQR;
        cmdVisitor.Parameters.Add("Veh_Code", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Veh_Code"].Value = "";
        cmdVisitor.Parameters.Add("Veh_No", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Veh_No"].Value = "";
        cmdVisitor.Parameters.Add("Parking", SqlDbType.NVarChar);
        cmdVisitor.Parameters["Parking"].Value = "";
        cmdVisitor.Parameters.Add("HostName", SqlDbType.NVarChar);
        cmdVisitor.Parameters["HostName"].Value = drpHostName.SelectedItem.Text;
        cmdVisitor.Parameters.Add("HostPhone", SqlDbType.NVarChar);
        cmdVisitor.Parameters["HostPhone"].Value = txtHostMob.Text;
        cmdVisitor.Parameters.Add("VisImage", SqlDbType.NVarChar);
        cmdVisitor.Parameters["VisImage"].Value = hdnupload.Value;

        Random rnd = new Random();
        int myRandomNo = rnd.Next(1000000, 9999999);
        string sPersonID = myRandomNo.ToString();

        string sStartTime = "2023-08-01";
        string sEndTime = "2023-08-31";

        string sInsertTBL_Temp_QR = "insert into[dbo].[TBL_Temp_QR]"
  + " (PersonName,PersonID,MeetingDate,VisImage) Values "
  + "('" + txtReqEmail.Text + "','" + sPersonID + "','" + sStartTime + "','" + hdnupload.Value + "') ";

        ocon.Execute(sInsertTBL_Temp_QR);

        DAL.DALSQL visobj = new DAL.DALSQL();
        PKval = visobj.ExecuteStoredProcedurereturn(cmdVisitor, MyConnection.ReadConStr("Local"));
        visobj = null;
        cmdVisitor.Dispose();
        cmdVisitor = null;

        string sVisID = PKval;

        string Hostname = txtReqName.Text;

        //SecuLobbyVMS.App_Code.Utils.sendEmployee_Visitormail(byteImageVis, sVisitorQR, "050", txtReqEmail.Text.ToString(), txtReqEmail.Text, txtReqName.Text, "", mstartTime, txtMeetname.Text.ToString(), txtLocation.Text, drpDuration.SelectedItem.Text, false, Hostname, "", txtReqCompanyName.Text.Trim());

        qrGenerator1 = null;



        var Hostlist = new List<string>() { };

        SqlCommand cmdHost = new SqlCommand("Visitor_Host_Invite");
        cmdHost.CommandType = CommandType.StoredProcedure;

        cmdHost.Parameters.Add("IDValue", SqlDbType.NVarChar);
        cmdHost.Parameters["IDValue"].Value = MeetingID;
        cmdHost.Parameters.Add("Module", SqlDbType.NVarChar);
        cmdHost.Parameters["Module"].Value = "Self";
        cmdHost.Parameters.Add("WebUser", SqlDbType.NVarChar);
        cmdHost.Parameters["WebUser"].Value = Session["UserID"].ToString();
        cmdHost.Parameters.Add("Loc_ID", SqlDbType.NVarChar);
        cmdHost.Parameters["Loc_ID"].Value = Session["Loc_ID"].ToString();

        cmdHost.Parameters.Add("MeetingID", SqlDbType.NVarChar);
        cmdHost.Parameters["MeetingID"].Value = MeetingID;

        cmdHost.Parameters.Add("HostEmail", SqlDbType.NVarChar);
        cmdHost.Parameters["HostEmail"].Value = txtHostEmail.Text;
        cmdHost.Parameters.Add("HostID", SqlDbType.NVarChar);
        cmdHost.Parameters["HostID"].Value = "0";

        cmdHost.Parameters.Add("Hostmessage", SqlDbType.NVarChar);
        cmdHost.Parameters["Hostmessage"].Value = drpHostName.SelectedItem.Text;

        Hostlist.Add(txtHostEmail.Text);

        DAL.DALSQL hostobj = new DAL.DALSQL();
        PKval = dbs1.ExecuteStoredProcedurereturn(cmdHost, MyConnection.ReadConStr("Local"));

        //if (System.Configuration.ConfigurationManager.AppSettings["InviteEmail"] == "True")
        //  SecuLobbyVMS.App_Code.Utils.sendVisitor_InviteCalendermail(txtReqEmail.Text, txtReqName.Text, mstartTime, txtRemarks.Text, txtLocation.Text, drpDuration.SelectedItem.Text, Visitorlist, Hostlist, txtMeetname.Text, txtLocation.Text, txtRemarks.Text, txtReqEmail.Text, txtReqName.Text, VisitorName);
        SecuLobbyVMS.App_Code.Utils.sendVisitor_InviteApprovemail(txtReqEmail.Text, txtReqName.Text, mstartTime, txtRemarks.Text, txtLocation.Text, drpDuration.SelectedItem.Text, Visitorlist, Hostlist, txtMeetname.Text, txtLocation.Text, txtRemarks.Text, txtReqEmail.Text, txtReqName.Text, VisitorName, sVisID);

   



     

        string sDestURL = string.Format("\"{0}\"", "VisitorInvite.aspx");
        string smessage = string.Format("\"{0}\"", "Invite sent Successfully");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
      catch (Exception ex)
      {

      }


    }

    protected void drpHostName_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (drpHostName.SelectedItem.Text != "Select")
      {
        DataTable dt = ocon.GetTable("SELECT isnull(pl_Data,'') as HostEmail,isnull(Other_Data1,'') as HostPhone FROM PickList_tran WHERE pl_head_id = 18 AND pl_id='" + drpHostName.SelectedValue + "' ", new DataSet());
        if (dt.Rows.Count > 0)
        {
          txtHostEmail.Text = dt.Rows[0]["HostEmail"].ToString();
          txtHostMob.Text = dt.Rows[0]["HostPhone"].ToString();
        }
        else
        {
          txtHostEmail.Text = "";
          txtHostMob.Text = "";
        }
      }
      else
      {
        txtHostEmail.Text = "";
        txtHostMob.Text = "";
      }
    }


    
  }
}
