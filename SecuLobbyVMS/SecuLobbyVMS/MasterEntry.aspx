<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="MasterEntry.aspx.cs" Inherits="SecuLobbyVMS.MasterEntry" %>

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
    <%-- <form id="form2" runat="server">--%>
    <div class="wrapper">
        <div class="content-wrapper">

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

                                <hr class="my-0" />
                                <div class="card-body">

                                    <div class="row">



                                        <div class="mb-3 col-md-6">
                                            <label for="txtID" class="form-label">
                                                <asp:Label ID="lblID" runat="server" class="form-label" Text="ID" Font-Bold="true"></asp:Label>
                                            </label>

                                            <asp:TextBox ID="txtID" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="mb-3 col-md-6">
                                            <label for="txtName" class="form-label">
                                                <asp:Label ID="lblName" runat="server" class="form-label" Text="Name" Font-Bold="true"></asp:Label>
                                            </label>

                                            <asp:TextBox ID="txtName" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                  
                                    <div class="row">
                                        <div class="mb-3 col-md-6" id="divEmail" runat="server">
                                            <label for="txtemail" class="form-label">
                                                <asp:Label ID="lblEmail" runat="server" class="form-label" Text="Email" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                                                <asp:TextBox ID="txtemail" runat="server" class="form-control" TextMode="Email"></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="mb-3 col-md-6" id="divDepartment" runat="server">
                                            <label for="drpDepartment" class="form-label">
                                                <asp:Label ID="lblDepartent" runat="server" class="form-label" Text="Department" Font-Bold="true"></asp:Label>
                                            </label>
                                            <asp:DropDownList ID="drpDepartment" runat="server" class="form-control select2" Style="width: 100%;">
                                            </asp:DropDownList>

                                        </div>


                                    </div>

                                   <div class="row">
                                        <div class="mb-3 col-md-6" id="divManagerName" runat="server">
                                            <label for="txtManName" class="form-label">
                                                <asp:Label ID="lblManName" runat="server" class="form-label" Text="Manager name" Font-Bold="true"></asp:Label>
                                            </label>

                                            <asp:TextBox ID="txtManName" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                  
                                    <div class="row">
                                        <div class="mb-3 col-md-6" id="divManagerEmail" runat="server">
                                            <label for="txtManEmail" class="form-label">
                                                <asp:Label ID="lblManEmail" runat="server" class="form-label" Text="Manager Email" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                                                <asp:TextBox ID="txtManEmail" runat="server" class="form-control" TextMode="Email"></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>

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
    <%--</form>--%>
</asp:Content>
