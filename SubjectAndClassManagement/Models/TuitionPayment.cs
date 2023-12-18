using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class TuitionPayment
    {
        [Key]
        [StringLength(10)]
        public string PaymentId { get; set; }

        [Required]
        [StringLength(10)]
        public string StudentId { get; set; }

        [Required]
        public int TotalCredits { get; set; }

        [Required]
        public decimal TuitionFee { get; set; }

        [Required]
        public decimal AmountToPay { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        public decimal ExcessMoney { get; set; }

        [Required]
        public decimal Debt { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<Subject> FeeSubjects { get; set; }
    }
}
