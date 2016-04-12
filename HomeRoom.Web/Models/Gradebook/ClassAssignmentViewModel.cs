using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using HomeRoom.Enumerations;
using HomeRoom.GradeBook;

namespace HomeRoom.Web.Models.Gradebook
{
    public class ClassAssignmentViewModel : IInputDto
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        [Required]
        public int AssignmentTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public AssignmentStatus Status { get; set; }

        public IEnumerable<SelectListItem> AssignmentTypeSelectList { get; }

        public ClassAssignmentViewModel()
        {
            AssignmentTypeSelectList = null;
        }

        public ClassAssignmentViewModel(Assignment assignment, IEnumerable<AssignmentType> assignmentTypes)
        {
            AssignmentTypeSelectList = assignmentTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = string.Format("{0}-({1}%)", x.Name, (x.Percentage * 100))
            });

            ClassId = assignment.ClassId;
            AssignmentId = assignment.Id;
            AssignmentTypeId = assignment.AssignmentTypeId;
            Name = assignment.Name;
            Description = assignment.Description;
            StartDate = assignment.StartDate;
            DueDate = assignment.DueDate;
            Status = assignment.Status;
        }

        public ClassAssignmentViewModel(int classId, IEnumerable<AssignmentType> assignmentTypes)
        {
            ClassId = classId;

            AssignmentTypeSelectList = assignmentTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = string.Format("{0}-({1}%)", x.Name, (x.Percentage * 100))
            });

            StartDate = DateTime.Now;
            DueDate = DateTime.Now;
        }

    }
}