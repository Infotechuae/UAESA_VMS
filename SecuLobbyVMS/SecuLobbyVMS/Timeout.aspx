<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="Timeout.aspx.cs" Inherits="SecuLobbyVMS.Timeout" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="JavaScript" type="text/javascript">
        function CheckAll() {
            var chkAll = document.getElementById("ContentPlaceHolder1_grdVisDetails_checkAll");
            var i = 0;

            while (1) {

                var chkSelect = '';
                chkSelect = document.getElementById("ContentPlaceHolder1_grdVisDetails_check_" + i);

                if (chkSelect) {
                    if (chkAll.checked)
                        chkSelect.checked = true;
                    else
                        chkSelect.checked = false;

                    i++;
                }
                else {
                    break;
                }
            }
        }
        function padleft(val, ch, num) {
            var re = new RegExp(".{" + num + "}$");
            var pad = "";
            if (!ch) ch = " ";

            do {
                pad += ch;
            }
            while (pad.length < num);

            return re.exec(pad + val);
        }
 </script>

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

    <div class="wrapper">
        <div class="content-wrapper">

                         <iframe src="SubTimeout.aspx" frameborder="0" style="overflow: hidden; height: 781px; width: 100%"></iframe>


        </div>
    </div>


</asp:Content>
