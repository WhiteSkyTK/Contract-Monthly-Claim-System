using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class LecturerModules
    {
        [Key]
        public int LecturerModulesID { get; set; }

        // Foreign Key to Lecturer
        [ForeignKey("Lecturer")]
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        // Foreign Key to Module
        [ForeignKey("Module")]
        public string ModuleCode { get; set; }
        public Module Module { get; set; }
    }
}
