using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class Claims
    {
        [Key]
        public int ClaimID { get; set; }

        public int HoursWorked { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string AdditionalNotes { get; set; }

        [ForeignKey("Lecturer")]
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalClaimAmount { get; set; }
        public string RejectionFeedback { get; set; }

        public ICollection<ClaimsModules> ClaimsModules { get; set; } = new List<ClaimsModules>();
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
        public ICollection<SupportingDocuments> SupportingDocuments { get; set; } = new List<SupportingDocuments>();
    }
}
