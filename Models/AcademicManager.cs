﻿using System.Collections.Generic;
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

        public ICollection<Claims> Claims { get; set; } = new List<Claims>();
        public ICollection<ApprovalProcess> ApprovalProcesses { get; set; } = new List<ApprovalProcess>();
    }
}
