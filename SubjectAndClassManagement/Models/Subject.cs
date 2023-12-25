using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Subject
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Subject ID")]
        public string subject_id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Subject Name")]
        public string subject_name { get; set; }

        [Display(Name = "Subject Description")]
        public string subject_description { get; set; }

        [Required]
        [Display(Name = "Number of Credits")]
        public int credit { get; set; }

        public virtual ICollection<TuitionPayment> TuitionPayments { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
