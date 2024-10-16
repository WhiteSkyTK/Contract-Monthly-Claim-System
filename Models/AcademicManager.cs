using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class AcademicManager
    {
        [Key]
        public int ManagerID { get; set; }
        public string ManagerName { get; set; }
        public string ManagerSurname { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPhone { get; set; }

        // Navigation property to Claims
        public ICollection<Claims> Claims { get; set; }

        // Navigation property to ApprovalProcess
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; }
    }
}
