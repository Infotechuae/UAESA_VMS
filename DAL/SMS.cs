using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Xml;

namespace DAL
{
    public class SMS
    {
        //public static string AdnocGenerateQRCode(string badgeNumber, string expiryTimestamp)
        //{
        //    badgeNumber = badgeNumber.PadLeft(20, '0');
        //    //  string expiryTimestamp = DateTime.Now.AddSeconds(60 * 10 /* Minutes as sample */).ToString("ffffffssmmHHddMMyyyy");
        //    Random rand = new Random();
        //    string randomNumber = rand.Next(10000000, 99999999).ToString().PadLeft(10, '0');
        //    BigInteger qrCodeValue = 0;

        //    string initialQRCode = "";
        //    bool isKeyValue = true;

        //    for (int i = 0; i < badgeNumber.Length; i++)
        //    {
        //        initialQRCode += $"{badgeNumber[i]}{expiryTimestamp[i]}" + (isKeyValue ? key[i / 2] : randomNumber[i / 2]);
        //        isKeyValue = !isKeyValue;
        //    }


        //    qrCodeValue = BigInteger.Parse(initialQRCode) << 7;

        //    string qrCode = qrCodeValue.ToString("X");

        //    return qrCode;
        //}

        public bool sendXMLSMS(string strMobile, string strMessage, string UserId, string pwd, string url, string strCustname,bool EnableSMS)
        {
            bool rval = false;
            string TxtResponse = "";
           

            try
            {
               
                string strReturn = string.Empty;

                if (EnableSMS)
                {

                    if (strMobile.Length > 0)
                    {
                        //for (int i = 0; i < strMobile.Length; i++)
                        //{
                        if (strMobile.Trim() != "")
                        {
                            try
                            {

                                MemoryStream memoryStream = new MemoryStream();
                                XmlWriterSettings settings = new XmlWriterSettings();
                                settings.Indent = true;
                                settings.Encoding = Encoding.UTF8;
                                settings.CloseOutput = true;

                                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(memoryStream, settings))
                                {
                                    writer.WriteStartDocument();
                                    writer.WriteStartElement("message");
                                    writer.WriteElementString("username", UserId.Trim());
                                    writer.WriteElementString("password", pwd.Trim());
                                    writer.WriteElementString("to", strMobile.Trim());
                                    writer.WriteElementString("senderid", strCustname);
                                    writer.WriteElementString("text", strMessage);
                                    writer.WriteElementString("type", "unicode");

                                    writer.WriteEndElement();
                                    writer.WriteEndDocument();
                                }



                                TxtResponse = postXMLData(url, memoryStream);


                                if (TxtResponse.ToLower().Contains("insufficient credits, please updates your credits"))
                                {

                                    //  updatesenddata(EID);
                                    rval = false;
                                }
                                else if (TxtResponse.ToLower().Contains("Fail") || TxtResponse.ToLower().Contains("invalid userId"))
                                {


                                    rval = false;
                                }

                                else
                                {

                                    //   updatesenddata(EID);
                                    rval = true;
                                }
                            }
                            catch
                            {
                                //  WriteDataline("**SMS Message Error ** : " + err);
                            }
                        }

                    }
                }
                    return rval;

            }


