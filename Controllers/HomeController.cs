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
using Microsoft.AspNetCore.Authorization;


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

        public IActionResult Login()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }
            return View();
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
            byte[] salt = Convert.FromBase64String(user.Salt);

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

            // Redirect to the appropriate landing page based on role
            if (user.Role == "Lecturer")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (user.Role == "Programme Coordinator")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (user.Role == "Academic Manager")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Default action if role is not recognized
                return RedirectToAction("Index", "Home");
            }
        }

        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "You have been logged out."; // Optionally, set a success message
            return RedirectToAction("Login", "Home"); // Redirect to the Login action
        }

        public IActionResult RegisterM()
        {
            return View();
        }

        // Handles form submission for lecturer registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterM(AcademicManager academicManager)
        {
            // Check if the email already exists
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == academicManager.ManagerEmail);
            if (existingUser != null)
            {
                ModelState.AddModelError("MangerEmail", "This email is already registered.");
                return View(academicManager);
            }

            if (ModelState.IsValid)
            {
                var (hashedPassword, salt) = HashPassword(academicManager.ManagerPassword);
                academicManager.ManagerPassword = hashedPassword;

                _context.AcademicManagers.Add(academicManager);

                var user = new Users
                {
                    Username = academicManager.ManagerEmail,
                    PasswordHash = hashedPassword,
                    Salt = Convert.ToBase64String(salt),
                    Role = "Academic Manager"
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }

            return View(academicManager);
        }

        public IActionResult RegisterC()
        {
            return View();
        }

        // POST: RegisterC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterC(ProgrammeCoordinator coordinator)
        {
            // Check if email already exists in the Users table
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == coordinator.CoordinatorEmail);

            if (existingUser != null)
            {
                ModelState.AddModelError("CoordinatorEmail", "This email is already registered.");
                return View(coordinator);
            }

            if (ModelState.IsValid)
            {
                var (hashedPassword, salt) = HashPassword(coordinator.CoordinatorPassword);
                coordinator.CoordinatorPassword = hashedPassword;

                var user = new Users
                {
                    Username = coordinator.CoordinatorEmail,
                    PasswordHash = hashedPassword,
                    Salt = Convert.ToBase64String(salt),
                    Role = "Programme Coordinator"
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                _context.ProgrammeCoordinators.Add(coordinator);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Account created successfully!";
                return RedirectToAction("Index", "Home");
            }

            return View(coordinator);
        }

        public async Task<IActionResult> About()
        {
            var userEmail = User.Identity.Name; // Fetch the logged-in user's email
            var model = new SubmitClaimsViewModel();

            if (User.IsInRole("Programme Coordinator"))
            {
                var coordinator = await GetProgrammeCoordinator();
                if (coordinator == null)
                {
                    TempData["ErrorMessage"] = "Programme Coordinator not found.";
                    return RedirectToAction("Index");
                }

                model.ProgrammeCoordinator = coordinator;

                var academicManager = await GetAcademicManager();
                model.AcademicManager = academicManager; // Use the retrieved Academic Manager
            }
            else if (User.IsInRole("Lecturer"))
            {
                var lecturer = await _context.Lecturers
                    .SingleOrDefaultAsync(l => l.LecturerEmail == userEmail);
                if (lecturer == null)
                {
                    TempData["ErrorMessage"] = "Lecturer not found.";
                    return RedirectToAction("Index");
                }

                model.Lecturer = lecturer; // Add lecturer details to the model
            }
            else if (User.IsInRole("Academic Manager"))
            {
                var manager = await _context.AcademicManagers
                    .SingleOrDefaultAsync(l => l.ManagerEmail == userEmail);
                if (manager == null)
                {
                    TempData["ErrorMessage"] = "Lecturer not found.";
                    return RedirectToAction("Index");
                }

                model.AcademicManager = manager; // Add lecturer details to the model
            }
            else
            {
                TempData["ErrorMessage"] = "User role not recognized.";
                return RedirectToAction("Index");
            }

            return View(model); // Return the model to the view
        }

        private async Task<ProgrammeCoordinator> GetProgrammeCoordinator()
        {
            var userEmail = User.Identity.Name; // Fetch the logged-in user's email
            return await _context.ProgrammeCoordinators
                .SingleOrDefaultAsync(pc => pc.CoordinatorEmail == userEmail);
        }

        private async Task<AcademicManager> GetAcademicManager()
        {
            var userEmail = User.Identity.Name; // Fetch the logged-in user's email
            return await _context.AcademicManagers
                .SingleOrDefaultAsync(am => am.ManagerEmail == userEmail);
        }

        public async Task<IActionResult> Index()
        {
            var model = new SubmitClaimsViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name; // Fetch the logged-in user's email

                if (User.IsInRole("Lecturer"))
                {
                    // Fetch the lecturer details from the database asynchronously
                    var lecturer = await _context.Lecturers
                        .SingleOrDefaultAsync(l => l.LecturerEmail == userEmail);

                    if (lecturer == null)
                    {
                        // Lecturer not found in the database
                        TempData["ErrorMessage"] = "Lecturer not found in the system.";
                        return View(model); // Return the view with an error message
                    }

                    model.Lecturer = lecturer;
                }
                else if (User.IsInRole("Programme Coordinator"))
                {
                    // Fetch the program coordinator details from the database asynchronously
                    var coordinator = await _context.ProgrammeCoordinators
                        .SingleOrDefaultAsync(pc => pc.CoordinatorEmail == userEmail);

                    if (coordinator == null)
                    {
                        // Coordinator not found in the database
                        TempData["ErrorMessage"] = "Programme Coordinator not found in the system.";
                        return View(model); // Return the view with an error message
                    }

                    model.ProgrammeCoordinator = coordinator;
                }
                //Academic Manager
                else if (User.IsInRole("Academic Manager"))
                {
                    // Fetch the program coordinator details from the database asynchronously
                    var manager = await _context.AcademicManagers
                        .SingleOrDefaultAsync(pc => pc.ManagerEmail == userEmail);

                    if (manager == null)
                    {
                        // Coordinator not found in the database
                        TempData["ErrorMessage"] = "Academic Manager not found in the system.";
                        return View(model); // Return the view with an error message
                    }

                    model.AcademicManager = manager;
                }
                else
                {
                    TempData["ErrorMessage"] = "User role not recognized.";
                    return View(model); // Return the view with an error message
                }

                return View(model); // Return the view with appropriate user details (lecturer or coordinator)
            }

            return View(); // Return the guest view if not authenticated
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
            // Check if email already exists in the Users table
            var existingUser = _context.Users.SingleOrDefault(u => u.Username == lecturer.LecturerEmail);

            if (existingUser != null)
            {
                ModelState.AddModelError("LecturerEmail", "This email is already registered.");
                return View(lecturer);
            }

            if (ModelState.IsValid)
            {
                var (hashedPassword, salt) = HashPassword(lecturer.LecturerPassword);
                lecturer.LecturerPassword = hashedPassword;

                _context.Lecturers.Add(lecturer);

                var user = new Users
                {
                    Username = lecturer.LecturerEmail,
                    PasswordHash = hashedPassword,
                    Salt = Convert.ToBase64String(salt),
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

        // GET: Submit Claims
        public IActionResult Submit()
        {
            var lecturerEmail = User.Identity.Name;
            var lecturer = _context.Lecturers.SingleOrDefault(l => l.LecturerEmail == lecturerEmail);

            // Initialize the ClaimSubmissionModel
            var model = new ClaimSubmissionModel
            {
                Lecturer = lecturer,
                Claim = new Claims(), // Initialize the Claim property
                Modules = _context.Modules.Select(m => m.ModuleCode).ToList() // Assuming Modules has a ModuleCode property
            };

            return View(model);
        }

        // POST: Submit Claims
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ClaimSubmissionModel model, List<string> selectedModules, IFormFile SupportingDocuments)
        {
            // Get the logged-in lecturer
            var lecturer = await _context.Lecturers.SingleOrDefaultAsync(l => l.LecturerEmail == User.Identity.Name);
            if (lecturer == null)
            {
                ModelState.AddModelError("", "Lecturer not found.");
                return View(model);
            }

            // Ensure the model state is valid and at least one module is selected
            if (!ModelState.IsValid)
            {
                // Repopulate the modules for the view if validation fails
                model.Modules = await _context.Modules.Select(m => m.ModuleCode).ToListAsync(); // Use ModuleCode
                model.Lecturer = lecturer;
                return View(model);
            }

            if (selectedModules == null || !selectedModules.Any())
            {
                ModelState.AddModelError("SelectedModules", "Please select at least one module.");
                return View(model);
            }

            // Create a new claim object
            var claim = new Claims
            {
                HoursWorked = model.Claim.HoursWorked,
                HourlyRate = model.Claim.HourlyRate,
                SubmissionDate = DateTime.Now,
                AdditionalNotes = model.Claim.AdditionalNotes,
                LecturerID = lecturer.LecturerID, // Assign the lecturer ID here
                Status = "Pending",
                TotalClaimAmount = model.Claim.HoursWorked * model.Claim.HourlyRate * selectedModules.Count,
                ClaimsModules = new List<ClaimsModules>()
            };

            // Handle the selected modules
            foreach (var moduleCode in selectedModules)
            {
                claim.ClaimsModules.Add(new ClaimsModules
                {
                    ModuleCode = moduleCode, // Ensure you're storing the ModuleCode
                    Claims = claim
                });
            }

            // Handle the file upload
            if (SupportingDocuments != null && SupportingDocuments.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, SupportingDocuments.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SupportingDocuments.CopyToAsync(stream);
                }

                claim.SupportingDocuments.Add(new SupportingDocuments
                {
                    DocumentName = SupportingDocuments.FileName,
                    FilePath = filePath,
                    SubmissionDate = DateTime.Now,
                    Claims = claim
                });
            }

            // Save the claim to the database
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Claim submitted successfully!";
            return RedirectToAction("TrackClaims"); // Redirect after post to prevent duplicate submissions
        }
    }
}
