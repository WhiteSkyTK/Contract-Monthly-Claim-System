using System.Collections.Generic;
using System.Security.Claims;

namespace Contract_Monthly_Claim_System.Models
{
    public class ManageClaimsViewModel
    {
        public List<Claims> ApprovedClaims { get; set; } // Ensure this matches the claims model
        public List<Lecturer> Lecturers { get; set; }
        public List<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }
        public List<AcademicManager> AcademicManagers { get; set; }
        public List<Claims> Claims { get; set; } = new List<Claims>();

        public ManageClaimsViewModel()
        {
            ApprovedClaims = new List<Claims>(); // Updated to match the type
            Lecturers = new List<Lecturer>();
            ProgrammeCoordinators = new List<ProgrammeCoordinator>();
            AcademicManagers = new List<AcademicManager>();
        }
    }
}
