using Microsoft.AspNetCore.Mvc;
using School_Management_System_ASP_CORE.Models;

namespace School_Management_System_ASP_CORE.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                // Save to database (implementation depends on your data layer)
                return RedirectToAction("Index"); // Redirect after success
            }
            return View(model); // Return view with validation messages
        }
    }
}
