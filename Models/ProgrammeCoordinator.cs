using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ProgrammeCoordinator
    {
        [Key]
        public int CoordinatorID { get; set; }
        public string CoordinatorName { get; set; }
        public string CoordinatorSurname { get; set; }
        public string CoordinatorEmail { get; set; }
        public string CoordinatorPhone { get; set; }

        public ICollection<Claims> Claims { get; set; } = new List<Claims>();
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
    }
}
