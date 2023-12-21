using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class User
    {
        [Key]
        public string user_id { get; set; }

        [Required]
        [StringLength(255)]
        public string username { get; set; }

        [Required]
        [StringLength(255)]
        public string password { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("admin|student|teacher", ErrorMessage = "Invalid user type.")]
        public string user_type { get; set; }

        public string student_id { get; set; } = String.Empty;

        public string teacher_id { get; set; } = String.Empty;

        // Navigation properties
        public Student Student { get; set; }

        public Teacher Teacher { get; set; }
    }
}
