using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class TuitionPayment
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Payment ID")]
        public string payment_id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Student ID")]
        public string student_id { get; set; }

        [Required]
        [Display(Name = "Total Credits")]
        public int total_credits { get; set; }

        [Required]
        [Display(Name = "Tuition Fee")]
        public decimal tuition_fee { get; set; }

        [Required]
        [Display(Name = "Amount to Pay")]
        public decimal amount_to_pay { get; set; }

        [Required]
        [Display(Name = "Amount Paid")]
        public decimal amount_paid { get; set; }

        [Required]
        [Display(Name = "Payment Time")]
        public DateTime payment_time { get; set; }

        [Required]
        [Display(Name = "Excess Money")]
        public decimal excess_money { get; set; }

        [Required]
        [Display(Name = "Debt")]
        public decimal debt { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<Subject> FeeSubjects { get; set; }
    }
}
