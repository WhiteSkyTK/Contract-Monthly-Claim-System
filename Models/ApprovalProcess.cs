using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class ApprovalProcess
    {
        [Key]
        public int ApprovalID { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime ApprovalDate { get; set; }

        [ForeignKey("Claims")]
        public int ClaimID { get; set; }
        public Claims Claims { get; set; }

        [ForeignKey("Coordinator")]
        public int CoordinatorID { get; set; }
        public ProgrammeCoordinator Coordinator { get; set; }

        [ForeignKey("Manager")]
        public int ManagerID { get; set; }
        public AcademicManager Manager { get; set; }
    }
}
