using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class SupportingDocuments
    {
        [Key]
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmissionDate { get; set; }

        [ForeignKey("Claims")]
        public int ClaimID { get; set; }
        public Claims Claims { get; set; }
    }
}
