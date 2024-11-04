using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class EditViewModel
    {
        public Lecturer? Lecturer { get; set; }
        public ProgrammeCoordinator? ProgrammeCoordinator { get; set; }
        public AcademicManager? AcademicManager { get; set; }
        [Required]
        public string Role { get; set; } // Add this property

        public List<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public List<ProgrammeCoordinator> ProgrammeCoordinators { get; set; } = new List<ProgrammeCoordinator>();
        public List<AcademicManager> AcademicManagers { get; set; } = new List<AcademicManager>();
    }
}
