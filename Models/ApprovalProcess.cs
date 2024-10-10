namespace Contract_Monthly_Claim_System.Models
{
    public class ApprovalProcess
    {
        public int ApprovalID { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime ApprovalDate { get; set; }
        public int ClaimID { get; set; }
        public int CoordinatorID { get; set; }
        public int ManagerID { get; set; }
    }
}
