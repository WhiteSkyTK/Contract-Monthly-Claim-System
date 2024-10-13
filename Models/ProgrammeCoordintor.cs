using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ProgrammeCoordintor
    {
        [Key]
        public int CoordinatorID { get; set; }
        public string CoordintorName { get; set; }
        public string CoordintorSurname { get; set; }
        public string CoordintorEmail { get; set; }

        // Navigation property to Claims
        public ICollection<Claims> Claims { get; set; }

        // Navigation property to ApprovalProcess
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; }
    }
}
