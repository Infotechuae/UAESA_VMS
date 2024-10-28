<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selfrejection.aspx.cs" Inherits="SecuLobbyVMS.selfrejection" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <div id="mywaitmsg" style="width: 300px">
                <%-- <h3>Loading..</h3>--%>
                <div style="text-align: center">
                    <img src="dist/img/Loader.gif" style="position: fixed; z-index: 9999; height: 64px; left: 50%; top: 175px; z-index: 9999; width: 70px;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
