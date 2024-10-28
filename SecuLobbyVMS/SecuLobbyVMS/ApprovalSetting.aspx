<%@ Page Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="ApprovalSetting.aspx.cs" Inherits="SecuLobbyVMS.ApprovalSetting" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper">
        <div class="content-wrapper">

            <section class="content">
                <div class="container-fluid">
                    <!-- Info boxes -->
                    <div class="row">
                        <div class="col-md-12">

                            <div class="card">
                                <h5 class="card-header">
                                    <asp:Label ID="header1" runat="server" Text="Approval Settings"></asp:Label>
                                </h5>
                                <!-- Account -->
                                <div class="card-body" style="padding-bottom: 0px!important; padding-top: 10px!important; padding-left: 30px!important;">
                                    <div class="d-flex align-items-start align-items-sm-center gap-4">
                                        <%--  <div class="button-wrapper" style="width: 100%; height: 50px;"></div>--%>
                                    </div>
                                </div>
                              <div class="card-body">
                                    <div class="card-header">
                                        <%--  <asp:Button ID="btnAdd" runat="server" Text="Add New" class="btn btn-primary me-2 mb-4" OnClick="btnAdd_Click" CausesValidation="false" />--%>


                                        <div class="card-tools">
                                            <div class="input-group input-group-sm" style="width: 300px; margin: 0!important; padding-bottom: 10px; padding-right: 10px;">
                                            </div>
                                        </div>
                                        <div class="card-body table-responsive p-0">
                                            <table class="table table-bordered">
                                                <tr>
                                                    <td></td>
                                                    <td>Visitor Type</td>
                                                    <td>Approval Required</td>
                                                    <td>Host</td>
                                                    <td>Line Manager</td>
                                                    <td>Service Level</td>
                                                </tr>
                                                <tr>
                                                    <%--   <td>Check-In</td>--%>
                                                    <td>
                                                        <asp:DropDownList ID="dd1" runat="server" class="form-control select2" Style="width: 100%;">

                                                            <asp:ListItem Enabled="true" Text="Select" Value="-1"></asp:ListItem>

                                                            <asp:ListItem Text="Check-In" Value="4"></asp:ListItem>

                                                            <asp:ListItem Text="Check-Out" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Visitor Invite" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="Pre-Registration" Value="26"></asp:ListItem>
                                                        </asp:DropDownList></td>
                                                    <td>
                                                        <div class="form-check">
                                                            <asp:DropDownList ID="drpVisitorType" runat="server" class="form-control select2" Style="width: 100%;">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>

                                                    <td>
                                                        <div class="form-check">
                                                            <asp:CheckBox ID="checkin_A" runat="server" AutoPostBack="True" TextAlign="Right" OnCheckedChanged="Check_Clicked" />
                                                            <%-- <input class="form-check-input" runat="server" type="checkbox" value="" id="checkin_A" onclick="javascript: CheckApproved('C');">--%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-check">
                                                            <input class="form-check-input checkin" runat="server" type="checkbox" disabled value="" id="checkin_H">
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-check">
                                                            <input class="form-check-input checkin" runat="server" type="checkbox" disabled value="" id="checkin_L">
                                                        </div>
                                                    </td>
                                                    <td>

                                                        <asp:TextBox ID="txtID" runat="server" class="form-control" EnableViewState="false"></asp:TextBox>
                                                    </td>
                                                </tr>


                                            </table>
                                        </div>
                                        <div class="mb-3 col-md-12 mt-3" style="text-align: right;">
                                            <asp:Button ID="btnSaveSettings" runat="server" Text="Save Settings" class="btn btn-primary me-2 mb-4" OnClick="btnSaveSettings_Click" />
                                            <%-- <input type="submit" runat="server" text="Save Settings" onclick="btnSaveSettings_Click" id="btnCheckin" class="btn btn-success me-2 mb-4">--%>
                                        </div>

                                    </div>
                                </div>
                                 <hr class="my-0" />

                                <div class="card-body table-responsive p-0">
                                    <div class="card-header">
                                        <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-hover text-nowrap" GridLines="None" OnRowDeleting="grdDetails_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh1" runat="server" Font-Size="Small" Text="ID"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("ID") %></span>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small" Text="Menu"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("MenuName") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small" Text="Visitor Type"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("VisitorType") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small" Text="Host Approval"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("HostApproval") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small" Text="Line Manager Approval"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("LineManagerApproval") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh2" runat="server" Font-Size="Small" Text="Service Level"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span><%# Eval("ServiceLevel") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                   <HeaderTemplate>
                                                        <asp:Label ID="lblh80" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("Edit") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>


                                                        <asp:HyperLink runat="server" ToolTip="Edit" ImageUrl="~/dist/img/Smalledit.png" Height="16px" Width="16px"
                                                            NavigateUrl='<%# "ApprovalSetting.aspx?ID="+ Eval("ID")%>'
                                                            ID="hplProdId"></asp:HyperLink>


                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text" Wrap="false" Width="80px" VerticalAlign="Middle" />
                                                    <HeaderStyle CssClass="head" />
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblh8" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("Delete") %>'></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>


                                                        <asp:ImageButton ID="btndel" runat="server" CausesValidation="false"
                                                            ImageUrl="~/dist/img/jstaclkcancel.png" CommandName="Delete"
                                                            OnClientClick="return confirm('Record will be Deleted. Are you sure you want to delete!!');" />
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
            </section>
        </div>
    </div>


</asp:Content>
