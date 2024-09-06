using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971
{
    class Assessment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }
        public bool Notifications { get; set; }
    }
}
