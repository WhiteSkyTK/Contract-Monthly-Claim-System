using Contract_Monthly_Claim_System.Context;
using System.Globalization;
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
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;


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

        public IActionResult Edit(int? userId)
        {
            if (!userId.HasValue)
            {
                return NotFound();
            }

            var userToEdit = _context.Users.Find(userId.Value);
            if (userToEdit == null)
            {
                return NotFound();
            }

            var model = new EditViewModel
            {
                Lecturers = _context.Lecturers.ToList(),
                ProgrammeCoordinators = _context.ProgrammeCoordinators.ToList(),
                AcademicManagers = _context.AcademicManagers.ToList(),
            };

            // Populate the model based on the user role
            switch (userToEdit.Role)
            {
                case "Lecturer":
                    model.Lecturer = _context.Lecturers.Find(userToEdit.userID);
                    break;
                case "Programme Coordinator":
                    model.ProgrammeCoordinator = _context.ProgrammeCoordinators.Find(userToEdit.userID);
                    break;
                case "Academic Manager":
                    model.AcademicManager = _context.AcademicManagers.Find(userToEdit.userID);
                    break;
            }

            return View(model);
        }

        // Action to fetch user data for editing
        [HttpGet]
        public IActionResult GetUserData(int id, string role)
        {
            // Fetch the user by ID
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            // Initialize EditViewModel to hold the user data
            var model = new EditViewModel();

            // Use the role to fetch the corresponding user details
            switch (user.Role)
            {
                case "Lecturer":
                    model.Lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerID == id);
                    break;

                case "Programme Coordinator":
                    model.ProgrammeCoordinator = _context.ProgrammeCoordinators.FirstOrDefault(c => c.CoordinatorID == id);
                    break;

                case "Academic Manager":
                    model.AcademicManager = _context.AcademicManagers.FirstOrDefault(m => m.ManagerID == id);
                    break;

                default:
                    return NotFound();
            }

            if (model.Lecturer == null && model.ProgrammeCoordinator == null && model.AcademicManager == null)
            {
                return NotFound();
            }

            // Return the user details as JSON
            return Json(new
            {
                userID = user.userID,
                Name = user.Role switch
                {
                    "Lecturer" => model.Lecturer.LecturerName,
                    "Programme Coordinator" => model.ProgrammeCoordinator.CoordinatorName,
                    "Academic Manager" => model.AcademicManager.ManagerName,
                    _ => string.Empty
                },
                Surname = user.Role switch
                {
                    "Lecturer" => model.Lecturer.LecturerSurname,
                    "Programme Coordinator" => model.ProgrammeCoordinator.CoordinatorSurname,
                    "Academic Manager" => model.AcademicManager.ManagerSurname,
                    _ => string.Empty
                },
                Email = user.Role switch
                {
                    "Lecturer" => model.Lecturer.LecturerEmail,
                    "Programme Coordinator" => model.ProgrammeCoordinator.CoordinatorEmail,
                    "Academic Manager" => model.AcademicManager.ManagerEmail,
                    _ => string.Empty
                },
                Phone = user.Role switch
                {
                    "Lecturer" => model.Lecturer.LecturerPhone,
                    "Programme Coordinator" => model.ProgrammeCoordinator.CoordinatorPhone,
                    "Academic Manager" => model.AcademicManager.ManagerPhone,
                    _ => string.Empty
                },
                Role = user.Role
            });
        }

        // POST: Update User
        [HttpPost]
        public IActionResult UpdateUser(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update based on the type of user in the EditViewModel
                if (model.Lecturer != null)
                {
                    var lecturerToUpdate = _context.Lecturers.Find(model.Lecturer.LecturerID);
                    if (lecturerToUpdate != null)
                    {
                        lecturerToUpdate.LecturerName = model.Lecturer.LecturerName;
                        lecturerToUpdate.LecturerSurname = model.Lecturer.LecturerSurname;
                        lecturerToUpdate.LecturerEmail = model.Lecturer.LecturerEmail;
                        lecturerToUpdate.LecturerPhone = model.Lecturer.LecturerPhone;
                    }
                }
                else if (model.ProgrammeCoordinator != null)
                {
                    var coordinatorToUpdate = _context.ProgrammeCoordinators.Find(model.ProgrammeCoordinator.CoordinatorID);
                    if (coordinatorToUpdate != null)
                    {
                        coordinatorToUpdate.CoordinatorName = model.ProgrammeCoordinator.CoordinatorName;
                        coordinatorToUpdate.CoordinatorSurname = model.ProgrammeCoordinator.CoordinatorSurname;
                        coordinatorToUpdate.CoordinatorEmail = model.ProgrammeCoordinator.CoordinatorEmail;
                        coordinatorToUpdate.CoordinatorPhone = model.ProgrammeCoordinator.CoordinatorPhone;
                    }
                }
                else if (model.AcademicManager != null)
                {
                    var managerToUpdate = _context.AcademicManagers.Find(model.AcademicManager.ManagerID);
                    if (managerToUpdate != null)
                    {
                        managerToUpdate.ManagerName = model.AcademicManager.ManagerName;
                        managerToUpdate.ManagerSurname = model.AcademicManager.ManagerSurname;
                        managerToUpdate.ManagerEmail = model.AcademicManager.ManagerEmail;
                        managerToUpdate.ManagerPhone = model.AcademicManager.ManagerPhone;
                    }
                }

                // Save changes to the database
                _context.SaveChanges();
                TempData["Message"] = "User updated successfully!";

                // Redirect to the Edit action with the correct user ID
                return RedirectToAction("Edit", new { userId = model.Lecturer?.LecturerID ?? model.ProgrammeCoordinator?.CoordinatorID ?? model.AcademicManager?.ManagerID });
            }

            // If model state is invalid, return to the Edit view with the current model
            return View("Edit", model);
        }

        // Action for the ManageClaims page
        public IActionResult ManageClaims()
        {
            var model = new ManageClaimsViewModel();

            // Fetch only approved claims from the database
            model.ApprovedClaims = _context.Claims
                .Where(c => c.Status == "Approved")
                .ToList();

            return View(model);
        }


        // Action to generate a report
        [HttpPost]
        public IActionResult GenerateInvoice(List<int> claimIds)
        {
            if (claimIds == null || claimIds.Count == 0)
            {
                _logger.LogWarning("No claims were selected for invoice generation.");
                TempData["ErrorMessage"] = "No claims selected.";
                return RedirectToAction("ManageClaims");
            }

            foreach (var claimId in claimIds)
            {
                _logger.LogInformation($"Attempting to generate invoice for Claim ID: {claimId}");

                // Check if an invoice already exists for this claim
                bool invoiceExists = _context.ReportMetadata.Any(r => r.ClaimID == claimId && r.ReportType == "Invoice");
                if (invoiceExists)
                {
                    _logger.LogInformation($"Invoice already exists for Claim ID: {claimId}. Skipping generation.");
                    continue;
                }

                var claim = _context.Claims
                    .Include(c => c.Lecturer)
                    .Include(c => c.ApprovalProcesses)
                        .ThenInclude(ap => ap.Coordinator)
                    .Include(c => c.ApprovalProcesses)
                        .ThenInclude(ap => ap.Manager)
                    .FirstOrDefault(c => c.ClaimID == claimId);

                if (claim == null || claim.TotalClaimAmount <= 0)
                {
                    _logger.LogWarning($"Invalid or zero claim amount for Claim ID: {claimId}. Skipping generation.");
                    TempData["ErrorMessage"] = "Invalid claim data. Please check your input.";
                    continue;
                }

                var latestApproval = claim.ApprovalProcesses
                    .OrderByDescending(ap => ap.ApprovalDate)
                    .FirstOrDefault();

                var invoiceFilePath = GenerateInvoicePdf(claim);

                var reportMetadata = new ReportMetadata
                {
                    ReportName = $"Invoice_{claim.ClaimID}.pdf",
                    ReportType = "Invoice",
                    DateGenerated = DateTime.Now,
                    FilePath = invoiceFilePath,
                    ClaimID = claim.ClaimID,
                    TotalApprovedClaims = claim.TotalClaimAmount,
                    LecturerID = claim.LecturerID,
                    ApprovalID = latestApproval?.ApprovalID,
                    GeneratedBy = User.Identity.Name
                };

                _context.ReportMetadata.Add(reportMetadata);
            }

            var changesSaved = _context.SaveChanges() > 0;
            TempData["Message"] = changesSaved ? "Invoices generated successfully!" : "Failed to save invoice metadata.";

            return RedirectToAction("ViewInvoices");
        }

        // Generate PDF invoice for each claim
        private string GenerateInvoicePdf(Claims claim)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/invoices", $"Invoice_{claim.ClaimID}.pdf");

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    var document = new iTextSharp.text.Document();
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Add content to the PDF
                    document.Add(new Paragraph($"Invoice ID: {claim.ClaimID}"));
                    document.Add(new Paragraph($"Lecturer: {claim.Lecturer?.LecturerName}"));
                    document.Add(new Paragraph($"Hours Worked: {claim.HoursWorked}"));
                    document.Add(new Paragraph($"Hourly Rate: {claim.HourlyRate.ToString("C", new CultureInfo("en-ZA"))}"));
                    document.Add(new Paragraph($"Total Amount: {claim.TotalClaimAmount.ToString("C", new CultureInfo("en-ZA"))}"));
                    document.Add(new Paragraph($"Generated On: {DateTime.Now}"));

                    var latestApproval = claim.ApprovalProcesses
                        .OrderByDescending(ap => ap.ApprovalDate)
                        .FirstOrDefault();

                    if (latestApproval != null)
                    {
                        document.Add(new Paragraph($"Approved By:"));
                        document.Add(new Paragraph($"  Coordinator: {latestApproval.Coordinator?.CoordinatorName ?? "N/A"}"));
                        document.Add(new Paragraph($"  Manager: {latestApproval.Manager?.ManagerName ?? "N/A"}"));
                        document.Add(new Paragraph($"Approval Date: {latestApproval.ApprovalDate.ToShortDateString()}"));
                    }

                    document.Close();
                }
                _logger.LogInformation($"Invoice PDF generated successfully at: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating invoice PDF.");
            }

            return filePath;
        }

        // Action to download a report by ID
        public IActionResult DownloadInvoice(int id)
        {
            var report = _context.ReportMetadata.Find(id);
            if (report == null || report.ReportType != "Invoice")
            {
                _logger.LogWarning($"Invoice not found for ID: {id}");
                TempData["ErrorMessage"] = "Invoice not found.";
                return RedirectToAction("ViewInvoices");
            }

            var filePath = report.FilePath;
            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError($"File not found at path: {filePath}");
                TempData["ErrorMessage"] = "The requested invoice file is missing.";
                return RedirectToAction("ViewInvoices");
            }

            return PhysicalFile(filePath, "application/pdf", report.ReportName);
        }

        // Action to view invoices
        public IActionResult ViewInvoices()
        {
            var invoices = _context.ReportMetadata
                .Where(r => r.ReportType == "Invoice")
                .Include(r => r.Claims)
                    .ThenInclude(c => c.Lecturer) // Include Lecturer from Claims
                .Include(r => r.ApprovalProcess) // Include ApprovalProcess
                    .ThenInclude(ap => ap.Coordinator) // Include Coordinator in ApprovalProcess
                .Include(r => r.ApprovalProcess)
                    .ThenInclude(ap => ap.Manager) // Include Manager in ApprovalProcess
                .ToList();

            return View(invoices);
        }


        // Action to update lecturer information
        [HttpPost]
        public IActionResult UpdateLecturer(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                // Logic to update lecturer information in the database
                // Example: Call a service to update lecturer
                TempData["Message"] = "Lecturer updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating lecturer. Please check the input.";
            }
            return RedirectToAction("ManageClaims");
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