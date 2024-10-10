namespace Contract_Monthly_Claim_System.Models
{
    public class Claims
    {
        public int ClaimID { get; set; }
        public decimal HoursWorked { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int LecturerID { get; set; }
        public int CoordinatorID { get; set; }
        public int ManagerID { get; set; }
    }
}
