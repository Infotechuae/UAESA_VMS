using DAL;
using QRCoder;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace SecuLobbyVMS
{
  public partial class action : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    protected void Page_Load(object sender, EventArgs e)
    {
      string sID = Request.QueryString["tabid"].ToString();

      DataTable dt = ocon.GetTable("SELECT * FROM Visit_Request WHERE ID='" + sID + "'", new DataSet());
      if (dt.Rows.Count > 0)
      {
        int IsApproved = Convert.ToInt32(dt.Rows[0]["IsApproved"]);

        if (IsApproved != 1)
        {
          string sUpdate = "UPDATE Visit_Request SET IsApproved=1 WHERE ID='" + sID + "'";
          ocon.Execute(sUpdate);




          Random rnd = new Random();
          int myRandomNo = rnd.Next(1000000, 9999999);
          string sPersonID = myRandomNo.ToString();

          string sUserID = Convert.ToString(dt.Rows[0]["UserID"]);
          string sVisitorName = Convert.ToString(dt.Rows[0]["VisitorName"]);
          string sVisitorCompany = Convert.ToString(dt.Rows[0]["Company"]);
          string sVisitorEmail = Convert.ToString(dt.Rows[0]["VisEmailID"]);
          string sVisitorPhone = Convert.ToString(dt.Rows[0]["VisPhone"]);
          string sMeetingName = Convert.ToString(dt.Rows[0]["PurposeMsg"]);
          string sDesc = sMeetingName + " - " + sVisitorCompany;

          string sStartTime = Convert.ToString(dt.Rows[0]["VisDate"]);
          string sEndTime = Convert.ToString(dt.Rows[0]["VisEndTime"]);

          string dateString = sStartTime;

          DateTime epochTime = DateTime.Parse("1970-01-01");
          DateTime date = DateTime.Parse(dateString);

          var milliseconds = date.Subtract(epochTime).TotalSeconds;


          string sQRCode = "qrc:1;" + sPersonID + ";" + milliseconds + ";1;";

          string sMeetingLocation = Convert.ToString(dt.Rows[0]["Location"]);

          string sHostName = Convert.ToString(dt.Rows[0]["HostName"]);

          string sLocID = Convert.ToString(dt.Rows[0]["Loc_ID"]);

          string sDuration = Convert.ToString(dt.Rows[0]["VisDuration"]);
          string sDurName = "";
          DataTable dtDur = ocon.GetTable("SELECT pl_Value FROM PickList_tran WHERE pl_head_id=6 AND pl_id='" + sDuration + "'", new DataSet());
          if (dtDur.Rows.Count > 0)
          {
            sDurName = dtDur.Rows[0]["pl_Value"].ToString();
          }

          string sOrganizer = Convert.ToString(dt.Rows[0]["HostEmailID"]);

          string sVisImage = Convert.ToString(dt.Rows[0]["VisImage"]);


          string ssQRCodeUpdate = "UPDATE Visit_Request SET QRcode=1 WHERE ID='" + sQRCode + "'";
          ocon.Execute(ssQRCodeUpdate);

          string sInsertScheduling = "insert into[dbo].[Scheduling]"
          + "(cId, UserId,Status,Subject, Description,[Label],StartTime,EndTime,Location,ContactInfo,EntryID,QRCode,Organizer,Loc_ID, HostName, VisPhone, VisCompany) values "
          + "('" + sID + "', '" + sUserID + "',1,'" + sVisitorName + "','" + sDesc + "',1,'" + sStartTime + "','" + sEndTime + "','" + sMeetingLocation + "','" + sVisitorEmail + "','" + sID + "','" + sQRCode + "','" + sOrganizer + "', '" + sLocID + "', '" + sHostName + "', '" + sVisitorPhone + "', '" + sVisitorCompany + "')";

          ocon.Execute(sInsertScheduling);



          string sInsertTBL_Temp_QR = "insert into[dbo].[TBL_Temp_QR]"
            + " (PersonName,PersonID,MeetingDate,VisImage) Values "
            + "('" + sVisitorName + "','" + sPersonID + "','" + sStartTime + "','" + sVisImage + "') ";

          ocon.Execute(sInsertTBL_Temp_QR);

          DateTime dtmts = DateTime.Parse(sStartTime);
          string mstartTime = Convert.ToDateTime(dtmts).ToString("yyyy-MM-dd") + " " + dtmts.ToString("HH:mm");




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
              imgBarCodeVis.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImageVis);
            }


            //ASPxImage1.Controls.Add(imgBarCode);
          }


          SecuLobbyVMS.App_Code.Utils.sendEmployee_Visitormail(byteImageVis, sQRCode, sVisitorPhone, sOrganizer, sVisitorEmail, sVisitorName, "", mstartTime, sMeetingName, sMeetingLocation, sDurName, false, sHostName, "", sVisitorCompany);

        }
      }
    }
  }
}
