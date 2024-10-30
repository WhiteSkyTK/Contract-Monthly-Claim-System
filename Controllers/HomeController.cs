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
        // Mock data for demonstration purposes
        private static List<EditViewModel.UserViewModel> users = new List<EditViewModel.UserViewModel>
        {
            new EditViewModel.UserViewModel { Name = "John Doe", Email = "john.doe@example.com", Role = "Lecturer" },
            new EditViewModel.UserViewModel { Name = "Jane Smith", Email = "jane.smith@example.com", Role = "Programme Coordinator" },
            new EditViewModel.UserViewModel { Name = "Alice Johnson", Email = "alice.johnson@example.com", Role = "Academic Manager" }
        };

        // Inject DbContext in the constructor
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Edit
        public IActionResult Edit()
        {
            var model = new EditViewModel();
            return View(model);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                ModelState.AddModelError("", "Search term cannot be empty.");
                return View(model);
            }

            // Simulating search for a user based on search term (could be email or name)
            var user = users.FirstOrDefault(u => u.Email.Equals(model.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                  u.Name.Equals(model.SearchTerm, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                model.User = user;
                model.SearchPerformed = true;
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
            }

            return View(model);
        }

        // POST: Update User
        [HttpPost]
        public IActionResult UpdateUser(EditViewModel model)
        {
            if (ModelState.IsValid && model.User != null)
            {
                // Here, update the user in the database or the mock list
                var userToUpdate = users.FirstOrDefault(u => u.Email.Equals(model.User.Email, StringComparison.OrdinalIgnoreCase));
                if (userToUpdate != null)
                {
                    userToUpdate.Name = model.User.Name;
                    userToUpdate.Role = model.User.Role; // Assuming role can be changed
                }
                return RedirectToAction("Index"); // Redirect to a suitable page after update
            }
            return View("Edit", model);
        }

        // GET: Submit Claims
        public IActionResult Submit()
        {
            var lecturerEmail = User.Identity.Name; // Gets the currently logged-in user's email
            var lecturer = _context.Lecturers.SingleOrDefault(l => l.LecturerEmail == lecturerEmail);

            if (lecturer == null)
            {
                ModelState.AddModelError("", "Lecturer not found.");
                return View(new ClaimSubmissionDTO()); // Return a new model if lecturer not found
            }

            // Populate the model with the lecturer's information from the database
            var model = new ClaimSubmissionDTO
            {
                LecturerID = lecturer.LecturerID,
                LecturerName = lecturer.LecturerName,
                LecturerSurname = lecturer.LecturerSurname,
                LecturerPhone = lecturer.LecturerPhone,
                LecturerEmail = lecturer.LecturerEmail,
                Claim = new Claims(),
                Modules = _context.Modules.Select(m => m.ModuleCode).ToList() // Populate available modules
            };

            return View(model);
        }

        // POST: Submit Claims
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ClaimSubmissionDTO model, IFormFile SupportingDocuments)
        {
            var lecturer = await _context.Lecturers.SingleOrDefaultAsync(l => l.LecturerEmail == User.Identity.Name);

            if (lecturer == null)
            {
                ModelState.AddModelError("", "Lecturer not found.");
                return View(model);
            }

            // Programmatically populate lecturer fields in the model
            model.LecturerID = lecturer.LecturerID;
            model.LecturerName = lecturer.LecturerName;
            model.LecturerSurname = lecturer.LecturerSurname;
            model.LecturerPhone = lecturer.LecturerPhone;
            model.LecturerEmail = lecturer.LecturerEmail;

            // Remove lecturer-related validation
            ModelState.Remove("Lecturer");
            ModelState.Remove("LecturerName");
            ModelState.Remove("LecturerSurname");
            ModelState.Remove("LecturerPhone");
            ModelState.Remove("LecturerEmail");

            // Bypass ModelState validation for any remaining errors (force submission)
            if (!ModelState.IsValid)
            {
                // Ignore errors related to the Lecturer fields
                var errorsToRemove = new[] { "Lecturer", "LecturerName", "LecturerSurname", "LecturerPhone", "LecturerEmail" };
                foreach (var errorKey in errorsToRemove)
                {
                    ModelState.Remove(errorKey);
                }
            }

            // Handle selected modules validation
            if (model.SelectedModules == null || !model.SelectedModules.Any())
            {
                ModelState.AddModelError("SelectedModules", "Please select at least one module.");
                model.Modules = await _context.Modules.Select(m => m.ModuleCode).ToListAsync(); // Repopulate on error
                return View(model);
            }

            // Create a new claim object
            var claim = new Claims
            {
                HoursWorked = model.Claim.HoursWorked,
                HourlyRate = model.Claim.HourlyRate,
                SubmissionDate = DateTime.Now,
                AdditionalNotes = model.Claim.AdditionalNotes,
                LecturerID = model.LecturerID,
                Status = "Pending",
                TotalClaimAmount = model.Claim.HoursWorked * model.Claim.HourlyRate * model.SelectedModules.Count,
                ClaimsModules = new List<ClaimsModules>()
            };

            // Handle the selected modules
            foreach (var moduleCode in model.SelectedModules)
            {
                claim.ClaimsModules.Add(new ClaimsModules
                {
                    ModuleCode = moduleCode,
                    Claims = claim
                });
            }

            // Handle file upload
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
            return RedirectToAction("TrackClaims");
        }

        public IActionResult Login()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }
            return View();
        }

        // POST: Login
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
        new Claim(ClaimTypes.Role, user.Role),
        new Claim(ClaimTypes.NameIdentifier, user.userID.ToString()) // Add the user ID claim
    };

            // Add role-specific claims for Coordinators, Managers, and HR
            if (user.Role == "Programme Coordinator")
            {
                claims.Add(new Claim("CoordinatorID", user.userID.ToString())); // Add CoordinatorID claim
            }
            else if (user.Role == "Academic Manager")
            {
                claims.Add(new Claim("ManagerID", user.userID.ToString())); // Add ManagerID claim
            }
            else if (user.Role == "HR")
            {
                claims.Add(new Claim("HRID", user.userID.ToString())); // Add HRID claim
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
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

        // GET: RegisterH
        public IActionResult RegisterH()
        {
            return View();
        }

        // POST: RegisterH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterH(HR hr)
        {
            // Check if email already exists in the Users table
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == hr.HREmail);

            if (existingUser != null)
            {
                ModelState.AddModelError("HREmail", "This email is already registered.");
                return View(hr);
            }

            if (ModelState.IsValid)
            {
                // Hash the password
                var (hashedPassword, salt) = HashPassword(hr.HRPassword);
                hr.HRPassword = hashedPassword;

                // Create a new user entry
                var user = new Users
                {
                    Username = hr.HREmail,
                    PasswordHash = hashedPassword,
                    Salt = Convert.ToBase64String(salt),
                    Role = "HR"
                };

                // Add user and HR details to the database
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                _context.HRs.Add(hr);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "HR account created successfully!";
                return RedirectToAction("Index", "Home");
            }

            return View(hr);
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
                    TempData["ErrorMessage"] = "Academic Manager not found.";
                    return RedirectToAction("Index");
                }

                model.AcademicManager = manager; // Add Academic Manager details to the model
            }
            else if (User.IsInRole("HR"))
            {
                var hr = await _context.HRs
                    .SingleOrDefaultAsync(h => h.HREmail == userEmail);
                if (hr == null)
                {
                    TempData["ErrorMessage"] = "HR not found.";
                    return RedirectToAction("Index");
                }

                model.HR = hr; // Add HR details to the model
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
                else if (User.IsInRole("Academic Manager"))
                {
                    // Fetch the academic manager details from the database asynchronously
                    var manager = await _context.AcademicManagers
                        .SingleOrDefaultAsync(pc => pc.ManagerEmail == userEmail);

                    if (manager == null)
                    {
                        // Academic Manager not found in the database
                        TempData["ErrorMessage"] = "Academic Manager not found in the system.";
                        return View(model); // Return the view with an error message
                    }

                    model.AcademicManager = manager;
                }
                else if (User.IsInRole("HR"))
                {
                    // Fetch the HR details from the database asynchronously
                    var hr = await _context.HRs
                        .SingleOrDefaultAsync(h => h.HREmail == userEmail);

                    if (hr == null)
                    {
                        // HR not found in the database
                        TempData["ErrorMessage"] = "HR not found in the system.";
                        return View(model); // Return the view with an error message
                    }

                    model.HR = hr; // Add HR details to the model
                }
                else
                {
                    TempData["ErrorMessage"] = "User role not recognized.";
                    return View(model); // Return the view with an error message
                }

                return View(model); // Return the view with the appropriate user details
            }

            return View(); // Return the guest view if not authenticated
        }


        public async Task<IActionResult> VerifyClaims()
        {
            // Retrieve claims that are pending approval
            var pendingClaims = await _context.Claims
                                              .Include(c => c.Lecturer) // Ensure Lecturer is loaded
                                              .Include(c => c.ClaimsModules)
                                                  .ThenInclude(cm => cm.Module) // Include related modules
                                              .Include(c => c.SupportingDocuments) // Include documents
                                              .Include(c => c.ApprovalProcesses) // Include approvals
                                              .Where(c => c.Status == "Pending")
                                              .ToListAsync();

            // Pass the claims to the view
            return View(pendingClaims);
        }

        // Approve Claim
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId, string feedback)
        {
            // Fetch the claim from the database
            var claim = await _context.Claims
                .Include(c => c.ApprovalProcesses)
                .FirstOrDefaultAsync(c => c.ClaimID == claimId);

            if (claim == null)
            {
                return NotFound("Claim not found.");
            }

            // Retrieve the CoordinatorID and ManagerID from user claims
            var coordinatorIdString = User.FindFirst("CoordinatorID")?.Value;
            var managerIdString = User.FindFirst("ManagerID")?.Value;

            Console.WriteLine($"CoordinatorID: {coordinatorIdString}, ManagerID: {managerIdString}");

            // Check if the user has the necessary role (Coordinator or Manager)
            if (coordinatorIdString == null && managerIdString == null)
            {
                return BadRequest("User is neither a Coordinator nor a Manager.");
            }

            int coordinatorId = coordinatorIdString != null ? int.Parse(coordinatorIdString) : 1; // Default to 1 if null
            int managerId = managerIdString != null ? int.Parse(managerIdString) : 1; // Default to 1 if null

            if (coordinatorId <= 0  || coordinatorId >= 2)
            {
                coordinatorId = 1; // Fallback to default ID
            }

            if (managerId <= 0 || managerId >= 2)
            {
                managerId = 1; // Fallback to default ID
            }

            var approvalProcess = new ApprovalProcess
            {
                ClaimID = claimId,
                ApprovalStatus = "Approved",
                ApprovalDate = DateTime.Now,
                CoordinatorID = coordinatorId,
                ManagerID = managerId,
                Feedback = feedback
            };

            _context.ApprovalProcesses.Add(approvalProcess);

            // Set claim status to Approved
            claim.Status = "Approved";
            claim.RejectionFeedback = feedback;  // Update rejection feedback in case it was previously rejected

            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction("VerifyClaims"); // Redirect back to claims page
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId, string feedback)
        {
            // Fetch the claim from the database
            var claim = await _context.Claims
                .Include(c => c.ApprovalProcesses)
                .FirstOrDefaultAsync(c => c.ClaimID == claimId);

            if (claim == null)
            {
                return NotFound("Claim not found.");
            }

            // Retrieve the CoordinatorID and ManagerID from user claims
            var coordinatorIdString = User.FindFirst("CoordinatorID")?.Value;
            var managerIdString = User.FindFirst("ManagerID")?.Value;

            // Check if the user has the necessary role (Coordinator or Manager)
            if (coordinatorIdString == null && managerIdString == null)
            {
                return BadRequest("User is neither a Coordinator nor a Manager.");
            }

            int coordinatorId = coordinatorIdString != null ? int.Parse(coordinatorIdString) : 1; // Default to 1 if null
            int managerId = managerIdString != null ? int.Parse(managerIdString) : 1; // Default to 1 if null

            if (coordinatorId <= 0 || coordinatorId >= 2)
            {
                coordinatorId = 1; // Fallback to default ID
            }

            if (managerId <= 0 || managerId >= 2)
            {
                managerId = 1; // Fallback to default ID
            }

            var approvalProcess = new ApprovalProcess
            {
                ClaimID = claimId,
                ApprovalStatus = "Rejected",
                ApprovalDate = DateTime.Now,
                CoordinatorID = coordinatorId,
                ManagerID = managerId,
                Feedback = feedback
            };

            _context.ApprovalProcesses.Add(approvalProcess);

            // Set claim status to Rejected
            claim.Status = "Rejected";
            claim.RejectionFeedback = feedback;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction("VerifyClaims"); // Redirect back to the verification page
        }


        // Change Track to TrackClaims to retrieve claims with related data
        public async Task<IActionResult> TrackClaims()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(l => l.LecturerEmail == user.Username);

            if (lecturer == null)
            {
                return NotFound("Lecturer not found.");
            }

            var claims = await _context.Claims
                .Include(c => c.ClaimsModules)
                    .ThenInclude(cm => cm.Module)
                .Include(c => c.ApprovalProcesses)
                    .ThenInclude(ap => ap.Coordinator)
                .Include(c => c.ApprovalProcesses)
                    .ThenInclude(ap => ap.Manager)
                .Where(c => c.LecturerID == lecturer.LecturerID)
                .ToListAsync();

            // Create a list of ClaimSubmissionInfo objects
            var claimSubmissionInfos = claims.Select(c => new ClaimSubmissionInfo
            {
                LecturerID = lecturer.LecturerID,
                LecturerName = lecturer.LecturerName,
                LecturerSurname = lecturer.LecturerSurname,
                LecturerPhone = lecturer.LecturerPhone,
                LecturerEmail = lecturer.LecturerEmail,
                Claim = c,
                ApprovalProcess = c.ApprovalProcesses.FirstOrDefault(),
                Modules = c.ClaimsModules.Select(cm => cm.Module.ModuleName).ToList(),
                SelectedModules = c.ClaimsModules.Select(cm => cm.Module.ModuleName).ToList()
            }).ToList();

            // Pass claims and lecturer info to the view
            ViewBag.ClaimSubmissionInfos = claimSubmissionInfos; // Using ViewBag to pass claims to the view
            return View(claimSubmissionInfos); // Pass the list to the view
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
    }
}