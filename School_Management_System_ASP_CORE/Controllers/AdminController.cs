using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Protocol.Plugins;
using Microsoft.EntityFrameworkCore;
using School_Management_System_ASP_CORE.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using static School_Management_System_ASP_CORE.Controllers.AdminController;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace School_Management_System_ASP_CORE.Controllers
{
    
    public class AdminController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 🔹 First, check if the email exists
            var user = _context.Users.FirstOrDefault(u => u.email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email is not registered.");
                return View(model);
            }

            // 🔹 Then, check if the password is correct
            if (user.password != model.Password)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return View(model);
            }

            // 🔹 Store common session values
            HttpContext.Session.SetString("email", user.email);
            HttpContext.Session.SetString("role", user.role);

            // 🔹 Role-specific logic with full session setup
            if (user.role == "admin")
            {
                var admin = _context.Admin.FirstOrDefault(a => a.email == user.email);
                if (admin != null)
                {
                    HttpContext.Session.SetString("first_name", admin.firstname);
                    HttpContext.Session.SetString("last_name", admin.lastname);
                    HttpContext.Session.SetString("gender", admin.gender);
                    HttpContext.Session.SetString("dob", admin.dob.ToString("yyyy-MM-dd"));
                    HttpContext.Session.SetString("phonenumber", admin.phonenumber);
                    HttpContext.Session.SetString("ephonenumber", admin.ephonenumber);
                    HttpContext.Session.SetString("image", admin.image);

                    TempData["ToastMessage"] = $"Welcome {admin.firstname} {admin.lastname}!";
                    return RedirectToAction("Dashboard");
                }
            }
            else if (user.role == "faculty")
            {
                var faculty = _context.Faculty.FirstOrDefault(f => f.email == user.email);
                if (faculty != null)
                {
                    HttpContext.Session.SetString("first_name", faculty.firstname);
                    HttpContext.Session.SetString("last_name", faculty.lastname);
                    HttpContext.Session.SetString("gender", faculty.gender);
                    HttpContext.Session.SetString("dob", faculty.dob.ToString("yyyy-MM-dd"));
                    HttpContext.Session.SetString("phonenumber", faculty.phonenumber);
                    HttpContext.Session.SetString("ephonenumber", faculty.ephonenumber);
                    HttpContext.Session.SetString("image", faculty.image);

                    TempData["ToastMessage"] = $"Welcome {faculty.firstname} {faculty.lastname}!";
                    return RedirectToAction("Dashboard");
                }
            }
            else if (user.role == "student")
            {
                var student = _context.Students.FirstOrDefault(s => s.Email == user.email);
                if (student != null)
                {
                    HttpContext.Session.SetString("first_name", student.FirstName);
                    HttpContext.Session.SetString("last_name", student.LastName);
                    HttpContext.Session.SetString("gender", student.Gender);
                    HttpContext.Session.SetString("dob", student.DateOfBirth.ToString("yyyy-MM-dd"));
                    HttpContext.Session.SetString("phonenumber", student.PhoneNumber);
                    HttpContext.Session.SetString("ephonenumber", student.EmergencyPhoneNumber);
                    HttpContext.Session.SetString("image", student.ImagePath);

                    TempData["ToastMessage"] = $"Welcome {student.FirstName} {student.LastName}!";
                    return RedirectToAction("Dashboard");
                }
            }

            // 🔹 Fallback if role found but not mapped properly
            ModelState.AddModelError("", "User details not found for the specified role.");
            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // 🔹 Clear all session data
            return RedirectToAction("Login"); // 🔹 Redirect to login or home
        }
        public IActionResult Dashboard()
        {
            return View();
        }


        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            var classList = _context.Classes
                .Select(c => new SelectListItem
                {
                    Value = c.class_id.ToString(),
                    Text = c.class_name
                }).ToList();

            ViewBag.ClassList = classList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (model.StudentImage != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.StudentImage.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.StudentImage.CopyToAsync(stream);
                    }

                    model.ImagePath = "/uploads/" + fileName;
                }

                // Save student to database
                _context.Students.Add(model);
                await _context.SaveChangesAsync();

                // Add user login credentials
                var user = new User
                {
                    email = model.Email,
                    password = "123456", // You should hash this in production
                    role = "student"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Student added successfully!";
                return RedirectToAction("AddStudent");
            }

            // Reload class list in case of validation error
            ViewBag.ClassList = _context.Classes
                .Select(c => new SelectListItem
                {
                    Value = c.class_id.ToString(),
                    Text = c.class_name
                }).ToList();

            return View(model);
        }



        public IActionResult ManageStudent()
        {
            var students = _context.Students.ToList(); // Fetch all students from the database
            return View(students);
        }
        public class Student
        {
            public int Sid { get; set; }  // Primary Key
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string EmergencyPhoneNumber { get; set; }
            public DateTime DateOfBirth { get; set; }  // Database Date
            public string Address { get; set; }
            public string Class { get; set; }
            public string Gender { get; set; }
            public string ImagePath { get; set; }

            // Not mapped to the database
            [NotMapped]
            public IFormFile StudentImage { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudent(StudentModel model, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                model.ImagePath = "/uploads/" + uniqueFileName;
            }
            else
            {
                model.ImagePath = model.ImagePath;
            }

            // Save updated model to DB
            _context.Students.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageStudent");
        }

        // ✅ Delete Student
        public IActionResult Delete_Student(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.sid == id);

            if (student != null)
            {
                // Find and remove the user with the same email
                var user = _context.Users.FirstOrDefault(u => u.email == student.Email);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                // Remove student
                _context.Students.Remove(student);
                _context.SaveChanges();
            }

            TempData["Success"] = "Student deleted successfully!";
            return RedirectToAction("ManageStudent");
        }

        [HttpGet]
        public IActionResult AddFaculty()
        {
            return View();
        }
        // AddFaculty
        [HttpPost]
        public async Task<IActionResult> AddFaculty(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = null;

                    // Image saving logic
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        imagePath = "/uploads/" + uniqueFileName;
                    }

                    // Map FacultyModel to Faculty entity
                    var faculty = new Faculty
                    {
                        firstname = model.FirstName,
                        lastname = model.LastName,
                        email = model.Email,
                        phonenumber = model.PhoneNumber,
                        ephonenumber = model.EmergencyPhoneNumber,
                        dob = model.DateOfBirth,
                        address = model.Address,
                        gender = model.Gender,
                        image = imagePath
                    };

                    // Add faculty to database
                    _context.Faculty.Add(faculty);
                    await _context.SaveChangesAsync();

                    // Add entry to users table
                    var user = new User
                    {
                        email = model.Email,
                        password = "123456", // In real apps, hash this!
                        role = "faculty"
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Faculty added successfully!";
                    return RedirectToAction("AddFaculty");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught while saving faculty: " + ex.ToString());
                    TempData["Error"] = "An error occurred while adding the faculty. Please try again later.";
                    return RedirectToAction("AddFaculty");
                }
            }

            // If validation fails, return the view with validation errors
            return View(model);
        }


        // GET: Faculty List
        public IActionResult ManageFaculty()
        {
            // Fetching all the faculties from the database
            var faculties = _context.Faculty
                .Select(f => new FacultyModel
                {
                    fid = f.fid,
                    FirstName = f.firstname,
                    LastName = f.lastname,
                    Email = f.email,
                    PhoneNumber = f.phonenumber,
                    EmergencyPhoneNumber = f.ephonenumber,
                    DateOfBirth = f.dob,
                    Address = f.address,
                    Gender = f.gender,
                    ImagePath = f.image // Assuming Image is a property in the Faculty entity
                }).ToList();

            return View(faculties); // Passing the faculty data to the view
        }
        [HttpPost]
        public IActionResult UpdateFaculty(FacultyModel faculty, IFormFile ImageFile)
        {
            // Check if a new image is uploaded
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Save the image to a folder
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", ImageFile.FileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // Assign the new image path to the faculty model
                faculty.ImagePath = "/uploads/" + ImageFile.FileName;
            }

            // Update the faculty details in the database
            var existingFaculty = _context.Faculty.Find(faculty.fid);
            if (existingFaculty != null)
            {
                existingFaculty.firstname = faculty.FirstName;
                existingFaculty.lastname = faculty.LastName;
                existingFaculty.phonenumber = faculty.PhoneNumber;
                existingFaculty.ephonenumber = faculty.EmergencyPhoneNumber;
                existingFaculty.email = faculty.Email;
                existingFaculty.dob = faculty.DateOfBirth;
                existingFaculty.gender = faculty.Gender;
                existingFaculty.address = faculty.Address;
                existingFaculty.image = faculty.ImagePath;

                _context.SaveChanges();
            }

            return RedirectToAction("ManageFaculty");
        }
        // Delete Faculty
        public IActionResult Delete_Faculty(int id)
        {
            var faculty = _context.Faculty.FirstOrDefault(f => f.fid == id);

            if (faculty != null)
            {
                // Delete matching user from Users table
                var user = _context.Users.FirstOrDefault(u => u.email == faculty.email);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                // Delete faculty from Faculty table
                _context.Faculty.Remove(faculty);
                _context.SaveChanges();

                TempData["DeleteSuccess"] = "Faculty deleted successfully!";
            }

            return RedirectToAction("ManageFaculty");
        }


        public IActionResult ClassView()
        {
            var model = _context.Classes.ToList();
            return View(model);
        }

        // ✅ Add a new class
        [HttpPost]
        public IActionResult AddClass(ManageClassModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ClassView", _context.Classes.ToList());
            }

            _context.Classes.Add(model);
            _context.SaveChanges();
            return RedirectToAction("ClassView");
        }

        [HttpGet]
        public IActionResult Class_Edit(int id)
        {
            var classItem = _context.Classes.Find(id);
            if (classItem == null) return NotFound();

            return View(classItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Class_Edit(ManageClassModel model)
        {
            if (!ModelState.IsValid) return View("Class_Edit", model);

            _context.Classes.Update(model);
            _context.SaveChanges();

            return RedirectToAction("ClassView"); 
        }

        // ✅ Delete class
        public IActionResult Class_Delete(int id)
        {
            var classItem = _context.Classes.Find(id);
            if (classItem != null)
            {
                _context.Classes.Remove(classItem);
                _context.SaveChanges();
            }
            return RedirectToAction("ClassView");
        }


        public IActionResult SubjectView()
        {
            var subjects = _context.Subject_Master.ToList();
            return View(subjects);
        }

        // ✅ Add a new subject (GET)
        public IActionResult AddSubject()
        {
            return View();
        }

        // ✅ Add a new subject (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubject(ManageSubjectModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Subject_Master.Add(model);
            _context.SaveChanges();
            return RedirectToAction("SubjectView");
        }

        // ✅ Edit subject (GET)
        [HttpGet]
        public IActionResult Edit_Subject(int id)
        {
            var subject = _context.Subject_Master.FirstOrDefault(s => s.subject_id == id);
            if (subject == null) return NotFound();

            return View(subject);
        }

        // ✅ Edit subject (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit_Subject(ManageSubjectModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Subject_Master.Update(model);
            _context.SaveChanges();
            return RedirectToAction("SubjectView");
        }

        // ✅ Delete subject
        public IActionResult Delete_Subject(int id)
        {
            var subject = _context.Subject_Master.FirstOrDefault(s => s.subject_id == id);
            if (subject != null)
            {
                _context.Subject_Master.Remove(subject);
                _context.SaveChanges();
            }
            return RedirectToAction("SubjectView");
        }


        public IActionResult AddHoliday()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHoliday(HolidayModel holiday)
        {
            if (!ModelState.IsValid)
            {
                return View(holiday);
            }

            // Map HolidayModel to Holiday entity (if needed)
            var holidayEntity = new Holidays
            {
                Name = holiday.Name,
                Date = holiday.Date,
                Month = holiday.Date.Month,
            };

            // Save to database
            _context.Holidays.Add(holidayEntity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Holiday added successfully!";
            return RedirectToAction("ManageHolidays");
        }

        // ✅ Route for displaying the holidays page
        [HttpGet]
        public IActionResult ManageHolidays(int month = 0)  // Default to 0 if no month is provided
        {
            if (month == 0)
            {
                month = DateTime.Now.Month;  // Get current month if no month is passed
            }

            var holidays = _context.Holidays.Where(h => h.Month == month).ToList();
            ViewBag.Month = month;  // Set the selected month dynamically
            return View(holidays);
        }

        // ✅ Route for fetching holidays by month
        [HttpGet]
        [Route("Holiday/GetHolidaysByMonth")]
        public JsonResult GetHolidaysByMonth(int month)
        {
            var holidays = _context.Holidays.Where(h => h.Month == month).ToList();
            return Json(holidays);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHoliday(Holidays model)
        {
            if (ModelState.IsValid)
            {
                var existingHoliday = await _context.Holidays.FindAsync(model.Id);
                if (existingHoliday != null)
                {
                    existingHoliday.Name = model.Name;
                    existingHoliday.Date = model.Date;

                    _context.Holidays.Update(existingHoliday);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Holiday updated successfully!";

                    var month = model.Date.Month;
                    return RedirectToAction("ManageHolidays", new { month });
                }
            }

            TempData["Error"] = "Failed to update holiday!";
            return RedirectToAction("ManageHolidays");
        }

        public IActionResult Delete_Holiday(int id)
        {
            var holiday = _context.Holidays.FirstOrDefault(h => h.Id == id);
            if (holiday != null)
            {
                _context.Holidays.Remove(holiday);
                _context.SaveChanges();
                TempData["Success"] = "Holiday deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Holiday not found!";
            }

            // Redirect to the same month view
            var selectedMonth = holiday?.Date.Month ?? DateTime.Now.Month;
            return RedirectToAction("ManageHolidays", new { month = selectedMonth });
        }

        
        public IActionResult Profile()
        {
            string email = HttpContext.Session.GetString("email");
            string role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login");
            }

            ProfileModel model = null;

            if (role == "student")
            {
                var student = _context.Students.FirstOrDefault(x => x.Email == email);
                if (student != null)
                {
                    model = new ProfileModel
                    {
                        ProfileID = student.sid,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        PhoneNumber = student.PhoneNumber,
                        EmergencyPhoneNumber = student.EmergencyPhoneNumber,
                        DateOfBirth = student.DateOfBirth,
                        Gender = student.Gender,
                        Address = student.Address,
                        Role = role,
                        ImagePath = student.ImagePath ?? "/images/default-profile.png"
                    };
                }
            }
            else if (role == "faculty")
            {
                var faculty = _context.Faculty.FirstOrDefault(x => x.email == email);
                if (faculty != null)
                {
                    model = new ProfileModel
                    {
                        ProfileID = faculty.fid,
                        FirstName = faculty.firstname,
                        LastName = faculty.lastname,
                        Email = faculty.email,
                        PhoneNumber = faculty.phonenumber,
                        EmergencyPhoneNumber = faculty.ephonenumber,
                        DateOfBirth = faculty.dob,
                        Gender = faculty.gender,
                        Address = faculty.address,
                        Role = role,
                        ImagePath = faculty.image ?? "/images/default-profile.png"
                    };
                }
            }
            else if (role == "admin")
            {
                var admin = _context.Admin.FirstOrDefault(x => x.email == email);
                if (admin != null)
                {
                    model = new ProfileModel
                    {
                        ProfileID = admin.Id,
                        FirstName = admin.firstname,
                        LastName = admin.lastname,
                        Email = admin.email,
                        PhoneNumber = admin.phonenumber,
                        EmergencyPhoneNumber = admin.ephonenumber,
                        DateOfBirth = admin.dob,
                        Gender = admin.gender,
                        Address = admin.address,
                        Role = role,
                        ImagePath = admin.image ?? "/images/default-profile.png"
                    };
                }
            }

            if (model == null)
            {
                return NotFound("Profile not found.");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateProfile(ProfileModel model, IFormFile ProfileImage)
        {
            string email = HttpContext.Session.GetString("email");
            string role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            string imagePath = null;

            // Debug: Check if ProfileImage is being received
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);

                // Debug: Check if fileName is being generated correctly
                Console.WriteLine($"Generated file name: {fileName}");

                // Ensure we are saving the image in the correct folder
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                // Debug: Check the path where image will be saved
                Console.WriteLine($"Saving image to path: {path}");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ProfileImage.CopyTo(stream);
                }

                // Store the relative path in the database
                imagePath = "/uploads/" + fileName;

                // Update the session with the new image path
                HttpContext.Session.SetString("image", imagePath);
            }
            else
            {
                // Debug: Check if no image was uploaded
                Console.WriteLine("No new image uploaded, using previous image path from session.");

                // If no new image is uploaded, fallback to the current image path in session
                imagePath = HttpContext.Session.GetString("image");
            }

            // Update based on role
            if (role == "student")
            {
                var student = _context.Students.FirstOrDefault(x => x.Email == email);
                if (student != null)
                {
                    student.FirstName = model.FirstName;
                    student.LastName = model.LastName;
                    student.PhoneNumber = model.PhoneNumber;
                    student.DateOfBirth = model.DateOfBirth.Value;
                    student.Gender = model.Gender;
                    student.Address = model.Address;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        student.ImagePath = imagePath;
                        Console.WriteLine($"Updating student image path to: {imagePath}");
                    }
                    _context.SaveChanges();
                }
            }
            else if (role == "faculty")
            {
                var faculty = _context.Faculty.FirstOrDefault(x => x.email == email);
                if (faculty != null)
                {
                    faculty.firstname = model.FirstName;
                    faculty.lastname = model.LastName;
                    faculty.phonenumber = model.PhoneNumber;
                    faculty.dob = model.DateOfBirth.Value;
                    faculty.gender = model.Gender;
                    faculty.address = model.Address;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        faculty.image = imagePath;
                        Console.WriteLine($"Updating faculty image path to: {imagePath}");
                    }
                    _context.SaveChanges();
                }
            }
            else if (role == "admin")
            {
                var admin = _context.Admin.FirstOrDefault(x => x.email == email);
                if (admin != null)
                {
                    admin.firstname = model.FirstName;
                    admin.lastname = model.LastName;
                    admin.phonenumber = model.PhoneNumber;
                    admin.dob = model.DateOfBirth.Value;
                    admin.gender = model.Gender;
                    admin.address = model.Address;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        admin.image = imagePath;
                        Console.WriteLine($"Updating admin image path to: {imagePath}");
                    }
                    _context.SaveChanges();
                }
            }

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile ProfilePicture)
        {
            var email = HttpContext.Session.GetString("email");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login", "Auth"); // Or show unauthorized
            }

            string fileName = "";

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                // Generate a unique file name
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                // Save the file in the uploads directory
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ProfilePicture.CopyToAsync(stream);
                }
            }

            // Update the profile picture in the database based on role
            switch (role.ToLower())
            {
                case "student":
                    var student = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
                    if (student != null)
                    {
                        student.ImagePath = "/uploads/" + fileName; // Save the relative path
                        _context.Students.Update(student);
                    }
                    break;

                case "faculty":
                    var faculty = await _context.Faculty.FirstOrDefaultAsync(x => x.email == email);
                    if (faculty != null)
                    {
                        faculty.image = "/uploads/" + fileName; // Save the relative path
                        _context.Faculty.Update(faculty);
                    }
                    break;

                case "admin":
                    var admin = await _context.Admin.FirstOrDefaultAsync(x => x.email == email);
                    if (admin != null)
                    {
                        admin.image = "/uploads/" + fileName; // Save the relative path
                        _context.Admin.Update(admin);
                    }
                    break;

                default:
                    TempData["Error"] = "Role not authorized to update profile picture.";
                    return RedirectToAction("Profile");
            }

            await _context.SaveChangesAsync();
            TempData["Profile_Success"] = "Profile picture updated!";
            return RedirectToAction("Profile");
        }



        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();        
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get user from session
            string email = HttpContext.Session.GetString("email");

            if (string.IsNullOrEmpty(email))
            {
                TempData["SuccessMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            // Get user from database
            var user = _context.Users.FirstOrDefault(u => u.email == email);

            if (user == null)
            {
                TempData["SuccessMessage"] = "User not found.";
                return RedirectToAction("Login");
            }

            // Check current password
            if (user.password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return View(model);
            }

            // Update password
            user.password = model.NewPassword;
            _context.Users.Update(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("ChangePassword");
        }

        public ActionResult AddLeave()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLeave(LeaveModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = HttpContext.Session.GetString("email");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            model.Email = email;
            model.Role = role;
            model.Status = 0; 

            _context.Leave_Master.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Leave application submitted successfully!";
            return RedirectToAction("ManageLeave");
        }
        public ActionResult ManageLeave()
        {
            // Get the current user's role from the session
            var role = HttpContext.Session.GetString("role");
            var email = HttpContext.Session.GetString("email");

            List<LeaveModel> leaveData;

            if (role == "faculty")
            {
                // If role is faculty, filter to only show student's leave records
                leaveData = _context.Leave_Master.Where(l => l.Role == "student" && l.Status == 0).ToList();
            }
            else if (role == "admin")
            {
                // If role is admin, show both student and faculty leave records
                leaveData = _context.Leave_Master
                                     .Where(l => (l.Role == "student" || l.Role == "faculty") && l.Status == 0)
                                     .ToList();
            }
            else if (role == "student")
            {
                // If role is student, filter to only show their own leave records
                leaveData = _context.Leave_Master
                             .Where(l => l.Email == email && l.Status == 1)
                             .ToList();
            }
            else
            {
                // For other roles, you can decide to show no data or return an error view
                leaveData = new List<LeaveModel>(); // Show empty list or handle accordingly
            }

            return View(leaveData);
        }

        [HttpPost]
        public IActionResult UpdateLeaveStatus(int id, bool status)
        {
            var leave = _context.Leave_Master.FirstOrDefault(l => l.lid == id);
            if (leave != null)
            {
                leave.Status = status ? 1 : 0;  // Update status to 1 or 0 based on checkbox
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
