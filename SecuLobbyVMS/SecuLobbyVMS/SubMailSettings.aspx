<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubMailSettings.aspx.cs" Inherits="SecuLobbyVMS.SubMailSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/jpg" href="dist/img/fav.png" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome Icons -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <!-- daterange picker -->
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css" />
    <!-- Tempusdominus Bootstrap 4 -->
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <!-- overlayScrollbars -->
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />

    <link rel="stylesheet" href="plugins/select2/css/select2.min.css" />
    <link rel="stylesheet" href="plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css" />
    <!-- Bootstrap4 Duallistbox -->
    <link rel="stylesheet" href="plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css" />
    <!-- BS Stepper -->
    <link rel="stylesheet" href="plugins/bs-stepper/css/bs-stepper.min.css" />
    <!-- dropzonejs -->
    <link rel="stylesheet" href="plugins/dropzone/min/dropzone.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <script src="dist/js/sweetalert2.all.min.js"></script>
    <link href="dist/css/sweetalert2.min.css" rel="stylesheet" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            //Date picker
            $('#reservationdate').datetimepicker({
                format: 'L'
            });

        }

    </script>
    <style>
        .btn-primary, .btn-warning, .btn-success, .btn-danger {
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
    </script>
    <script language="JavaScript" type="text/javascript">
        function errorsalert(smessage) {

                swal.fire({
                    title: smessage, text: "", type: "success"
                }).then(function () {

                });
            }

    </script>

    <%--      <script type="text/javascript">
        function UploadFile(fileUpload) {
                if (fileUpload.value != '') {
                    document.getElementById("<%=btnUpload.ClientID %>").click();
                }
            }
        function selectFile() {
                $('#faUpload').click();


            }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <section class="content">
            <div class="container-fluid">
                <!-- Info boxes -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <h5 class="card-header">
                                <asp:Label ID="header1" runat="server" Text="Mail Setting"></asp:Label>
                            </h5>
                            <!-- Account -->

                            <hr class="my-0" />
                            <div class="card-body">

                                <div class="row">

                                    <div class="mb-3 col-md-6">
                                        <label for="drpEmailType" class="form-label">
                                            <asp:Label ID="lblEmailType" runat="server" class="form-label" Text="Email Type" Font-Bold="true"></asp:Label>
                                        </label>
                                        <asp:DropDownList ID="drpEmailType" runat="server" class="form-control select2" Style="width: 100%;">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                            <asp:ListItem Text="Microsoft 365" Value="Microsoft 365"></asp:ListItem>
                                            <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="mb-3 col-md-6">
                                        <label for="txtSMTP" class="form-label">
                                            <asp:Label ID="lblSMTP" runat="server" class="form-label" Text="SMTP Server" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtSMTP" runat="server" class="form-control" required="required"></asp:TextBox>
                                    </div>

                                    <div class="mb-3 col-md-6">
                                        <label for="txtEmail" class="form-label">
                                            <asp:Label ID="lblEmail" runat="server" class="form-label" Text="Email ID" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtEmail" runat="server" class="form-control" required="required" TextMode="Email"></asp:TextBox>
                                    </div>

                                    <div class="mb-3 col-md-6">
                                        <label for="txtPassword" class="form-label">
                                            <asp:Label ID="lblPassword" runat="server" class="form-label" Text="Password" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password"></asp:TextBox>
                                    </div>

                                    <div class="mb-3 col-md-6">
                                        <label for="txtPassword" class="form-label">
                                            <asp:Label ID="lblPort" runat="server" class="form-label" Text="Port" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtPort" runat="server" class="form-control" required="required" TextMode="Number"></asp:TextBox>
                                    </div>
                                    <div class="mb-3 col-md-6">
                                        <label for="drpSsl" class="form-label">
                                            <asp:Label ID="lblSSl" runat="server" class="form-label" Text="SSl Requied" Font-Bold="true"></asp:Label>
                                        </label>
                                        <asp:DropDownList ID="drpSsl" runat="server" class="form-control select2" Style="width: 100%;">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                            <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="mb-3 col-md-6">
                                        <label for="txtEmailName" class="form-label">
                                            <asp:Label ID="lblEmailName" runat="server" class="form-label" Text="Email Name" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtEmailName" runat="server" class="form-control" required="required"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <!-- /Account -->
                        </div>
                        <div class="card">
                            <h5 class="card-header">
                                <asp:Label ID="Label4" runat="server" Text="Active Directory Setting"></asp:Label>
                            </h5>
                            <div class="card-body">

                                <div class="row">
                                    <div class="mb-3 col-md-4">
                                        <label for="txtServiceAccountName" class="form-label">
                                            <asp:Label ID="Label1" runat="server" class="form-label" Text="Service AccountName" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtServiceAccountName" runat="server" class="form-control" required="required"></asp:TextBox>
                                    </div>
                                    <div class="mb-3 col-md-4">
                                        <label for="txtDomainPath" class="form-label">
                                            <asp:Label ID="Label2" runat="server" class="form-label" Text="Domain Path" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtDomainPath" runat="server" class="form-control" required="required"></asp:TextBox>
                                    </div>
                                    <div class="mb-3 col-md-4">
                                        <label for="txtDomainPath" class="form-label">
                                            <asp:Label ID="Label5" runat="server" class="form-label" Text="Account Password" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtAccountPass" runat="server" class="form-control"  TextMode="Password"></asp:TextBox>
                                    </div>
                                    <div class="mb-3 col-md-12">
                                        <label for="txtOUS" class="form-label">
                                            <asp:Label ID="Label3" runat="server" class="form-label" Text="OUS" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="textOUS" runat="server" class="form-control" required="required"></asp:TextBox>
                                    </div>

                                </div>

                            </div>

                        </div>
                        <div class="card-body">
                            <div class="d-flex align-items-start align-items-sm-center gap-4">



                                <div class="button-wrapper" style="width: 100%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 50%; text-align: left;"></td>
                                            <td style="width: 50%; text-align: right;">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary me-2 mb-4" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnAD" runat="server" Text="Get AD" class="btn btn-primary me-2 mb-4" OnClick="btnGetAD_Click" />
                                                <button type="button" class="btn btn-primary me-2 mb-4" data-toggle="modal" data-target="#modal-default">
                                                    Test Mail
                                                </button>
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" class="btn btn-outline-secondary account-image-reset mb-4" OnClick="btnReset_Click" />


                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="modal fade" id="modal-default">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title">Email ID</h4>
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>
                                            <div class="modal-body">
                                                <asp:TextBox ID="txtTestemail" runat="server"  CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="modal-footer justify-content-between">
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                                                <asp:Button ID="btnSend" runat="server" Text="Send" class="btn btn-primary" OnClick="btnSend_Click" />
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- overlayScrollbars -->
    <script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>

    <script src="plugins/select2/js/select2.full.min.js"></script>

    <!-- Bootstrap4 Duallistbox -->
    <script src="plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
    <!-- InputMask -->
    <script src="plugins/moment/moment.min.js"></script>
    <script src="plugins/inputmask/jquery.inputmask.min.js"></script>
    <!-- date-range-picker -->
    <script src="plugins/daterangepicker/daterangepicker.js"></script>
    <!-- bootstrap color picker -->
    <script src="plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <!-- Tempusdominus Bootstrap 4 -->
    <script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
    <!-- Bootstrap Switch -->
    <script src="plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
    <!-- BS-Stepper -->
    <script src="plugins/bs-stepper/js/bs-stepper.min.js"></script>
    <!-- dropzonejs -->
    <script src="plugins/dropzone/min/dropzone.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.js"></script>

    <!-- PAGE PLUGINS -->
    <!-- jQuery Mapael -->
    <script src="plugins/jquery-mousewheel/jquery.mousewheel.js"></script>
    <script src="plugins/raphael/raphael.min.js"></script>
    <script src="plugins/jquery-mapael/jquery.mapael.min.js"></script>
    <script src="plugins/jquery-mapael/maps/usa_states.min.js"></script>
    <!-- ChartJS -->
    <script src="plugins/chart.js/Chart.min.js"></script>

    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>
    <!-- AdminLTE dashboard demo (This is only for demo purposes) -->
    <script src="dist/js/pages/dashboard2.js"></script>

    <script>
        $(function () {
                                                                  //Initialize Select2 Elements
                                                                  $('.select2').select2()

                                                                  //Initialize Select2 Elements
                                                                  $('.select2bs4').select2({
                                                                      theme: 'bootstrap4'
                                                                  })

                                                                  //Datemask dd/mm/yyyy
                                                                  $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
                                                                  //Datemask2 mm/dd/yyyy
                                                                  $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
                                                                  //Money Euro
                                                                  $('[data-mask]').inputmask()

                                                                  //Date picker
                                                                  $('#reservationdate').datetimepicker({
                                                                      format: 'L'
                                                                  });

                                                                  //Date and time picker
                                                                  $('#reservationdatetime').datetimepicker({ icons: { time: 'far fa-clock' } });

                                                                  //Date range picker
                                                                  $('#reservation').daterangepicker()
                                                                  //Date range picker with time picker
                                                                  $('#reservationtime').daterangepicker({
                                                                      timePicker: true,
                                                                      timePickerIncrement: 30,
                                                                      locale: {
                                                                          format: 'MM/DD/YYYY hh:mm A'
                                                                      }
                                                                  })
                                                                  //Date range as a button
                                                                  $('#daterange-btn').daterangepicker(
                                                                      {
                                                                          ranges: {
                                                                              'Today': [moment(), moment()],
                                                                              'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                                                                              'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                                                                              'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                                                                              'This Month': [moment().startOf('month'), moment().endOf('month')],
                                                                              'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                                                                          },
                                                                          startDate: moment().subtract(29, 'days'),
                                                                          endDate: moment()
                                                                      },
                                                                      function (start, end) {
                                                                          $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
                                                                      }
                                                                  )

                                                                  //Timepicker
                                                                  $('#timepicker').datetimepicker({
                                                                      format: 'LT'
                                                                  })

                                                                  //Bootstrap Duallistbox
                                                                  $('.duallistbox').bootstrapDualListbox()

                                                                  //Colorpicker
                                                                  $('.my-colorpicker1').colorpicker()
                                                                  //color picker with addon
                                                                  $('.my-colorpicker2').colorpicker()

                                                                  $('.my-colorpicker2').on('colorpickerChange', function (event) {
                                                                      $('.my-colorpicker2 .fa-square').css('color', event.color.toString());
                                                                  })

                                                                  $("input[data-bootstrap-switch]").each(function () {
                                                                      $(this).bootstrapSwitch('state', $(this).prop('checked'));
                                                                  })

                                                              })
        // BS-Stepper Init
        document.addEventListener('DOMContentLoaded', function () {
                                                                  window.stepper = new Stepper(document.querySelector('.bs-stepper'))
                                                              })

        // DropzoneJS Demo Code Start
        Dropzone.autoDiscover = false

        // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
        var previewNode = document.querySelector("#template")
                                                          previewNode.id = ""
                                                          var previewTemplate = previewNode.parentNode.innerHTML
                                                          previewNode.parentNode.removeChild(previewNode)

                                                          var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
                                                              url: "/target-url", // Set the url
                                                              thumbnailWidth: 80,
                                                              thumbnailHeight: 80,
                                                              parallelUploads: 20,
                                                              previewTemplate: previewTemplate,
                                                              autoQueue: false, // Make sure the files aren't queued until manually added
                                                              previewsContainer: "#previews", // Define the container to display the previews
                                                              clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
                                                          })

                                                          myDropzone.on("addedfile", function (file) {
                                                              // Hookup the start button
                                                              file.previewElement.querySelector(".start").onclick = function () { myDropzone.enqueueFile(file) }
                                                          })

                                                          // Update the total progress bar
                                                          myDropzone.on("totaluploadprogress", function (progress) {
                                                              document.querySelector("#total-progress .progress-bar").style.width = progress + "%"
                                                          })

                                                          myDropzone.on("sending", function (file) {
                                                              // Show the total progress bar when upload starts
                                                              document.querySelector("#total-progress").style.opacity = "1"
                                                              // And disable the start button
                                                              file.previewElement.querySelector(".start").setAttribute("disabled", "disabled")
                                                          })

                                                          // Hide the total progress bar when nothing's uploading anymore
                                                          myDropzone.on("queuecomplete", function (progress) {
                                                              document.querySelector("#total-progress").style.opacity = "0"
                                                          })

                                                          // Setup the buttons for all transfers
                                                          // The "add files" button doesn't need to be setup because the config
                                                          // `clickable` has already been specified.
                                                          document.querySelector("#actions .start").onclick = function () {
                                                              myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED))
                                                          }
                                                          document.querySelector("#actions .cancel").onclick = function () {
                                                              myDropzone.removeAllFiles(true)
                                                          }
  // DropzoneJS Demo Code End
    </script>
</body>
</html>
