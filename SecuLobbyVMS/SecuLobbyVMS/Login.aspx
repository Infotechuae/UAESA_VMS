<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SecuLobbyVMS.Login" %>

<!-- =========================================================
* Sneat - Bootstrap 5 HTML Admin Template - Pro | v1.0.0
==============================================================

* Product Page: https://themeselection.com/products/sneat-bootstrap-html-admin-template/
* Created by: ThemeSelection
* License: You must have a valid license purchased in order to legally use the theme for your project.
* Copyright ThemeSelection (https://themeselection.com)

=========================================================
 -->
<!-- beautify ignore:start -->
<html
lang="en"
class="light-style customizer-hide"
dir="ltr"
data-theme="theme-default"
data-assets-path="../assets/"
data-template="vertical-menu-template-free">
<head>
    <meta charset="utf-8" />
    <meta
        name="viewport"
        content="width=device-width, initial-scale=1.0, user-scalable=no, minimum-scale=1.0, maximum-scale=1.0" />

    <title>UAESA | VMS </title>

    <meta name="description" content="" />

    <!-- Favicon -->
    <link rel="icon" type="image/x-icon" href="dist/img/favicon.ico" />

    <!-- Fonts -->
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link
        href="https://fonts.googleapis.com/css2?family=Public+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;1,300;1,400;1,500;1,600;1,700&display=swap"
        rel="stylesheet" />

    <!-- Icons. Uncomment required icon fonts -->
    <link rel="stylesheet" href="../assets/vendor/fonts/boxicons.css" />

    <!-- Core CSS -->
    <link rel="stylesheet" href="../assets/vendor/css/core.css" class="template-customizer-core-css" />
    <link rel="stylesheet" href="../assets/vendor/css/theme-default.css" class="template-customizer-theme-css" />
    <link rel="stylesheet" href="../assets/css/demo.css" />

    <!-- Vendors CSS -->
    <link rel="stylesheet" href="../assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css" />

    <!-- Page CSS -->
    <!-- Page -->
    <link rel="stylesheet" href="../assets/vendor/css/pages/page-auth.css" />
    <!-- Helpers -->
    <script src="../assets/vendor/js/helpers.js"></script>

    <!--! Template customizer & Theme config files MUST be included after core stylesheets and helpers.js in the <head> section -->
    <!--? Config:  Mandatory theme config file contain global vars & default theme options, Set your preferred theme option in this file.  -->
    <script src="../assets/js/config.js"></script>

    <link href="dist/css/sweetalert2.min.css" rel="stylesheet" />
    <script src="dist/js/sweetalert2.all.min.js"></script>
     <style> .btn-primary, .btn-warning, .btn-success, .btn-danger,.btn-info {
            background-color: #b68a35 !important;
            border-color: #b68a35 !important;
            color: #FFF !important;
        }

  </style>
    <script language="JavaScript" type="text/javascript">


        function Successalert(desturl, message) {
            var url = desturl;
            swal.fire({
                title: message, text: "", type: "success"
            }).then(function () {
                window.parent.location.href = url;
            });
        }

        function errorsalert(smessage) {

            swal.fire({
                title: smessage, text: "", type: "success"
            }).then(function () {

            });
        }

    </script>

</head>

<body>
    <!-- Content -->
    <form id="form1" runat="server" style=" background-color: white ">
<%--  <form id="form1" runat="server" style="  background-image: url('../../../../dist/img/LoginBG.jpg');">--%>
    <div class="container-xxl">
        <div class="authentication-wrapper authentication-basic container-p-y">
            <div class="authentication-inner">
                <!-- Register -->
                <div class="card">
                    <div class="card-body">
                        <!-- Logo -->
                        <div class="app-brand justify-content-center">
                            <a class="app-brand-link gap-2">
                                <span class="app-brand-logo demo"></span>
                                <img class="animation__wobble" src="dist/img/Space.png" alt="Seculobby" height="80" width="300">
                                <%--<span class="app-brand-text demo text-body fw-bolder">Seculobby</span>--%>
                            </a>
                        </div>
                        <!-- /Logo -->
                       <%-- <h4 class="mb-2">Welcome to Seculobby!</h4>
                        <p class="mb-4">Please sign-in to your account and start the adventure</p>--%>

                      
                            <div class="mb-3">
                                <label for="email" class="form-label">Email or Username</label>

                                <asp:TextBox ID="txtusername" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3 form-password-toggle">
                             <%--   <div class="d-flex justify-content-between">
                                    <label class="form-label" for="password">Password</label>
                                    <a href="ForgotPassword.aspx">
                                        <small>Forgot Password?</small>
                                    </a>
                                </div>--%>
                                <div class="input-group input-group-merge">

                                    <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" class="form-control"></asp:TextBox>
                                    <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                                </div>
                            </div>
                            <div class="mb-3">
                                <div class="form-check">

                                    <asp:CheckBox ID="chkRemember" Width="15" runat="server" class="form-check-input" />
                                    <label class="form-check-label" for="remember-me">Remember Me </label>
                                </div>
                            </div>
                            <div class="mb-3">
                                <asp:Button ID="btnLogin" runat="server" Text="Sign in" class="btn btn-primary d-grid w-100" OnClick="btnLogin_Click" />

                            </div>
                   

                        <p class="text-center" style="display:none;">
                            <span>New on our platform?</span>
                            <a href=".\Register.aspx">
                                <span>Create an account</span>
                            </a>
                        </p>
                    </div>
                </div>
                <!-- /Register -->
            </div>
        </div>
    </div>

    <!-- / Content -->

    </form>

    <!-- Core JS -->
    <!-- build:js assets/vendor/js/core.js -->
    <script src="../assets/vendor/libs/jquery/jquery.js"></script>
    <script src="../assets/vendor/libs/popper/popper.js"></script>
    <script src="../assets/vendor/js/bootstrap.js"></script>
    <script src="../assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js"></script>

    <script src="../assets/vendor/js/menu.js"></script>
    <!-- endbuild -->

    <!-- Vendors JS -->

    <!-- Main JS -->
    <script src="../assets/js/main.js"></script>

    <!-- Page JS -->

    <!-- Place this tag in your head or just before your close body tag. -->
    <script async defer src="https://buttons.github.io/buttons.js"></script>
</body>
</html>
