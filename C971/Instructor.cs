using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace C971
{

   public class Instructor
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("InstructorId")]
        public int InstructorId { get; set; }
        [Column("InstructorName")]
        public string InstructorName { get; set; }
        [Column("InstructorEmail")]
        public string InstructorEmail { get; set; }
        [Column("InstructorPhone")]
        public string InstructorPhone { get; set; }
        public int CourseId { get ; set; }
    }
}
