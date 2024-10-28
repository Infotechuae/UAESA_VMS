<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="EmailTemplate.aspx.cs" ValidateRequest="false" Inherits="SecuLobbyVMS.EmailTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/summernote/0.8.18/summernote-bs4.min.css" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/summernote/0.8.18/summernote-bs4.min.js"></script>

    <script type="text/javascript">
        function populateSummernote() {
            var hiddenValue = document.getElementById('<%= hdnSummernote.ClientID %>').value;
            $('#summernote').val(hiddenValue);
        }

        function copySummernoteContent() {
            var summernoteContent = $('#summernote').val();
            document.getElementById('<%= hdnSummernote.ClientID %>').value = summernoteContent;
        }

        $(document).ready(function () {
            populateSummernote();
        });
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper">
        <div class="content-wrapper">
            <section class="content">

                <div class="row">
                    <div class="col-md-12">
                        <label for="drpEmailFor" class="form-label">
                            <asp:Label ID="lblEmailFor" runat="server" class="form-label" Text="Email Type" Font-Bold="true"></asp:Label>
                        </label>
                        <asp:DropDownList ID="drpEmailType" runat="server" class="form-control select2" Style="width: 100%;" AutoPostBack="true" OnSelectedIndexChanged="drpEmailType_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                            <asp:ListItem Text="Check In" Value="Check In"></asp:ListItem>
                            <asp:ListItem Text="Check Out" Value="Check Out"></asp:ListItem>
                            <asp:ListItem Text="Visitor Invite" Value="Visitor Invite"></asp:ListItem>
                            <asp:ListItem Text="Pre Registration" Value="Visitor Invite"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <label for="drpEmailFor" class="form-label">
                            <asp:Label ID="Label3" runat="server" class="form-label" Text="Email For" Font-Bold="true"></asp:Label>
                        </label>
                        <asp:DropDownList ID="drpEmailFor" runat="server" class="form-control select2" Style="width: 100%;" AutoPostBack="true" OnSelectedIndexChanged="drpEmailFor_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                            <asp:ListItem Text="Request Without Approval" Value="Request Without Approval"></asp:ListItem>
                            <asp:ListItem Text="Request With Approval" Value="Request With Approval"></asp:ListItem>
                            <asp:ListItem Text="Approval" Value="Approval"></asp:ListItem>
                            <asp:ListItem Text="Rejection" Value="Rejection"></asp:ListItem>
                        
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-12">
                    <label for="drpEmailFor" class="form-label">
                        <asp:Label ID="Label2" runat="server" class="form-label" Text="Email Stage" Font-Bold="true"></asp:Label>
                    </label>
                    <asp:DropDownList ID="drpEmailStage" runat="server" class="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpEmailStage_SelectedIndexChanged" Style="width: 100%;">
                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                        <asp:ListItem Text="Requester" Value="Request"></asp:ListItem>
                        <asp:ListItem Text="Host" Value="Host"></asp:ListItem>
                   
                        <asp:ListItem Text="Line manager" Value="Line manager"></asp:ListItem>
                      
                        <asp:ListItem Text="Service level" Value="Service level"></asp:ListItem>
                        
                    </asp:DropDownList>
                </div>


                <div class="row">
                    <div class="col-md-12">
                        <label for="txtEmailSubject" class="form-label">
                            <asp:Label ID="lblSubject" runat="server" class="form-label" Text="Email Subject" Font-Bold="true"></asp:Label>
                        </label>
                        <asp:TextBox ID="txtEmailSubject" runat="server" class="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">


                        <label for="txtEmailSubject" class="form-label">
                            <asp:Label ID="Label1" runat="server" class="form-label" Text="Email Body" Font-Bold="true"></asp:Label>
                        </label>

                        <textarea id="summernote" style="height: 450px!important;">        
                        </textarea>
                        <asp:HiddenField ID="hdnSummernote" runat="server" />
                        <asp:HiddenField ID="hdnID" runat="server" />

                    </div>
                    <!-- /.col-->
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="card-body" style="text-align: right;">

                            <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" Text="Save" OnClientClick="copySummernoteContent()" OnClick="btnsave_Click" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

</asp:Content>
