using Microsoft.EntityFrameworkCore;
using School_Management_System_ASP_CORE.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ManageClassModel> Classes { get; set; }
    public DbSet<ManageSubjectModel> Subject_Master { get; set; }
    public DbSet<StudentModel> Students { get; set; }

    public DbSet<Faculty> Faculty { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ManageClassModel>().ToTable("master_class");
        modelBuilder.Entity<ManageClassModel>().HasKey(c => c.class_id);

        modelBuilder.Entity<ManageSubjectModel>().ToTable("master_subject");
        modelBuilder.Entity<ManageSubjectModel>().HasKey(s => s.subject_id);

        modelBuilder.Entity<StudentModel>().ToTable("student_master");
        modelBuilder.Entity<StudentModel>().HasKey(s => s.sid);

        modelBuilder.Entity<Faculty>().ToTable("faculty_master");
        modelBuilder.Entity<Faculty>().HasKey(f => f.fid);// Ensure the primary key is correctly mapped
    }
}
