using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class StudentRegistration
    {
        [Key]
        [StringLength(10)]
        public string RegistrationId { get; set; }

        [Required]
        [StringLength(10)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(10)]
        public string ClassId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public string Reason { get; set; }

        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }
    }
}
