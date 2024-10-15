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

            
        }


    }
}