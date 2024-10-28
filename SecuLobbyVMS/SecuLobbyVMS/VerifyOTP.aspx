<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyOTP.aspx.cs" Inherits="SecuLobbyVMS.VerifyOTP" %>

<!DOCTYPE html>

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

  <script language="javascript">  

    window.onload = function () {
      show();  // Call the show function when the page has finished loading
    };
  </script>
  <script language="javascript">  
    //var a = 49, b = 65;
    //var c = 100;
    //var d = 70;
    function show() {
      var a = getRandomInt(65, 90);  // ASCII values for uppercase letters
      var b = getRandomInt(97, 122); // ASCII values for lowercase letters
      var c = getRandomInt(48, 57);  // ASCII values for digits
      var d = getRandomInt(65, 122); // ASCII values for letters (exclude special characters)
      var e = getRandomInt(65, 122); // ASCII values for letters (exclude special characters)
      var f = getRandomInt(65, 122); // ASCII values for letters (exclude special characters)
      //var g = getRandomInt(48, 57); // ASCII values for letters (exclude special characters)
      //var h = getRandomInt(65, 122); // ASCII values for letters (exclude special characters)

      var main = document.getElementById('txt1');
      var a1 = String.fromCharCode(a);
      var b1 = String.fromCharCode(b);
      var c1 = String.fromCharCode(c);
      var d1 = String.fromCharCode(d);
      var e1 = String.fromCharCode(e);
      var f1 = String.fromCharCode(f);
      //var g1 = String.fromCharCode(g);
      //var h1 = String.fromCharCode(h);


      /*main.value = a1 + b1 + c1 + d1 + e1 + f1 + g1 + h1;*/
      main.value = a1 + b1 + c1 + d1 +e1+ f1;
      a = a + 1;
      b = b + 1;
      c = c + 1;
      d = d + 1;

      e = e + 1;
      f = f + 1;
      //g = g + 1;
      //h = h + 1;
    }


    function getRandomInt(min, max) {
      return Math.floor(Math.random() * (max - min + 1)) + min;
    }
  </script>

</head>

<body>
  <!-- Content -->
  <form id="form1" runat="server">
    <div class="container-xxl">
      <div class="authentication-wrapper authentication-basic container-p-y">
        <div class="authentication-inner">
          <!-- Register Card -->
          <div class="card">
            <div class="card-body">
              <!-- Logo -->
              <div class="app-brand justify-content-center">
                <a class="app-brand-link gap-2">
                  <span class="app-brand-logo demo"></span>
                  
                    <span class="app-brand-text demo text-body fw-bolder" style="color:#b68a35">UAE SPACE AGENCY </span>
                               
                   <img class="animation__wobble" src="dist/img/favicon.ico" alt="Seculobby" height="60" width="60">
                
                </a>
              </div>
              <!-- /Logo -->
              <h5 class="mb-2"  style="color:black;text-align: center;">OTP Verification</h5>
           <p  style="text-align: center;" class="mb-4">One Time Password (OTP) has been sent via email.</p>
                

              <div class="mb-3">
          <label for="username" class="form-label">OTP</label>
                <asp:TextBox ID="txtOtp" runat="server" class="form-control" required="required"></asp:TextBox>
              </div>
              <div class="mb-3">
                <table style="width: 100%;">
                  <tr>
                    <td style="width: 90%;">
                      <input type="text" id="txt1" runat="server" class="text-control" style="color: #EA7D1E; font-size: x-large; font-weight: bold; font-variant: normal; letter-spacing: 10pt; background-image: url('/images/glitter_background_b4.gif');" readonly="readonly" />
                    </td>
                    <td style="width: 10%; vertical-align: top;">
                      <%--                       <asp:ImageButton ID="img1" runat="server" ImageUrl="~/Images/circular-arrow.png" Width="30" Height="30" OnClientClick="show()" />--%>
                      <img src="images/circular-arrow.png" width="30" height="30" onclick="show()" />
                    </td>
                  </tr>
                </table>
              </div>
              <div class="mb-3">
                <label for="username" class="form-label">Captcha</label>
                <asp:TextBox ID="txtverification" runat="server" class="form-control" required="required"></asp:TextBox>
              </div>

              <asp:Button ID="btnSignup" style="color: #fff; background-color: #b68a35; border-color: #b68a35;" runat="server" Text="Verify" class="btn btn-primary d-grid w-100" OnClick="btnSignup_Click" />



              <p class="text-center">
                <span>Already have an account?</span>
                <a href="LoginSelfRegistration.aspx">
                  <span>Sign in instead</span>
                </a>
              </p>
            </div>
          </div>
          <!-- Register Card -->
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
