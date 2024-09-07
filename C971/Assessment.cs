using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971
{
public   class Assessment
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("AsessmentId")]
        public int AssessmentId { get; set; }
        [Column("AssessmentTitle")]

        public string AssessmentTitle { get; set; }
        [Column("AssessmentType")]

        public string AssessmentType { get; set; }
        [Column("CourseId")]

        public int CourseId { get; set; }
 
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("StartDate")]

        public DateTime StartDate { get; set; }


    }
}
