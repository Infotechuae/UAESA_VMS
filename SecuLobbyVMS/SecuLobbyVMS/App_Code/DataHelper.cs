using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using SecuLobby.App_Code;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNet.EntityDataSource;
namespace SecuLobby.App_Code
{
    public static class DataHelper
    {
        public static void SetupDefaultMappings(ASPxScheduler control)
        {
            //if (Utils.IsSiteMode)
            //    SetupDefaultMappingsSiteMode(control);
            //else
            SetupDefaultMappingsLocalMode(control);
        }

        public static void SetupCustomEventsMappings(ASPxScheduler control)
        {
            SetupDefaultMappingsSiteMode(control);
        }

        static void SetupDefaultMappingsLocalMode(ASPxScheduler control)
        {
            ASPxSchedulerStorage storage = control.Storage;
            storage.BeginUpdate();
            try
            {
                ASPxResourceMappingInfo resourceMappings = storage.Resources.Mappings;
                resourceMappings.ResourceId = "ID";
                resourceMappings.Caption = "Model";

                ASPxAppointmentMappingInfo appointmentMappings = storage.Appointments.Mappings;
                appointmentMappings.AppointmentId = "ID";
                appointmentMappings.Start = "StartTime";
                appointmentMappings.End = "EndTime";
                appointmentMappings.Subject = "Subject";
                appointmentMappings.Description = "Description";
                appointmentMappings.Location = "Location";
                appointmentMappings.AllDay = "AllDay";
                appointmentMappings.Type = "EventType";
                appointmentMappings.RecurrenceInfo = "RecurrenceInfo";
                appointmentMappings.ReminderInfo = "ReminderInfo";
                appointmentMappings.Label = "Label";
                appointmentMappings.Status = "Status";
                appointmentMappings.ResourceId = "CarId";
            }
            finally
            {
                storage.EndUpdate();
            }
        }
        private static void SetupDefaultMappingsSiteMode(ASPxScheduler control)
        {
            ASPxSchedulerStorage storage = control.Storage;
            storage.BeginUpdate();
            try
            {
                ASPxResourceMappingInfo resourceMappings = storage.Resources.Mappings;
                resourceMappings.ResourceId = "Id";
                resourceMappings.Caption = "Caption";

                ASPxAppointmentMappingInfo appointmentMappings = storage.Appointments.Mappings;
                appointmentMappings.AppointmentId = "Id";
                appointmentMappings.Start = "StartTime";
                appointmentMappings.End = "EndTime";
                appointmentMappings.Subject = "Subject";
                appointmentMappings.AllDay = "AllDay";
                appointmentMappings.Description = "Description";
                appointmentMappings.Label = "Label";
                appointmentMappings.Location = "Location";
                appointmentMappings.RecurrenceInfo = "RecurrenceInfo";
                appointmentMappings.ReminderInfo = "ReminderInfo";
                appointmentMappings.ResourceId = "OwnerId";
                appointmentMappings.Status = "Status";
                appointmentMappings.Type = "EventType";
            }
            finally
            {
                storage.EndUpdate();
            }
        }
        public static void ProvideRowInsertion(ASPxScheduler control, DataSourceControl dataSource)
        {

            bool isDbDataSource = dataSource is Microsoft.AspNet.EntityDataSource.EntityDataSource || dataSource is AccessDataSource;
            if (isDbDataSource)
            {
                DbRowInsertionProvider provider = new DbRowInsertionProvider();
                provider.ProvideRowInsertion(control, dataSource);
                return;
            }

            ObjectDataSource objectDataSource = dataSource as ObjectDataSource;
            if (objectDataSource != null)
            {
                ObjectDataSourceRowInsertionProvider provider = new ObjectDataSourceRowInsertionProvider();
                provider.ProvideRowInsertion(control, objectDataSource);
            }
        }
        public static void EnsureOnlineVersionModificationLock(ASPxScheduler control)
        {
            //if (Utils.IsSiteMode)
            //{
            DemoOnlineRowModificationProvider provider = new DemoOnlineRowModificationProvider();
            provider.ProvideModification(control);
            //  }
        }
        public static void EnsureOnlineVersionModificationLock(ASPxScheduler control, SqlDataSource dataSource)
        {
            //if (Utils.IsSiteMode)
            //{
            DemoOnlineRowModificationProvider provider = new DemoOnlineRowModificationProvider();
            provider.ProvideModification(control, dataSource);
            // }
        }
    }
    public class DemoOnlineRowModificationProvider
    {
        public void ProvideModification(ASPxScheduler control, SqlDataSource dataSource)
        {
            dataSource.Updating += OnSqlDataSourceModifying;
            dataSource.Inserting += OnSqlDataSourceModifying;
            dataSource.Deleting += OnSqlDataSourceModifying;
        }
        void OnSqlDataSourceModifying(object sender, SqlDataSourceCommandEventArgs e)
        {
            // Utils.AssertNotReadOnlyText();
        }
        public void ProvideModification(ASPxScheduler control)
        {
            control.AppointmentRowInserting += AppointmentRowChanging;
            control.AppointmentRowUpdating += AppointmentRowChanging;
            control.AppointmentRowDeleting += AppointmentRowChanging;
        }
        void AppointmentRowChanging(object sender, CancelEventArgs e)
        {
            //Utils.AssertNotReadOnlyText();
        }
    }

