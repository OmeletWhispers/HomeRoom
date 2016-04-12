using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;
using HomeRoom.GradeBook;

namespace HomeRoom.Web.Models.Gradebook
{
    public class AssignmentTypeViewModel : IInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int PercentageValue { get; set; }

        public AssignmentTypeViewModel()
        {
            
        }

        public AssignmentTypeViewModel(int classId)
        {
            ClassId = classId;
        }

        public AssignmentTypeViewModel(AssignmentType type)
        {
            Id = type.Id;
            ClassId = type.ClassId;
            Name = type.Name;
            PercentageValue = (int) (type.Percentage * 100);
        }
    }
}