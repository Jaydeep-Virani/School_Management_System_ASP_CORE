using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Please enter your current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter a new password")]
        [RegularExpression(@"^(?=[A-Z\d]{8}$)(?=.*[A-Z])(?=.*\d)[A-Z\d]*$", ErrorMessage = "Password must be exactly 8 characters long, contain only one uppercase letter, and include only digits")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
    }
    
}
