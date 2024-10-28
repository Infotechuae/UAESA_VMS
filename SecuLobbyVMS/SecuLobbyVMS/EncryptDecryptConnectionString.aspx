<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EncryptDecryptConnectionString.aspx.cs" Inherits="SecuLobbyVMS.EncryptDecryptConnectionString" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" align="center">
                <table width="98%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ffffff">
                    <tr>
                        <td class="all-pad" align="center" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="center">
                                        <div style="overflow: auto; width: auto; height: auto;" id="dvError" runat="server">
                                            <asp:Panel ID="pnlError" runat="server" Width="100%">
                                                <div style="padding: 5px">
                                                    <div class="tablewarning">
                                                        <div class="titleTop">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="align-left" width="90%" style="padding-left: 5px;">
                                                                        <asp:Label ID="Label2" runat="server" Text="Error"></asp:Label>
                                                                    </td>
                                                                    <td align="right" valign="middle" width="10%" class="closeBg" style="padding-right: 5px;">
                                                                        <a href="#">
                                                                            <img src="images/spacer.gif" width="15px" height="15px" alt="" border="0" />
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <table width="388" border="0" cellspacing="4" cellpadding="0" bgcolor="#FFF6B8" style="padding: 10px 0 5px 0;">
                                                            <tr>
                                                                <td class="align-left" valign="middle" width="30%">
                                                                    <asp:Image SkinID="imgErr" BorderWidth="0" ID="imgErr" runat="server" />
                                                                </td>
                                                                <td class="align-left" valign="top" width="70%">
                                                                    <asp:Label ID="lblError" ForeColor="0xff0000" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="PanelLogin">
                                            <asp:Panel runat="server" ID="pnlLogin">
                                                <div class="login-area">
                                                    <div class="login-top">
                                                        &nbsp;</div>
                                                    <div class="login-content">
                                                        <div class="login-content-border">
                                                            <fieldset>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                                                                    <tr align="left" class="">
                                                                        <td class="" align="left">
                                                                            <span class="">
                                                                                <asp:Label ID="Label1" runat="server" Text="Encrypt/Decrypt Connection String"></asp:Label>
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 20px;">
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblWarningMsg" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 20px;">
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 10px;">
                                                                            <asp:Label ID="lblCurrentStatus" runat="server" CssClass="Mandatory" Text="Current Status"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 20px;">
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="">
                                                                        <td height="27" align="left" style="padding-left: 20px;">
                                                                            <asp:Button SkinID="Normal" ID="btnEncrypt" runat="server" OnClick="btnEncrypt_Click"
                                                                                CssClass="button_bg" Text="Encrypt" ToolTip="Encrypt" AccessKey="E" />
                                                                            &nbsp;
                                                                            <asp:Button SkinID="Normal" ID="btnDecrypt" runat="server" OnClick="btnDecrypt_Click"
                                                                                CssClass="button_bg" Text="Decrypt" ToolTip="Decrypt" AccessKey="E" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </div>
                                                    </div>
                                                    <div class="login-bottom">
                                                        &nbsp;</div>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
        </div>
    </form>
</body>
</html>
