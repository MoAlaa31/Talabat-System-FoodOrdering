using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Calender;
using Talabat.Repository.Data.Config;

namespace Talabat.Repository.Data
{
    public class StoreContext: DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductCategoryConfigurations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // This line replaces the above three lines

            // Relationships
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.WorkSchedules)
                .WithOne(ws => ws.Doctor)
                .HasForeignKey(ws => ws.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.ScheduleExceptions)
                .WithOne(se => se.Doctor)
                .HasForeignKey(se => se.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>() 
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorPolicy>()
                .HasOne(dps => dps.Doctor)
                .WithMany(d => d.Policies)
                .HasForeignKey(dps => dps.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Adjust delete behavior as needed

            modelBuilder.Entity<DoctorPolicy>()
                .Property(dp => dp.PartialRefundPercentage)
                .HasPrecision(5, 2); // Adjust precision & scale as needed

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Policy)
                .WithMany()
                .HasForeignKey(a => a.PolicyId)
                .OnDelete(DeleteBehavior.SetNull); // 🔹 Prevents cascade delete

            // Doctor-Active Policy Relationship
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.ActivePolicy)
                .WithMany() // 🔹 A policy is shared, so no navigation back to Doctor
                .HasForeignKey(d => d.ActivePolicyId)
                .OnDelete(DeleteBehavior.NoAction); // 🔹 If policy is deleted, doctor should not break

            // Doctor-Policies Relationship (Each doctor can create multiple policies)
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Policies)
                .WithOne(dp => dp.Doctor)
                .HasForeignKey(dp => dp.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed default policy (Ensure ID=1 for easy reference)
            modelBuilder.Entity<DoctorPolicy>().HasData(
               new DoctorPolicy
               {
                   Id = 1,  // Default policy always has Id = 1
                   IsDefault = true,
                   //SlotDurationMinutes = 20,

                   // Cancellation Rules
                   AllowPatientCancellation = true,
                   MinCancellationHours = 24,
                   AllowLateCancellationReschedule = true,
                   MaxRescheduleAttempts = 1,

                   // Rescheduling Rules
                   AllowRescheduling = true,
                   MinRescheduleHours = 12,

                   // Refund & Payment Rules
                   AllowFullRefund = true,
                   AllowPartialRefund = true,
                   PartialRefundPercentage = 50,
                   RequirePrePayment = true,
                   UnpaidReservationTimeoutMinutes = 30,

                   // Doctor Availability
                   AllowMultipleBookingsPerDay = false,
                   MaxBookingsPerPatientPerDay = 1,
                   AllowLastMinuteBooking = true,
                   MinBookingAdvanceHours = 2,

                   // ✅ Fix: Use a static date to avoid model changes on each migration
                   CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
               }
            );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<ScheduleException> scheduleExceptions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorPolicy> DoctorPolicies { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("");
    }
}
