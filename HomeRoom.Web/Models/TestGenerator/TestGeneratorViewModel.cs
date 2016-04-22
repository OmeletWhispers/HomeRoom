using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.GradeBook;
using HomeRoom.TestGenerator;

namespace HomeRoom.Web.Models.TestGenerator
{
    public class TestGeneratorViewModel
    {
        public IEnumerable<SelectListItem> AssignmentSelectList { get; set; } 

        public List<QuestionDto> QuestionList { get; set; } 

        public int AssignmentId { get; set; }

        public TestGeneratorViewModel(IEnumerable<Assignment> assignments, List<Question> questions)
        {
            AssignmentSelectList = assignments.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            QuestionList = questions.ToList().Select(x => new QuestionDto
            {
                QuestionId = x.Id,
                Question = x.Value
            }).ToList();

            AssignmentId = 0;
        }
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }

        public string Question { get; set; }
    }
}