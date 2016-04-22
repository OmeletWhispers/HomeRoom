using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using HomeRoom.Enumerations;
using HomeRoom.TestGenerator;

namespace HomeRoom.Web.Models.TestGenerator
{
    public class QuestionViewModel : IInputDto
    {
        public int Id { get; set; }

        public string QuestionValue { get; set; }

        public int CategoryId { get; set; }

        public int SubjectId { get; set; }

        public QuestionType Type { get; set; }

        public bool IsPublic { get; set; }

        public AnswerChoiceViewModel Answer { get; set; }

        public IEnumerable<AnswerChoiceViewModel> AnswerChoices { get; set; } 

        public IEnumerable<SelectListItem> CategorySelectList { get; set; }

        public IEnumerable<SelectListItem> SubjectSelectList { get; set; }

        public QuestionViewModel()
        {
            AnswerChoices = new List<AnswerChoiceViewModel>();
        }

        public QuestionViewModel(IEnumerable<Category> categories)
        {
            CategorySelectList = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            Answer = new AnswerChoiceViewModel();
        } 
    }

    public class AnswerChoiceViewModel : IInputDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Answer{ get; set; }

        public bool IsCorrect { get; set; }
    }
}