using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class HR
    {
        [Key]
        public int HRID { get; set; }

        [Required]
        public string HRName { get; set; }

        [Required]
        public string HRSurname { get; set; }

        [Required]
        public string HREmail { get; set; }

        [Required]
        public string HRPhone { get; set; }

        [Required]
        public string HRPassword { get; set; }

        // Additional properties and collections as needed
    }
}
