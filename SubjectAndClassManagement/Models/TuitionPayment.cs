using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class TuitionPayment
    {
        [Key]
        [StringLength(10)]
        public string payment_id { get; set; }

        [Required]
        [StringLength(10)]
        public string student_id { get; set; }

        [Required]
        public int total_credits { get; set; }

        [Required]
        public decimal tuition_fee { get; set; }

        [Required]
        public decimal amount_to_pay { get; set; }

        [Required]
        public decimal amount_paid { get; set; }

        [Required]
        public DateTime payment_time { get; set; }

        [Required]
        public decimal excess_money { get; set; }

        [Required]
        public decimal debt { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<Subject> FeeSubjects { get; set; }
    }
}
