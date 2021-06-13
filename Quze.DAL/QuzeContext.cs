using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quze.Infrastruture.Security;

namespace Quze.DAL
{
    public class QuzeContext : DbContext
    {

        IUserService _userService;
        public QuzeContext(DbContextOptions options, IUserService userService) : base(options)
        {
            _userService = userService;
        }


        public DbSet<User> Users { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<RequiredDocument> RequiredDocuments { get; set; }
        public DbSet<RequiredTask> RequiredTasks { get; set; }
        public DbSet<RequiredTest> RequiredTests { get; set; }  
        public DbSet<Street> Streets { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<ServiceProvidersServiceType> ServiceProvidersServiceTypes { get; set; }
        public DbSet<ServiceQueue> ServiceQueues { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Fellow> Fellows { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<TimeTableLine> TimeTableLines { get; set; }
        public DbSet<TimeTableException> TimeTableExceptions { get; set; }
        public DbSet<TimeTableVacation> TimeTableVacations { get; set; }
        public DbSet<AppointmentDocument> AppointmentDocuments { get; set; }
        public DbSet<AppointmentTask> AppointmentTasks { get; set; }
        public DbSet<AppointmentTest> AppointmentTests { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AlertRule> AlertRule { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<MinimalKitRules> MinimalKitRules { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Experty> Experties { get; set; }
        public DbSet<DocumentsContent> DocumentsContent { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Conflict> Conflicts { get; set; }
        //public DbSet<Device> Devices { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<OperationsStatuses> operationsStatuses { get; set; }
        public DbSet<ServiceStation> ServiceStation { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<UploadOperationDetails> uploadOperationDetails { get; set; }
        public DbSet<CancelationReasons> CancelationReasons { get; set; }
        public DbSet<TeamReady> TeamReady { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<AllocationOfEquipment> AllocationOfEquipment { get; set; }
        public DbSet<EquipmentAppointmentRequest> EquipmentAppointmentRequest { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<servicetypes>()
            //.HasKey(c => c.Id);

            //modelBuilder.ApplyConfiguration()
            modelBuilder.Entity<Appointment>(e =>
            {
                e.HasOne(a => a.ServiceQueue).WithMany(q => q.Appointments).HasForeignKey(a => a.ServiceQueueId);
            })
              .Entity<TimeTableLine>()
                .HasOne(c => c.TimeTable)
                .WithMany(b => b.TimeTableLines)
                .HasForeignKey(c => c.TimeTableId);

            modelBuilder.Entity<ServiceProvidersServiceType>()
                .HasOne(sp => sp.ServiceProvider)
                .WithMany(sp => sp.ServiceProvidersServiceTypes)
                .HasForeignKey(s => s.ServiceProviderId);

            modelBuilder.Entity<ServiceProvidersServiceType>()
                .HasOne(sp => sp.ServiceType)
                .WithMany(sp => sp.ServiceProvidersServiceTypes)
                .HasForeignKey(s => s.ServiceTypeId);

            //modelBuilder.Entity<RequiredTask>(e =>
            //{
            //    e.HasOne(a => a.ServiceType).WithMany(q => q.RequiredTasks).HasForeignKey(a => a.ServiceTypeID);
            //});
            modelBuilder.Entity<ServiceType>().HasMany<RequiredTask>(sT => sT.RequiredTasks);

            modelBuilder.Entity<AllocationOfEquipment>()
              .HasOne(e =>e.Appointment)
              .WithMany(sp => sp.AllocationOfEquipment)
              .HasForeignKey(s => s.AppointmentId);

            modelBuilder.Entity<AllocationOfEquipment>()
             .HasOne(e => e.Equipment)
             .WithMany(sp => sp.AllocationOfEquipment)
             .HasForeignKey(s => s.EqpId);

            modelBuilder.Entity<EquipmentAppointmentRequest>()
             .HasOne(e => e.Operation)
             .WithMany(app => app.EquipmentAppointmentRequest)
             .HasForeignKey(e => e.OperationId);

            modelBuilder.Entity<EquipmentAppointmentRequest>()
            .HasOne(e => e.Equipment)
            .WithMany(eq => eq.EquipmentAppointmentRequest)
            .HasForeignKey(e => e.EqpId);

            modelBuilder.Entity<ServiceProvidersBranches>()
               .HasOne(sp => sp.ServiceProvider)
               .WithMany(sp => sp.ServiceProvidersBranches)
               .HasForeignKey(s => s.ServiceProviderId);

            modelBuilder.Entity<ServiceProvidersBranches>()
                .HasOne(sp => sp.Branch)
                .WithMany(sp => sp.ServiceProvidersBranches)
                .HasForeignKey(s => s.BranchId);

            modelBuilder.Entity<User>().HasMany<Fellow>(u => u.Fellows).WithOne(f => f.ApplicationUser).HasPrincipalKey(u => u.IdentityNumber);

            //modelBuilder.Entity<Device>().HasOne<User>(d => d.User);

            modelBuilder.Entity<Operation>().HasOne(o => o.Surgeon);
            //modelBuilder.Entity<Operation>().HasOne(o => o.NursingUnit);

            //modelBuilder.Entity<Appointment>().HasMany<AppointmentTest>(app => app.AppointmentTests);

        }

        public override int SaveChanges()
        {
            AddTimestampsAndUserInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestampsAndUserInformation();
            var recordsAffected = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return recordsAffected;
        }

        private void AddTimestampsAndUserInformation()
        {
            var saveTime = DateTime.Now;
            var entities = ChangeTracker.Entries().Where(x => x.Entity is Quze.Models.Entities.EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));


            var currentUserId = _userService.GetCurrentUserId();// user.Id;


            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Property("TimeCreated").CurrentValue = saveTime;
                    entity.Property("CreatedUserId").CurrentValue = currentUserId;
                }

                entity.Property("TimeUpdated").CurrentValue = saveTime;
                entity.Property("LastUpdateUserId").CurrentValue = currentUserId;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

        }
    }
}
