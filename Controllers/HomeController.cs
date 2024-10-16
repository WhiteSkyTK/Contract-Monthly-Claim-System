using Contract_Monthly_Claim_System.Context;
using Contract_Monthly_Claim_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Contract_Monthly_Claim_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Add DbContext

        // Inject DbContext in the constructor
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerifyClaims()
        {
            return View();
        }

        public IActionResult Track()
        {
            return View();
        }

        public IActionResult Submit()
        {
            return View();
        }

        // GET: Register (Displays the registration form)
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register (Handles form submission)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before storing
                var hashedPassword = HashPassword(lecturer.LecturerPassword);
                lecturer.LecturerPassword = hashedPassword;

                // Add the lecturer to the Lecturers table
                _context.Lecturers.Add(lecturer);

                // Create a user entry for login purposes
                var user = new Users
                {
                    Username = lecturer.LecturerEmail, // Use LecturerEmail as the username
                    PasswordHash = hashedPassword,      // Store the hashed password
                    Role = "Lecturer"                   // Set role as "Lecturer"
                };
                _context.Users.Add(user);

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to a confirmation page or home page
                return RedirectToAction("Index");
            }

            // If the model is invalid, return the same view with validation messages
            return View(lecturer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Password hashing method
        private string HashPassword(string password)
        {
            // Generate a salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using PBKDF2 algorithm
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
