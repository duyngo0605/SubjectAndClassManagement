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
            .HasKey(s => s.subject_id);

        modelBuilder.Entity<Subject>()
            .Property(s => s.subject_name)
            .IsRequired();

        modelBuilder.Entity<Subject>()
            .Property(s => s.credit)
            .IsRequired();

        modelBuilder.Entity<Room>()
            .HasKey(r => r.room_id);

        modelBuilder.Entity<Room>()
            .Property(r => r.room_name)
            .IsRequired();

        modelBuilder.Entity<Room>()
            .Property(r => r.room_capacity)
            .IsRequired();

        modelBuilder.Entity<Teacher>()
            .HasKey(t => t.teacher_id);

        modelBuilder.Entity<Teacher>()
            .Property(t => t.teacher_name)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .HasKey(s => s.student_id);

        modelBuilder.Entity<Student>()
            .Property(s => s.student_name)
            .IsRequired();

        modelBuilder.Entity<Class>()
            .HasKey(c => c.class_id);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Subject)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.subject_id);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Room)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.room_id);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.teacher_id);

        modelBuilder.Entity<StudentRegistration>()
            .HasKey(sr => sr.registration_id);

        modelBuilder.Entity<StudentRegistration>()
            .HasOne(sr => sr.Student)
            .WithMany()
            .IsRequired()
            .HasForeignKey(sr => sr.student_id);

        modelBuilder.Entity<StudentRegistration>()
            .HasOne(sr => sr.Class)
            .WithMany()
            .IsRequired()
            .HasForeignKey(sr => sr.class_id);

        modelBuilder.Entity<TuitionPayment>()
            .HasKey(tp => tp.payment_id);

        modelBuilder.Entity<TuitionPayment>()
            .HasOne(tp => tp.Student)
            .WithMany()
            .IsRequired()
            .HasForeignKey(tp => tp.student_id);
    }

}