using SecuLobbyVMS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SecuLobbyVMS
{
  public partial class Print : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        Control ctrl = (Control)Session["ctrl"];
        PrintHelper.PrintWebControl(ctrl, "<link href='dist/css/Print.css' rel='stylesheet' type='text/css' />");
      }
    }
  }
}
