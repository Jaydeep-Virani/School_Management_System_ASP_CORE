using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class LeaveModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Leave Reason is required.")]
        public string LeaveReason { get; set; }

        [Required(ErrorMessage = "Leave Day is required.")]
        [Range(1, 365, ErrorMessage = "Leave days must be between 1 and 365.")]
        public int LeaveDay { get; set; }

        [Required(ErrorMessage = "Leave Date is required.")]
        public DateTime LeaveDate { get; set; }

        // ✅ Add this missing Status property
        public bool Status { get; set; } = false;
    }
}
