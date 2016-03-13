using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;

namespace HomeRoom.Web.Models.ClassEnrollment
{
    public class ClassFormViewModel : IInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }
    }
}