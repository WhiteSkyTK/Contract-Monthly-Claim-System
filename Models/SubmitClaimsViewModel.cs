﻿using System.Reflection;

namespace Contract_Monthly_Claim_System.Models
{
    public class SubmitClaimsViewModel
    {
        public Claims Claim { get; set; }
        public Lecturer Lecturer { get; set; }
        public ProgrammeCoordinator ProgrammeCoordinator { get; set; }
        public AcademicManager AcademicManager { get; set; }

        // Add the Modules property
        public List<string> Modules { get; set; } // List to store module names

        public List<string> SelectedModules { get; set; } // Store selected modules

        public SubmitClaimsViewModel()
        {
            Modules = new List<string>(); // Initialize to an empty list
            SelectedModules = new List<string>(); // Initialize to store selected modules
        }
    }
}

