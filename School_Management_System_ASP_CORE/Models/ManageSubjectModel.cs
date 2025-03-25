using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class ManageSubjectModel
    {
        [Key] // ✅ Primary Key (Keep subject_id)
        public int subject_id { get; set; }

        [Required(ErrorMessage = "Subject Name is required!")]
        [StringLength(100, ErrorMessage = "Subject Name cannot exceed 100 characters.")]
        public string subject_name { get; set; }
    }
}
