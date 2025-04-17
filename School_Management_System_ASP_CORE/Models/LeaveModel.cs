using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System_ASP_CORE.Models
{
    [Table("leave_master")]
    public class LeaveModel
    {
        [Key]
        public int lid { get; set; }

        [Required(ErrorMessage = "Leave Reason is required.")]
        public string LeaveReason { get; set; }

        [Required(ErrorMessage = "Leave Day is required.")]
        [Range(1, 365, ErrorMessage = "Leave days must be between 1 and 365.")]
        public int LeaveDay { get; set; }

        [Required(ErrorMessage = "Leave Date is required.")]
        public DateTime LeaveDate { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public int Status { get; set; } = 0;
    }
}
