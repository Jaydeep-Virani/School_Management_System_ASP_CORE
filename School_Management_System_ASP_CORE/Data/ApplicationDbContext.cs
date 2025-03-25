using Microsoft.EntityFrameworkCore;
using School_Management_System_ASP_CORE.Models;

namespace School_Management_System_ASP_CORE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // ✅ Define DbSet for Classes
        public DbSet<ManageClassModel> Classes { get; set; }

        // ✅ Define DbSet for Subjects
        public DbSet<ManageSubjectModel> Subject_Master { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Ensure correct table mapping for Classes
            modelBuilder.Entity<ManageClassModel>().ToTable("master_class");

            // ✅ Ensure correct table mapping for Subjects
            modelBuilder.Entity<ManageSubjectModel>().ToTable("master_subject");

            // ✅ Define primary key for Subject
            modelBuilder.Entity<ManageSubjectModel>().HasKey(s => s.subject_id);
        }
    }
}
