<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubCheckout.aspx.cs" Inherits="SecuLobbyVMS.SubCheckout" %>

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
    <script language="JavaScript" type="text/javascript">
        function CheckAll() {
            var chkAll = document.getElementById("grdVisDetails_checkAll");
            var i = 0;

            while (1) {

                var chkSelect = '';
                chkSelect = document.getElementById("grdVisDetails_check_" + i);

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
   <style> .btn-primary, .btn-warning, .btn-success, .btn-danger {
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
</head>
<body>
    <form id="form1" runat="server">
      <div id="mywaitmsg" style="display: none; width: 300px">
            <%-- <h3>Loading..</h3>--%>
            <div style="text-align: center">
                <img src="dist/img/Loader.gif" style="position: fixed; z-index: 9999; height: 64px; left: 50%; top: 175px; z-index: 9999; width: 70px;" />
            </div>
        </div>
        <section class="content">
            <div class="container-fluid">
                <!-- Info boxes -->
                <div class="row">
                    <div class="col-md-12">

                        <div class="card">
                            <h5 class="card-header">
                                <asp:Label ID="header1" runat="server" Text="Check Out"></asp:Label>
                            </h5>
                            <!-- Account -->
                            <div class="card-body" style="padding-bottom: 0px!important; padding-top: 10px!important; padding-left: 30px!important;">
                                <div class="d-flex align-items-start align-items-sm-center gap-4">
                                    <div class="button-wrapper" style="width: 100%; height: 50px;">
                                        <asp:Button ID="btncheckOut" runat="server" Text="Check Out" class="btn btn-danger me-2 mb-4" OnClick="btncheckOut_Click" CausesValidation="false" OnClientClick="mywaitdialog()" />

                                        <button type="button" class="btn btn-warning me-2 mb-4" data-toggle="modal" data-target="#modal-default">
                                            <asp:Label ID="lblwach" runat="server" Text="Watchlist"></asp:Label>
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <div class="modal fade" id="modal-default">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title"> <asp:Label ID="lblReas" runat="server" Text="Reason"></asp:Label></h4>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                            <asp:TextBox ID="txtWatchlistReason" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="modal-footer justify-content-between">
                                            <button type="button" class="btn btn-default" data-dismiss="modal"><asp:Label ID="lblClose" runat="server" Text="Close"></asp:Label></button>

                                            <asp:Button ID="btnWatchlist" runat="server" Text="Save" class="btn btn-primary" OnClick="btnWatchlist_Click" />
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
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
                                                        <asp:CheckBox ID="checkAll" runat="server" onclick="javascript: CheckAll();" /></td>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="check" runat="server" Font-Size="Small" Width="20px" />
                                                    </ItemTemplate>



                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Ref No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRefNo" runat="server" Font-Size="Small" Text='<%#Eval("Ref_No") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Visitor ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVisitorID" runat="server" Font-Size="Small"  Text='<%#Eval("Visitor_ID") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                               <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh1" runat="server" Font-Size="Small"  Text='<%# GetHeader("Visitor ID") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                 <ItemTemplate>
                                                        <span   ><%# Eval("EmiratesID") %></span>
                                                    </ItemTemplate>
                                                  
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small"  Text='<%# GetHeader("Visitor Name") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span   ><%# Eval("Name") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh3" runat="server" Font-Size="Small"  Text='<%# GetHeader("Mobile") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Mobile") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh4" runat="server" Font-Size="Small"  Text='<%# GetHeader("Email ID") %>'></asp:Label>

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

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh6" runat="server" Font-Size="Small"  Text='<%# GetHeader("Visitor Type") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Visitor_Type") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                              
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh7" runat="server" Font-Size="Small"  Text='<%# GetHeader("Department") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Aptment_Dept") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh8" runat="server" Font-Size="Small" Text='<%# GetHeader("Host") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Host_to_Visit") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh9" runat="server" Font-Size="Small" Text='<%# GetHeader("Check In") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Checkin_Time") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh10" runat="server" Font-Size="Small" Text="Card" ></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Access_Card_No") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                               <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh11" runat="server" Font-Size="Small" Text='<%# GetHeader("Purpose") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("Purpose") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh12" runat="server" Font-Size="Small" Text='<%# GetHeader("Duration") %>'></asp:Label>

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
        function mywaitdialog() {
            var mywait = document.getElementById("mywaitmsg")
            mywait.style.display = 'block';

            //var MainDiv = document.getElementById("divmain")
            //MainDiv.setAttribute('style', 'background-color: Gray; filter: alpha(opacity=80); opacity: 0.8; z-index: 10000');
        }
    </script>
</body>
</html>
