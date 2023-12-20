using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SubjectAndClassManagement.Models
{
    public class User : IdentityUser
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("admin|student|teacher", ErrorMessage = "Invalid user type.")]
        public string UserType { get; set; }

        public string StudentId { get; set; } = String.Empty;

        public string TeacherId { get; set; } = String.Empty;

        public string AdminPermissions { get; set; } = String.Empty;

        // Navigation properties
        public Student Student { get; set; } = null;

        public Teacher Teacher { get; set; } = null;
    }
}
