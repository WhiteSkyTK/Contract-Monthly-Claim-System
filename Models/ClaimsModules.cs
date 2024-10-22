using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class ClaimsModules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure auto-increment
        public int ClaimsModulesID { get; set; }

        [ForeignKey("Claims")]
        public int ClaimID { get; set; }
        public Claims Claims { get; set; }

        [ForeignKey("Module")]
        public string ModuleCode { get; set; }
        public Module Module { get; set; }
    }

}
