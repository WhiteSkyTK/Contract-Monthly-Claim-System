using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ClaimSubmissionDTO
    {
        public Claims Claim { get; set; }
        // List of available modules for selection
        public List<string> Modules { get; set; } = new List<string>();
        // Modules selected for the claim
        [Required(ErrorMessage = "Please select at least one module.")]
        public List<string> SelectedModules { get; set; } = new List<string>();

        
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string LecturerSurname { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerEmail { get; set; }

        // Supporting document for the claim
        public IFormFile SupportingDocuments { get; set; }
    }

}
