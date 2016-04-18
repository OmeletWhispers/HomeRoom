using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.Gradebook.GradeBookDto;
using HomeRoom.GradeBook;
using HomeRoom.Users;

namespace HomeRoom.Web.Models.Gradebook
{
    public class GradebookViewModel
    {
        public IEnumerable<string> GradebookColumns { get; set; }

        public IEnumerable<GradeDto> StudentGrades { get; set; } 

        public int ClassId { get; set; }

        public int AssignmentId { get; set; }

        public IEnumerable<SelectListItem> AssignmentSelectList { get; set; }

        public GradebookViewModel()
        {
            StudentGrades = new List<GradeDto>();
        } 

        public GradebookViewModel(GradeBookDto gradeBook)
        {
            AssignmentSelectList = gradeBook.Assignments.Select(x => new SelectListItem
            {
                Value = x.AssignmentId.ToString(),
                Text = x.AssignmentName

            });

            StudentGrades = gradeBook.Grades;
        }

        public GradebookViewModel(IEnumerable<string> columns)
        {
            GradebookColumns = columns;
        }
    }

    public class StudentAssignmentGradesViewModel
    {
        public long StudentId { get; set; }

        public int ClassId { get; set; }

        public ManageStudentGradesDto StudentGrades { get; set; }

        public StudentAssignmentGradesViewModel()
        {
            StudentGrades = new ManageStudentGradesDto();
        }

        public StudentAssignmentGradesViewModel(long studentId, int classId, ManageStudentGradesDto grades)
        {
            StudentId = studentId;
            ClassId = classId;
            StudentGrades = grades;
        }
    }
}