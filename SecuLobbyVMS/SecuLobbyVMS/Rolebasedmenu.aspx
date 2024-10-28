<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="Rolebasedmenu.aspx.cs" Inherits="SecuLobbyVMS.Rolebasedmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                                    <asp:Label ID="header1" runat="server" Text="Role"></asp:Label>
                                </h5>
                                <!-- Account -->

                                <hr class="my-0" />
                                <div class="card-body">


                                    <div class="row">
                                        <div class="mb-3 col-md-6">
                                            <label for="txtID" class="form-label">
                                                <asp:Label ID="Label4" runat="server" class="form-label" Text="User Type Name" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <asp:TextBox ID="txtID" runat="server" class="form-control" EnableViewState="false"></asp:TextBox>
                                             <%--   <asp:Button ID="Button1" runat="server" Text="Search" class="btn btn-primary me-2 mb-4" OnClick="btnusersearch_Click" />--%>
                                            </div>

                                        </div>
                                    </div>
              
                                      <asp:Panel ID="Panel1" runat="server">
                                        <br />
                                   <%--  <asp:CheckBox id="CheckAll" runat="server"  AutoPostBack="True"  Text="Select All"  TextAlign="Right"
                    OnCheckedChanged="Check_Clicked"/> 
                                          <br />--%>

                                      <br /></asp:Panel>
     
                                </div>
                                <!-- /Account -->
                            </div>


                            <div class="card-body">
                                <div class="d-flex align-items-start align-items-sm-center gap-4">



                                    <div class="button-wrapper" style="width: 100%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%; text-align: left;"></td>
                                                <td style="width: 50%; text-align: right;">
                                             <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary me-2 mb-4" OnClick="btnSave_Click" />

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
        </div>
    </div>
</asp:Content>
