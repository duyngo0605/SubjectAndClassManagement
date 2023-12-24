using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SubjectAndClassManagement.Models
{
    public class Profile
    {
        [Key]
        [DisplayName("Profile ID")]
        public int profile_id { get; set; }

        [Required]
        [DisplayName("Username")]
        [StringLength(255)]
        public string username { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? date_of_birth { get; set; }

        [DisplayName("Gender")]
        [StringLength(10)]
        public string gender { get; set; }

        [DisplayName("Address")]
        [StringLength(255)]
        public string address { get; set; }

        [DisplayName("Citizen ID Card")]
        [StringLength(12)]
        public string citizen_id_card { get; set; }

        // Navigation property for the User
        public virtual User User { get; set; }

        // Default constructor
        public Profile() { }

        // Constructor with parameters
        public Profile(string para)
        {
            username = para;

        }

        
    }
}
