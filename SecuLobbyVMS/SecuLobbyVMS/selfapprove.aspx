<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selfapprove.aspx.cs" Inherits="SecuLobbyVMS.selfapprove" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <link rel="shortcut icon" type="image/jpg" href="dist/img/fav.png" />
    <script src="dist/js/sweetalert2.all.min.js"></script>
    <link href="dist/css/sweetalert2.min.css" rel="stylesheet" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
      <script language="JavaScript" type="text/javascript">


        function Successalert(message) {
          
            swal.fire({
                title: message, text: "", type: "success"
            }).then(function () {
                window.open('', '_self').close();
            });
        }
    </script>
<%--  <style>
        body {
            margin: 0;
            padding: 0;
            background-image: url('dist/img/Loader.gif'); /* Replace with your image URL */
            background-size:; /* Adjust as needed */
            background-position: center;
            background-repeat: no-repeat;
            height: 64px;
            width:70px;
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        }
    </style>--%>
</head>
<body >
    <form id="form1" runat="server">
        <div>
          <asp:Button ID="dynamicButton" runat="server" Text="" BackColor="Transparent" BorderColor="White" BorderWidth="0px" OnClientClick="mywaitdialog()" />

               <div id="mywaitmsg" style="display: none; width: 300px">
             
                <div style="text-align: center">
                    <img src="dist/img/Loader.gif" style="position: fixed; z-index: 9999; height: 64px; left: 50%; top: 175px; z-index: 9999; width: 70px;" />
                </div>
            </div>
        </div>
    </form>

  <script>
      $(document).ready(function () {
          //document.getElementById("dynamicButton").click();
            var mywait = document.getElementById("mywaitmsg")
            mywait.style.display = 'block';
      });
</script>
    <script>
        function mywaitdialog() {
            var mywait = document.getElementById("mywaitmsg")
            mywait.style.display = 'block';

            //var MainDiv = document.getElementById("divmain")
            //MainDiv.setAttribute('style', 'background-color: Gray; filter: alpha(opacity=80); opacity: 0.8; z-index: 10000');
        }
    </script>
</body>
</html>
