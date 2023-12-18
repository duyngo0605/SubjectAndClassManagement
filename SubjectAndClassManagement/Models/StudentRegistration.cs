using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class StudentRegistration
    {
        // Existing properties

        [Required]
        public string RegistrationId { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public string ClassId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public string Status { get; set; }

        public string Reason { get; set; }

        public virtual Student Student { get; set; }

        [Required]
        public virtual Class Class { get; set; }
    }
}
