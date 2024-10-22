using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Contract_Monthly_Claim_System.Models
{
    public class ClaimSubmissionInfo
    {
        // Claim details
        public Claims Claim { get; set; }

        // List of available modules for selection
        public List<string> Modules { get; set; } = new List<string>();

        // Modules selected for the claim
        [Required(ErrorMessage = "Please select at least one module.")]
        public List<string> SelectedModules { get; set; } = new List<string>();

        // Lecturer information
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string LecturerSurname { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerEmail { get; set; }

        // Supporting document for the claim
        public IFormFile SupportingDocument { get; set; } // Singular, assuming one document per claim
    }
}
