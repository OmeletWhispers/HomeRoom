using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using HomeRoom.TestGenerator;

namespace HomeRoom.Web.Models.TestGenerator
{
    public class CategoryViewModel : IInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public IEnumerable<SelectListItem> SubjectSelectList { get; set; }

        public CategoryViewModel()
        {
            SubjectSelectList = null;
        }

        public CategoryViewModel(Category category, List<Subject> subjects)
        {
            Id = category.Id;
            CategoryName = category.Name;
            SubjectId = category.SubjectId;

            SubjectSelectList = subjects.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        }

        public CategoryViewModel(List<Subject> subjects)
        {
            SubjectSelectList = subjects.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        }
    }
}