using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Room
    {
        [Key]
        [StringLength(10)]
        public string room_id { get; set; }

        [Required]
        [StringLength(255)]
        public string room_name { get; set; }

        [Required]
        public int room_capacity { get; set; }

        public string building_name { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
