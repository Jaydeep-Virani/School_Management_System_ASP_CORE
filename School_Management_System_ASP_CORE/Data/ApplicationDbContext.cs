using Microsoft.EntityFrameworkCore;
using School_Management_System_ASP_CORE.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ManageClassModel> Classes { get; set; }
    public DbSet<ManageSubjectModel> Subject_Master { get; set; }
    public DbSet<StudentModel> Students { get; set; }

    public DbSet<Faculty> Faculty { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Holidays> Holidays { get; set; }

    public DbSet<Admin> Admin { get; set; }

    public DbSet<LeaveModel> Leave_Master { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ManageClassModel>().ToTable("master_class");
        modelBuilder.Entity<ManageClassModel>().HasKey(c => c.class_id);

        modelBuilder.Entity<ManageSubjectModel>().ToTable("master_subject");
        modelBuilder.Entity<ManageSubjectModel>().HasKey(s => s.subject_id);

        modelBuilder.Entity<StudentModel>().ToTable("student_master");
        modelBuilder.Entity<StudentModel>().HasKey(s => s.sid);

        modelBuilder.Entity<Faculty>().ToTable("faculty_master");
        modelBuilder.Entity<Faculty>().HasKey(f => f.fid);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<Holidays>().ToTable("holiday_master");
        modelBuilder.Entity<Holidays>().HasKey(h => h.Id);

        modelBuilder.Entity<Admin>().ToTable("admin_master");
        modelBuilder.Entity<Admin>().HasKey(a => a.Id);

        modelBuilder.Entity<LeaveModel>().ToTable("leave_master");
        modelBuilder.Entity<LeaveModel>().HasKey(l => l.lid);
    }
}
