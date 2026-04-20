using BookAppoinmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAppoinmentAPI
{
    public class HealthCareContext:DbContext
    {
        public HealthCareContext(DbContextOptions<HealthCareContext> options):base(options) { }
        public DbSet<Appointment> Appointments {  get; set; }
        public DbSet<Availability> Availabilities {  get; set; }
        public DbSet<Doctor> Doctors {  get; set; }
        public DbSet<Employee> Employees {  get; set; }
        public DbSet<LoginUser> LoginUsers {  get; set; }
        public DbSet<Specilization> Specilizations {  get; set; }
        public DbSet<HealthCareCenter> healthCareCenters {  get; set; }
       
        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppointmentStatus>().HasData(
                new AppointmentStatus { StatusId=1, Name="Pending"},
                new AppointmentStatus { StatusId=2, Name="Confirmed"},
                new AppointmentStatus { StatusId=3, Name="Rejected"},
                new AppointmentStatus { StatusId=4, Name="Cancelled"}
                );

            //Create Unique Row or value
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmailId).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.Code).IsUnique();

            modelBuilder.Entity<LoginUser>().HasIndex(e => e.Username).IsUnique();

            //modelBuilder.Entity<Appointment>().HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.SlotStartTime, a.Status }).IsUnique();

            modelBuilder.Entity<Specilization>().HasIndex(e => e.Name).IsUnique();

            modelBuilder.Entity<Availability>().HasIndex(a => new { a.DoctorId, a.Date, a.SlotStartTime }).IsUnique();

            //End

            //Start Indexing
            modelBuilder.Entity<Doctor>().HasOne(d => d.Specilization)
                .WithMany(e=>e.Doctors).HasForeignKey(d => d.SpecilizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>().HasOne(d=>d.HealthCareCenter)
                .WithMany(e => e.Doctors).HasForeignKey(d=>d.HealthCareCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>().HasOne(d => d.Doctor)
                .WithMany(e=>e.Appointments).HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>().HasOne(d => d.Employee)
                .WithMany(e=>e.Appointments).HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Availability)
               .WithOne(av => av.appointment)
               .HasForeignKey<Appointment>(a => a.AvailabilityId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.AppointmentStatus)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.AppointmentStatusId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.AvailabilityId, a.AppointmentStatusId })
                .HasFilter("[AppointmentStatusId] IN (1,2)")
                .IsUnique();

            modelBuilder.Entity<LoginUser>().HasOne(d => d.Employee)
                .WithMany().HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Availability>().HasOne(d => d.Doctor)
                .WithMany(d=>d.Availabilities).HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            //End Indexing
        }
    }
}
