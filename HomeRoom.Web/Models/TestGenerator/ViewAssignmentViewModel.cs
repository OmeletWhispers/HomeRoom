using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.TestGenerator.Dto;
using HomeRoom.Users;

namespace HomeRoom.Web.Models.TestGenerator
{
    public class ViewAssignmentViewModel
    {
        public List<AssignmentQuestionDto> Questions { get; set; }

        public string AssignmentName { get; set; }

        public long UserId { get; set; }

        public int AssignmentId { get; set; }

        public IEnumerable<SelectListItem> StudentSelectList { get; set; }

        public ViewAssignmentViewModel(string name, int assignmentId, IEnumerable<User> users)
        {
            AssignmentName = name;
            StudentSelectList = users.ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            });
            AssignmentId = assignmentId;
        }

        public ViewAssignmentViewModel(List<AssignmentQuestionDto> question)
        {
            Questions = question;
        }

        public ViewAssignmentViewModel(string name, List<AssignmentQuestionDto> question, long userId, int assignmentId)
        {
            AssignmentName = name;
            Questions = question;
            UserId = userId;
            AssignmentId = assignmentId;
        }
    }
}