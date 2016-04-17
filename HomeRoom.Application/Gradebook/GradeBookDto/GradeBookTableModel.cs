using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.Gradebook.GradeBookDto
{
    internal class GradeBookTableModel
    {
        public long StudentId { get; set; }

        public string StudentName { get; set; }
        
        public double OverallGrade { get; set; }

        public GradeBookTableModel(long studentId, string studentName, double overallGrade)
        {
            StudentId = studentId;
            StudentName = studentName;
            OverallGrade = overallGrade;
        }
    }

}
