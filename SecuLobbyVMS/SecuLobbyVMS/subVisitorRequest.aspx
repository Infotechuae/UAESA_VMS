<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subVisitorRequest.aspx.cs" Inherits="SecuLobbyVMS.subVisitorRequest" %>

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
   <style> .btn-primary, .btn-warning, .btn-success, .btn-danger {
            background-color: #b68a35 !important;
            border-color: #b68a35 !important;
            color: #FFF !important;
        }

  </style>
    <script type="text/javascript">
        function UploadFile(fileUpload) {
                if (fileUpload.value != '') {
                    document.getElementById("<%=btnUpload.ClientID %>").click();
                }
            }
        function selectFile() {
                $('#faUpload').click();


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
    <script language="JavaScript" type="text/javascript">
        function errorsalert(smessage) {

                swal.fire({
                    title: smessage, text: "", type: "success"
                }).then(function () {

                });
            }

    </script>
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
                                <asp:Label ID="header1" runat="server" Text="Requester Information"></asp:Label>
                            </h5>
                            <!-- Account -->

                            <hr class="my-0" />
                            <div class="card-body">

                                <div class="row">
                                    <div class="mb-3 col-md-2">
                                        <div class="row">
                                            <div class="mb-3 col-md-12 align-content-center">

                                                <asp:Image ID="img_PhotoBase64" runat="server" class="d-block rounded" Height="100" Width="100" /><br />
                                                <asp:Button ID="btnLoad" runat="server" Text="Upload Image" class="btn btn-primary me-2 mb-4" OnClientClick="selectFile(); return false;" />

                                                <asp:FileUpload ID="faUpload" runat="server" class="form-control-file" Style="color: transparent; font-size: small; width: 120px; display: none" />
                                                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" CausesValidation="false" Style="display: none" />
                                                <asp:HiddenField ID="hdnupload" runat="server" />
                                            </div>

                                        </div>
                                    </div>

                                    <div class="mb-3 col-md-10">
                                        <div class="row">
                                            <div class="mb-3 col-md-6">
                                                <label for="txtReqName" class="form-label">
                                                    <asp:Label ID="lblFullName" runat="server" class="form-label" Text="Requester Name" Font-Bold="true"></asp:Label>
                                                </label>

                                                <asp:TextBox ID="txtReqName" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                            <div class="mb-3 col-md-6">
                                                <label for="txtReqCompanyName" class="form-label">
                                                    <asp:Label ID="lblCompanyName" runat="server" class="form-label" Text="Company Name" Font-Bold="true"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="txtReqCompanyName" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="mb-3 col-md-6">
                                                <label for="txtReqEmail" class="form-label">
                                                    <asp:Label ID="lblEmail" runat="server" class="form-label" Text="Email" Font-Bold="true"></asp:Label>
                                                </label>
                                                <div class="input-group input-group-merge">
                                                    <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                                                    <asp:TextBox ID="txtReqEmail" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="mb-3 col-md-6">
                                                <label for="txtReqEmail" class="form-label">
                                                    <asp:Label ID="lblVisMobile" runat="server" class="form-label" Text="Phone" Font-Bold="true"></asp:Label>
                                                </label>
                                                <div class="input-group input-group-merge">
                                                    <span class="input-group-text"><i class="fas fa-mobile"></i></span>
                                                    <asp:TextBox ID="txtVisMobile" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <!-- /Account -->
                        </div>
                        <div class="card">
                            <h5 class="card-header">
                                <asp:Label ID="header2" runat="server" Text="Meeting Information"></asp:Label></h5>
                            <div class="card-body">

                                <div class="row">

                                    <div class="mb-3 col-md-4">
                                        <label for="txtMeetname" class="form-label">
                                            <asp:Label ID="lblMeetName" runat="server" class="form-label" Text="Meeting Name" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtMeetname" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="mb-3 col-md-4">
                                        <label for="txtLocation" class="form-label">
                                            <asp:Label ID="lblLocation" runat="server" class="form-label" Text="Location" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:TextBox ID="txtLocation" runat="server" class="form-control"></asp:TextBox>
                                    </div>

                                    <div class="mb-3 col-md-4">
                                        <label for="txtDate" class="form-label">
                                            <asp:Label ID="lblDate" runat="server" class="form-label" Text="Date" Font-Bold="true"></asp:Label>
                                        </label>
                                        <div class="input-group input-group-merge">
                                            <div class="input-group date" id="reservationdate" data-target-input="nearest">
                                                <asp:TextBox ID="txtDate" runat="server" class="form-control datetimepicker-input" data-target="#reservationdate"></asp:TextBox>

                                                <div class="input-group-append" data-target="#reservationdate" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="mb-3 col-md-4">
                                        <label for="txtTime" class="form-label">
                                            <asp:Label ID="lblTime" runat="server" class="form-label" Text="Time" Font-Bold="true"></asp:Label>
                                        </label>
                                        <div class="input-group date" id="timepicker" data-target-input="nearest">
                                            <asp:TextBox ID="txtTime" runat="server" class="form-control datetimepicker-input" data-target="#timepicker"></asp:TextBox>

                                            <div class="input-group-append" data-target="#timepicker" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="far fa-clock"></i></div>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="mb-3 col-md-4">
                                        <label for="drpDuration" class="form-label">
                                            <asp:Label ID="lblDuration" runat="server" class="form-label" Text="Duration" Font-Bold="true"></asp:Label>
                                        </label>
                                        <asp:DropDownList ID="drpDuration" runat="server" class="form-control select2" Style="width: 100%;">
                                        </asp:DropDownList>
                                    </div>


                                    <div class="mb-3 col-md-4">
                                        <label for="txtRemarks" class="form-label">
                                            <asp:Label ID="lblRemarks" runat="server" class="form-label" Text="Remarks" Font-Bold="true"></asp:Label>
                                        </label>
                                        <asp:TextBox ID="txtRemarks" runat="server" class="form-control"></asp:TextBox>
                                    </div>

                                </div>
                            </div>


                        </div>

                        <div class="card">
                            <h5 class="card-header">
                                <asp:Label ID="Label1" runat="server" Text="Host Information"></asp:Label></h5>
                            <div class="card-body">

                                <div class="row">

                                    <div class="mb-3 col-md-4">
                                        <label for="txtMeetname" class="form-label">
                                            <asp:Label ID="lblHostName" runat="server" class="form-label" Text="Host Name" Font-Bold="true"></asp:Label>
                                        </label>

                                        <asp:DropDownList ID="drpHostName" runat="server" class="form-control select2" AutoPostBack="true" Style="width: 100%;" OnSelectedIndexChanged="drpHostName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="mb-3 col-md-4">
                                        <label for="txtMeetname" class="form-label">
                                            <asp:Label ID="lblHostEmail" runat="server" class="form-label" Text="Host Email" Font-Bold="true"></asp:Label>
                                        </label>

                                        <div class="input-group input-group-merge">
                                            <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                                            <asp:TextBox ID="txtHostEmail" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="mb-3 col-md-4">
                                        <label for="txtMeetname" class="form-label">
                                            <asp:Label ID="Label2" runat="server" class="form-label" Text="Mobile" Font-Bold="true"></asp:Label>
                                        </label>

                                        <div class="input-group input-group-merge">
                                            <span class="input-group-text"><i class="fas fa-mobile"></i></span>
                                            <asp:TextBox ID="txtHostMob" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card-body">
                            <div class="d-flex align-items-start align-items-sm-center gap-4">

                            

                                <%--                                    <asp:Image ID="img_PhotoBase64" runat="server" class="d-block rounded" Height="100" Width="100" />
                                    &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <div class="button-wrapper" style="width: 100%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 50%; text-align: left;"></td>
                                            <td style="width: 50%; text-align: right;">
                                                <asp:Button ID="btnInvite" runat="server" Text="Send Request" class="btn btn-primary me-2 mb-4" OnClick="btnInvite_Click" />

                                                <asp:Button ID="btnReset" runat="server" Text="Reset" class="btn btn-outline-secondary account-image-reset mb-4" OnClick="btnReset_Click" />


                                            </td>
                                        </tr>
                                    </table>
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
