using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecuLobbyVMS
{
    public partial class EncryptDecryptConnectionString : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlError.Visible = false;

            //---open the web.config file
            string strFilePath = Server.MapPath("~/web.config");
            ExeConfigurationFileMap fileConfig = new ExeConfigurationFileMap();
            fileConfig.ExeConfigFilename = strFilePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileConfig, ConfigurationUserLevel.None);
            //---indicate the section to protect
            ConfigurationSection section = config.Sections.Get("connectionStrings");
            //---specify the protection provider
            if (section.SectionInformation.IsProtected)
            {
                lblCurrentStatus.Text = "Current Status : Encrypted";

                btnEncrypt.Visible = false;
                btnDecrypt.Visible = true;
            }
            else
            {
                lblCurrentStatus.Text = "Current Status : Not Encrypted";

                btnEncrypt.Visible = true;
                btnDecrypt.Visible = false;
            }

        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            EncryptConnStr("DataProtectionConfigurationProvider");
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            DecryptConnStr();
        }

        public void EncryptConnStr(string protectionProvider)
        {
            try
            {
                //---open the web.config file
                string strFilePath = Server.MapPath("~/web.config");
                ExeConfigurationFileMap fileConfig = new ExeConfigurationFileMap();
                fileConfig.ExeConfigFilename = strFilePath;
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileConfig, ConfigurationUserLevel.None);
                //---indicate the section to protect
                ConfigurationSection section = config.Sections.Get("connectionStrings");
                //---specify the protection provider
                section.SectionInformation.ProtectSection(protectionProvider);
                //---Apple the protection and update
                config.Save(ConfigurationSaveMode.Modified);

                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = ex.Message.ToString().Substring(0, 100) + "....";
            }
        }

        public void DecryptConnStr()
        {
            try
            {
                string strFilePath = Server.MapPath("~/web.config");
                ExeConfigurationFileMap fileConfig = new ExeConfigurationFileMap();
                fileConfig.ExeConfigFilename = strFilePath;
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileConfig, ConfigurationUserLevel.None);
                ConfigurationSection section = config.Sections.Get("connectionStrings");
                section.SectionInformation.UnprotectSection();
                config.Save();

                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
        }
    }
}
