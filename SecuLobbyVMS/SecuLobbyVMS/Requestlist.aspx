<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="Requestlist.aspx.cs" Inherits="SecuLobbyVMS.Requestlist" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
      $(function () {

        setInterval(function () {
            console.clear();
            console.log(Array(100).join("\n".repeat('25')));
          }, 500);


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

        //Timepicker
        $('#timepicker').datetimepicker({
          format: 'LT'
        })
      })
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
        function CheckAll() {
            var chkAll = document.getElementById("ContentPlaceHolder1_grdDetails_checkAll");
            var i = 0;

            while (1) {

                var chkSelect = '';
                chkSelect = document.getElementById("ContentPlaceHolder1_grdDetails_check_" + i);

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
    <style>
        .btn-primary, .btn-warning, .btn-success, .btn-danger {
            background-color: #b68a35 !important;
            border-color: #b68a35 !important;
            color: #FFF !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper">
        <div class="content-wrapper">
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
                                    <asp:Label ID="header1" runat="server"></asp:Label>
                                </h5>
                                <!-- Account -->
                                <div class="card-body" style="padding-bottom: 0px!important; padding-top: 10px!important; padding-left: 30px!important;">
                                    <div class="d-flex align-items-start align-items-sm-center gap-4">

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
        
           <asp:Button ID="Search" runat="server" Text="Search" class="btn btn-primary me-2 mb-4" OnClick="Search_Click" />
         </td>
     </tr>
 </table>


                                
                                    </div>
                                </div>


                                <hr class="my-0" />
                                <div class="card-body">
                                    
                                    <div class="card-header">

                                       <asp:Button ID="btnApproved" runat="server" Text="Approved" class="btn btn-primary me-2 mb-4" OnClick="btnApproved_Click" CausesValidation="false" OnClientClick="mywaitdialog()"  />

 <asp:Button ID="btnReject" runat="server" Text="Reject" class="btn btn-outline-secondary account-image-reset mb-4" OnClick="btnReject_Click" CausesValidation="false" OnClientClick="mywaitdialog()" />

                                        <div class="card-tools">
                                                                      
     

                                            <div class="input-group input-group-sm" style="width: 300px; margin: 0!important; padding-bottom: 10px; padding-right: 10px;">

                                                <asp:TextBox ID="txtSearch" runat="server" class="form-control float-right" placeholder="Search" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="card-body table-responsive p-0">
                                            <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-hover text-nowrap" GridLines="None">
                                                <Columns>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="checkAll" runat="server" onclick="javascript: CheckAll();" /></td>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="check" runat="server" Font-Size="Small" Width="20px" />
                                                        </ItemTemplate>



                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">

                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" runat="server" Text='<%# Eval("Ref_No") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblHostEmail" runat="server" Text='<%# Eval("HostEmail") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblPurpose" runat="server" Text='<%# Eval("Purpose") %>' Visible="false"></asp:Label>
                                                             <asp:Label ID="lblVisitorID" runat="server" Text='<%# Eval("Visitor_ID") %>' Visible="false"></asp:Label>


                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh1" runat="server" Font-Size="Small" Width="60px" Text="Visitor Name"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh2" runat="server" Font-Size="Small" Width="30px" Text="Visitor ID"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lblEmiratesID" runat="server" Text='<%# Eval("EmiratesID") %>'></asp:Label>
                                                            <asp:Label ID="lblEmiratesIDType" runat="server" Text='<%# Eval("Doc_type") %>' Visible="false"></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh3" runat="server" Font-Size="Small" Width="30px" Text="Mobile"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("Mobile") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh4" runat="server" Font-Size="Small" Width="30px" Text="Email ID"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>appr
                                                            <asp:Label ID="lblh5" runat="server" Font-Size="Small" Width="50px" Text="Company"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh6" runat="server" Font-Size="Small" Width="40px" Text="Location"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lblLocationID" runat="server" Text='<%# Eval("LocationID") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh13" runat="server" Font-Size="Small" Width="30px" Text="Department"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFloor" runat="server" Text='<%# Eval("Area_Floor") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Aptment_Dept") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh14" runat="server" Font-Size="Small" Width="30px" Text="Visitor Type"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lblVisitorType" runat="server" Text='<%# Eval("Visitor_Type") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh24" runat="server" Font-Size="Small" Width="30px" Text="Purpose"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("Purpose") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh7" runat="server" Font-Size="Small" Width="30px" Text="Host"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHostName" runat="server" Text='<%# Eval("Host") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh8" runat="server" Font-Size="Small" Width="30px" Text="Date"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("Checkin_Time","{0:dd/MM/yyyy}") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh9" runat="server" Font-Size="Small" Width="30px" Text="Duration"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("Duration") %></span>
                                                           <asp:Label ID="lblDurID" runat="server" Text='<%# Eval("DurID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh15" runat="server" Font-Size="Small" Width="30px" Text="Status"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("Req_Stat") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>




                                                    <asp:TemplateField>
                                                       <HeaderTemplate>
                                                         <asp:Label ID="lblh16" runat="server" Font-Size="Small" Width="30px" Text="View"></asp:Label>
                                                          </HeaderTemplate>
                                                         <ItemTemplate>


                                                            <asp:HyperLink runat="server" ToolTip="Edit" ImageUrl="~/dist/img/Smalledit.png" Height="16px" Width="16px" Target="_blank"
                                                                NavigateUrl='<%# "SelfRegistrationApproval.aspx?tabid="+ Eval("Ref_No") +"&eid="+Eval("HostEmail")%>'
                                                                ID="hplProdId"></asp:HyperLink>


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="text" Wrap="false" Width="80px" VerticalAlign="Middle" />
                                                        <HeaderStyle CssClass="head" />
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
        </div>
    </div>

    <script>
        function mywaitdialog() {
            var mywait = document.getElementById("mywaitmsg")
            mywait.style.display = 'block';

            //var MainDiv = document.getElementById("divmain")
            //MainDiv.setAttribute('style', 'background-color: Gray; filter: alpha(opacity=80); opacity: 0.8; z-index: 10000');
        }
    </script>
</asp:Content>
