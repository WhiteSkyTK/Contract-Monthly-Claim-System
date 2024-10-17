using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class AcademicManager
    {
        [Key]
        public int ManagerID { get; set; }
        [Required]
        public string ManagerName { get; set; }
        [Required]
        public string ManagerSurname { get; set; }
        [Required]
        public string ManagerEmail { get; set; }
        [Required]
        public string ManagerPhone { get; set; }
        [Required]
        public string ManagerPassword { get; set; }

        public ICollection<Claims> Claims { get; set; } = new List<Claims>();
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
    }
}
