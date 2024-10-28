
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using System.Web.Configuration;
using System.Web.Security;
using System.Configuration;
using System.Web;
/// <summary>
/// Summary description for MyConnection
/// </summary>
public static class MyConnection
{
    public static string ReadConStr(String ProjCode)
    {
        string strConn = "";
        if (ProjCode=="Local")
        {
            strConn = System.Configuration.ConfigurationManager.ConnectionStrings["SecuLobbyConnectionStringLocal"].ToString();
        }
            else if (ProjCode=="Central")
        {
            strConn = System.Configuration.ConfigurationManager.ConnectionStrings["SecuLobbyConnectionStringCentral"].ToString();
        }
        
        else
        {
            strConn = System.Configuration.ConfigurationManager.ConnectionStrings["SecuLobbyConnectionStringLocal"].ToString();
        }
       
        //SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["eusisconnectionstring"].ConnectionStrings);
        return strConn;
    }



    public static string sendMail(string From, string To, string Subject, string data)
    {
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            if (To == "")
                To = "xxx@xxxx.com";
            MailAddressCollection m = new MailAddressCollection();
            m.Add(To);
            mail.Subject = Subject;
            mail.From = new MailAddress(From);
            mail.Body = data;
            mail.IsBodyHtml = true;
            mail.ReplyTo = new MailAddress(From);
            mail.To.Add(m[0]);
            smtp.Host = "smtp.xxxx.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("xxxx@zoho.com", "xxxxx");

            smtp.Send(mail);

            return "done";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}