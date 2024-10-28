<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="Userlists.aspx.cs" Inherits="SecuLobbyVMS.Userlists" EnableEventValidation="false" %>

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
                                    <asp:Label ID="header1" runat="server" Text="User"></asp:Label>
                                </h5>
                                <!-- Account -->
                                <div class="card-body" style="padding-bottom: 0px!important; padding-top: 10px!important; padding-left: 30px!important;">
                                    <div class="d-flex align-items-start align-items-sm-center gap-4">
                                        <%--  <div class="button-wrapper" style="width: 100%; height: 50px;">
                                        

                                    </div>--%>
                                    </div>
                                </div>


                                <hr class="my-0" />
                                <div class="card-body">
                                    <div class="card-header">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add New" class="btn btn-primary me-2 mb-4" OnClick="btnAdd_Click" CausesValidation="false" />


                                        <div class="card-tools">
                                            <div class="input-group input-group-sm" style="width: 300px; margin: 0!important; padding-bottom: 10px; padding-right: 10px;">

                                                <asp:TextBox ID="txtSearch" runat="server" class="form-control float-right" placeholder="Search" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="card-body table-responsive p-0">
                                            <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-hover text-nowrap" GridLines="None" OnRowDeleting="grdDetails_RowDeleting">
                                                <Columns>

                                                    <asp:TemplateField Visible="false">

                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" runat="server" Text='<%# Eval("UserID") %>' Visible="false"></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh1" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("User ID") %>' ></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("UserCode") %></span>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh2" runat="server" Font-Size="Small" Width="20px"  Text='<%# GetHeader("Name") %>'  ></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("UserName") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh3" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("Email") %>'></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("UserEmail") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                  <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh4" runat="server" Font-Size="Small" Width="10px" Text='<%# GetHeader("Location") %>'></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("Location") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh5" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("Phone") %>'></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("Phone") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblh6" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("User Type") %>'></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span><%# Eval("UserType") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField >
                                                       <HeaderTemplate>
                                                            <asp:Label ID="lblh7" runat="server" Font-Size="Small" Width="20px" Text='<%# GetHeader("Edit") %>'></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>


                                                            <asp:HyperLink runat="server" ToolTip="Edit" ImageUrl="~/dist/img/Smalledit.png" Height="16px" Width="16px"
                                                                NavigateUrl='<%# "User.aspx?ID="+ Eval("UserID")%>'
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
                </div>
            </section>
        </div>
    </div>


</asp:Content>