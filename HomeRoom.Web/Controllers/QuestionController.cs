using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.TestGenerator;
using HomeRoom.Web.Models.TestGenerator;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    public class QuestionController : HomeRoomControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ISubjectService _subjectService;
        private readonly ICategoryService _categoryService;
        private readonly IAnswerService _answerService;

        public QuestionController(IQuestionService questionService, ISubjectService subjectService, ICategoryService categoryService, IAnswerService answerService)
        {
            _questionService = questionService;
            _subjectService = subjectService;
            _categoryService = categoryService;
            _answerService = answerService;
        }

        public ActionResult GetDataTable([ModelBinder(typeof (ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var categories = _questionService.GetAllQuestions(dataTableRequest);

            return Json(categories.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Question(int? id)
        {
            var categories = _categoryService.GetAllCategories();

            if (id.HasValue)
            {
                var model = new QuestionViewModel(categories);
                return PartialView("Forms/_QuestionForm", model);
            }
            else
            {
                var model = new QuestionViewModel(categories);
                return PartialView("Forms/_QuestionForm", model);
            }
        }

        [HttpPost]
        public JsonResult Question(QuestionViewModel question)
        {
            var questionType = question.AnswerChoices.Any() ? QuestionType.MultipleChoice : QuestionType.ShortAnswer;

            var newQuestion = new Question
            {
                Id = question.Id,
                QuestionType = questionType,
                IsPublic = question.IsPublic,
                Value = question.QuestionValue,
                CategoryId = question.CategoryId
            };

            _questionService.SaveQuestion(newQuestion);
            
            // if it is a multiple choice question make a list for all the answers.
            // short answer just save one answer choice
            if (questionType == QuestionType.MultipleChoice)
            {
                var answers = question.AnswerChoices.Select(item => new AnswerChoices
                {
                    IsCorrect = item.IsCorrect, QuestionId = newQuestion.Id, Value = item.Answer
                });

                _answerService.SaveAnswerChoices(answers);
            }
            else
            {
                var answer = new AnswerChoices
                {
                    QuestionId = newQuestion.Id,
                    IsCorrect = true,
                    Value = question.Answer.Answer
                };

                _answerService.SaveAnswerChoice(answer);
            }


            return Json(new {msg = "Save Successful!", error = false});
        }
    }
}