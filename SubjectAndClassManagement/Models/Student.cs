using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Student
    {
        // Existing properties

        [Required]
        public string StudentId { get; set; }

        [Required]
        public string StudentName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
