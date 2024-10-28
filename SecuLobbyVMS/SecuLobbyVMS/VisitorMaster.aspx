<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="VisitorMaster.aspx.cs" Inherits="SecuLobbyVMS.VisitorMaster" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper">
        <div class="content-wrapper">

                         <iframe src="SubVisitorMaster.aspx" frameborder="0" style="overflow: hidden; height: 781px; width: 100%"></iframe>


        </div>
    </div>


</asp:Content>
