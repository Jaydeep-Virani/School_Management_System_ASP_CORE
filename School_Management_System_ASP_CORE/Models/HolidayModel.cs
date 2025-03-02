using System;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System_ASP_CORE.Models
{
    public class HolidayModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Holiday name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Month is required")]
        public int Month { get; set; }
    }
}
