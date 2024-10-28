<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSchedule.aspx.cs" Inherits="SecuLobbyVMS.frmSchedule" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UAESA | VMS</title>
 <link rel="shortcut icon" type="image/jpg" href="dist/img/favicon.ico" />
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

    <script type="text/javascript">
        // <![CDATA[
        function OnMenuItemClick(s, e) {
            e.handled = true;
            switch (e.itemName) {
                case ASPx.SchedulerMenuItemId.NewAppointment:
                    NewAppointment(scheduler);
                    break;
                case ASPx.SchedulerMenuItemId.NewRecurringAppointment:
                    NewRecurringAppointment(scheduler);
                    break;
                case ASPx.SchedulerMenuItemId.NewAllDayEvent:
                    NewAllDayEvent(scheduler);
                    break;
                case ASPx.SchedulerMenuItemId.NewRecurringEvent:
                    NewRecurringEvent(scheduler);
                    break;
                case ASPx.SchedulerMenuItemId.OpenAppointment:
                    OpenAppointment(scheduler);
                    break;
                case ASPx.SchedulerMenuItemId.EditSeries:
                    EditSeries(scheduler);
                    break;
                default:
                    e.handled = false;
            }
        }
        function OpenAppointment(scheduler) {
            var apt = GetSelectedAppointment(scheduler);
            scheduler.RefreshClientAppointmentProperties(apt, AppointmentPropertyNames.Normal, OnAppointmentRefresh);
        }
        function EditSeries(scheduler) {
            var apt = GetSelectedAppointment(scheduler);
            scheduler.RefreshClientAppointmentProperties(apt, AppointmentPropertyNames.Pattern, OnAppointmentEditSeriesRefresh);
        }
        function OnAppointmentRefresh(apt) {
            ShowAppointmentForm(apt);
        }
        function OnAppointmentEditSeriesRefresh(apt) {
            if (apt.GetRecurrencePattern()) {
                ShowAppointmentForm(apt.GetRecurrencePattern());
            }
        }
        function NewAppointment(scheduler) {
            var apt = CreateAppointment(scheduler)
            ShowAppointmentForm(apt);
        }
        function NewRecurringAppointment(scheduler) {
            var apt = CreateRecurringAppointment(scheduler);
            ShowAppointmentForm(apt);
        }
        function NewRecurringEvent(scheduler) {
            var apt = CreateRecurringEvent(scheduler);
            ShowAppointmentForm(apt);
        }
        function NewAllDayEvent(scheduler) {
            var apt = CreateAllDayEvent(scheduler);
            ShowAppointmentForm(apt);
        }
        function ShowAppointmentForm(apt) {
            MyScriptForm.Clear();
            MyScriptForm.Update(apt);
            if (apt.GetSubject() != "")
                myFormPopup.SetHeaderText(apt.GetSubject() + " - Appointment");
            else
                myFormPopup.SetHeaderText("Untitled - Appointment");
            myFormPopup.Show();
        }
        function CloseAppointmentForm() {
            myFormPopup.Hide();
        }
        function CreateAppointment(scheduler) {
            var apt = new ASPxClientAppointment();
            var selectedInterval = scheduler.GetSelectedInterval();
            apt.SetStart(selectedInterval.GetStart());
            apt.SetEnd(selectedInterval.GetEnd());
            apt.AddResource(scheduler.GetSelectedResource());
            apt.SetLabelId(0);
            apt.SetStatusId(0);
            return apt;
        }
        function CreateRecurringAppointment(scheduler) {
            var apt = CreateAppointment(scheduler);
            apt.SetAppointmentType(ASPxAppointmentType.Pattern);
            var recurrenceInfo = new ASPxClientRecurrenceInfo();
            recurrenceInfo.SetStart(apt.GetStart());
            recurrenceInfo.SetEnd(apt.GetEnd());
            apt.SetRecurrenceInfo(recurrenceInfo);
            return apt;
        }
        function CreateAllDayEvent(scheduler) {
            var apt = CreateAppointment(scheduler);
            var start = apt.interval.start;
            var today = new Date(start.getFullYear(), start.getMonth(), start.getDate());
            apt.SetStart(today);
            apt.SetDuration(24 * 60 * 60 * 1000);
            apt.SetAllDay(true);
            return apt;
        }
        function CreateRecurringEvent(scheduler) {
            var apt = CreateAllDayEvent(scheduler);
            apt.SetAppointmentType(ASPxAppointmentType.Pattern);
            var recurrenceInfo = new ASPxClientRecurrenceInfo();
            recurrenceInfo.SetStart(apt.GetStart());
            recurrenceInfo.SetEnd(apt.GetEnd());
            apt.SetRecurrenceInfo(recurrenceInfo);
            return apt;
        }
        function GetSelectedAppointment(scheduler) {
            var aptIds = scheduler.GetSelectedAppointmentIds();
            if (aptIds.length == 0)
                return;
            var apt = scheduler.GetAppointment(aptIds[0]);
            return apt;
        }
    // ]]>
    </script>

    <style type="text/css">
        .Print {
            width: 20px !important;
            height: 20px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">


        <div class="container-fluid">
            <!-- Info boxes -->
            <div class="row">
                <div class="col-md-12">

                    <div class="card">
                        <h5 class="card-header">
                            <asp:Label ID="header1" runat="server" Text="Appointment"></asp:Label>
                        </h5>
                        <!-- Account -->

                        <hr class="my-0" />
                        <div class="card-body">
                            <div class="card-header">


                                <div class="card-tools">
                                    <div class="input-group input-group-sm" style="width: 300px; margin: 0!important; padding-bottom: 10px; padding-right: 30px;">

                                        <asp:TextBox ID="txtSearch" runat="server" class="form-control float-right" placeholder="Search" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="card-body">

                                    <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="false" class="table table-bordered table-hover">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh1" runat="server" class="form-label" Text="Host"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHost" runat="server" class="form-label" Text='<%#Eval("Organizer") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh2" runat="server" class="form-label" Text="Visitor"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitor" runat="server" class="form-label" Text='<%#Eval("VisitorName") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh3" runat="server" class="form-label" Text="Visitor Email"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitorEmail" runat="server" class="form-label" Text='<%#Eval("Visitor_Email") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh4" runat="server" class="form-label" Text="Department"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocation" runat="server" class="form-label" Text='<%#Eval("Location") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh5" runat="server" class="form-label" Text="Start Date"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" class="form-label" Text='<%#Eval("StartTime") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh6" runat="server" class="form-label" Text="End Date"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" class="form-label" Text='<%#Eval("EndTime") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh8" runat="server" class="form-label" Text="Purpose"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPurpose" runat="server" class="form-label" Text='<%#Eval("Body") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh10" runat="server" class="form-label" Text="QR Code"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQR" runat="server" class="form-label" Text='<%#Eval("QRCode") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh80" runat="server" class="form-label" Text="Print"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ToolTip="Print" CssClass="Print" Target="_blank"
                                                        NavigateUrl='<%# "PrintBadge.aspx?selfid="+ Eval("ID") %>'
                                                        ID="hplProdId">
    <img src="dist/img/printer.png" alt="Print" style="width: 16px; height: 16px;" />
</asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblh8" runat="server" class="form-label" Text="Checkin"></asp:Label>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ToolTip="Edit" ImageUrl="~/dist/img/Smalledit.png" Height="16px" Width="16px"
                                                        NavigateUrl='<%# "Checkin.aspx?selfid="+ Eval("ID") %>'
                                                        ID="hplProdId"></asp:HyperLink>
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

            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <h5 class="card-header">
                            <asp:Label ID="header2" runat="server" Text="Appointment Calender"></asp:Label>
                        </h5>

                        <div class="card-body">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ShowCollapseButton="true" Width="100%" Height="100%" HeaderText="Calender View">
                                <panelcollection>
                            <dx:PanelContent runat="server">

                                <dx:ASPxScheduler ID="ASPxScheduler1" ClientInstanceName="Scheduler1" runat="server" Width="100%"
                                    OnBeforeExecuteCallbackCommand="SPxScheduler1_OnBeforeExecuteCallbackCommand"
                                    OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing"
                                    OnQueryWorkTime="ASPxScheduler1_QueryWorkTime">
                                    <Storage EnableReminders="false">
                                        <Appointments CommitIdToDataSource="false" AutoRetrieveId="true">
                                            <Mappings AppointmentId="ID" End="EndTime" Start="StartTime" Subject="Subject" Description="Description"
                                                Location="Location" AllDay="AllDay" Type="EventType" RecurrenceInfo="RecurrenceInfo"
                                                ReminderInfo="ReminderInfo" Label="Label" Status="Status" ResourceId="cId" />
                                        </Appointments>
                                        <Resources>
                                            <Mappings ResourceId="ID" Caption="Model" />
                                        </Resources>
                                    </Storage>
                                    <%--  <OptionsForms AppointmentFormTemplateUrl="~/UserForms/MyAppointmentForm.ascx" />--%>
                                    <Views>
                                        <DayView>
                                            <TimeRulers>
                                                <dx:TimeRuler />
                                            </TimeRulers>
                                        </DayView>
                                        <WorkWeekView>
                                            <TimeRulers>
                                                <dx:TimeRuler />
                                            </TimeRulers>
                                        </WorkWeekView>
                                        <FullWeekView Enabled="true" />
                                        <WeekView Enabled="false" />
                                    </Views>
                                </dx:ASPxScheduler>

                                <script type="text/javascript">
                                    function OnBtnCreateAptClick() {
                                        Scheduler1.RaiseCallback("CREATAPTWR|");
                                    }
                                </script>
                                <script type="text/javascript">
                                    function OnMenuClick(s, e) {
                                        if (e.item.GetItemCount() <= 0) {
                                            if (e.item.name == "ExportAppointment")
                                                Scheduler1.SendPostBack("EXPORTAPT|");
                                            else
                                                Scheduler1.RaiseCallback("MNUAPT|" + e.item.name);
                                        }
                                    }
                                </script>



                            </dx:PanelContent>
                        </panelcollection>
                            </dx:ASPxRoundPanel>
                        </div>
                    </div>
                </div>
            </div>

        </div>

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
