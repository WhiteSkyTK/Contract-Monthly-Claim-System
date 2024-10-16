using Contract_Monthly_Claim_System.Context;
using Contract_Monthly_Claim_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


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

        public async Task<IActionResult> Index()
        {
            var model = new SubmitClaimsViewModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Lecturer = await _context.Lecturers.SingleOrDefaultAsync(l => l.LecturerEmail == User.Identity.Name);
            }

            return View(model);
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

        public async Task<IActionResult> Submit()
        {
            var model = new SubmitClaimsViewModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Lecturer = await _context.Lecturers.SingleOrDefaultAsync(l => l.LecturerEmail == User.Identity.Name);
            }

            return View(model);
        }




        // GET: Register (Displays the registration form)
        public IActionResult Register()
        {
            return View();
        }

        // Password verification method using PBKDF2
        private bool VerifyPassword(string hashedPassword, string password, byte[] salt)
        {
            // Hash the provided password using the same salt
            string hashedInputPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Compare the hashed input password with the stored hashed password
            return hashedInputPassword == hashedPassword;
        }

        // Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username, string Password, bool RememberMe)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == Username);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "This user does not exist.");
                return View(); // Return to the login view
            }

            // Convert the stored salt back from base64
            byte[] salt = Convert.FromBase64String(user.Salt); // Assuming you have a Salt field in your Users table

            // Verify the password using PBKDF2
            if (!VerifyPassword(user.PasswordHash, Password, salt))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return View(); // Return to the login view
            }

            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect to the Submit Claim page
            return RedirectToAction("Submit", "Home");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaimsAsync(Claims claim, IFormFile SupportingDocuments)
        {
            if (ModelState.IsValid)
            {
                var lecturer = await _context.Lecturers.SingleOrDefaultAsync(l => l.LecturerEmail == User.Identity.Name);
                if (lecturer != null)
                {
                    claim.LecturerID = lecturer.LecturerID;
                    claim.SubmissionDate = DateTime.Now;
                    claim.Status = "Pending";

                    if (SupportingDocuments != null && SupportingDocuments.Length > 0)
                    {
                        var filePath = Path.Combine("wwwroot/uploads", SupportingDocuments.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await SupportingDocuments.CopyToAsync(stream);
                        }
                    }

                    _context.Claims.Add(claim);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TrackClaims");
                }
                else
                {
                    ModelState.AddModelError("", "Lecturer not found. Please log in.");
                }
            }

            return View(claim);
        }

        // POST: Register (Handles form submission)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                var (hashedPassword, salt) = HashPassword(lecturer.LecturerPassword);
                lecturer.LecturerPassword = hashedPassword;

                _context.Lecturers.Add(lecturer);

                var user = new Users
                {
                    Username = lecturer.LecturerEmail,
                    PasswordHash = hashedPassword,
                    Salt = Convert.ToBase64String(salt), // Store the salt
                    Role = "Lecturer"
                };
                _context.Users.Add(user);

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Register");
            }

            return View(lecturer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Password hashing method
        private (string hashedPassword, byte[] salt) HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return (hashed, salt);
        }
    }
}