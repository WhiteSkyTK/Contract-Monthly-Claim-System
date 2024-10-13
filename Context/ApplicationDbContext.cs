using Contract_Monthly_Claim_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Contract_Monthly_Claim_System.Context
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions)
        {
        }

        // DbSets for each model
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<ProgrammeCoordintor> ProgrammeCoordinators { get; set; }
        public DbSet<AcademicManager> AcademicManagers { get; set; }
        public DbSet<ApprovalProcess> ApprovalProcesses { get; set; }
        public DbSet<SupportingDocuments> SupportingDocuments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ClaimsModules> ClaimsModules { get; set; }
        public DbSet<LecturerModules> LecturerModules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring LecturerModules relationships with OnDelete restrict
            modelBuilder.Entity<LecturerModules>()
                .HasOne(lm => lm.Lecturer)
                .WithMany(l => l.LecturerModules)
                .HasForeignKey(lm => lm.LecturerID)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<LecturerModules>()
                .HasOne(lm => lm.Module)
                .WithMany(m => m.LecturerModules)
                .HasForeignKey(lm => lm.ModuleCode)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<ApprovalProcess>()
                .HasOne(a => a.Claims)
                .WithMany(c => c.ApprovalProcesses)
                .HasForeignKey(a => a.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seeding data for the Lecturer table
            modelBuilder.Entity<Lecturer>().HasData(
                new Lecturer { LecturerID = 1, LecturerName = "John", LecturerSurname = "Doe", LecturerPhone = "083-123-4567", LecturerEmail = "john.doe@example.com" }
            );

            // Seeding data for the Claims table
            modelBuilder.Entity<Claims>().HasData(
                new Claims { ClaimID = 1, LecturerID = 1, Module = "PROG6212", HoursWorked = 10, HourlyRate = 100, AdditionalNotes = "Worked on project.", CoordinatorID = 1, Status = "Pending" }
            );

            // Seeding data for the ProgrammeCoordinator table first
            modelBuilder.Entity<ProgrammeCoordintor>().HasData(
                new ProgrammeCoordintor { CoordinatorID = 1, CoordintorName = "Alice", CoordintorSurname = "Smith", CoordintorEmail = "Alicesmith@gmail.com" }
            );

            // Seeding data for the AcademicManager table
            modelBuilder.Entity<AcademicManager>().HasData(
                new AcademicManager { ManagerID = 1, ManagerName = "Bob", ManagerSurname = "Brown", ManagerEmail = "Bobrown@gmail.com" }
            );

            // Seeding data for the ApprovalProcess table
            modelBuilder.Entity<ApprovalProcess>().HasData(
                new ApprovalProcess { ApprovalID = 1, ClaimID = 1, ApprovalStatus = "Pending", ApprovalDate = DateTime.Now, CoordinatorID = 1, ManagerID = 1 }
            );

            // Add seeding for other tables/models similarly...

            // Apply similar logic for other entities that may cause cascade issues
        }


    }
}