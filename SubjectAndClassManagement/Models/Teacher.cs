using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Teacher
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Teacher ID")]
        public string teacher_id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Teacher Name")]
        public string teacher_name { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        public string phone_number { get; set; }

        public virtual ICollection<Class> Classes { get; set; }

        public User User { get; set; }
    }
}
