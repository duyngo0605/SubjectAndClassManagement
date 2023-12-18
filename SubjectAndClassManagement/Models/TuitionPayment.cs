using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class TuitionPayment
    {
        // Existing properties

        [Required]
        public string PaymentId { get; set; }

        [Required]
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

        [Required]
        public virtual Student Student { get; set; }
    }
}
