<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDisplayForm.aspx.cs" Inherits="SecuLobbyVMS.ReportDisplayForm" %>
<%@ Register Assembly="C1.Web.C1WebReport.2" Namespace="C1.Web.C1WebReport" TagPrefix="cc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
         <asp:ScriptManager ID="script1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="0"></asp:ScriptManager>
        <div>
            <asp:Panel runat="server" ID="pnlReport">
                <cc2:c1webreport id="C1WebReport1" runat="server" height="187px" borderstyle="Inset">
                    <cache enabled="False" />
                    <navigationbar style-backcolor="Control" hasexportbutton="True" hasgotopagebutton="True">
                    </navigationbar>
                </cc2:c1webreport>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
