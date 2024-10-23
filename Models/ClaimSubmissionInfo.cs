using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Contract_Monthly_Claim_System.Models
{
    [NotMapped] // This prevents EF from mapping the entire class to the database
    public class ClaimSubmissionInfo
    {
        // Claim details
        public Claims Claim { get; set; }

        // Approval process details
        public ApprovalProcess ApprovalProcess { get; set; }

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
