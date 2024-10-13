using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class SupportingDocuments
    {
        [Key]
        public int DocumentID { get; set; }
        public DateTime UploaddDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [ForeignKey("Claims")]
        public int ClaimID { get; set; }
        public Claims Claims { get; set; }
    }
}
