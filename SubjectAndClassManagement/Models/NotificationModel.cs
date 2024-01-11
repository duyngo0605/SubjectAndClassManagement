using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SubjectAndClassManagement.Models
{
    public class NotificationModel
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Notify Id")]
        public string notify_id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Title")]
        public string notify_header { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime notify_day { get; set; }

        [Required]
        [Display(Name = "Decription")]
        public string notify_decription { get; set;}

    }
}
