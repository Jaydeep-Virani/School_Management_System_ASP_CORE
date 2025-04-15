using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace School_Management_System_ASP_CORE.Models
{
    public class FacultyModel
    {
        public int fid { get; set; }

        [Required(ErrorMessage = "Please enter First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Emergency phone number is required")]
        public string EmergencyPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }

        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Please upload an image")]
        public IFormFile ImageFile { get; set; }
    }
}
