<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SecuLobbyVMS.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbl1" runat="server"></asp:Label>
        </div>
        <div>
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false"></asp:GridView>
        </div>
    </form>
</body>
</html>
