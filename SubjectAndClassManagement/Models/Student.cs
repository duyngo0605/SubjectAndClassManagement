using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Student
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Student ID")]
        public string student_id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Student Name")]
        public string student_name { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        public string phone_number { get; set; }

        [Display(Name = "Academic Year")]
        public int academic_year { get; set; }

        public virtual ICollection<StudentRegistration> Registrations { get; set; }
        public virtual ICollection<TuitionPayment> Payments { get; set; }



        public User User { get; set; }
    }
}
