using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;
using HomeRoom.TestGenerator;

namespace HomeRoom.Web.Models.TestGenerator
{
    public class SubjectViewModel : IInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public SubjectViewModel()
        {
            
        }

        public SubjectViewModel(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
        }
    }
}