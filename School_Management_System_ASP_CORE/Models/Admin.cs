namespace School_Management_System_ASP_CORE.Models
{
    public class Admin
    {
        public int Id { get; set; } // Primary key

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }

        public string phonenumber { get; set; }

        public string ephonenumber { get; set; }

        public DateTime dob { get; set; }

        public string address { get; set; }

        public string gender { get; set; }

        public string image { get; set; }
    }
}
