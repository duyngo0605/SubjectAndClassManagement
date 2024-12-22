using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class StudentResult
    {
        [Key]
        [StringLength(30)]
        [DisplayName("Student Results ID")]
        public string student_results_id { get; set; }

        [StringLength(10)]
        [DisplayName("Registration ID")]
        public string registration_id { get; set; }

        [DisplayName("Grade")]
        public double grade { get; set; }

        public StudentRegistration StudentRegistration { get; set; }
        public virtual ICollection<ResultColumn> ResultColumns { get; set; }
    }

    public class ResultColumn
    {
        [Key]
        [DisplayName("Result Column ID")]
        public int resultcolumn_id { get; set; }

        [StringLength(30)]
        [DisplayName("Student Results ID")]
        public string student_results_id { get; set; }

        [StringLength(30)]
        [DisplayName("Column Name")]
        public string column_name { get; set; }

        [DisplayName("Grade")]
        public double grade { get; set; }

        public StudentResult StudentResult { get; set; }
    }

    public class StudentResultDTO
    {
        public Student Student { get; set; }
        public double AttendanceGrade { get; set; }
        public double MidtermGrade { get; set; }
        public double FinalGrade { get; set; }
        public double AverageGrade { get; set; }
    }

    public class ClassWeights
    {
        [Key]
        [StringLength(30)]
        public string classweight_id { get; set; }

        [StringLength(30)]
        public string class_id { get; set; }

        [Required]
        [Range(0.01, 1.0, ErrorMessage = "Attendance weight must be between 0 and 1.")]
        public double attendance_weight { get; set; }

        [Required]
        [Range(0.01, 1.0, ErrorMessage = "Midterm weight must be between 0 and 1.")]
        public double midterm_weight { get; set; }

        [Required]
        [Range(0.01, 1.0, ErrorMessage = "Final weight must be between 0 and 1.")]
        public double final_weight { get; set; }

        // Navigation property
        public virtual Class Class { get; set; }
    }
}
