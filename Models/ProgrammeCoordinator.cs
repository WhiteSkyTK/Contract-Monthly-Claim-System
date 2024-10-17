using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ProgrammeCoordinator
    {
        [Key]
        public int CoordinatorID { get; set; }
        [Required]
        public string CoordinatorName { get; set; }
        [Required]
        public string CoordinatorSurname { get; set; }
        [Required]
        public string CoordinatorEmail { get; set; }
        [Required]
        public string CoordinatorPhone { get; set; }
        [Required]
        public string CoordinatorPassword { get; set; }

        public ICollection<Claims> Claims { get; set; } = new List<Claims>();
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
    }
}
