using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerID { get; set; }
        [Required]
        public string LecturerName { get; set; }
        [Required]
        public string LecturerSurname { get; set; }
        [Required]
        public string LecturerPhone { get; set; }
        [Required]
        public string LecturerEmail { get; set; }
        [Required]
        public string LecturerPassword { get; set; }

        // Navigation properties (if needed)
        public ICollection<Claims> Claims { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<LecturerModules> LecturerModules { get; set; } = new List<LecturerModules>();
    }
}
