using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class Claims
    {
        [Key]
        public int ClaimID { get; set; }

        public string Module { get; set; }

        public int HoursWorked { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        public string Status { get; set; }

        public DateTime SubmissionDate { get; set; }

        public string AdditionalNotes { get; set; }

        [ForeignKey("Lecturer")]
        public int LecturerID { get; set; }

        public Lecturer Lecturer { get; set; }

        // Add a property for supporting document file path
        public string SupportingDocumentPath { get; set; }

        // Foreign key for Coordinator and Manager
        public int CoordinatorID { get; set; }
        public ProgrammeCoordintor Coordinator { get; set; }

        public int ManagerID { get; set; }
        public AcademicManager Manager { get; set; }

        // Relationship to approval process
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
    }
}

