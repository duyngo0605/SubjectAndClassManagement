using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Subject
    {
        [Key]
        [StringLength(10)]
        public string SubjectId { get; set; }

        [Required]
        [StringLength(255)]
        public string SubjectName { get; set; }

        public string SubjectDescription { get; set; }

        [Required]
        public int Credit { get; set; }

        public virtual ICollection<Subject> PrerequisiteSubjects { get; set; }
        public virtual ICollection<Subject> CurrentSubjects { get; set; }
    }
}
