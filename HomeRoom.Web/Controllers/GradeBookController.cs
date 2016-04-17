using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.ClassEnrollment;
using HomeRoom.Enumerations;
using HomeRoom.Gradebook;
using HomeRoom.Gradebook.GradeBookDto;
using HomeRoom.Web.Models.Gradebook;

namespace HomeRoom.Web.Controllers
{
    public class GradeBookController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly IGradeBookService _gradeBookService;
        private readonly IAssignmentService _assignmentService;
        private readonly IClassService _classService;

        #endregion

        #region Constructors
        public GradeBookController(IAssignmentService assignmentService, IGradeBookService gradeBookService, IClassService classService)
        {
            _assignmentService = assignmentService;
            _gradeBookService = gradeBookService;
            _classService = classService;
        }
        #endregion

        #region Public Methods

        [HttpGet]
        public PartialViewResult GradeBook(int classId)
        {
            var gradeBook = _gradeBookService.GetGradeBook(classId);

            var model = new GradebookViewModel(gradeBook);

            return PartialView("Forms/_GradeBookForm", model);
        }

        [HttpPost]
        public JsonResult GradeBook(GradebookViewModel grades)
        {
            var assignmentId = grades.AssignmentId;
            var gradeBook = grades.StudentGrades;

            var gradeBookDto = new GradeBookDto
            {
                AssignmentId = assignmentId,
                Grades = gradeBook.ToList()
            };

            _gradeBookService.SaveGrades(gradeBookDto);

            // once we grade an assignment, make it closed.
            var assignment = _assignmentService.GetById(assignmentId);
            assignment.Status = AssignmentStatus.Closed;
            _assignmentService.SaveAssignment(assignment);

            return Json(new {error = false, msg = "Save Successful!"});
        }

        [HttpGet]
        public PartialViewResult StudentGrades(int classId, long studentId)
        {
            var studentGradebook = _gradeBookService.GetStudentGradeBook(studentId, classId);

            return PartialView("Tables/_ViewStudentGradesTable", studentGradebook);
        }

        #endregion
    }
}