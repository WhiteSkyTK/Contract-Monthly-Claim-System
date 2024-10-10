namespace Contract_Monthly_Claim_System.Models
{
    public class SupportingDocuments
    {
        public int DocumentID { get; set; }
        public DateTime UploaddDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int ClaimID { get; set; }
    }
}
