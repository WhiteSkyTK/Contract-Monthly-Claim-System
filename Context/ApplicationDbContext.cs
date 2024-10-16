using Contract_Monthly_Claim_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Contract_Monthly_Claim_System.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }
        public DbSet<AcademicManager> AcademicManagers { get; set; }
        public DbSet<ApprovalProcess> ApprovalProcesses { get; set; }
        public DbSet<SupportingDocuments> SupportingDocuments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ClaimsModules> ClaimsModules { get; set; }
        public DbSet<LecturerModules> LecturerModules { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LecturerModules>()
                .HasKey(lm => new { lm.LecturerID, lm.ModuleCode });

            modelBuilder.Entity<ClaimsModules>()
                .HasKey(cm => new { cm.ClaimID, cm.ModuleCode });

            modelBuilder.Entity<Claims>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Claims)
                .HasForeignKey(c => c.LecturerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claims>()
                .HasOne(c => c.Coordinator)
                .WithMany(c => c.Claims)
                .HasForeignKey(c => c.CoordinatorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claims>()
                .HasOne(c => c.Manager)
                .WithMany(m => m.Claims)
                .HasForeignKey(c => c.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claims>()
                .HasMany(c => c.ClaimsModules)
                .WithOne(cm => cm.Claims)
                .HasForeignKey(cm => cm.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClaimsModules>()
                .HasOne(cm => cm.Module)
                .WithMany(m => m.ClaimsModules)
                .HasForeignKey(cm => cm.ModuleCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalProcess>()
                .HasOne(a => a.Claims)
                .WithMany(c => c.ApprovalProcesses)
                .HasForeignKey(a => a.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
