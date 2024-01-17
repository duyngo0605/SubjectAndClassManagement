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
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Subject)
            .WithMany(s => s.Classes)
            .HasForeignKey(c => c.subject_id);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Room)
            .WithMany(r => r.Classes)
            .HasForeignKey(c => c.room_id);

        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Classes)
            .HasForeignKey(c => c.teacher_id);

        modelBuilder.Entity<StudentRegistration>()
            .HasOne(sr => sr.Student)
            .WithMany(s => s.Registrations)
            .HasForeignKey(sr => sr.student_id);

        modelBuilder.Entity<StudentRegistration>()
            .HasOne(sr => sr.Class)
            .WithMany(c => c.StudentRegistrations)
            .HasForeignKey(sr => sr.class_id);

        modelBuilder.Entity<TuitionPayment>()
            .HasOne(tp => tp.Student)
            .WithMany(s => s.Payments)
            .HasForeignKey(tp => tp.student_id);

        modelBuilder.Entity<TuitionPayment>()
            .HasMany(tp => tp.FeeSubjects)
            .WithMany(s => s.TuitionPayments)
            .UsingEntity(j => j.ToTable("Fee_Subjects"));


        modelBuilder.Entity<Subject>()
            .Property(s => s.subject_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Subject>()
            .Property(s => s.subject_name)
            .HasMaxLength(255);

        modelBuilder.Entity<Room>()
            .Property(r => r.room_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Teacher>()
            .Property(t => t.teacher_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Teacher>()
            .Property(t => t.teacher_name)
            .HasMaxLength(255);

        modelBuilder.Entity<Student>()
            .Property(s => s.student_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Student>()
            .Property(s => s.student_name)
            .HasMaxLength(255);

        modelBuilder.Entity<Student>()
            .HasOne(u => u.User)
            .WithOne(s => s.Student)
            .HasForeignKey<Student>(u => u.student_id)
            .IsRequired(false);

        modelBuilder.Entity<Class>()
            .Property(c => c.class_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Class>()
            .Property(c => c.subject_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Class>()
            .Property(c => c.room_id)
            .HasMaxLength(10);

        modelBuilder.Entity<Class>()
            .Property(c => c.teacher_id)
            .HasMaxLength(10);

        modelBuilder.Entity<StudentRegistration>()
            .Property(sr => sr.registration_id)
            .HasMaxLength(10);

        modelBuilder.Entity<StudentRegistration>()
            .Property(sr => sr.student_id)
            .HasMaxLength(10);

        modelBuilder.Entity<StudentRegistration>()
            .Property(sr => sr.class_id)
            .HasMaxLength(10);

        modelBuilder.Entity<TuitionPayment>()
            .Property(tp => tp.payment_id)
            .HasMaxLength(10);

        modelBuilder.Entity<TuitionPayment>()
            .Property(tp => tp.student_id)
            .HasMaxLength(10);
        modelBuilder.Entity<User>()
        .HasKey(u => u.username);

        modelBuilder.Entity<User>()
            .Property(u => u.username)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.password)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.user_type)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Student)
            .WithOne(s => s.User)
            .HasForeignKey<User>(u => u.student_id)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Teacher)
            .WithOne(t => t.User)
            .HasForeignKey<User>(u => u.teacher_id)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.username)
            .IsUnique();

        modelBuilder.Entity<Profile>()
            .Property(u => u.profile_id);

        modelBuilder.Entity<Profile>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<Profile>(p => p.username)
            .IsRequired();

        modelBuilder.Entity<Notification>()
            .Property(u => u.notify_id);
    }

}