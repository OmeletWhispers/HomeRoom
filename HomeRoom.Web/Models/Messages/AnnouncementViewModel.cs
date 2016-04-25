using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;

namespace HomeRoom.Web.Models.Messages
{
    public class AnnouncementViewModel : IInputDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int ClassId { get; set; }
    }
}