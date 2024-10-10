namespace Contract_Monthly_Claim_System.Models
{
    public class SupportingDocuments
    {
        public int DocumentID { get; set; }
        public DateTime UploaddDate { get; set; }
        public String FileName { get; set; }
        public String FilePath { get; set; }
        public int ClaimID { get; set; }
    }
}
