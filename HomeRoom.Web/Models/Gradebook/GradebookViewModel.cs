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
}