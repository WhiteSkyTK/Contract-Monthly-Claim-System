using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ClaimSubmissionModel
    {
        public Claims Claim { get; set; }
        public List<string> Modules { get; set; } = new List<string>();
        // This property will hold the selected modules
        [Required(ErrorMessage = "Please select at least one module.")]
        public List<string> SelectedModules { get; set; } = new List<string>();

        // Add these properties as needed
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string LecturerSurname { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerEmail { get; set; }
    }
}
