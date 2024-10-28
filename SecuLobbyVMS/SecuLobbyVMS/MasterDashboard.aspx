<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_Master.Master" AutoEventWireup="true" CodeBehind="MasterDashboard.aspx.cs" Inherits="SecuLobbyVMS.MasterDashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">


    <style>
        .btn-primary, .btn-warning, .btn-success, .btn-danger {
            background-color: #b68a35 !important;
            border-color: #b68a35 !important;
            color: #FFF !important;
        }

        .bg-flat-color-1 {
            background: #20a8d8;
        }

        .bg-flat-color-2 {
            background: #63c2de;
        }

        .bg-flat-color-3 {
            background: #ffc107;
        }

        .bg-flat-color-4 {
            background: #f86c6b;
        }

        .bg-flat-color-5 {
            background: #4dbd74;
        }
    </style>

    <script language="JavaScript" type="text/javascript">
        $(document).ready(function () {
            var chartLabel2 = [];
            var chartData2 = [];

            $.ajax({
                url: 'MasterDashboard.aspx/GetMonthlyVisitorReport',
                method: 'post',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // Get json data from d, check it from dev-tools => Press F12

                    data = JSON.parse(data.d);

                    $(data).each(function (index, item) {
                        chartLabel2.push(item.MonthName);
                        chartData2.push(item.TotalVisitor);
                    });

                    var barChartCanvas = $('#VisitorChart').get(0).getContext('2d')
                    var barChartData = {
                        labels: chartLabel2,
                        datasets: [
                            {
                                //label: 'Visitor',
                                //backgroundColor: 'rgba(60,141,188,0.9)',
                                backgroundColor: ["#20a8d8", "#ffc107", "#f86c6b", "#4dbd74", '#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],

                                borderColor: 'rgba(60,141,188,0.8)',
                                pointRadius: false,
                                pointColor: '#3b8bba',
                                pointStrokeColor: 'rgba(60,141,188,1)',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(60,141,188,1)',
                                data: chartData2
                            },

                        ]
                    }
                    var barChartOptions = {
                        responsive: true,
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: true,
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    padding: 10,
                                    callback: function (value, index, values) {
                                        if (Math.floor(value) === value) {
                                            return value;
                                        }
                                    }
                                },
                                gridLines: {
                                    display: true,
                                }
                            }]
                        }
                    }






                    new Chart(barChartCanvas, {
                        type: 'bar',
                        data: barChartData,
                        options: barChartOptions
                    })


                },
                error: function (err) {
                    alert(err);
                }
            });
            //$.ajax({
            //    url: 'MasterDashboard.aspx/GetMonthlyVisitorReport',
            //    method: 'post',
            //    dataType: 'json',
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        // Get json data from d, check it from dev-tools => Press F12

            //        data = JSON.parse(data.d);

            //        $(data).each(function (index, item) {
            //            chartLabel2.push(item.MonthName);
            //            chartData2.push(item.TotalVisitor);
            //        });

            //        var areaChartCanvas = $('#VisitorChart').get(0).getContext('2d')
            //        var areaChartData = {
            //            labels: chartLabel2,
            //            datasets: [
            //                {
            //                    label: 'Visitor',
            //                    borderColor: "rgb(255, 99, 132)",
            //                    backgroundColor: "rgba(255, 40, 86, 0.1)",
            //                    //backgroundColor: 'rgba(60,141,188,0.9)',
            //                    //borderColor: 'rgba(60,141,188,0.8)',
            //                    pointRadius: false,
            //                    pointColor: '#3b8bba',
            //                    pointStrokeColor: 'rgba(60,141,188,1)',
            //                    pointHighlightFill: '#fff',
            //                    pointHighlightStroke: 'rgba(60,141,188,1)',
            //                    data: chartData2
            //                },

            //            ]
            //        }
            //        var areaChartOptions = {
            //            maintainAspectRatio: false,
            //            responsive: true,
            //            legend: {
            //                display: false
            //            },
            //            scales: {
            //                xAxes: [{
            //                    gridLines: {
            //                        display: false,
            //                    }
            //                }],
            //                yAxes: [{
            //                    gridLines: {
            //                        display: false,
            //                    }
            //                }]
            //            }
            //        }
            //        new Chart(areaChartCanvas, {
            //            type: 'line',
            //            data: areaChartData,
            //            options: areaChartOptions
            //        })



            //    },
            //    error: function (err) {
            //        alert(err);
            //    }
            //});

            var chartLabel3 = [];
            var chartData3 = [];

            $.ajax({
                url: 'MasterDashboard.aspx/GetVistorTypeReport',
                method: 'post',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // Get json data from d, check it from dev-tools => Press F12

                    data = JSON.parse(data.d);

                    $(data).each(function (index, item) {
                        chartLabel3.push(item.VisitorType);
                        chartData3.push(item.TotalVisitor);
                    });

                    var donutChartCanvas = $('#VisitorTypeChart').get(0).getContext('2d')
                    var donutData = {
                        labels: chartLabel3,
                        datasets: [
                            {
                                label: 'Visitor',
                                backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
                                data: chartData3
                            },

                        ]
                    }
                    var donutOptions = {
                        maintainAspectRatio: false,
                        responsive: true,
                        legend: {
                            display: true,
                            position: 'right'
                        }
                    }
                    new Chart(donutChartCanvas, {
                        type: 'doughnut',
                        data: donutData,
                        options: donutOptions
                    })


                },
                error: function (err) {
                    alert(err);
                }
            });

            var chartLabel4 = [];
            var chartData4 = [];

            $.ajax({
                url: 'MasterDashboard.aspx/GetDepartmentReport',
                method: 'post',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // Get json data from d, check it from dev-tools => Press F12

                    data = JSON.parse(data.d);

                    $(data).each(function (index, item) {
                        chartLabel4.push(item.Nationality);
                        chartData4.push(item.TotalVisitor);
                    });

                    var barChartCanvas = $('#NationalityChart').get(0).getContext('2d')
                    var barChartData = {
                        labels: chartLabel4,
                        datasets: [
                            {
                                //label: 'Visitor',
                                //backgroundColor: 'rgba(60,141,188,0.9)',
                                backgroundColor: ["#20a8d8", "#ffc107", "#f86c6b", "#4dbd74"],
                                borderColor: 'rgba(60,141,188,0.8)',
                                pointRadius: false,
                                pointColor: '#3b8bba',
                                pointStrokeColor: 'rgba(60,141,188,1)',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(60,141,188,1)',
                                data: chartData4
                            },

                        ]
                    }
                    var barChartOptions = {
                        responsive: true,
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: true,
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    padding: 10,
                                    callback: function (value, index, values) {
                                        if (Math.floor(value) === value) {
                                            return value;
                                        }
                                    }
                                },
                                gridLines: {
                                    display: true,
                                }
                            }]
                        }
                    }






                    new Chart(barChartCanvas, {
                        type: 'bar',
                        data: barChartData,
                        options: barChartOptions
                    })


                },
                error: function (err) {
                    alert(err);
                }
            });

            var chartLabel5 = [];
            var chartData5 = [];

            $.ajax({
                url: 'MasterDashboard.aspx/GetLocationReport',
                method: 'post',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // Get json data from d, check it from dev-tools => Press F12

                    data = JSON.parse(data.d);

                    $(data).each(function (index, item) {
                        chartLabel5.push(item.Gender);
                        chartData5.push(item.TotalVisitor);
                    });

                    var pieChartCanvas = $('#GenderChart').get(0).getContext('2d')
                    var pieData = {
                        labels: chartLabel5,
                        datasets: [
                            {
                                label: 'Visitor',
                                backgroundColor: ['#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
                                data: chartData5
                            },

                        ]
                    }
                    var pieOptions = {
                        responsive: true,
                        maintainAspectRatio: false,
                        datasetFill: false,
                        legend: {
                            display: true,
                            position: 'right'
                        }
                    }

                    new Chart(pieChartCanvas, {
                        type: 'pie',
                        data: pieData,
                        options: pieOptions
                    })


                },
                error: function (err) {
                    alert(err);
                }
            });



        });

    </script>


