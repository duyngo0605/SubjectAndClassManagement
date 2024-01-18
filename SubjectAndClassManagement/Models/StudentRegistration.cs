using System;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class StudentRegistration
    {
        [Key]
        [Display(Name = "Registration ID")]
        public string registration_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Student ID")]
        public string student_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Class ID")]
        public string class_id { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        public DateTime registration_date { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Reason")]
        public string reason { get; set; }

        public Student Student { get; set; }
        public Class Class { get; set; }

        public StudentResult StudentResult { get; set; }
    }
}
