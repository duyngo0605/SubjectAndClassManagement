using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class StudentRegistration
    {
        [Key]
        [StringLength(10)]
        public string registration_id { get; set; }

        [Required]
        [StringLength(10)]
        public string student_id { get; set; }

        [Required]
        [StringLength(10)]
        public string class_id { get; set; }

        [Required]
        public DateTime registration_date { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; }

        public string reason { get; set; }

        public  Student Student { get; set; }
        public  Class Class { get; set; }
    }
}
