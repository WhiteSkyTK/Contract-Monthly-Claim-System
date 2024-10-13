using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string LecturerSurname { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerEmail { get; set; }
        [Column(TypeName = "decimal(18,2)")] // Specify precision and scale
        public decimal HourlyRate { get; set; }

        // Navigation property to Claims
        public ICollection<Claims> Claims { get; set; }

        // Navigation property to Module
        public ICollection<Module> Modules { get; set; }

        // Navigation property to LecturerModules
        public ICollection<LecturerModules> LecturerModules { get; set; } = new List<LecturerModules>();
    }
}
