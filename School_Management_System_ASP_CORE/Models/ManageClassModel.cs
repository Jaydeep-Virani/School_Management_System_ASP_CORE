using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System_ASP_CORE.Models
{
    [Table("master_class")] 
    public class ManageClassModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int class_id { get; set; } // ✅ Match column name with DB

        [Required(ErrorMessage = "Class Name is required!")]
        public string class_name { get; set; } // ✅ Match column name with DB
    }
}
