﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SubjectAndClassManagement.Models
{
    public class StudentResult
    {
        [Key]
        [StringLength(20)]
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

        [StringLength(20)]
        [DisplayName("Student Results ID")]
        public string student_results_id { get; set; }

        [StringLength(30)]
        [DisplayName("Column Name")]
        public string column_name { get; set; }

        [DisplayName("Grade")]
        public double grade { get; set; }

        [DisplayName("Weight")]
        public double weight { get; set; }

        public StudentResult StudentResult { get; set; }
    }
}
