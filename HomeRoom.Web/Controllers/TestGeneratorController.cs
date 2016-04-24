using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using HomeRoom.ClassEnrollment;
using HomeRoom.Gradebook;
using HomeRoom.GradeBook;
using HomeRoom.TestGenerator;
using HomeRoom.Web.Models.JsonModels;
using HomeRoom.Web.Models.TestGenerator;
using Newtonsoft.Json;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TestGeneratorController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly ICategoryService _categoryService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IAssignmentService _assignmentService;
        private readonly IClassService _classService;
        private readonly IGradeBookService _gradeBookService;

        #endregion

        #region Constructors

        public TestGeneratorController(ICategoryService categoryService, IQuestionService questionService, IAnswerService answerService, IAssignmentService assignmentService, IClassService classService, IGradeBookService gradeBookService)
        {
            _categoryService = categoryService;
            _questionService = questionService;
            _answerService = answerService;
            _assignmentService = assignmentService;
            _classService = classService;
            _gradeBookService = gradeBookService;
        }

        #endregion

        #region Public Methods

        public JsonResult CreateTest(TestJson testJson)
        {
            _questionService.DeleteAssignmentQuestions(testJson.AssignmentId);

            foreach (var assignmentQuestion in testJson.Questions.Select(item => new AssignmentQuestions
            {
                AssignmentId = testJson.AssignmentId,
                QuestionId = item.QuestionId,
                PointValue = item.PointValue
            }))
            {
                _questionService.SaveAssignmentQuestion(assignmentQuestion);
            }

            return Json(new {msg = "Save Successful!"});
        }

        public PartialViewResult GetCategories(int subjectId)
        {
            var categories = _categoryService.GetAllCategoriesBySubject(subjectId);

            IEnumerable<SelectListItem> categorySelectList = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            return PartialView("_CategorySelectList", categorySelectList);
        }

        public PartialViewResult GetTestGenerator(int assignmentId)
        {
            // get all the questions that are not assignment questions for this assignment
            var questionsNotAsked = _questionService.GetAllQuestionsNotAsked(assignmentId);
            var assignmentQuestions = _questionService.GetAllAssignmentQuestions(assignmentId);

            var model = new TestGeneratorViewModel(assignmentQuestions, questionsNotAsked);

            return PartialView("_GeneratorPanels", model);
        }

        public PartialViewResult GetAnswers(long studentId, int assignmentId)
        {
            var answers = _answerService.GetAllAssignmentAnswers(studentId, assignmentId);

            var model = new ViewAssignmentViewModel(answers);

            return PartialView("_GradeAssignment", model);
        }

        [HttpGet]
        public JsonResult GetQuestions(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                var questions = _questionService.GetAllQuestionByCategory(categoryId.Value);

                var questionList = questions.Select(x => new QuestionDto
                {
                    Question = x.Value,
                    QuestionId = x.Id
                }).ToList();

                return Json(questionList, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var questions = _questionService.GetAllQuestions();

                var questionList = questions.Select(x => new QuestionDto
                {
                    Question = x.Value,
                    QuestionId = x.Id
                }).ToList();

                return Json(questionList, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewAssignment(int assignmentId)
        {
            var assignmentQuestions = _questionService.GetQuestionsForAssignment(assignmentId);
            var assignment = _assignmentService.GetById(assignmentId);
            var enrollments = _classService.GetAllEnrollments(assignment.ClassId).ToList();

            var model = new ViewAssignmentViewModel(assignment.Name, assignmentId, enrollments);

            return View("ViewAssignment", model);
        }

        public ActionResult TakeAssignment(int assignmentId)
        {
            var userId = AbpSession.GetUserId();
            var assignmentQuestions = _questionService.GetQuestionsForAssignment(assignmentId);
            var assignment = _assignmentService.GetById(assignmentId);

            var model = new ViewAssignmentViewModel(assignment.Name, assignmentQuestions, userId, assignment.Id);

            return View("TakeAssignment", model);
        }

        [HttpPost]
        public JsonResult SubmitAssignment(List<AnswerChoiceJson> answerJson)
        {
            foreach (var choice in answerJson.Select(x => new AssignmentAnswers
            {
                AnswerChoiceId = x.AnswerChoiceId,
                AssignmentId = x.AssignmentId,
                StudentId = x.StudentId,
                QuestionId = x.QuestionId,
                Text = x.Text
            }))
            {
                choice.StudentId = 27; //Todo: Remove this hardcoded value! Only for Testing!!
                _answerService.SaveAssignmentAnswer(choice);
            }
            return Json(new {msg = "Save Successful!"});
        }

        [HttpPost]
        public JsonResult GradeAssignment(List<GradesJson> grades)
        {
            var possibleTotalPoints = grades.Sum(x => x.PointsWorth);
            var pointsReceived = grades.Sum(x => x.PointsReceived);
            var gradePercent = Math.Round(((double)pointsReceived/possibleTotalPoints)*100, MidpointRounding.AwayFromZero);
            var assignmentId = grades.Select(x => x.AssignmentId).FirstOrDefault();
            var studentId = grades.Select(x => x.StudentId).FirstOrDefault();

            var newGrade = new Grade
            {
                AssignmentId = assignmentId,
                StudentId = studentId,
                Value = gradePercent
            };
            _gradeBookService.SaveAssignmentGrade(newGrade);
            var msg = string.Format("Save Grade of: {0}", newGrade.Value);

            return Json(new {msg = msg});
        }
        #endregion
    }
}