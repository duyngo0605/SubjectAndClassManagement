using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(255)]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("admin|student|teacher", ErrorMessage = "Invalid user type.")]
        [Display(Name = "User Type")]
        public string user_type { get; set; }

        [Display(Name = "Student ID")]
        public string student_id { get; set; } = string.Empty;

        [Display(Name = "Teacher ID")]
        public string teacher_id { get; set; } = string.Empty;

        // Navigation properties
        public Student Student { get; set; }

        public Teacher Teacher { get; set; }

        public Profile Profile {
            get; set;
        }
    }
}
