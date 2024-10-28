<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="SecuLobbyVMS.User" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" EnableViewState="false">
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
                                    <asp:Label ID="header1" runat="server" Text="User"></asp:Label>
                                </h5>
                                <!-- Account -->

                                <hr class="my-0" />
                                <div class="card-body">


                                    <div class="row">
                                        <div class="mb-3 col-md-6">
                                            <label for="txtID" class="form-label">
                                                <asp:Label ID="Label4" runat="server" class="form-label" Text="User Id" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <asp:TextBox ID="txtID" runat="server" class="form-control" EnableViewState="false"></asp:TextBox>
                                                <asp:Button ID="Button1" runat="server" Text="Search" class="btn btn-primary me-2 mb-4" OnClick="btnusersearch_Click" />
                                            </div>

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
                                        <div class="mb-3 col-md-6">
                                            <label for="txtPassword" class="form-label">
                                                <asp:Label ID="Label1" runat="server" class="form-label" Text="Password" Font-Bold="true"></asp:Label>
                                            </label>

                                            <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" EnableViewState="false"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="Regex2" runat="server" ControlToValidate="txtPassword"
                                                ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"
                                                ErrorMessage="Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character" ForeColor="Red" />

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
                                        <div class="mb-3 col-md-6" id="dibPhone" runat="server">
                                            <label for="txtPhone" class="form-label">
                                                <asp:Label ID="lblPhone" runat="server" class="form-label" Text="Phone" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <span class="input-group-text"><i class="fas fa-phone"></i></span>
                                                <asp:TextBox ID="txtPhone" runat="server" class="form-control" TextMode="Phone"></asp:TextBox>
                                            </div>

                                        </div>


                                    </div>
                                    <div class="row">
                                        <div class="mb-3 col-md-6" id="Div1" runat="server">
                                            <label for="txtPhone" class="form-label">
                                                <asp:Label ID="Label2" runat="server" class="form-label" Text="User Type" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <asp:DropDownList ID="drpType" runat="server" class="form-control select2" Style="width: 100%;">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="mb-3 col-md-6" id="Div2" runat="server">
                                            <label for="txtPhone" class="form-label">
                                                <asp:Label ID="Label3" runat="server" class="form-label" Text="Location" Font-Bold="true"></asp:Label>
                                            </label>
                                            <div class="input-group input-group-merge">
                                                <asp:DropDownList ID="drpLoc" runat="server" class="form-control select2" Style="width: 100%;">
                                                </asp:DropDownList>
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
