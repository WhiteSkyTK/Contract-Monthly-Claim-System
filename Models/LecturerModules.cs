using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class LecturerModules
    {
        [Key]
        public int LecturerModulesID { get; set; }

        [ForeignKey("Lecturer")]
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        [ForeignKey("Module")]
        public string ModuleCode { get; set; }
        public Module Module { get; set; }
    }
}
