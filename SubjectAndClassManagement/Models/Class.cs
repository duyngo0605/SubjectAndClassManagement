using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Class
    {
        [Key]
        [StringLength(10)]
        public string ClassId { get; set; }

        [Required]
        [StringLength(10)]
        public string SubjectId { get; set; }

        [Required]
        [StringLength(10)]
        public string RoomId { get; set; }

        [Required]
        [StringLength(10)]
        public string TeacherId { get; set; }

        [Required]
        public int NumberOfMembers { get; set; }

        [Required]
        [StringLength(10)]
        public string DaysOfWeek { get; set; }

        [Required]
        public int ClassPeriod { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int Year { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Room Room { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<StudentRegistration> Registrations { get; set; }
    }

}
