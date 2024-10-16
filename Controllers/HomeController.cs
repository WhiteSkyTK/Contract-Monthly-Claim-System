using Contract_Monthly_Claim_System.Context;
using Contract_Monthly_Claim_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;


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

        // Change Track to TrackClaims to retrieve claims
        public async Task<IActionResult> TrackClaims()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer) // Include lecturer info if needed
                .ToListAsync();

            return View(claims);
        }

        // Change this method to redirect to TrackClaims
        public IActionResult Track()
        {
            return RedirectToAction("TrackClaims");
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

        //Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Username, string Password, bool RememberMe)
        {
            // Fetch the user from the database using the provided username
            var user = _context.Users.SingleOrDefault(u => u.Username == Username);

            if (user == null)
            {
                // If user does not exist
                ModelState.AddModelError(string.Empty, "This user does not exist.");
                return View(); // Return to the login view
            }

            // Verify the password (assuming you have a method to hash and compare passwords)
            if (!VerifyPassword(user.PasswordHash, Password))
            {
                // If password is incorrect
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return View(); // Return to the login view
            }

            // If login is successful, set up authentication (if needed)
            // This example assumes you have a method to sign in the user
            SignInUser(user, RememberMe);

            // Redirect to the Submit Claim page
            return RedirectToAction("SubmitClaim", "Claims"); // Adjust as per your actual action/controller names
        }

        // Example method to sign in the user
        private void SignInUser(User user, bool rememberMe)
        {
            // You can set up authentication cookies here, or use any authentication method
            // For example, using ASP.NET Identity
        }

        // Example method to verify the password
        private bool VerifyPassword(string hashedPassword, string password)
        {
            // Implement your password verification logic here (e.g., using BCrypt)
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        //Form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaimsAsync(Claims claim, IFormFile SupportingDocuments)
        {
            if (ModelState.IsValid)
            {
                // Associate the current logged-in lecturer with the claim
                var lecturer = _context.Lecturers.SingleOrDefault(l => l.LecturerEmail == User.Identity.Name);
                if (lecturer != null)
                {
                    claim.LecturerID = lecturer.LecturerID;
                    claim.SubmissionDate = DateTime.Now;
                    claim.Status = "Pending"; // Default status when claim is submitted

                    // Handle file upload if there's a supporting document
                    if (SupportingDocuments != null && SupportingDocuments.Length > 0)
                    {
                        // Save the file, e.g., to wwwroot/uploads or a database
                        var filePath = Path.Combine("wwwroot/uploads", SupportingDocuments.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            SupportingDocuments.CopyTo(stream);
                        }

                        // You can store the file path or any additional info if needed
                    }

                    // Add the claim to the database
                    _context.Claims.Add(claim);
                    await _context.SaveChangesAsync(); // Use async save

                    return RedirectToAction("TrackClaims"); // Redirect to the tracking page
                }
                else
                {
                    // If no lecturer is found (unexpected case)
                    ModelState.AddModelError("", "Lecturer not found. Please log in.");
                }
            }

            // If validation fails or other issues, return the same view
            return View(claim);
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

                // Set success message
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";

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
