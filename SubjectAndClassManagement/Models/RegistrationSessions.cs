using System;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class RegistrationSession
    {
        [Key]
        [Display(Name = "Session ID")]
        [StringLength(30)]
        public string session_id { get; set; }

        [Display(Name = "Start Date")]
        public DateTime start_date { get; set; }

        [Display(Name = "End Date")]
        public DateTime end_date { get; set; }

        [Display(Name = "Semester")]
        public int semester { get; set; }

        [Display(Name = "Academic Year")]
        [StringLength(20)]
        public string academic_year { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string status { get; set; } = "open";
    }
}
