using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class Room
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "Room ID")]
        public string room_id { get; set; }

        [Required]
        [Display(Name = "Room Capacity")]
        public int room_capacity { get; set; }

        [Display(Name = "Building Name")]
        public string building_name { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
