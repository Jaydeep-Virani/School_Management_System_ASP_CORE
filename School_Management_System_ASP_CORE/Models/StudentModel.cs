using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter First name")]
        [StringLength(03, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last name")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Emergency Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string EmergencyPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select Class")]
        public string Class { get; set; }

        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }

        public string ImagePath { get; set; } // Path to the saved image file

        [Required(ErrorMessage = "Please upload an Image")]
        public IFormFile StudentImage { get; set; } // Uploaded file
    }

}
