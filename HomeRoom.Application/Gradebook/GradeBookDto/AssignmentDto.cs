using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.Gradebook.GradeBookDto
{
    public class AssignmentDto
    {
        public int AssignmentId { get; set; }

        public string AssignmentName { get; set; }

    }

    public class AssignmentTypeDto
    {
        public double Percentage { get; set; }
        public double Average { get; set; }
    }

    public class GradeBookDto
    {
        public int ClassId { get; set; }

        public List<AssignmentDto>  Assignments { get; set; }

        public List<GradeDto> Grades { get; set; }

        public int AssignmentId { get; set; }
    }

    public class StudentGradeBookDto
    {
        public long StudentId { get; set; }

        public string StudentName { get; set; }

        public double OverallGrade { get; set; }

        public List<AssignmentGradeDto> Assignments { get; set; } 
    }

    public class AssignmentGradeDto
    {
        public int AssignmentId { get; set; }

        public string AssignmentName { get; set; }

        public double AssignmentGrade { get; set; }

        public bool CanView { get; set; }
    }

    public class GradeDto
    {
        public int GradeValue { get; set; }

        public string StudentName { get; set; }

        public long StudentId { get; set; }
    }

    public class StudentAssignmentGradeDto
    {
        public int GradeId { get; set; }
        public double Value { get; set; }
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
    }

    public class ManageStudentGradesDto
    {
        public List<StudentAssignmentGradeDto> StudentAssignmentGrades { get; set; } 
    }
}
