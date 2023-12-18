using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Room
    {
        [Key]
        [StringLength(10)]
        public string RoomId { get; set; }

        [Required]
        [StringLength(255)]
        public string RoomName { get; set; }

        [Required]
        public int RoomCapacity { get; set; }

        public string BuildingName { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
