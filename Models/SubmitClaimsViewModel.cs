using System.Collections.Generic;
using System.Reflection;

namespace Contract_Monthly_Claim_System.Models
{
    public class SubmitClaimsViewModel
    {
        public Claims Claim { get; set; }
    public Lecturer Lecturer { get; set; } // Assuming this is a reference to the Lecturer model
    public ProgrammeCoordinator ProgrammeCoordinator { get; set; } // Optional
    public AcademicManager AcademicManager { get; set; } // Optional
    public List<string> Modules { get; set; } = new List<string>();
    public List<string> SelectedModules { get; set; } = new List<string>();

    // Add properties with getters and setters
    public int LecturerID { get; set; }  // Change this to have a setter
    public string LecturerName { get; set; } // Ensure there is a setter
    public string LecturerSurname { get; set; } // Ensure there is a setter
    public string LecturerPhone { get; set; } // Ensure there is a setter
    public string LecturerEmail { get; set; } // Ensure there is a setter

        public SubmitClaimsViewModel()
        {
            Modules = new List<string>();
            SelectedModules = new List<string>();
        }
    }

}
