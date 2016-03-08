using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using HomeRoom.Enumerations;

namespace HomeRoom.Web.Models.Membership
{
    public class UserViewModel : IInputDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public AccountType AccountType { get; set; }
    }
}