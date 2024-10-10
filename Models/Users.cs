namespace Contract_Monthly_Claim_System.Models
{
    public class Users
    {
        public int userID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
