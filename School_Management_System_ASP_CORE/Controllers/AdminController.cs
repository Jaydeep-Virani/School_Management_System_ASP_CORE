using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Protocol.Plugins;

using School_Management_System_ASP_CORE.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

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

            // Authentication logic
            if (model.Email == "jvirani820@rku.ac.in" && model.Password == "password123")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
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

                _context.Students.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Student added successfully!";
                return RedirectToAction("AddStudent");
            }

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
                _context.Students.Remove(student); // ✅ Pass the student object here
                _context.SaveChanges();
            }
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
                        image = imagePath // Map to Image property in Faculty entity
                    };

                    // Directly add faculty to the database without migrations
                    _context.Faculty.Add(faculty);
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
        public IActionResult AddHoliday(HolidayModel holiday)
        {
            if (!ModelState.IsValid)
            {
                return View(holiday);
            }

            holiday.Id = holidayList.Count + 1;
            holiday.Month = holiday.Date.Month;
            holidayList.Add(holiday);

            return RedirectToAction("ManageHolidays");
        }
        private static List<HolidayModel> holidayList = new List<HolidayModel>
        {
            new HolidayModel { Id = 1, Name = "Makarsankranti", Date = new DateTime(2025, 1, 14), Month = 1 },
            new HolidayModel { Id = 2, Name = "Republic Day", Date = new DateTime(2025, 1, 26), Month = 1 },
            new HolidayModel { Id = 3, Name = "Independence Day", Date = new DateTime(2025, 8, 15), Month = 8 },
            new HolidayModel { Id = 4, Name = "Christmas", Date = new DateTime(2025, 12, 25), Month = 12 }
        };

        // ✅ Route for displaying the holidays page
        [HttpGet]
        public IActionResult ManageHolidays()
        {
            ViewBag.Month = 1;
            var holidays = holidayList.Where(h => h.Month == 1).ToList();
            return View(holidays);
        }

        // ✅ Route for fetching holidays by month
        [HttpGet]
        [Route("Holiday/GetHolidaysByMonth")]
        public JsonResult GetHolidaysByMonth(int month)
        {
            var holidays = holidayList.Where(h => h.Month == month).ToList();
            return Json(holidays);
        }

        public IActionResult Profile()
        {
            ProfileModel model = new ProfileModel
            {
                ProfileID = 1,
                FirstName = "Ethan",
                LastName = "Leo",
                Email = "abcd@gmail.com",
                PhoneNumber = "8200606297",
                EmergencyPhoneNumber = "9876543210", // Added emergency contact
                DateOfBirth = DateTime.Parse("2007-10-18"),
                Gender = "Male",
                Address = "Mountain View, California",
                Role = "Admin",
                ImagePath = "/images/default-profile.png"
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return form with validation errors
            }

            // ✅ Simulating password change (TODO: Implement actual password update logic)
            TempData["SuccessMessage"] = "Password changed successfully!";

            return RedirectToAction("ChangePassword");
        }


        private static List<LeaveModel> leaveData = new List<LeaveModel>();

        public ActionResult ManageLeave()
        {
            return View(leaveData);
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

            model.Id = leaveData.Count + 1;
            model.Status = false; // Default new leave requests to "Pending"
            leaveData.Add(model);

            TempData["SuccessMessage"] = "Leave application submitted successfully!";
            return RedirectToAction("ManageLeave");
        }

        [HttpPost]
        public JsonResult UpdateLeaveStatus(int id, bool status)
        {
            var leave = leaveData.FirstOrDefault(l => l.Id == id);
            if (leave != null)
            {
                leave.Status = status;
                return Json(new { success = true, updatedStatus = status });
            }
            return Json(new { success = false });
        }
    }
}