</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper">
        <div class="content-wrapper">
            <!-- /.content-header -->
            <div class="content-header" id="dvheader" runat="server">
                <div class="container-fluid">
                    <div class="row mb-2">
                        <div class="col-sm-12">
                            <h1 class="m-0"><asp:Label ID="header1" runat="server" Text="Dashboard"></asp:Label></h1>
                        </div>
                        <!-- /.col -->
                        <%--     <div class="col-sm-6">
                            <label for="txtLocation" class="form-label">
                                <asp:Label ID="Label1" runat="server" class="form-label" Text="Location" Font-Bold="true"></asp:Label>
                            </label>


                            <asp:DropDownList ID="drpLocation" runat="server" class="form-control select2" OnSelectedIndexChanged="drpLocation_SelectedIndexChanged" AutoPostBack="true" Style="width: 100%;">
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                <asp:ListItem Text="Location 1" Value="Location 1"></asp:ListItem>
                                <asp:ListItem Text="Location 2" Value="Location 3"></asp:ListItem>
                                <asp:ListItem Text="Location 3" Value="Location 4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>--%>
                        <!-- /.col -->
                    </div>
                    <!-- /.row -->
                </div>
                <!-- /.container-fluid -->
            </div>
            <!-- /.content-header -->

            <!-- Main content -->
            <section class="content">
                <%--style="background: radial-gradient(circle, rgba(238,174,202,1) 0%, rgba(148,187,233,1) 100%)"--%>
                <div class="container-fluid" style="margin-top: 20px">
                    <!-- Info boxes -->
                    <div class="row">
                        <div class="col-sm-6 col-lg-3">
                            <a href="VisitorTransaction.aspx" target="_blank">
                                <div class="card text-white bg-flat-color-1">
                                    <div class="card-body pb-0">

                                        <%-- <div class="dropdown float-right">
                                        <button class="btn bg-transparent dropdown-toggle theme-toggle text-light" type="button" id="dropdownMenuButton1" data-toggle="dropdown">
                                            <i class="fa fa-cog"></i>
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                                            <div class="dropdown-menu-content">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                    </div>--%>
                                        <h4 class="mb-0">
                                            <span class="count">
                                                <asp:Label ID="lblcheckinValue" runat="server"></asp:Label></span>
                                        </h4>
                                        <p class="text-light">
                                            <asp:Label ID="checkin" runat="server" Text="Total Visitors Check In"></asp:Label>
                                        </p>

                                        <div class="chart-wrapper px-0" style="height: 70px;" height="70">
                                            <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                    <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                    <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                                                </div>
                                            </div>
                                            <canvas id="widgetChart1" height="70" style="display: block; width: 324px; height: 70px;" width="324" class="chartjs-render-monitor"></canvas>
                                        </div>

                                    </div>

                                </div>
                            </a>
                        </div>


                        <div class="col-sm-6 col-lg-3">
                            <a href="frmSchedule.aspx" target="_blank">
                                <div class="card text-white bg-flat-color-4">
                                    <div class="card-body pb-0">
                                        <%-- <div class="dropdown float-right">
                                        <button class="btn bg-transparent dropdown-toggle theme-toggle text-light" type="button" id="dropdownMenuButton4" data-toggle="dropdown">
                                            <i class="fa fa-cog"></i>
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton4">
                                            <div class="dropdown-menu-content">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                    </div>--%>

                                        <h4 class="mb-0">
                                            <span class="count">
                                                <asp:Label ID="lblAppointmentValue" runat="server"></asp:Label></span>
                                        </h4>
                                        <p class="text-light">
                                            <asp:Label ID="lblAppointment" runat="server" Text="Appointment"></asp:Label>
                                        </p>

                                        <div class="chart-wrapper px-3" style="height: 70px;" height="70">
                                            <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                    <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                    <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                                                </div>
                                            </div>
                                            <canvas id="widgetChart4" height="68" style="display: block; width: 292px; height: 68px;" width="292" class="chartjs-render-monitor"></canvas>
                                        </div>

                                    </div>
                                </div>
                            </a>
                        </div>

                        <div class="col-sm-6 col-lg-3">
                           <a href="#" style="color:white; text-decoration:none;" data-toggle="modal" data-target="#modal-default">
                            <div class="card text-white bg-flat-color-2">
                                <div class="card-body pb-0">
                                    <%-- <div class="dropdown float-right">
                                        <button class="btn bg-transparent dropdown-toggle theme-toggle text-light" type="button" id="dropdownMenuButton2" data-toggle="dropdown">
                                            <i class="fa fa-cog"></i>
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton2">
                                            <div class="dropdown-menu-content">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                    </div>--%>



                                    <h4 class="mb-0">
                                        <span class="count">
                                            <asp:Label ID="lblEmergencyValue" runat="server"></asp:Label></span>
                                        <asp:Label ID="lblCheckOutValue" runat="server" Visible="false"></asp:Label>
                                     <%--  <button type="button" class="btn btn-success me-2 mb-4" data-toggle="modal" data-target="#modal-default">
                                        Watchlist
                                    </button>--%>
                                    </h4>
                                    <asp:Label ID="lblCheckOut" runat="server" Text="Check Out" Visible="false"></asp:Label>
                                    <%--<p class="text-light">
                                        <asp:Label ID="lblEmergency" runat="server" Text="Emergency Evacuation"></asp:Label>
                                       <button id="btnAlert" style="background-color: orangered; border-color: orangered; color: white; font-weight: bold; float: right">Alert</button>
                                    </p>--%>
                                   
                                    <%--<div class="chart-wrapper px-0" style="height: 10px;">
                                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                                            </div>
                                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                                            </div>
                                        </div>
                                        <canvas id="widgetChart2" height="70" style="display: block; width: 324px; height: 70px;" width="324" class="chartjs-render-monitor"></canvas>
                                    </div>--%>
                                  <p class="text-light">
                                            <asp:Label ID="lblEmergency" runat="server" Text="Emergency Evacuation"></asp:Label>
                                   <%-- <a href="#" style="color:white; text-decoration:none;" data-toggle="modal" data-target="#modal-default">Emergency Evacuation</a>--%>
                                        </p>

                                        <div class="chart-wrapper px-3" style="height: 70px;" height="70">
                                            <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                    <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                    <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                                                </div>
                                            </div>
                                            <canvas id="widgetChart2" height="68" style="display: block; width: 292px; height: 68px;" width="292" class="chartjs-render-monitor"></canvas>
                                        </div>
                                </div>
                            </div>
                          </a>
                        </div>

                        <div class="modal fade" id="modal-default">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Message</h4>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <asp:TextBox ID="txtWatchlistReason" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="modal-footer justify-content-between">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                                        <asp:Button ID="btnEmail" runat="server" Text="Send Email" class="btn btn-primary" OnClick="btnEmail_Click" />
                                        <asp:Button ID="btnSMS" runat="server" Text="Send SMS" class="btn btn-primary" Visible="false" OnClick="btnSMS_Click" />
                                    </div>
                                </div>
                                <!-- /.modal-content -->
                            </div>
                            <!-- /.modal-dialog -->
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <a href="#">
                                <div class="card text-white bg-flat-color-3">
                                    <div class="card-body pb-0">
                                       

                                        <h4 class="mb-0">
                                            <span class="count">
                                                <asp:Label ID="lblWatchlistValue" runat="server"></asp:Label></span>
                                        </h4>
                                        <p class="text-light">
                                            <asp:Label ID="lblWatchlist" runat="server" Text="Today Request"></asp:Label>
                                        </p>

                                    </div>

                                    <div class="chart-wrapper px-0" style="height: 70px;" height="70">
                                        <div class="chartjs-size-monitor" style="position: absolute; inset: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0"></div>
                                            </div>
                                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0"></div>
                                            </div>
                                        </div>
                                        <canvas id="widgetChart3" height="84" style="display: block; width: 364px; height: 84px;" width="364" class="chartjs-render-monitor"></canvas>
                                    </div>
                                </div>
                            </a>
                        </div>
                    </div>
                    <%-- <div class="col-12 col-sm-6 col-md-3">
                            <a href="Checkin.aspx?selfid=">
                                <div class="info-box">
                                    <span><img src="images/check-in.png" /></span>

                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <asp:Label ID="checkin" runat="server" Text="Check IN"></asp:Label>
                                        </span>
                                        <span class="info-box-number">
                                            <asp:Label ID="lblcheckinValue" runat="server"></asp:Label>


                                        </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                            </a>
                            <!-- /.info-box -->
                        </div>--%>
                    <!-- /.col -->
                    <%--  <div class="col-12 col-sm-6 col-md-3">
                            <a href="Checkout.aspx">
                                <div class="info-box mb-3">
                                    <span>
                                        <img src="images/check-out.png" /></span>

                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <asp:Label ID="lblCheckOut" runat="server" Text="Check Out"></asp:Label>

                                        </span>
                                        <span class="info-box-number">
                                            <asp:Label ID="lblCheckOutValue" runat="server"></asp:Label>

                                        </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                            </a>
                            <!-- /.info-box -->
                        </div>--%>
                    <!-- /.col -->

                    <!-- fix for small devices only -->
                    <%-- <div class="clearfix hidden-md-up"></div>--%>

                    <%--  <div class="col-12 col-sm-6 col-md-3">
                            <a href="Timeout.aspx">
                                <div class="info-box mb-3">
                                    <span>
                                        <img src="images/TimeOut.png" /></span>

                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <asp:Label ID="lblTimeOut" runat="server" Text="Time Out"></asp:Label>
                                        </span>
                                        <span class="info-box-number">
                                            <asp:Label ID="lblTimeOutValue" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                            </a>
                            <!-- /.info-box -->
                        </div>--%>
                    <!-- /.col -->
                    <%--  <div class="col-12 col-sm-6 col-md-3">
                            <a href="frmSchedule.aspx" target="_blank">
                                <div class="info-box mb-3">
                                    <span>
                                        <img src="images/appointment.png" /></span>

                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <asp:Label ID="lblAppointment" runat="server" Text="Appointment"></asp:Label>
                                        </span>
                                        <span class="info-box-number">
                                            <asp:Label ID="lblAppointmentValue" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                            </a>
                            <!-- /.info-box -->
                        </div>
                        <!-- /.col -->
                    </div>--%>
                    <!-- /.row -->


                    <!-- /.row -->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="card-title">
                                        <asp:Label ID="lblVisstatis" runat="server" Text="Visitor Statistics Report"></asp:Label>
                                    </h5>

                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>

                                    </div>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <p class="text-center">
                                                <strong>
                                                    <asp:Label ID="Label2" runat="server" Text="Visitor Type"></asp:Label>
                                                </strong>
                                            </p>

                                            <div class="chart">
                                                <!-- Sales Chart Canvas -->
                                                <canvas id="VisitorTypeChart" height="180" style="height: 180px;"></canvas>
                                            </div>
                                            <!-- /.chart-responsive -->
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-md-4">
                                            <p class="text-center">
                                                <strong>
                                                    <asp:Label ID="Label3" runat="server" Text="Department"></asp:Label>
                                                </strong>
                                            </p>

                                            <div class="chart">
                                                <!-- Sales Chart Canvas -->
                                                <canvas id="NationalityChart" height="180" style="height: 180px;"></canvas>
                                            </div>
                                            <!-- /.chart-responsive -->
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-md-4">
                                            <p class="text-center">
                                                <strong>
                                                    <asp:Label ID="Label4" runat="server" Text="Location"></asp:Label>
                                                </strong>
                                            </p>

                                            <div class="chart">
                                                <!-- Sales Chart Canvas -->
                                                <canvas id="GenderChart" height="180" style="height: 180px;"></canvas>
                                            </div>
                                            <!-- /.chart-responsive -->
                                        </div>
                                        <!-- /.col -->
                                    </div>
                                    <!-- /.row -->
                                </div>
                                <!-- ./card-body -->

                                <!-- /.card-footer -->
                            </div>
                            <!-- /.card -->
                        </div>
                        <!-- /.col -->
                    </div>
                    <div class="row" id="divVisHis" runat="server">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="card-title">
                                        <asp:Label ID="lblMVR" runat="server" Text="Visitor History"></asp:Label>
                                    </h5>

                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>
                                        <%--   <div class="btn-group">
                                            <button type="button" class="btn btn-tool dropdown-toggle" data-toggle="dropdown">
                                                <i class="fas fa-wrench"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right" role="menu">
                                                <a href="#" class="dropdown-item">Action</a>
                                                <a href="#" class="dropdown-item">Another action</a>
                                                <a href="#" class="dropdown-item">Something else here</a>
                                                <a class="dropdown-divider"></a>
                                                <a href="#" class="dropdown-item">Separated link</a>
                                            </div>
                                        </div>--%>
                                        <%--  <button type="button" class="btn btn-tool" data-card-widget="remove">
                                            <i class="fas fa-times"></i>
                                        </button>--%>
                                    </div>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <p class="text-center">
                                                <strong>
                                                    <asp:Label ID="lblVisitorrange" runat="server"></asp:Label>
                                                </strong>
                                            </p>

                                            <div class="chart">
                                                <!-- Sales Chart Canvas -->
                                                <canvas id="VisitorChart" height="180" style="height: 180px;"></canvas>
                                            </div>
                                            <!-- /.chart-responsive -->
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-md-4">
                                            <p class="text-center">
                                                <strong>
                                                    <asp:Label ID="lblStatistics" runat="server" Text="Statistics"></asp:Label></strong>
                                            </p>

                                            <div class="progress-group">
                                                Total Check In
                     
                                                <span class="float-right"><b>
                                                    <asp:Label ID="lblTotalCheckin" runat="server"></asp:Label>
                                                </b></span>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar bg-primary" style="width: 100%"></div>
                                                </div>
                                            </div>
                                            <!-- /.progress-group -->

                                            <%--    <div class="progress-group">
                                                Total Check Out
                     
                                                <span class="float-right"><b>
                                                    <asp:Label ID="lblTotalCheckout" runat="server"></asp:Label>
                                                </b></span>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar bg-danger" style="width: 100%"></div>
                                                </div>
                                            </div>--%>
                                            <asp:Label ID="lblTotalCheckout" runat="server" Visible="false"></asp:Label>
                                            <!-- /.progress-group -->
                                            <div class="progress-group">
                                                <span class="progress-text">Total Watchlist</span>
                                                <span class="float-right"><b>
                                                    <asp:Label ID="lblTotalWatchlist" runat="server"></asp:Label>
                                                </b></span>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar bg-warning" style="width: 100%"></div>
                                                </div>
                                            </div>

                                            <!-- /.progress-group -->
                                            <div class="progress-group">
                                                Total Appointment
                     
                                                <span class="float-right"><b>
                                                    <asp:Label ID="lblTotalAppointment" runat="server"></asp:Label>
                                                </b></span>
                                                <div class="progress progress-sm">
                                                    <div class="progress-bar bg-success" style="width: 100%"></div>
                                                </div>
                                            </div>
                                            <!-- /.progress-group -->
                                        </div>
                                        <!-- /.col -->
                                    </div>
                                    <!-- /.row -->
                                </div>
                                <!-- ./card-body -->

                                <!-- /.card-footer -->
                            </div>
                            <!-- /.card -->
                        </div>
                        <!-- /.col -->
                    </div>

                    <%--     <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="card-title">
                                        <asp:Label ID="Label1" runat="server" Text="Today's Visiting Details"></asp:Label>
                                    </h5>

                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>

                                    </div>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <p class="text-center">
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" Text="Current Visiting Details"></asp:Label>
                                                </strong>
                                            </p>

                                            <div class="card">

                                                <!-- /.card-header -->
                                                <div class="card-body p-0">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="grdVisDeta" runat="server" AutoGenerateColumns="false" CssClass="table m-0" GridLines="None">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Visitor Name">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Name") %></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Visitor ID">
                                                                    <ItemTemplate>
                                                                        <%# Eval("EmiratesID") %></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mobile">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Mobile") %></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <span class='<%# Eval("CheckInOutColur") %>'><%# Eval("CheckInOutStatus") %></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <!-- /.table-responsive -->
                                                </div>
                                                <!-- /.card-body -->

                                                <!-- /.card-footer -->
                                            </div>
                                            <!-- /.chart-responsive -->
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-md-7">
                                            <p class="text-center" style="display: none;">
                                                <strong>
                                                    <asp:Label ID="Label6" runat="server" Text="Appointment Calender"></asp:Label>
                                                </strong>
                                            </p>

                                            <div class="chart">
                                                <!-- Sales Chart Canvas -->
                                                <div id="scheduler"></div>
                                            </div>
                                            <!-- /.chart-responsive -->
                                        </div>
                                        <!-- /.col -->

                                        <!-- /.col -->
                                    </div>
                                    <!-- /.row -->
                                </div>
                                <!-- ./card-body -->

                                <!-- /.card-footer -->
                            </div>
                            <!-- /.card -->
                        </div>
                        <!-- /.col -->
                    </div>--%>
                </div>
                <!--/. container-fluid -->
            </section>
            <!-- /.content -->
        </div>
    </div>
</asp:Content>
