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

        [ForeignKey("Module")]
        public string ModuleCode { get; set; }
        public Module Module { get; set; }

        public int HoursWorked { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string AdditionalNotes { get; set; }

        [ForeignKey("Lecturer")]
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        [ForeignKey("Coordinator")]
        public int CoordinatorID { get; set; }
        public ProgrammeCoordinator Coordinator { get; set; }

        [ForeignKey("Manager")]
        public int ManagerID { get; set; }
        public AcademicManager Manager { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalClaimAmount { get; set; }

        public virtual ICollection<ClaimsModules> ClaimsModules { get; set; } = new List<ClaimsModules>();
        public virtual ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
        public virtual ICollection<SupportingDocuments> SupportingDocuments { get; set; } = new List<SupportingDocuments>();
    }
}
