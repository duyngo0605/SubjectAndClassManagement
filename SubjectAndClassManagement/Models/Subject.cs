using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Subject
    {
        [Key]
        [StringLength(10)]
        public string subject_id { get; set; }

        [Required]
        [StringLength(255)]
        public string subject_name { get; set; }

        public string subject_description { get; set; }

        [Required]
        public int credit { get; set; }

        public virtual ICollection<Subject> PrerequisiteSubjects { get; set; }
    }
}