            catch (Exception ex)
            {
               // WriteDataline("**Send SMS  Fail ** : " + ex.Message.ToString());
                return false;
                // MessageBox.Show(ex.ToString());

            }
            finally
            {

            }

        }


//        <?xml version="1.0" encoding="utf-8"?>
//<message>
//    <username>infosmart</username>
//    <password>api_password</password>
//    <to>971525184526;971525184527</to>
//    <senderid>Info Smart</senderid>
//    <text>
//        %30%00%00%04%54%65%73%74%02%01%00%00%48%1C%01%66%66%66%66%66%66%66%66%66
//        %99%99%99%99%99%99%99%99%99%80%00%00%00%00%00%00%00%01%40%00%00%00%60%00
//        %E0%00%02%40%00%00%0E%90%03%10%00%02%80%00%00%31%08%0C%F3%B8%01%80%00%00
//        %40%04%11%04%44%01%40%00%00%FF%FE%2F%8B%12%02%40%00%00%00%00%53%8C%AA%02
//        %80%00%00%00%00%62%89%C4%01%80%00%00%00%00%41%41%40%01%40%00%00%00%00%01
//        %42%80%02%40%00%20%00%00%0B%05%04%15%8A%00%00%00%03%01%03%02%01%42%80%01
//        %F0%00%00%00%A2%80%01%80%0F%FE%00%00%00%A5%00%01%5F%FF%FF%FF%FF%FE%A5%7F
//        %FA%40%0A%AA%00%00%00%55%00%02%82%01%50%04%40%01%5D%08%A1%88%10%24%80%00
//        %40%FF%02%01%40%41%00%01%00%03%AB%E0%02%44%00%00%08%20%0D%55%58%82%80%10
//        %14%40%00%1A%AA%AC%01%80%00%00%00%00%35%55%56%01%40%01%00%00%80%6A%AA%AB
//        %02%40%00%00%00%00%55%55%55%02%80%00%00%00%00%00%00%00%01%99%99%99%99%99
//        %99%99%0B%05%04%15%8A%00%00%00%03%01%03%03%99%99%66%66%66%66%66%66%66%66
//        %66
//    </text>
//    <type>picture</type>
//    <datetime>2020-02-15 16:23:46</datetime>
//</message>

        public bool sendXMLPicutreSMS(string strMobile,  string strMessage, string UserId, string pwd, string url, string strCustname)
        {
            bool rval = false;
            string TxtResponse = "";


            try
            {

                string strReturn = string.Empty;



                if (strMobile.Length > 0)
                {
                    //for (int i = 0; i < strMobile.Length; i++)
                    //{
                    if (strMobile.Trim() != "")
                    {
                        try
                        {

                            MemoryStream memoryStream = new MemoryStream();
                            XmlWriterSettings settings = new XmlWriterSettings();
                            settings.Indent = true;
                            settings.Encoding = Encoding.UTF8;
                            settings.CloseOutput = true;

                            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(memoryStream, settings))
                            {
                                writer.WriteStartDocument();
                                writer.WriteStartElement("message");
                                writer.WriteElementString("username", UserId.Trim());
                                writer.WriteElementString("password", pwd.Trim());
                                writer.WriteElementString("to", strMobile.Trim());
                                writer.WriteElementString("senderid", strCustname);
                                writer.WriteElementString("text", strMessage);
                                writer.WriteElementString("type", "picture");

                                writer.WriteEndElement();
                                writer.WriteEndDocument();
                            }



                            TxtResponse = postXMLData(url, memoryStream);


                            if (TxtResponse.ToLower().Contains("insufficient credits, please updates your credits"))
                            {

                                //  updatesenddata(EID);
                                rval = false;
                            }
                            else if (TxtResponse.ToLower().Contains("Fail") || TxtResponse.ToLower().Contains("invalid userId"))
                            {


                                rval = false;
                            }

                            else
                            {

                                //   updatesenddata(EID);
                                rval = true;
                            }
                        }
                        catch
                        {
                            //  WriteDataline("**SMS Message Error ** : " + err);
                        }
                    }

                }
                return rval;

            }


