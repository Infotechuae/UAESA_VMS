<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintBadge.aspx.cs" Inherits="SecuLobbyVMS.PrintBadge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UAESA | VMS</title>
    <link rel="shortcut icon" type="image/jpg" href="dist/img/favicon.ico" />
    <link href="dist/css/Print.css" rel="stylesheet" />
    <%--<style>
        .card {
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
            max-width: 300px;
            margin: auto;
            text-align: center;
            font-family: arial;
        }

        .title {
            color: grey;
            font-size: 18px;
        }

        button {
            border: none;
            outline: 0;
            display: inline-block;
            padding: 8px;
            color: white;
            background-color: #000;
            text-align: center;
            cursor: pointer;
            width: 100%;
            font-size: 18px;
        }

        a {
            text-decoration: none;
            font-size: 22px;
            color: black;
        }

            button:hover, a:hover {
                opacity: 0.7;
            }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="print" runat="server" Width="100%">
            <%--<div class="card" style="margin-top: 30px; height: 400px; width: 450px;">
                <div style="height: 30px;"></div>
                <div style="height: 200px;">
                    <asp:Image ID="imgQRCode" runat="server" Style="height: 150px; width: 150px;" />



                    <h4>Name:<asp:Label ID="lblName" runat="server"></asp:Label></h4>
                    <h4>Host Name:<asp:Label ID="lblHostName" runat="server"></asp:Label></h4>
                    <h5>Email ID:<asp:Label ID="lblEmail" runat="server"></asp:Label></h5>
                    <h5>Company:<asp:Label ID="lblCompany" runat="server"></asp:Label></h5>
                    <div>
                    </div>
                </div>

            </div>--%>

           <div class="card">
                <div class="spacer"></div>
                <div class="card-content">
                    <asp:Image ID="imgQRCode" runat="server" CssClass="qr-image" />

                    <h4 class="info-header">Name:<asp:Label ID="lblName" runat="server"></asp:Label></h4>
                    <h4 class="info-header">Host Name:<asp:Label ID="lblHostName" runat="server"></asp:Label></h4>
                    <h5 class="info-subheader">Email ID:<asp:Label ID="lblEmail" runat="server"></asp:Label></h5>
                    <h5 class="info-subheader">Company:<asp:Label ID="lblCompany" runat="server"></asp:Label></h5>
                </div>
            </div>
        </asp:Panel>

    </form>
</body>
</html>
