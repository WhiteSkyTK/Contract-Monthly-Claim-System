using System.Collections.Generic;
using System.Security.Claims;

namespace Contract_Monthly_Claim_System.Models
{
    public class ManageClaimsViewModel
    {
        public List<Claim> ApprovedClaims { get; set; }
        public List<Lecturer> Lecturers { get; set; }
        public List<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }
        public List<AcademicManager> AcademicManagers { get; set; }
        public List<ReportMetadata> ReportMetadata { get; set; }

        public ManageClaimsViewModel()
        {
            ApprovedClaims = new List<Claim>();
            Lecturers = new List<Lecturer>();
            ProgrammeCoordinators = new List<ProgrammeCoordinator>();
            AcademicManagers = new List<AcademicManager>();
        }
    }
}
