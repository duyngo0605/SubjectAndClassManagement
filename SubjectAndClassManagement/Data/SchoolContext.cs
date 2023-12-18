using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;
using Microsoft.EntityFrameworkCore;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
    {
    }

    public DbSet<Subject> Subjects { get; set; }    
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<StudentRegistration> StudentRegistrations { get; set; }
    public DbSet<TuitionPayment> TuitionPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>()
            .HasKey(s => s.SubjectId);

        modelBuilder.Entity<Subject>()
            .Property(s => s.SubjectName)
            .IsRequired();

        modelBuilder.Entity<Subject>()
            .Property(s => s.Credit)
            .IsRequired();

        modelBuilder.Entity<Room>()
            .HasKey(r => r.RoomId);

        modelBuilder.Entity<Room>()
            .Property(r => r.RoomName)
            .IsRequired();

        modelBuilder.Entity<Room>()
            .Property(r => r.RoomCapacity)
            .IsRequired();

        modelBuilder.Entity<Teacher>()
            .HasKey(t => t.TeacherId);

        modelBuilder.Entity<Teacher>()
            .Property(t => t.TeacherName)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .HasKey(s => s.StudentId);

        modelBuilder.Entity<Student>()
            .Property(s => s.StudentName)
            .IsRequired();

        modelBuilder.Entity<Class>()
            .HasKey(c => c.ClassId);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Subject)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.SubjectId);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Room)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.RoomId);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.TeacherId);

        modelBuilder.Entity<StudentRegistration>()
            .HasKey(sr => sr.RegistrationId);

        modelBuilder.Entity<StudentRegistration>()
            .HasOne(sr => sr.Student)
            .WithMany()
            .IsRequired()
            .HasForeignKey(sr => sr.StudentId);

        modelBuilder.Entity<StudentRegistration>()
            .HasOne(sr => sr.Class)
            .WithMany()
            .IsRequired()
            .HasForeignKey(sr => sr.ClassId);

        modelBuilder.Entity<TuitionPayment>()
            .HasKey(tp => tp.PaymentId);

        modelBuilder.Entity<TuitionPayment>()
            .HasOne(tp => tp.Student)
            .WithMany()
            .IsRequired()
            .HasForeignKey(tp => tp.StudentId);
    }

}