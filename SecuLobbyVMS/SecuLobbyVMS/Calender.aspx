<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calender.aspx.cs" Inherits="SecuLobbyVMS.Calender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="../plugins/fontawesome-free/css/all.min.css" />
    <!-- fullCalendar -->
    <link rel="stylesheet" href="../plugins/fullcalendar/main.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="../dist/css/adminlte.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">

            <div class="content-wrapper">
                <!-- Content Header (Page header) -->
                <section class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1>Calendar</h1>
                            </div>
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                                    <li class="breadcrumb-item active">Calendar</li>
                                </ol>
                            </div>
                        </div>
                    </div>
                    <!-- /.container-fluid -->
                </section>

                <!-- Main content -->
                <section class="content">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-3" style="display: none;">
                                <div class="sticky-top mb-3">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="card-title">Draggable Events</h4>
                                        </div>
                                        <div class="card-body">
                                            <!-- the events -->
                                            <div id="external-events">
                                                <div class="external-event bg-success">Lunch</div>
                                                <div class="external-event bg-warning">Go home</div>
                                                <div class="external-event bg-info">Do homework</div>
                                                <div class="external-event bg-primary">Work on UI design</div>
                                                <div class="external-event bg-danger">Sleep tight</div>
                                                <div class="checkbox">
                                                    <label for="drop-remove">
                                                        <input type="checkbox" id="drop-remove">
                                                        remove after drop
                     
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                    <%--<div class="card">
                <div class="card-header">
                  <h3 class="card-title">Create Event</h3>
                </div>
                <div class="card-body">
                  <div class="btn-group" style="width: 100%; margin-bottom: 10px;">
                    <ul class="fc-color-picker" id="color-chooser">
                      <li><a class="text-primary" href="#"><i class="fas fa-square"></i></a></li>
                      <li><a class="text-warning" href="#"><i class="fas fa-square"></i></a></li>
                      <li><a class="text-success" href="#"><i class="fas fa-square"></i></a></li>
                      <li><a class="text-danger" href="#"><i class="fas fa-square"></i></a></li>
                      <li><a class="text-muted" href="#"><i class="fas fa-square"></i></a></li>
                    </ul>
                  </div>
                  <!-- /btn-group -->
                  <div class="input-group">
                    <input id="new-event" type="text" class="form-control" placeholder="Event Title">

                    <div class="input-group-append">
                      <button id="add-new-event" type="button" class="btn btn-primary">Add</button>
                    </div>
                    <!-- /btn-group -->
                  </div>
                  <!-- /input-group -->
                </div>
              </div>--%>
                                </div>
                            </div>
                            <!-- /.col -->
                            <div class="col-md-9">
                                <div class="card card-primary">
                                    <div class="card-body p-0">
                                        <!-- THE CALENDAR -->
                                        <div id="calendar"></div>
                                    </div>
                                    <!-- /.card-body -->
                                </div>
                                <!-- /.card -->
                            </div>
                            <!-- /.col -->
                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.container-fluid -->
                </section>
                <!-- /.content -->
            </div>
        </div>
    </form>
    <!-- jQuery -->
    <script src="../plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- jQuery UI -->
    <script src="../plugins/jquery-ui/jquery-ui.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../dist/js/adminlte.min.js"></script>
    <!-- fullCalendar 2.2.5 -->
    <script src="../plugins/moment/moment.min.js"></script>
    <script src="../plugins/fullcalendar/main.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../dist/js/demo.js"></script>

    <script language="JavaScript" type="text/javascript">
        $(document).ready(function () {

            var chartLabel1 = [];
            var chartData1 = [];


            function ini_events(ele) {
                ele.each(function () {


                    var eventObject = {
                        title: $.trim($(this).text())
                    }


                    $(this).data('eventObject', eventObject)


                    $(this).draggable({
                        zIndex: 1070,
                        revert: true, // will cause the event to go back to its
                        revertDuration: 0  //  original position after the drag
                    })

                })
            }

            ini_events($('#external-events div.external-event'))


            var date = new Date()
            var d = date.getDate(),
                m = date.getMonth(),
                y = date.getFullYear()

            var Calendar = FullCalendar.Calendar;
            var Draggable = FullCalendar.Draggable;

            var containerEl = document.getElementById('external-events');
            var checkbox = document.getElementById('drop-remove');
            var calendarEl = document.getElementById('calendar');

            // initialize the external events
            // -----------------------------------------------------------------

            new Draggable(containerEl, {
                itemSelector: '.external-event',
                eventData: function (eventEl) {
                    return {
                        title: eventEl.innerText,
                        backgroundColor: window.getComputedStyle(eventEl, null).getPropertyValue('background-color'),
                        borderColor: window.getComputedStyle(eventEl, null).getPropertyValue('background-color'),
                        textColor: window.getComputedStyle(eventEl, null).getPropertyValue('color'),
                    };
                }
            });
            $.ajax({
                url: 'Calender.aspx/GetCalenderScheduleReport',
                method: 'post',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    data = JSON.parse(data.d);

                    $(data).each(function (index, item) {
                        chartLabel1.push(item.Nationality);
                        chartData1.push(item.TotalVisitor);
                    });
                    var calendar = new Calendar(calendarEl, {
                        headerToolbar: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'dayGridMonth,timeGridWeek,timeGridDay'
                        },
                        themeSystem: 'bootstrap',
                        //Random default events
                        events: [
                            {
                                title: chartLabel1,
                                start: new Date(chartData1),
                                allDay: false,
                                backgroundColor: '#0073b7',
                                borderColor: '#0073b7'
                            },
                        ],
                        editable: true,
                        droppable: true, // this allows things to be dropped onto the calendar !!!
                        drop: function (info) {
                            // is the "remove after drop" checkbox checked?
                            if (checkbox.checked) {
                                // if so, remove the element from the "Draggable Events" list
                                info.draggedEl.parentNode.removeChild(info.draggedEl);
                            }
                        }
                    });


                    calendar.render();


                },
                error: function (err) {
                    alert(err);
                }
                    // $('#calendar').fullCalendar()

                    /* ADDING EVENTS */
                    var currColor = '#3c8dbc' //Red by default
                    // Color chooser button
                    $('#color-chooser > li > a').click(function(e) {
                    e.preventDefault()
                    // Save color
                    currColor = $(this).css('color')
                    // Add color effect to button
                    $('#add-new-event').css({
                        'background-color': currColor,
                        'border-color': currColor
                    })
                })
            $('#add-new-event').click(function (e) {
                e.preventDefault()
                // Get value and make sure it is not null
                var val = $('#new-event').val()
                if (val.length == 0) {
                    return
                }

                // Create events
                var event = $('<div />')
                event.css({
                    'background-color': currColor,
                    'border-color': currColor,
                    'color': '#fff'
                }).addClass('external-event')
                event.text(val)
                $('#external-events').prepend(event)

                // Add draggable funtionality
                ini_events(event)

                // Remove event from text input
                $('#new-event').val('')
            })


        })
</script>
</body>
</html>
