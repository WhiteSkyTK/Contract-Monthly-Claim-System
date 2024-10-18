namespace Contract_Monthly_Claim_System.Models
{
    public class SubmitClaimsViewModel
    {
        public Claims Claim { get; set; }
        public Lecturer Lecturer { get; set; }
        public ProgrammeCoordinator ProgrammeCoordinator { get; set; }
        public AcademicManager AcademicManager { get; set; }
    }

}
