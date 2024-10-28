using DAL;
using QRCoder;
using SecuLobbyVMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class subVisitorInvite : System.Web.UI.Page
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

        BlankGrid();

        txtReqName.Text = Session["UserFullName"].ToString();
        txtReqCompanyName.Text = Session["Company"].ToString();
        txtReqEmail.Text = Session["Email"].ToString();


        txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtTime.Text = DateTime.Now.ToString("hh:mm tt");



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
      Label1.Text = rm.GetString("STR_1", ci);
      Label2.Text = rm.GetString("STR_12", ci);

    }
    protected string GetHeader(string str)
    {
      string sHeaderName = "";

      string sLang = Convert.ToString(Session["Lang"]);

      Thread.CurrentThread.CurrentCulture = new CultureInfo(sLang);
      rm = new ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"));
      ci = Thread.CurrentThread.CurrentCulture;

      if (str == "Visitor Name")
        sHeaderName = rm.GetString("STR_39", ci);

      if (str == "ID Number")
        sHeaderName = rm.GetString("STR_3", ci);
      if (str == "Email")
        sHeaderName = rm.GetString("STR_7", ci);
      if (str == "Mobile Number")
        sHeaderName = rm.GetString("STR_6", ci);
      if (str == "Company")
        sHeaderName = rm.GetString("STR_5", ci);

      return sHeaderName;
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

      #region Host

      string sSqlHost = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=18 order by pl_Value";

      DataTable dtHost = ocon.GetTable(sSqlHost, new DataSet());
      if (dtHost.Rows.Count > 0)
      {
        drpHost.DataSource = dtHost;
        drpHost.DataTextField = "pl_value";
        drpHost.DataValueField = "Pl_id";
        drpHost.DataBind();
        drpHost.Items.Insert(0, "Select");
      }
      else
      {
        drpHost.DataSource = null;
        drpHost.DataBind();
        drpHost.Items.Insert(0, "Select");
      }
      #endregion

      #region Location


      DataTable dtcompany = ocon.GetTable("SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=23 order by pl_Value", new DataSet());
      if (dtcompany.Rows.Count > 0)
      {
        drpLocation.DataSource = dtcompany;
        drpLocation.DataTextField = "pl_value";
        drpLocation.DataValueField = "pl_id";
        drpLocation.DataBind();
        drpLocation.Items.Insert(0, "Select");
      }
      else
      {
        drpLocation.DataSource = null;
        drpLocation.DataBind();
        drpLocation.Items.Insert(0, "Select");
      }
      #endregion

      #region Floor

      string sSqlFloor = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=9 order by pl_Value";

      DataTable dtFloor = ocon.GetTable(sSqlFloor, new DataSet());
      if (dtFloor.Rows.Count > 0)
      {
        drpFloor.DataSource = dtFloor;
        drpFloor.DataTextField = "pl_value";
        drpFloor.DataValueField = "pl_value";
        drpFloor.DataBind();
        drpFloor.Items.Insert(0, "Select");
      }
      else
      {
        drpFloor.DataSource = null;
        drpFloor.DataBind();
        drpFloor.Items.Insert(0, "Select");
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
      if (drpDepartment.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please select department');", true);
        return;
      }
      if (drpLocation.SelectedItem.Text == "Select")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please select location');", true);
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


      if (grdVisDetails.Rows.Count == 0)
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter visitor details');", true);
        return;
      }

      try
      {
        string MeetingID = "0";
        string sHostEmail = "";
        string Hostname = "";
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
        //added location by swati in sept24
        cmdOrl.Parameters.Add("Loc_ID", SqlDbType.NVarChar);
        cmdOrl.Parameters["Loc_ID"].Value = drpLocation.SelectedValue;
        cmdOrl.Parameters.Add("MeetingDate", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingDate"].Value = mstartTime;
        cmdOrl.Parameters.Add("MeetingTime", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingTime"].Value = txtTime.Text.ToString();

        cmdOrl.Parameters.Add("Duration", SqlDbType.NVarChar);
        cmdOrl.Parameters["Duration"].Value = drpDuration.SelectedValue;
        cmdOrl.Parameters.Add("MeetingName", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingName"].Value = txtMeetname.Text;

        cmdOrl.Parameters.Add("MeetingAgenda", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingAgenda"].Value = DBNull.Value;

        cmdOrl.Parameters.Add("MeetingLocation", SqlDbType.NVarChar);
        cmdOrl.Parameters["MeetingLocation"].Value = drpDepartment.SelectedItem.Text;




        DAL.DALSQL dbs1 = new DAL.DALSQL();
        string PKval = dbs1.ExecuteStoredProcedurereturn(cmdOrl, MyConnection.ReadConStr("Local"));


        MeetingID = PKval;

        string sVisitorQR = "";
        var Visitorlist = new List<string>() { };
        var VisitorName = new List<string>() { };

        foreach (GridViewRow row in grdVisDetails.Rows)
        {
          TextBox txtVisName = (TextBox)row.FindControl("txtVisName");
          DropDownList drpIDType = (DropDownList)row.FindControl("drpIDType");
          TextBox txtIDNumber = (TextBox)row.FindControl("txtIDNumber");
          TextBox VisEmail = (TextBox)row.FindControl("txtVisEmail");
          TextBox VisMobile = (TextBox)row.FindControl("txtVisMobile");
          TextBox Company = (TextBox)row.FindControl("txtVisCompany");

          Random rnd = new Random();
          int myRandomNo = rnd.Next(1000000, 9999999);
          string sPersonID = myRandomNo.ToString();

          sVisitorQR = sPersonID;

          Visitorlist.Add(VisEmail.Text);
          VisitorName.Add(txtVisName.Text);




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



          SqlCommand cmdVisitor = new SqlCommand("Visitor_Visitor_Request");
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
          cmdVisitor.Parameters["Duration"].Value = drpDuration.SelectedValue;
          cmdVisitor.Parameters.Add("VisPhone", SqlDbType.NVarChar);
          cmdVisitor.Parameters["VisPhone"].Value = VisMobile.Text;




          cmdVisitor.Parameters.Add("VisEmail", SqlDbType.NVarChar);
          cmdVisitor.Parameters["VisEmail"].Value = VisEmail.Text;

          cmdVisitor.Parameters.Add("Phone", SqlDbType.NVarChar);
          cmdVisitor.Parameters["Phone"].Value = VisMobile.Text;
          cmdVisitor.Parameters.Add("Email", SqlDbType.NVarChar);
          cmdVisitor.Parameters["Email"].Value = VisEmail.Text;
          cmdVisitor.Parameters.Add("VisCompany", SqlDbType.NVarChar);
          cmdVisitor.Parameters["VisCompany"].Value = Company.Text;

          cmdVisitor.Parameters.Add("VisName", SqlDbType.NVarChar);
          cmdVisitor.Parameters["VisName"].Value = txtVisName.Text;
          cmdVisitor.Parameters.Add("VisMessage", SqlDbType.NVarChar);
          cmdVisitor.Parameters["VisMessage"].Value = txtMeetname.Text;

          cmdVisitor.Parameters.Add("VisID", SqlDbType.NVarChar);
          cmdVisitor.Parameters["VisID"].Value = "";
          cmdVisitor.Parameters.Add("Location", SqlDbType.NVarChar);
          cmdVisitor.Parameters["Location"].Value = "0";

          cmdVisitor.Parameters.Add("MeetingLocation", SqlDbType.NVarChar);
          cmdVisitor.Parameters["MeetingLocation"].Value = drpDepartment.SelectedItem.Text;

          cmdVisitor.Parameters.Add("QRID", SqlDbType.NVarChar);
          cmdVisitor.Parameters["QRID"].Value = sVisitorQR;
          cmdVisitor.Parameters.Add("Veh_Code", SqlDbType.NVarChar);
          cmdVisitor.Parameters["Veh_Code"].Value = "";
          cmdVisitor.Parameters.Add("Veh_No", SqlDbType.NVarChar);
          cmdVisitor.Parameters["Veh_No"].Value = "";
          cmdVisitor.Parameters.Add("Parking", SqlDbType.NVarChar);
          cmdVisitor.Parameters["Parking"].Value = "";
          cmdVisitor.Parameters.Add("HostName", SqlDbType.NVarChar);
          cmdVisitor.Parameters["HostName"].Value = drpHost.SelectedItem.Text; 
          cmdVisitor.Parameters.Add("IDType", SqlDbType.NVarChar);
          cmdVisitor.Parameters["IDType"].Value = drpIDType.SelectedItem.Text;
          cmdVisitor.Parameters.Add("IDNumber", SqlDbType.NVarChar);
          cmdVisitor.Parameters["IDNumber"].Value = txtIDNumber.Text;
          cmdVisitor.Parameters.Add("FloorName", SqlDbType.NVarChar);
          cmdVisitor.Parameters["FloorName"].Value = drpFloor.SelectedItem.Text;
          //cmdVisitor.Parameters.Add("QRCodeImage", SqlDbType.Image);
          //cmdVisitor.Parameters["QRCodeImage"].Value = (object)byteImageVis1;

          DAL.DALSQL visobj = new DAL.DALSQL();
          PKval = visobj.ExecuteStoredProcedurereturn(cmdVisitor, MyConnection.ReadConStr("Local"));
          visobj = null;
          cmdVisitor.Dispose();
          cmdVisitor = null;

             // added host by swati in sept24 //
          
          Hostname = drpHost.SelectedItem.Text;
          string sSqlHostEmail = "select isnull(pl_data,'') as pl_data,isnull(Other_Data2,'') as Other_Data2 from PickList_tran where pl_head_id=18 and pl_value='" + Hostname + "'";
          DataTable dtHostEmail = ocon.GetTable(sSqlHostEmail, new DataSet());

          if (dtHostEmail.Rows.Count > 0)
          {
            sHostEmail = Convert.ToString(dtHostEmail.Rows[0]["pl_data"]);
          
          }


         // string Hostname = txtReqName.Text;


          SecuLobbyVMS.App_Code.Utils.sendEmployee_Visitormail(byteImageVis, sVisitorQR, "050", sHostEmail, VisEmail.Text, txtVisName.Text, "", mstartTime, txtMeetname.Text.ToString(), drpDepartment.SelectedItem.Text, drpDuration.SelectedItem.Text, false, Hostname, "", txtReqCompanyName.Text.Trim());

          qrGenerator1 = null;


        }
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
        cmdHost.Parameters["HostEmail"].Value = sHostEmail;
        cmdHost.Parameters.Add("HostID", SqlDbType.NVarChar);
        cmdHost.Parameters["HostID"].Value = "0";

        cmdHost.Parameters.Add("Hostmessage", SqlDbType.NVarChar);
        cmdHost.Parameters["Hostmessage"].Value = Hostname;

        Hostlist.Add(sHostEmail);

        DAL.DALSQL hostobj = new DAL.DALSQL();
        PKval = dbs1.ExecuteStoredProcedurereturn(cmdHost, MyConnection.ReadConStr("Local"));

        //if (System.Configuration.ConfigurationManager.AppSettings["InviteEmail"] == "True")
        SecuLobbyVMS.App_Code.Utils.sendVisitor_InviteCalendermail(sHostEmail, Hostname, mstartTime, "", drpDepartment.SelectedItem.Text, drpDuration.SelectedItem.Text, Visitorlist, Hostlist, txtMeetname.Text, drpDepartment.SelectedItem.Text, txtMeetname.Text, sHostEmail, Hostname, VisitorName);


        string sDestURL = string.Format("\"{0}\"", "VisitorInvite.aspx");
        string smessage = string.Format("\"{0}\"", "Invite sent Successfully");

        string sVar = sDestURL + "," + smessage;


        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      }
      catch (Exception ex)
      {

      }


    }

    protected void btndd_Click(object sender, ImageClickEventArgs e)
    {
      //if (ViewState["CurrentTable"] != null)
      //{

      DataTable dtCurrentTable = new DataTable();
      DataColumn dcColumn;
      DataRow drCurrentRow = null;

      dcColumn = new DataColumn("VisName", System.Type.GetType("System.String"));
      dtCurrentTable.Columns.Add(dcColumn);
      dcColumn = new DataColumn("IDType", System.Type.GetType("System.String"));
      dtCurrentTable.Columns.Add(dcColumn);
      dcColumn = new DataColumn("IDNumber", System.Type.GetType("System.String"));
      dtCurrentTable.Columns.Add(dcColumn);
      dcColumn = new DataColumn("VisEmail", System.Type.GetType("System.String"));
      dtCurrentTable.Columns.Add(dcColumn);
      dcColumn = new DataColumn("VisMobile", System.Type.GetType("System.String"));
      dtCurrentTable.Columns.Add(dcColumn);
      dcColumn = new DataColumn("Company", System.Type.GetType("System.String"));
      dtCurrentTable.Columns.Add(dcColumn);

      //if (dtCurrentTable.Rows.Count > 0)
      //{

      // ViewState["CurrentTable"] = dtCurrentTable;

      foreach (GridViewRow grgr in grdVisDetails.Rows)
      {
        TextBox txtVisName = (TextBox)(grgr.FindControl("txtVisName"));
        DropDownList drpIDType = (DropDownList)(grgr.FindControl("drpIDType"));
        TextBox txtIDNumber = (TextBox)(grgr.FindControl("txtIDNumber"));
        TextBox txtVisEmail = (TextBox)(grgr.FindControl("txtVisEmail"));
        TextBox txtVisMobile = (TextBox)(grgr.FindControl("txtVisMobile"));
        TextBox txtVisCompany = (TextBox)(grgr.FindControl("txtVisCompany"));

        drCurrentRow = dtCurrentTable.NewRow();
        drCurrentRow["VisName"] = txtVisName.Text;
        drCurrentRow["IDType"] = drpIDType.SelectedItem.Text;
        drCurrentRow["IDNumber"] = txtIDNumber.Text;
        drCurrentRow["VisEmail"] = txtVisEmail.Text;
        drCurrentRow["VisMobile"] = txtVisMobile.Text;
        drCurrentRow["Company"] = txtVisCompany.Text;

        dtCurrentTable.Rows.Add(drCurrentRow);

      }


      TextBox txtVisNameF = (TextBox)grdVisDetails.FooterRow.FindControl("txtVisNameF");
      DropDownList drpIDTypeF = (DropDownList)grdVisDetails.FooterRow.FindControl("drpIDTypeF");
      TextBox txtIDNumberF = (TextBox)grdVisDetails.FooterRow.FindControl("txtIDNumberF");
      TextBox txtVisEmailF = (TextBox)grdVisDetails.FooterRow.FindControl("txtVisEmailF");
      TextBox txtVisMobileF = (TextBox)grdVisDetails.FooterRow.FindControl("txtVisMobileF");
      TextBox txtVisCompanyF = (TextBox)grdVisDetails.FooterRow.FindControl("txtVisCompanyF");

      drCurrentRow = dtCurrentTable.NewRow();
      drCurrentRow["VisName"] = txtVisNameF.Text;
      drCurrentRow["IDType"] = drpIDTypeF.SelectedItem.Text;
      drCurrentRow["IDNumber"] = txtIDNumberF.Text;
      drCurrentRow["VisEmail"] = txtVisEmailF.Text;
      drCurrentRow["VisMobile"] = txtVisMobileF.Text;
      drCurrentRow["Company"] = txtVisCompanyF.Text;

      dtCurrentTable.Rows.Add(drCurrentRow);

      ViewState["CurrentTable"] = dtCurrentTable;

      grdVisDetails.DataSource = dtCurrentTable;
      grdVisDetails.DataBind();

      //}
      //}


    }

    protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (drpDepartment.SelectedItem.Text != "Select")
      {
        string sSql = "SELECT Pl_id,pl_value FROM PickList_tran WHERE pl_head_id=18 AND Other_Data1='" + drpDepartment.SelectedItem.Text + "' order by pl_Value";

        DataTable dtHost = ocon.GetTable(sSql, new DataSet());

        if (dtHost.Rows.Count > 0)
        {
          drpHost.DataSource = dtHost;
          drpHost.DataTextField = "pl_value";
          drpHost.DataValueField = "Pl_id";
          drpHost.DataBind();
          drpHost.Items.Insert(0, "Select");
        }
        else
        {
          drpHost.DataSource = null;
          drpHost.DataBind();
          drpHost.Items.Insert(0, "Select");
        }
      }
    }


    private void BlankGrid()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(new DataColumn("VisName", typeof(string)));
      dt.Columns.Add(new DataColumn("IDType", typeof(string)));
      dt.Columns.Add(new DataColumn("IDNumber", typeof(string)));
      dt.Columns.Add(new DataColumn("VisEmail", typeof(string)));
      dt.Columns.Add(new DataColumn("VisMobile", typeof(string)));
      dt.Columns.Add(new DataColumn("Company", typeof(string)));



      for (int i = 0; i < 1; i++)
      {
        DataRow dr = dt.NewRow();
        dr["VisName"] = "";
        dr["IDType"] = "";
        dr["IDNumber"] = "";
        dr["VisEmail"] = "";
        dr["VisMobile"] = "";
        dr["Company"] = "";


        dt.Rows.Add(dr);
      }

      grdVisDetails.DataSource = dt;
      grdVisDetails.DataBind();
    }

    protected void btndel_Click(object sender, ImageClickEventArgs e)
    {
      ImageButton lb = (ImageButton)sender;
      GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
      int rowID = gvRow.RowIndex;

      if (ViewState["CurrentTable"] != null)
      {
        DataTable dt = (DataTable)ViewState["CurrentTable"];
        if (dt.Rows.Count > 1)
        {
          dt.Rows.Remove(dt.Rows[rowID]);
        }

        ViewState["CurrentTable"] = dt;

        grdVisDetails.DataSource = dt;
        grdVisDetails.DataBind();
      }

      //SetPreviousData();
    }


  }
}
