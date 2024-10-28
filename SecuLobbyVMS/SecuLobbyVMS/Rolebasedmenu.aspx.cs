using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
  public partial class Rolebasedmenu : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    CheckBox chk;
    //List<Object> menulist = new List<object>();

    private List<string> menulist
    {
      get
      {
        if (Session["MenuList"] == null)
        {
          Session["MenuList"] = new List<string>();
        }
        return (List<string>)Session["MenuList"];
      }
      set
      {
        Session["MenuList"] = value;
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      this.PopulateCheckBoxes();
      if (!Page.IsPostBack)
      {
        System.Web.UI.HtmlControls.HtmlGenericControl lnkDashboard = Master.FindControl("lnkDashboard") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkSelfReg = Master.FindControl("lnkSelfReg") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkRegis = Master.FindControl("lnkRegis") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCheckin = Master.FindControl("lnkCheckin") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCheckOut = Master.FindControl("lnkCheckOut") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkTimeOut = Master.FindControl("lnkTimeOut") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkWatchList = Master.FindControl("lnkWatchList") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkVisMas = Master.FindControl("lnkVisMas") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkVisTran = Master.FindControl("lnkVisTran") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkPrereg = Master.FindControl("lnkPrereg") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liDepart = Master.FindControl("liDepart") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liFloor = Master.FindControl("liFloor") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liEmployee = Master.FindControl("liEmployee") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liVisitorType = Master.FindControl("liVisitorType") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl lnkCard = Master.FindControl("lnkCard") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liUser = Master.FindControl("liUser") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlGenericControl liMail = Master.FindControl("liMail") as System.Web.UI.HtmlControls.HtmlGenericControl;


        System.Web.UI.HtmlControls.HtmlAnchor aDash = Master.FindControl("aDash") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aSelfRegis = Master.FindControl("aSelfRegis") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aRegis = Master.FindControl("aRegis") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCheckIN = Master.FindControl("aCheckIN") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCheckOut = Master.FindControl("aCheckOut") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aTimeOut = Master.FindControl("aTimeOut") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aWatchList = Master.FindControl("aWatchList") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisMas = Master.FindControl("aVisMas") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisTrans = Master.FindControl("aVisTrans") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aPrereg = Master.FindControl("aPrereg") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor adepart = Master.FindControl("adepart") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aFloor = Master.FindControl("aFloor") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aEmployee = Master.FindControl("aEmployee") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aVisitorType = Master.FindControl("aVisitorType") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aCard = Master.FindControl("aCard") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aUser = Master.FindControl("aUser") as System.Web.UI.HtmlControls.HtmlAnchor;
        System.Web.UI.HtmlControls.HtmlAnchor aMail = Master.FindControl("aMail") as System.Web.UI.HtmlControls.HtmlAnchor;


        lnkDashboard.Attributes.Remove("class");
        lnkSelfReg.Attributes.Remove("class");
        lnkRegis.Attributes.Remove("class");
        lnkCheckin.Attributes.Remove("class");
        lnkCheckOut.Attributes.Remove("class");
        lnkTimeOut.Attributes.Remove("class");
        lnkWatchList.Attributes.Remove("class");
        lnkVisMas.Attributes.Remove("class");
        lnkVisTran.Attributes.Remove("class");
        lnkPrereg.Attributes.Remove("class");
        liDepart.Attributes.Remove("class");
        liFloor.Attributes.Remove("class");
        liEmployee.Attributes.Remove("class");
        liVisitorType.Attributes.Remove("class");
        lnkCard.Attributes.Remove("class");
        liUser.Attributes.Remove("class");
        liMail.Attributes.Remove("class");

        aDash.Attributes.Remove("class");
        aSelfRegis.Attributes.Remove("class");
        aRegis.Attributes.Remove("class");
        aCheckIN.Attributes.Remove("class");
        aCheckOut.Attributes.Remove("class");
        aTimeOut.Attributes.Remove("class");
        aWatchList.Attributes.Remove("class");
        aVisMas.Attributes.Remove("class");
        aVisTrans.Attributes.Remove("class");
        aPrereg.Attributes.Remove("class");
        adepart.Attributes.Remove("class");
        aFloor.Attributes.Remove("class");
        aEmployee.Attributes.Remove("class");
        aVisitorType.Attributes.Remove("class");
        aCard.Attributes.Remove("class");
        aUser.Attributes.Remove("class");
        aMail.Attributes.Remove("class");

        lnkDashboard.Attributes.Add("class", "nav-item");
        lnkSelfReg.Attributes.Add("class", "nav-item");
        lnkRegis.Attributes.Add("class", "nav-item");
        lnkCheckin.Attributes.Add("class", "nav-item");
        lnkCheckOut.Attributes.Add("class", "nav-item");
        lnkTimeOut.Attributes.Add("class", "nav-item");
        lnkWatchList.Attributes.Add("class", "nav-item");
        lnkVisMas.Attributes.Add("class", "nav-item");
        lnkVisTran.Attributes.Add("class", "nav-item");
        lnkPrereg.Attributes.Add("class", "nav-item");
        liDepart.Attributes.Add("class", "nav-item");
        liFloor.Attributes.Add("class", "nav-item");
        liEmployee.Attributes.Add("class", "nav-item");
        liVisitorType.Attributes.Add("class", "nav-item");
        lnkCard.Attributes.Add("class", "nav-item");
        liUser.Attributes.Add("class", "nav-item");
        liMail.Attributes.Add("class", "nav-item");


        aDash.Attributes.Add("class", "nav-link");
        aSelfRegis.Attributes.Add("class", "nav-link");
        aRegis.Attributes.Add("class", "nav-link");
        aCheckIN.Attributes.Add("class", "nav-link");
        aCheckOut.Attributes.Add("class", "nav-link");
        aTimeOut.Attributes.Add("class", "nav-link");
        aWatchList.Attributes.Add("class", "nav-link");
        aVisMas.Attributes.Add("class", "nav-link");
        aVisTrans.Attributes.Add("class", "nav-link");
        aPrereg.Attributes.Add("class", "nav-link");
        adepart.Attributes.Add("class", "nav-link");
        aFloor.Attributes.Add("class", "nav-link");
        aEmployee.Attributes.Add("class", "nav-link");
        aVisitorType.Attributes.Add("class", "nav-link");
        aCard.Attributes.Add("class", "nav-link");
        aUser.Attributes.Add("class", "nav-link");
        liMail.Attributes.Add("class", "nav-link");

        System.Web.UI.HtmlControls.HtmlGenericControl lnkPurpose = Master.FindControl("lnkPurpose") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor aPurpose = Master.FindControl("aPurpose") as System.Web.UI.HtmlControls.HtmlAnchor;
        lnkPurpose.Attributes.Remove("class");
        aPurpose.Attributes.Remove("class");
        lnkPurpose.Attributes.Add("class", "nav-item");
        aPurpose.Attributes.Add("class", "nav-link");

        System.Web.UI.HtmlControls.HtmlGenericControl liMailtemplate = Master.FindControl("liMailtemplate") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor aMailtemplate = Master.FindControl("aMailtemplate") as System.Web.UI.HtmlControls.HtmlAnchor;
        liMailtemplate.Attributes.Remove("class");
        aMailtemplate.Attributes.Remove("class");
        liMailtemplate.Attributes.Add("class", "nav-item");
        aMailtemplate.Attributes.Add("class", "nav-link");


        System.Web.UI.HtmlControls.HtmlGenericControl li1 = Master.FindControl("li1") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor a1 = Master.FindControl("a1") as System.Web.UI.HtmlControls.HtmlAnchor;
        li1.Attributes.Remove("class");
        a1.Attributes.Remove("class");
        li1.Attributes.Add("class", "nav-item menu-open");
        a1.Attributes.Add("class", "nav-link active");


        System.Web.UI.HtmlControls.HtmlGenericControl li2 = Master.FindControl("li2") as System.Web.UI.HtmlControls.HtmlGenericControl;
        System.Web.UI.HtmlControls.HtmlAnchor a2 = Master.FindControl("a2") as System.Web.UI.HtmlControls.HtmlAnchor;
        li2.Attributes.Remove("class");
        a2.Attributes.Remove("class");
        li2.Attributes.Add("class", "nav-item");
        a2.Attributes.Add("class", "nav-link");


        Session["MenuList"] = null;

        int iTabId = Convert.ToInt32(Request.QueryString["ID"]);


        string sSql = "select ID,UsertypeName,RoleID from UserBasedMenu where ID ='" + iTabId + "'";
        DataTable dt = ocon.GetTable(sSql, new DataSet());
        //case when UserGroup = 1 then 'Admin' else 'User' end as
        if (dt.Rows.Count > 0)
        {
          txtID.Text = Convert.ToString(dt.Rows[0]["UsertypeName"]);
          string role = dt.Rows[0]["RoleID"].ToString();
          List<string> RoleList = role.Split(',').ToList();

          foreach (string roleID in RoleList)
          {
            // Find the checkbox with the corresponding ID
            CheckBox chk = (CheckBox)Panel1.FindControl(roleID);
            if (chk != null)
            {
              chk.Checked = true;
              if (!menulist.Contains(chk.ID))
              {
                menulist.Add(chk.ID);
              }
            }
          }

     

        }
        else
        {
          txtID.Text = "";
          txtID.ReadOnly = false;
        }


      }
      else
      {
        // Restore checkbox states from menulist
        foreach (string menuID in menulist)
        {
          CheckBox chk = (CheckBox)Panel1.FindControl(menuID);
          if (chk != null)
          {
            chk.Checked = true;
          }
        }
      }
    }

    private void PopulateCheckBoxes()
    {
      DataTable dtDur = ocon.GetTable("SELECT ID,MenuName FROM Tbl_Menu where Active = 'True'", new DataSet());
      foreach (DataRow row in dtDur.Rows)
      {
        chk = new CheckBox();
        chk.ID = row["ID"].ToString();
        chk.Text = row["MenuName"].ToString();
        chk.CheckedChanged += new EventHandler(CheckBox_Checked);
        chk.Width = 300; chk.Height = 50;
        Panel1.Controls.Add(chk);
      }
    }

    public void CheckBox_Checked(object sender, EventArgs e)
    {
      CheckBox chkid = (sender as CheckBox);
      if (chkid != null)
      {
        if (chkid.Checked)
        {
          if (!menulist.Contains(chkid.ID))
          {
            menulist.Add(chkid.ID);
          }
        }
        else
        {
          menulist.Remove(chkid.ID);
        }
        // Update the session variable
        Session["MenuList"] = menulist;
      }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      if (txtID.Text == "")
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "errorsalert('Please enter User Type');", true);
        return;
      }
      List<string> menulist = Session["MenuList"] as List<string>;
      if (menulist == null)
      {
        menulist = new List<string>();
      }

      int iTabId = Convert.ToInt32(Request.QueryString["ID"]);
      string sSql = "SELECT * FROM UserBasedMenu WHERE ID='" + iTabId + "'";
      DataTable dt = ocon.GetTable(sSql, new DataSet());
      if (dt.Rows.Count > 0)
      {
        string checkString = string.Join(",", menulist);
        string sUpdate = "UPDATE UserBasedMenu SET UsertypeName='" + txtID.Text.Trim() + "',RoleID='" + checkString + "' WHERE ID ='" + iTabId + "'";
        ocon.Execute(sUpdate);

      }
      else
      {
        string combinedString = string.Join(",", menulist);
        string sInsert = "insert into dbo.UserBasedMenu (UsertypeName,RoleID,ActiveRole)"
                 + " values ('" + txtID.Text.Trim() + "','" + combinedString + "',1)";
        ocon.Execute(sInsert);
      }
      string sDestURL = string.Format("\"{0}\"", "UserRole.aspx");
      string smessage = string.Format("\"{0}\"", "Role added successfully for" + " " + txtID.Text);
      string sVar = sDestURL + "," + smessage;
      ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "Successalert(" + sVar + ");", true);
      Session["MenuList"] = null;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
      Response.Redirect("UserRole.aspx");

    }
   
  }
}
