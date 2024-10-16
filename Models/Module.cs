using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class Module
    {
        [Key]
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }

        public ICollection<ClaimsModules> ClaimsModules { get; set; } = new List<ClaimsModules>();
        public ICollection<LecturerModules> LecturerModules { get; set; } = new List<LecturerModules>();
    }
}
