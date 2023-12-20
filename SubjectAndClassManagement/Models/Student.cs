using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Student
    {
        [Key]
        [StringLength(10)]
        public string student_id { get; set; }

        [Required]
        [StringLength(255)]
        public string student_name { get; set; }

        public string email { get; set; }

        public string phone_number { get; set; }

        public virtual ICollection<StudentRegistration> Registrations { get; set; }
        public virtual ICollection<TuitionPayment> Payments { get; set; }

        public User User { get; set; }
    }
}
