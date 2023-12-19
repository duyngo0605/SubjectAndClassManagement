using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Class
    {
        [Key]
        [StringLength(10)]
        public string class_id { get; set; }

        [Required]
        [StringLength(10)]
        public string subject_id { get; set; }

        [Required]
        [StringLength(10)]
        public string room_id { get; set; }

        [Required]
        [StringLength(10)]
        public string teacher_id { get; set; }

        [Required]
        public int number_of_members { get; set; }

        [Required]
        [StringLength(10)]
        public string days_of_week { get; set; }

        [Required]
        public int class_period { get; set; }

        [Required]
        public DateTime start_date { get; set; }

        [Required]
        public DateTime end_date { get; set; }

        [Required]
        public int year { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Room Room { get; set; }
        public virtual Teacher Teacher { get; set; }
    }

}
