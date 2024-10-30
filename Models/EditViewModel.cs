using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class EditViewModel
    {
        // Property to hold the search term entered by the HR user
        [Display(Name = "Search Term")]
        public string? SearchTerm { get; set; }

        // Indicates if the search was performed
        public bool SearchPerformed { get; set; }

        // Indicates if a user was found after the search
        public bool UserFound => User != null;

        // Property to hold the user details being edited
        public UserViewModel? User { get; set; }

        // Inner class to represent a user with editable properties
        public class UserViewModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; } = string.Empty;
        }
    }
}