            catch (Exception ex)
            {
                // WriteDataline("**Send SMS  Fail ** : " + ex.Message.ToString());
                return false;
                // MessageBox.Show(ex.ToString());

            }
            finally
            {

            }

        }
        public string postXMLData(string url, MemoryStream memoryStream)
        {
            string requeststr = "";
            var restext = "";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            //byte[] bytes;
            //string responseStr="";
            try
            {

//                Url: https://smartsmsgateway.com
//User: infosmart
//Pass:  ins901
//Sender ID: Info Smart


                 // url = "http://smartsmsgateway.com/api/api_xml.php";
                //string xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());
                requeststr = Encoding.UTF8.GetString(memoryStream.ToArray());
                using (var client = new WebClient())
                {
                    //var response = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection {{"IN",request}});
                    client.Encoding = Encoding.UTF8;
                    var response = client.UploadString(url, requeststr);
                    restext = response;
                }


               
            }
            catch (Exception er)
            { restext = null; }
            return restext;

        }
        public bool sendmail(string eto,string eSubject,string eMessage,string path,string smtpServer,string userID,string passwrd,string displayname )
        {
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Credentials = new System.Net.NetworkCredential(userID, passwrd);
            SmtpServer.Port = 25;//Port: 465 or 587 
            SmtpServer.Host = "smtp.gmail.com";//txtHost.Text.Trim (); //"smtp.gmail.com";

            //outbound.mailhop.org
            //smtp.mail.yahoo.com (port 25)
            //  smtp.gmail.com (SSL enabled, port 465)

            SmtpServer.EnableSsl = false;
            // SmtpServer.EnableSsl = true;
            MailMessage mail = new MailMessage();
            
            
            String[] addr = eto.Split(',');
            try
            {
                mail.From = new MailAddress("smartVMS@info.com", displayname, System.Text.Encoding.UTF8);
                Byte i;
                for (i = 0; i < addr.Length; i++)
                    mail.To.Add(addr[i]);
                mail.Subject =  eSubject;
                mail.Body = eMessage;
                //if (ListBox1.Items.Count != 0)
                //{
                //    for (i = 0; i < ListBox1.Items.Count; i++)
                //        mail.Attachments.Add(new Attachment(ListBox1.Items[i].ToString()));
                //}
                if (path.Length > 0)
                {
                    LinkedResource logo = new LinkedResource(path);
                    logo.ContentId = "Logo";
                    string htmlview;
                    //htmlview = "<html><body>" + txtmessage.Text + "<div style=\"width:100%\"><hr/><table border=1>" +
                    //            "<tr><td width=30%><img src=cid:Logo alt=\"" + txtFrom.Text.Trim() + "\" />" +
                    //            "</td><td><h1>" + txtFrom.Text.Trim() + "</h1><br/>" + "address" + "<br/>" +
                    //            "Web Developer<br/><b>Ph. " + "050-7864643" + "</b><br/>" + "Dubai" +
                    //            "<br/>" + "</td></tr>" +
                    //            "</table>" + "mail from " + txtFrom.Text.Trim() + "<hr/><br/>";

                    htmlview = eMessage;
                    AlternateView alternateView1 = AlternateView.CreateAlternateViewFromString(htmlview, null, MediaTypeNames.Text.Html);
                    alternateView1.LinkedResources.Add(logo);
                    mail.AlternateViews.Add(alternateView1);
                }
                else
                {
                    //    LinkedResource logo = new LinkedResource(path);
                    //  logo.ContentId = "Logo";
                    string htmlview;
                    //htmlview = "<html><body>" + txtmessage.Text + "<div style=\"width:100%\"><hr/><table border=1>" +
                    //            "<tr><td width=30%>" + txtFrom.Text.Trim() + "\" />" +
                    //            "</td><td><h1>" + txtFrom.Text.Trim() + "</h1><br/>" + "address" + "<br/>" +
                    //            "Web Developer<br/><b>Ph. " + "050-7864643" + "</b><br/>" + "Dubai" +
                    //            "<br/>" + "</td></tr>" +
                    //            "</table>" + "mail from " + txtFrom.Text.Trim() + "<hr/><br/>";
                    htmlview = eMessage;
                    AlternateView alternateView1 = AlternateView.CreateAlternateViewFromString(htmlview, null, MediaTypeNames.Text.Html);
                    //    alternateView1.LinkedResources.Add(logo);
                    mail.AlternateViews.Add(alternateView1);
                }

                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                // mail.ReplyTo = new MailAddress("NO Reply");
                SmtpServer.Send(mail);

                
                Thread.Sleep(100);
                return true;

            }

            catch (Exception ex)
            {
                
                return false;
                // MessageBox.Show(ex.ToString());

            }
            finally
            {
                //client = null;
                SmtpServer = null;
                mail = null;




            }

        }




    }
}
