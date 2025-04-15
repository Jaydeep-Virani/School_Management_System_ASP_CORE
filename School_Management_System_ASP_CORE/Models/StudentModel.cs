using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System_ASP_CORE.Models
{
    [Table("student_master")]
    public class StudentModel
    {
        [Key]
        [Column("sid")]
        public int sid { get; set; }

        [Required]
        [Column("firstname")]
        public string FirstName { get; set; }

        [Required]
        [Column("lastname")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("phonenumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("ephonenumber")]
        public string EmergencyPhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("dob")]
        public DateTime DateOfBirth { get; set; }


        [Required]
        [Column("address")]
        public string Address { get; set; }

        [Required]
        [Column("class")]
        public string Class { get; set; }

        [Required]
        [Column("gender")]
        public string Gender { get; set; }

        [Column("image")]
        public string ImagePath { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please upload an image")]
        public IFormFile StudentImage { get; set; }
    }
}
