﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Class
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Class ID")]
        public string class_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Subject ID")]
        public string subject_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Room ID")]
        public string room_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Teacher ID")]
        public string teacher_id { get; set; }

        [Required]
        [Display(Name = "Number of Members")]
        public int number_of_members { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Days of Week")]
        public string days_of_week { get; set; }

        [Required]
        [Display(Name = "Class Period")]
        public int class_period { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime start_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime end_date { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Room Room { get; set; }
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<StudentRegistration> StudentRegistrations { get; set; }
    }
}
