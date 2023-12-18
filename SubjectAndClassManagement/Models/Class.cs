using System;
using System.Collections.Generic;

namespace SubjectAndClassManagement.Models
{
    public class Class
    {
        public string ClassId { get; set; }
        public string SubjectId { get; set; }
        public string RoomId { get; set; }
        public string TeacherId { get; set; }
        public int NumberOfMembers { get; set; }
        public string DaysOfWeek { get; set; }
        public int ClassPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Room Room { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
