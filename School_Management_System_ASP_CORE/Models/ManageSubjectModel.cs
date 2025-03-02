using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class ManageSubjectModel
    {
        public int SubjectID { get; set; }
        public List<ManageSubjectModel> Subjects { get; set; } = new List<ManageSubjectModel>();

        [Required(ErrorMessage = "Subject Name is required!")]
        public string SubjectName { get; set; }

    }
}
