using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Teacher
    {
        [Key]
        [StringLength(10)]
        public string teacher_id { get; set; }

        [Required]
        [StringLength(255)]
        public string teacher_name { get; set; }

        public string email { get; set; }

        public string phone_number { get; set; }
    }

}
