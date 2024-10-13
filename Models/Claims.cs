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

        [Column(TypeName = "decimal(18,2)")] // Specify precision and scale
        public decimal HourlyRate { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string AdditionalNotes { get; set; }
        [ForeignKey("Lecturer")]
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        [ForeignKey("Coordinator")]
        public int CoordinatorID { get; set; }
        public ProgrammeCoordintor Coordinator { get; set; }

        [ForeignKey("Manager")]
        public int ManagerID { get; set; }
        public AcademicManager Manager { get; set; }
        // Add this property to represent the one-to-many relationship
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; }
    }
}

