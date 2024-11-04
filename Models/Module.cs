using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class Module
    {
        [Key]
        [Required]
        [RegularExpression(@"^[A-Z]{4}\d{4}$", ErrorMessage = "Module Code must be 4 uppercase letters followed by 4 digits (e.g., DATA6222).")]
        public string ModuleCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Module Name cannot exceed 100 characters.")]
        public string ModuleName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public ICollection<ClaimsModules> ClaimsModules { get; set; } = new List<ClaimsModules>();
    }
}
