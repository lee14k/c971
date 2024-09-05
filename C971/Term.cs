using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971
{
    public class Term
    {
         [PrimaryKey]
        [AutoIncrement]
        [Column("TermId")]
        public int TermId { get; set; }
        [Column("TermTitle")]
        public string TermTitle { get; set; }
        [Column("StartDate")]

        public DateTime StartDate { get; set; }
        [Column("EndDate")]

        public DateTime EndDate { get; set; }
    }
}
