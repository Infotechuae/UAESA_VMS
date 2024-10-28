using DAL;
using QRCoder;
using SecuLobbyVMS.App_Code;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;

namespace SecuLobbyVMS
{
  public partial class PrintBadge : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        string sselfID = Convert.ToString(Request.QueryString["selfid"]);

        DataSet dsVistor = new DataSet();
        DBSQL db = new DBSQL();//declaring class
        DataTable DT = new DataTable();

        string[,] infoArray = new string[1, 2];
        infoArray[0, 0] = "ID";
        infoArray[0, 1] = sselfID;

        dsVistor = db.GetDataset("GetAppointmentCalenderBYID", infoArray, MyConnection.ReadConStr("Local"));
        DT = dsVistor.Tables[0];

        if (DT.Rows.Count > 0)
        {
          lblName.Text = DT.Rows[0]["VisitorName"].ToString().Replace(",", " ");
          lblHostName.Text = DT.Rows[0]["Organizer"].ToString();
          lblCompany.Text = DT.Rows[0]["Visitor_Company"].ToString().Replace(",", " ");
          lblEmail.Text = DT.Rows[0]["Visitor_Email"].ToString();

          string sVisitorQR = DT.Rows[0]["QRCode"].ToString();

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
              imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImageVis);
            }


            //ASPxImage1.Controls.Add(imgBarCode);
          }


          print.Visible = true;
          Session["ctrl"] = print;

          // ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('PrintWithCss.aspx','PrintMe','height=1000px,width=800px,scrollbars=1');</script>");
          ScriptManager.RegisterStartupScript(Page, GetType(), "onclick", "window.open('Print.aspx','PrintMe','height=977px,width=720px,scrollbars=1');", true);


        }
      }

    }
  }
}
