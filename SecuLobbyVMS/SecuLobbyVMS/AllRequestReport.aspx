<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="AllRequestReport.aspx.cs" Inherits="SecuLobbyVMS.AllRequestReport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


      <script language="JavaScript" type="text/javascript">


        function Successalert(desturl, message) {
                var url = desturl;
                swal.fire({
                    title: message, text: "", type: "success"
                }).then(function () {
                    window.parent.location.href = url;
                });
            }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <style> .btn-primary, .btn-warning, .btn-success, .btn-danger,.btn-info {
            background-color: #b68a35 !important;
            border-color: #b68a35 !important;
            color: #FFF !important;
        }

  </style>
    <div class="wrapper">
        <div class="content-wrapper">

                         <iframe src="SubAllRequestReport.aspx" frameborder="0" style="overflow: hidden; height: 781px; width: 100%"></iframe>


        </div>
    </div>


</asp:Content>
