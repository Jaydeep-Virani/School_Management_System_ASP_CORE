using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using School_Management_System_ASP_CORE.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace School_Management_System_ASP_CORE.Controllers
{
    
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index"); // Redirect after success
            }
            return View(model); // Return view with validation messages
        }

        public IActionResult ManageStudent()
        {
            List<Student> students = new List<Student>
            {
                new Student { RollNo = 1, Name = "Tiger Nixon", Education = "M.COM, P.H.D.", Mobile = "123 456 7890", Email = "info@example.com", AdmissionDate = "25-04-2011" },
                new Student { RollNo = 2, Name = "Garrett Winters", Education = "M.COM, P.H.D.", Mobile = "987 654 3210", Email = "info@example.com", AdmissionDate = "25-07-2011" },
                new Student { RollNo = 3, Name = "Ashton Cox", Education = "B.COM, M.COM.", Mobile = "(123) 456 789", Email = "info@example.com", AdmissionDate = "12-01-2009" }
            };

            return View(students);
        }
        public class Student
        {
            public int RollNo { get; set; }
            public string Name { get; set; }
            public string Education { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string AdmissionDate { get; set; }
        }

        public IActionResult AddFaculty()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFaculty(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index"); // Redirect after success
            }
            return View(model); // Return view with validation messages
        }
        // GET: Faculty List
        public IActionResult ManageFaculty()
        {
            List<FacultyModel> faculties = new List<FacultyModel> // ✅ Use FacultyModel
            {
                new FacultyModel
                {
                    FacultyID = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    PhoneNumber = "1234567890",
                    EmergencyPhoneNumber = "9876543210",
                    DateOfBirth = new DateTime(1985, 04, 25),
                    Address = "New York",
                    Department = "IT",
                    Gender = "Male",
                    JoiningDate = new DateTime(2011, 04, 25),
                    ImagePath = "/images/faculty/john_doe.jpg" // ✅ Ensure ImagePath exists
                },
                new FacultyModel
                {
                    FacultyID = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    PhoneNumber = "9876543210",
                    EmergencyPhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(1987, 07, 15),
                    Address = "Los Angeles",
                    Department = "Finance",
                    Gender = "Female",
                    JoiningDate = new DateTime(2011, 07, 25),
                    ImagePath = "/images/faculty/jane_smith.jpg"
                }
            };

            return View(faculties); // ✅ Ensure you pass FacultyModel

        }
    

        // Updated Faculty Model
        public class Faculty
        {
            public int FacultyID { get; set; } // Renamed from RollNo

            public string Name { get; set; }

            public string Education { get; set; }

            public string Mobile { get; set; }

            public string Email { get; set; }

            [DataType(DataType.Date)]
            public DateTime? AdmissionDate { get; set; } // Changed from string to DateTime?
        }

        private static List<ManageClassModel> classList = new List<ManageClassModel>();

        public IActionResult ClassView()
        {
            var model = new ManageClassModel { Classes = classList };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddClass(ManageClassModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Classes = classList; // Preserve list data
                return View("ClassView", model);
            }

            // Add new class
            classList.Add(new ManageClassModel { ClassID = classList.Count + 1, ClassName = model.ClassName });

            return RedirectToAction("ClassView");
        }
        public IActionResult Delete(int id)
        {
            classList.RemoveAll(c => c.ClassID == id);
            return RedirectToAction("ClassView");
        }

        private static List<ManageSubjectModel> subjectList = new List<ManageSubjectModel>();

        public IActionResult SubjectView()
        {
            var model = new ManageSubjectModel { Subjects = subjectList };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddSubject(ManageSubjectModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = subjectList; // Preserve list data
                return View("SubjectView", model);
            }

            // Add new class
            subjectList.Add(new ManageSubjectModel { SubjectID = classList.Count + 1, SubjectName = model.SubjectName });

            return RedirectToAction("SubjectView");
        }
        public IActionResult SubjectDelete(int id)
        {
            subjectList.RemoveAll(c => c.SubjectID == id);
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

    }
}
