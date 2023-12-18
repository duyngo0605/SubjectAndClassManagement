using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Teacher
    {
        // Existing properties

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string TeacherName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }

}
