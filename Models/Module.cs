using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class Module
    {
        [Key]
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }

        // Navigation property to LecturerModules
        public ICollection<LecturerModules> LecturerModules { get; set; } = new List<LecturerModules>();
    }
}

