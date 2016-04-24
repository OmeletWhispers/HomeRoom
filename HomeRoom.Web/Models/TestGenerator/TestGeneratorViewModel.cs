using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.GradeBook;
using HomeRoom.TestGenerator;
using HomeRoom.TestGenerator.Dto;

namespace HomeRoom.Web.Models.TestGenerator
{
    public class TestGeneratorViewModel
    {
        public IEnumerable<SelectListItem> AssignmentSelectList { get; set; } 

        public IEnumerable<SelectListItem> SubjectSelectList { get; set; } 

        public List<QuestionDto> QuestionList { get; set; } 

        public List<AssignmentQuestionDto> AssignmentQuestions { get; set; } 

        public int AssignmentId { get; set; }

        public TestGeneratorViewModel(List<AssignmentQuestionDto> assignmentQuestions, List<Question> questions)
        {
            QuestionList = questions.Select(x => new QuestionDto
            {
                Question = x.Value,
                QuestionId = x.Id
            }).ToList();

            AssignmentQuestions = assignmentQuestions;
        }

        public TestGeneratorViewModel(IEnumerable<Assignment> assignments, IEnumerable<Subject> subjects)
        {
            AssignmentSelectList = assignments.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            SubjectSelectList = subjects.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            AssignmentId = 0;
        }
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }

        public string Question { get; set; }
    }
}