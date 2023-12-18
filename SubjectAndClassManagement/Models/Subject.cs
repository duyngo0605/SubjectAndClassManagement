using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Subject
    {
        // Existing properties

        [Required]
        public string SubjectId { get; set; }

        [Required]
        public string SubjectName { get; set; }

        [Required]
        public string SubjectDescription { get; set; }

        [Required]
        public int Credit { get; set; }
    }
}
