using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;

namespace HomeRoom.Web.Models.Account
{
    public class ChangePasswordViewModel : IInputDto
    {
        [Required]
        public long AccountId { get; set; }
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string NewPasswordConfirm { get; set; }

    }
}