    public class DbRowInsertionProvider
    {
        List<int> lastInsertedIdList = new List<int>();

        void ProvideRowInsertionCore(ASPxScheduler control)
        {
            control.AppointmentRowInserting += new ASPxSchedulerDataInsertingEventHandler(ControlOnAppointmentRowInserting);
            control.AppointmentRowInserted += new ASPxSchedulerDataInsertedEventHandler(ControlOnAppointmentRowInserted);
            control.AppointmentsInserted += new PersistentObjectsEventHandler(ControlOnAppointmentsInserted);
            control.AppointmentCollectionCleared += new EventHandler(OnControlAppointmentCollectionCleared);
        }

        public void ProvideRowInsertion(ASPxScheduler control, DataSourceControl dataSource)
        {
            if (dataSource is AccessDataSource)
                (dataSource as AccessDataSource).Inserted += AppointmentsAccessDataSource_Inserted;
            if (dataSource is Microsoft.AspNet.EntityDataSource.EntityDataSource)
                (dataSource as Microsoft.AspNet.EntityDataSource.EntityDataSource).Inserted += AppointmentsEntityDataSource_Inserted;
            ProvideRowInsertionCore(control);
        }

        void OnControlAppointmentCollectionCleared(object sender, EventArgs e)
        {
            this.lastInsertedIdList.Clear();
        }

        void ControlOnAppointmentRowInserting(object sender, ASPxSchedulerDataInsertingEventArgs e)
        {
            // Autoincremented primary key case
            e.NewValues.Remove("ID");
        }
        void ControlOnAppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e)
        {
            // Autoincremented primary key case
            int count = this.lastInsertedIdList.Count;
            System.Diagnostics.Debug.Assert(count > 0);
            e.KeyFieldValue = this.lastInsertedIdList[count - 1];
        }
        void AppointmentsAccessDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            // Autoincremented primary key case
            OleDbConnection connection = (OleDbConnection)e.Command.Connection;
            using (OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY", connection))
            {
                this.lastInsertedIdList.Add((int)cmd.ExecuteScalar());
            }
        }
        void AppointmentsEntityDataSource_Inserted(object sender, Microsoft.AspNet.EntityDataSource.EntityDataSourceChangedEventArgs e)
        {
            this.lastInsertedIdList.Add((e.Entity as ShedDb_Scheduling).ID);
        }
        void ControlOnAppointmentsInserted(object sender, PersistentObjectsEventArgs e)
        {
            // Autoincremented primary key case
            int count = e.Objects.Count;
            System.Diagnostics.Debug.Assert(count == this.lastInsertedIdList.Count);
            ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
            for (int i = 0; i < count; i++)
            {
                Appointment apt = (Appointment)e.Objects[i];
                int appointmentId = this.lastInsertedIdList[i];
                storage.SetAppointmentId(apt, appointmentId);
            }

            this.lastInsertedIdList.Clear();
        }
    }
    public class ObjectDataSourceRowInsertionProvider
    {
        List<Object> lastInsertedIdList = new List<object>();

        public void ProvideRowInsertion(ASPxScheduler control, ObjectDataSource dataSource)
        {
            control.AppointmentsInserted += new PersistentObjectsEventHandler(control_AppointmentsInserted);
            control.AppointmentCollectionCleared += new EventHandler(control_AppointmentCollectionCleared);
            dataSource.Inserted += new ObjectDataSourceStatusEventHandler(dataSource_Inserted);
        }

        void control_AppointmentCollectionCleared(object sender, EventArgs e)
        {
            this.lastInsertedIdList.Clear();
        }
        void dataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            this.lastInsertedIdList.Add(e.ReturnValue);
        }
        void control_AppointmentsInserted(object sender, PersistentObjectsEventArgs e)
        {
            ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
            int count = e.Objects.Count;
            System.Diagnostics.Debug.Assert(count == this.lastInsertedIdList.Count);
            for (int i = 0; i < count; i++)
            { //B184873
                Appointment apt = (Appointment)e.Objects[i];
                storage.SetAppointmentId(apt, this.lastInsertedIdList[i]);
            }
            this.lastInsertedIdList.Clear();
        }
    }
    public class AppointmentStatusSetter
    {
        ASPxScheduler scheduler;
        public void Attach(ASPxScheduler scheduler)
        {
            if (scheduler != null)
                scheduler.InitNewAppointment -= new AppointmentEventHandler(scheduler_InitNewAppointment);
            this.scheduler = scheduler;
            this.scheduler.InitNewAppointment += new AppointmentEventHandler(scheduler_InitNewAppointment);
        }

        void scheduler_InitNewAppointment(object sender, AppointmentEventArgs e)
        {
            e.Appointment.StatusKey = 0;
        }
    }
}