using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Class
    {
        // Existing properties

        [Required]
        public string ClassId { get; set; }

        [Required]
        public string SubjectId { get; set; }

        [Required]
        public string RoomId { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public int NumberOfMembers { get; set; }

        [Required]
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

        [Required]
        public virtual Room Room { get; set; }

        [Required]
        public virtual Teacher Teacher { get; set; }
    }

}
