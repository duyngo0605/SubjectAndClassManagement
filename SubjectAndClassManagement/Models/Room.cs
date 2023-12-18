using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Room
    {
        // Existing properties

        [Required]
        public string RoomId { get; set; }

        [Required]
        public string RoomName { get; set; }

        [Required]
        public int RoomCapacity { get; set; }

        [Required]
        public string BuildingName { get; set; }
    }
}
