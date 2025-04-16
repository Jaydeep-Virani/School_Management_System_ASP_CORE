using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class ProfileModel
    {
        public int ProfileID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Phone]
        public string EmergencyPhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string Role { get; set; }

        public string ImagePath { get; set; }


    }
}
