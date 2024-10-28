<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubVisitorMaster.aspx.cs" Inherits="SecuLobbyVMS.SubVisitorMaster" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/jpg" href="dist/img/fav.png" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->


    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <script src="dist/js/sweetalert2.all.min.js"></script>
    <link href="dist/css/sweetalert2.min.css" rel="stylesheet" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
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
    <script type="text/javascript">
        function showReport(val1) {

            var url = "ReportDisplayForm.aspx?Rpt_Name=VisitorMasterReport.xml&SearchText=" + val1;
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
                                <asp:Label ID="header1" runat="server" Text="Visitor Master Report"></asp:Label>
                            </h5>
                            <!-- Account -->
                            <div class="card-body" style="padding-bottom: 0px!important; padding-top: 10px!important; padding-left: 30px!important; padding-right: 60px!important;">
                                <div class="d-flex align-items-end align-items-sm-center gap-4">
                                    <div class="button-wrapper" style="width: 100%; height: 50px; text-align: right;">
                                        <a href="javascript:void();" id="A1" runat="server" class="btn btn-success me-2 mb-4">
                                            <asp:Label ID="btnPDF" runat="server" Text="PDF Export"></asp:Label>
                                        </a>
                                        
                                                  <%--<asp:Button ID="BtnExcel" runat="server" Text="Excel Expor" class="btn btn-info me-2 mb-4" OnClick="BtnExcel_Click" />--%>

                                    </div>
                                </div>
                            </div>
                            <hr class="my-0" />
                            <div class="card-body">
                                <div class="card-header">
                                    <h3 class="card-title">
                                        <asp:Label ID="lblVisDet" runat="server" Text="Visitor Details"></asp:Label></h3>

                                    <div class="card-tools">
                                        <div class="input-group input-group-sm" style="width: 300px; margin: 0!important; padding-bottom: 10px; padding-right: 30px;">

                                            <asp:TextBox ID="txtSearch" runat="server" class="form-control float-right" placeholder="Search" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="card-body">

                                        <%--                         <asp:GridView ID="grd1" runat="server" AutoGenerateColumns="false" OnRowDataBound="grd1_RowDataBound" class="table table-bordered table-hover">
                                        <Columns>

                                        </Columns>
                                      </asp:GridView>--%>

                                      <asp:Panel ID="pnlgrid" runat="server">
                                        <asp:Literal ID="ltrtable" runat="server"></asp:Literal>
                                        </asp:Panel>
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
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>

</body>
</html>
