using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class ManageClassModel
    {
        public int ClassID { get; set; }
        public List<ManageClassModel> Classes { get; set; } = new List<ManageClassModel>();

        [Required(ErrorMessage = "Class Name is required!")]
        public string ClassName { get; set; }
    }

}

