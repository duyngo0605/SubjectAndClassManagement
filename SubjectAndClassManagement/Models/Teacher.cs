using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Teacher
    {
        [Key]
        [StringLength(10)]
        public string TeacherId { get; set; }

        [Required]
        [StringLength(255)]
        public string TeacherName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }

}
