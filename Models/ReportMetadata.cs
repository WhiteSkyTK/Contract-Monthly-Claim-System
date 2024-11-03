using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class ReportMetadata
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public string ReportName { get; set; }

        public string ReportType { get; set; } // e.g., Invoice, Claim Summary, etc.

        public DateTime DateGenerated { get; set; }

        public string FilePath { get; set; } // File path on the server

        public decimal TotalApprovedClaims { get; set; }

        // Optional foreign key to Claims
        [ForeignKey("Claims")]
        public int? ClaimID { get; set; }
        public Claims Claims { get; set; }

        // Optional foreign key to Lecturer
        [ForeignKey("Lecturer")]
        public int? LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        // Optional foreign key to ApprovalProcess (for reports related to approval status)
        [ForeignKey("ApprovalProcess")]
        public int? ApprovalID { get; set; }
        public ApprovalProcess ApprovalProcess { get; set; }

        public string GeneratedBy { get; set; } // The user who generated the report

        public ICollection<ManageClaimsViewModel> ManageClaimsViewModels { get; set; } = new List<ManageClaimsViewModel>();
    }
}
