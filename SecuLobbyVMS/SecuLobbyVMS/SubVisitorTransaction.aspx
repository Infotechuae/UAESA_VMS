<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubVisitorTransaction.aspx.cs" Inherits="SecuLobbyVMS.SubVisitorTransaction" EnableEventValidation="false" %>

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
   <style> .btn-primary, .btn-warning, .btn-success, .btn-danger,.btn-info {
            background-color: #b68a35 !important;
            border-color: #b68a35 !important;
            color: #FFF !important;
        }

  </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

      <script type="text/javascript">
        function showReport(val1,val2,val3,val4,val5) {

            var url = "ReportDisplayForm.aspx?Rpt_Name=VisitorTransacReport.xml&FromDate=" + val1 + "&ToDate=" + val2 + "&Search=" + val3 + "&UserGroup=" + val4 + "&HostName=" + val5;
            window.open(url, "ShowReport", "scrollbars=yes,resizable=no, border=0, width=900,height=600,left=220,top=150,toolbars=no,titlebar=no,directory=no,scrolling=yes,location=no,directories=no,status=no,menubar=no,copyhistory=no");

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
                                <asp:Label ID="header1" runat="server" Text="Visitor Transaction Report"></asp:Label>
                            </h5>
                            <!-- Account -->
                            <div class="card-body" style="padding-bottom: 0px!important; padding-top: 10px!important; padding-left: 30px!important;">
                                <div class="d-flex align-items-start align-items-sm-center gap-4">
                                    <div class="button-wrapper" style="width: 100%;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%; text-align: left;">

                                                    <label for="txtFromDate" class="form-label">
                                                        <asp:Label ID="lblFromDate" runat="server" class="form-label" Text="From Date" Font-Bold="true"></asp:Label>
                                                    </label>
                                                    <div class="input-group date" id="Fromdate" data-target-input="nearest">
                                                        <asp:TextBox ID="txtFromDate" runat="server" class="form-control datetimepicker-input" data-target="#Fromdate"></asp:TextBox>

                                                        <div class="input-group-append" data-target="#Fromdate" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                    </div>
                                                     <label for="txtToDate" class="form-label">
                                                        <asp:Label ID="lblToDate" runat="server" class="form-label" Text="To Date" Font-Bold="true"></asp:Label>
                                                    </label>
                                                    <div class="input-group date" id="Todate" data-target-input="nearest">
                                                        <asp:TextBox ID="txtToDate" runat="server" class="form-control datetimepicker-input" data-target="#Todate"></asp:TextBox>

                                                        <div class="input-group-append" data-target="#Todate" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                        </div>

                                                      
                                                    </div>


                                                </td>
                                                <td style="width: 50%; text-align: right;">
                                                  <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" class="btn btn-primary me-2 mb-4" OnClick="btnGenerate_Click" />
                                                    <a href="javascript:void();" id="A1" runat="server" class="btn btn-success me-2 mb-4">
                                                        <asp:Label ID="btnPDF" runat="server" Text="PDF Export"></asp:Label>
                                                    </a>
                                                    

                                                  <asp:Button ID="BtnExcel" runat="server" Text="Excel Expor" class="btn btn-info me-2 mb-4" OnClick="BtnExcel_Click" />
                                                </td>
                                            </tr>
                                        </table>


                                    </div>
                                </div>
                            </div>
                            <hr class="my-0" />
                            <div class="card-body">
                                <div class="card-header">
                                    <h3 class="card-title">
                                        <asp:Label ID="lblVisDet" runat="server" Text="Visitor Details"></asp:Label></h3>

                                    <div class="card-tools">
                                        <div class="input-group input-group-sm" style="width: 300px; margin: 0!important; padding-bottom: 10px; padding-right: 10px;">

                                            <asp:TextBox ID="txtSearch" runat="server" class="form-control float-right" placeholder="Search" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="card-body table-responsive p-0">
                                        <asp:GridView ID="grdVisDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-hover text-nowrap" GridLines="None">
                                            <Columns>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh1" runat="server" Font-Size="Small"  Text='<%# GetHeader("Visitor Name") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Name") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small" Text='<%# GetHeader("Visitor ID") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("EmiratesID") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh3" runat="server" Font-Size="Small" Text='<%# GetHeader("Mobile") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Mobile") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh4" runat="server" Font-Size="Small" Text='<%# GetHeader("Email ID") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Email") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh5" runat="server" Font-Size="Small" Text='<%# GetHeader("Company Name") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Company") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh6" runat="server" Font-Size="Small" Text="Location"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("LocationID") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh13" runat="server" Font-Size="Small"  Text='<%# GetHeader("Department") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Aptment_Dept") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                          

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh14" runat="server" Font-Size="Small" Text='<%# GetHeader("Visitor Type") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Visitor_Type") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh24" runat="server" Font-Size="Small"  Text='<%# GetHeader("Purpose") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Purpose") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh7" runat="server" Font-Size="Small" Text='<%# GetHeader("Host") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Host_to_Visit") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh8" runat="server" Font-Size="Small"  Text='<%# GetHeader("Check In") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Checkin_Time") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh15" runat="server" Font-Size="Small" Text='<%# GetHeader("Check Out") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("CheckOut_Time") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh9" runat="server" Font-Size="Small" Text='<%# GetHeader("Duration") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Duration") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
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
            $('#Fromdate').datetimepicker({
                format: 'L'
            });
            //Date picker
            $('#Todate').datetimepicker({
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
