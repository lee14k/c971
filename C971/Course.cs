using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971
{
    public class Course
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("CourseId")]   
        public int CourseId { get; set; }
        [Column("TermId")]
        public int TermId { get; set; }
        [Column("CourseTitle")]
        public string CourseTitle { get; set; }
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("Status")]
        public string Status { get; set; }
        [Column("InstructorId")]
        public int InstructorId { get; set; }
        [Column("Notifications")]
        public bool Notifications { get; set; }
        [Column("Notes")]
        public string Notes { get; set; }
    }
}
