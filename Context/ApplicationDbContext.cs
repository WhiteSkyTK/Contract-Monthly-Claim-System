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
        public DbSet<Users> Users { get; set; }
        public DbSet<HR> HRs { get; set; }
        public DbSet<ReportMetadata> ReportMetadata { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClaimsModules>()
                .HasKey(cm => new { cm.ClaimID, cm.ModuleCode });

            modelBuilder.Entity<Claims>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Claims)
                .HasForeignKey(c => c.LecturerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claims>()
                .HasMany(c => c.ClaimsModules)
                .WithOne(cm => cm.Claims)
                .HasForeignKey(cm => cm.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure foreign key relationship with Claims
            modelBuilder.Entity<ClaimsModules>()
                .HasOne(cm => cm.Claims)
                .WithMany(c => c.ClaimsModules)
                .HasForeignKey(cm => cm.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure foreign key relationship with Module
            modelBuilder.Entity<ClaimsModules>()
                .HasOne(cm => cm.Module)
                .WithMany(m => m.ClaimsModules)
                .HasForeignKey(cm => cm.ModuleCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalProcess>()
                .HasOne(a => a.Manager)
                .WithMany(am => am.ApprovalProcesses)
                .HasForeignKey(a => a.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalProcess>()
                .HasOne(a => a.Coordinator)
                .WithMany(pc => pc.ApprovalProcesses)
                .HasForeignKey(a => a.CoordinatorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Add foreign key relationships for ReportMetadata
            modelBuilder.Entity<ReportMetadata>()
                .HasOne(rm => rm.Claims)
                .WithMany(c => c.ReportMetadata)
                .HasForeignKey(rm => rm.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReportMetadata>()
                .HasOne(rm => rm.Lecturer)
                .WithMany(l => l.ReportMetadata)
                .HasForeignKey(rm => rm.LecturerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReportMetadata>()
                .HasOne(rm => rm.ApprovalProcess)
                .WithMany(ap => ap.ReportMetadata)
                .HasForeignKey(rm => rm.ApprovalID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
