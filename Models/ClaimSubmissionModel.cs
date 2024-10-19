using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ClaimSubmissionModel
    {
        public Claims Claim { get; set; }

        [Required(ErrorMessage = "The Lecturer field is required.")]
        public Lecturer Lecturer { get; set; }

        [Required(ErrorMessage = "Please select at least one module.")]
        public List<string> Modules { get; set; } // List to store module names

        public List<string> SelectedModules { get; set; } // Store selected modules

        public ClaimSubmissionModel()
        {
            Modules = new List<string>(); // Initialize to an empty list
            SelectedModules = new List<string>(); // Initialize to store selected modules
        }
    }
}
