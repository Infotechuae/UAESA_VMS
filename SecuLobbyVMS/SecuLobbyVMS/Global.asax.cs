
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

 namespace SecuLobby
{
    public class Global : System.Web.HttpApplication
    {

         
        protected void Application_Start(object sender, EventArgs e)
{

     //       Session["Loc_ID"] = System.Configuration.ConfigurationManager.AppSettings["Loc_ID"];
            //if (Session["isLoginSucess"] != null)
            //{
            //    //Redirect to Welcome Page if Session is not null  
            //    Response.Redirect("~/Pages/EmployeeSelf_Register_Visitor.aspx");

            //}
            //else
            //{
            //    //Redirect to Login Page if Session is null & Expires   
            //    Response.Redirect("~/Pages/Login.aspx");

            //}
            // DevExpress.Web.ASPxWebControl.GlobalTheme = SecuLobbyVMS.App_Code.Utils.CurrentTheme;
            //if (Session != null && Session.IsNewSession)
            //{
            //    string szCookieHeader = Request.Headers["Cookie"];
            //    if ((szCookieHeader != null) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
            //    {
            //       // if (User.Indentity.IsAuthenticated)
            //       //if (Session["isLoginSucess"].ToString=="TRUE")
            //       // {
            //            FormsAuthentication.SignOut();
            //            // RedirectToAction(“ActionName”, “ControllerName”,  route values);
            //            Response.Redirect("~/Pages/Login.aspx");
            //        //}
            //    }
            //}

        }

protected void Session_Start(object sender, EventArgs e)
{

}

protected void Application_BeginRequest(object sender, EventArgs e)
{

}

protected void Application_AuthenticateRequest(object sender, EventArgs e)
{

}

protected void Application_Error(object sender, EventArgs e)
{

}

protected void Session_End(object sender, EventArgs e)
{
    //Response.Redirect("Login.aspx");
}

protected void Application_End(object sender, EventArgs e)
{
    //Response.Redirect("Login.aspx");
}

 }
 }
 