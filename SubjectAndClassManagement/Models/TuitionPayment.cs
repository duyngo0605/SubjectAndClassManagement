namespace SubjectAndClassManagement.Models
{
    public class TuitionPayment
    {
        public string PaymentId { get; set; }
        public string StudentId { get; set; }
        public int TotalCredits { get; set; }
        public decimal TuitionFee { get; set; }
        public decimal AmountToPay { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentTime { get; set; }
        public decimal ExcessMoney { get; set; }
        public decimal Debt { get; set; }

        public virtual Student Student { get; set; }
    }
}
