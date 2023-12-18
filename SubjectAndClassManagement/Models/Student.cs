using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Student
    {
        [Key]
        [StringLength(10)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(255)]
        public string StudentName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<StudentRegistration> Registrations { get; set; }
        public virtual ICollection<TuitionPayment> Payments { get; set; }
    }
}
