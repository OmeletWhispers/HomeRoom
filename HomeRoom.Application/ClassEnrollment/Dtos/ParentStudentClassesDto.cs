using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.ClassEnrollment.Dtos
{
    public class ParentStudentClassesDto
    {
        public int Id { get; set; }

        public string ClassName { get; set; }

        public string Teacher { get; set; }

        public double Grade { get; set; }

        public long StudentId { get; set; }
    }
}
