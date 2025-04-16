using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Password must be exactly 6 digits.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [Compare("NewPassword", ErrorMessage = "Confirm password must match the new password.")]
        public string ConfirmPassword { get; set; }
    }
    
}
