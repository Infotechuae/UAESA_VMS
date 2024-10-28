 
 using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
//namespace SecuLobby.App_Code
//{
    public partial class ShedDbContext : ContextBase
    {
        static ShedDbContext()
        {
            Database.SetInitializer<ShedDbContext>(null);
        }

        public ShedDbContext() : base("DbConnectionString") { }

        public DbSet<ShedDb_Car> Resource { get; set; }
        public DbSet<ShedDb_Scheduling> Schedulings { get; set; }
       
        public DbSet<ShedDb_UsageType> UsageTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ShedDb_CarMap());
            modelBuilder.Configurations.Add(new ShedDb_SchedulingMap());
           
            modelBuilder.Configurations.Add(new ShedDb_UsageTypeMap());
        }
    }

    public partial class ShedDb_Car
    {
        public int ID { get; set; }
        public string Trademark { get; set; }
        public string Model { get; set; }
        public Nullable<short> HP { get; set; }
        public Nullable<double> Liter { get; set; }
        public Nullable<byte> Cyl { get; set; }
        public Nullable<byte> TransmissSpeedCount { get; set; }
        public string TransmissAutomatic { get; set; }
        public Nullable<byte> MPG_City { get; set; }
        public Nullable<byte> MPG_Highway { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Hyperlink { get; set; }
        public byte[] Picture { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string RtfContent { get; set; }
    }

    public partial class ShedDb_Scheduling
    {
        public int ID { get; set; }
        public Nullable<int> cId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Status { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<int> Label { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public Nullable<int> EventType { get; set; }
        public string RecurrenceInfo { get; set; }
        public string ReminderInfo { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string ContactInfo { get; set; }
    }

  
 

    public partial class ShedDb_UsageType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Color { get; set; }
    }

    #region Mapping
    public class ShedDb_CarMap : EntityTypeConfiguration<ShedDb_Car>
    {
        public ShedDb_CarMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Trademark)
                .HasMaxLength(50);

            this.Property(t => t.Model)
                .HasMaxLength(50);

            this.Property(t => t.TransmissAutomatic)
                .HasMaxLength(3);

            this.Property(t => t.Category)
                .HasMaxLength(7);

            this.Property(t => t.Hyperlink)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Resource");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Trademark).HasColumnName("Trademark");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.HP).HasColumnName("HP");
            this.Property(t => t.Liter).HasColumnName("Liter");
            this.Property(t => t.Cyl).HasColumnName("Cyl");
            this.Property(t => t.TransmissSpeedCount).HasColumnName("TransmissSpeedCount");
            this.Property(t => t.TransmissAutomatic).HasColumnName("TransmissAutomatic");
            this.Property(t => t.MPG_City).HasColumnName("MPG_City");
            this.Property(t => t.MPG_Highway).HasColumnName("MPG_Highway");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Hyperlink).HasColumnName("Hyperlink");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.RtfContent).HasColumnName("RtfContent");
        }
    }

    public class ShedDb_SchedulingMap : EntityTypeConfiguration<ShedDb_Scheduling>
    {
        public ShedDb_SchedulingMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Subject)
                .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Scheduling");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.cId).HasColumnName("cId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Label).HasColumnName("Label");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.AllDay).HasColumnName("AllDay");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.RecurrenceInfo).HasColumnName("RecurrenceInfo");
            this.Property(t => t.ReminderInfo).HasColumnName("ReminderInfo");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.ContactInfo).HasColumnName("ContactInfo");
        }
    }

   
 
    public class ShedDb_UsageTypeMap : EntityTypeConfiguration<ShedDb_UsageType>
    {
        public ShedDb_UsageTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UsageType");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Color).HasColumnName("Color");
        }
    }

    #endregion
//}
