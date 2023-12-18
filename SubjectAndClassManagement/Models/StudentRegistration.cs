namespace SubjectAndClassManagement.Models
{
    public class StudentRegistration
    {
        public string RegistrationId { get; set; }
        public string StudentId { get; set; }
        public string ClassId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }
    }
}
