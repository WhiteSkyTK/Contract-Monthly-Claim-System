using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class ClaimsModules
    {
        [Key]
        public int ClaimsModulesID { get; set; }

        // Foreign Key to Claims
        [ForeignKey("Claims")]
        public int ClaimID { get; set; }
        public Claims Claims { get; set; }

        // Foreign Key to Module
        [ForeignKey("Module")]
        public string ModuleCode { get; set; }
        public Module Module { get; set; }
    }
}
