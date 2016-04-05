using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HomeRoom.Sessions.Dto;

namespace HomeRoom.Users.Dto
{
    public class UserDto
    {
        [Required]
        public string StudentFirstName { get; set; }

        [Required]
        public string StudentLastName { get; set; }

        [Required]
        public string StudentEmail { get; set; }

        public long? ParentId { get; set; }

        public string ParentFirstName { get; set; }
        
        public string ParentLastName { get; set; }

        public string ParentEmail { get; set; }

        public UserDto(string firstName, string lastName, string email)
        {
            StudentFirstName = firstName;
            StudentLastName = lastName;
            StudentEmail = email;
        }

        public UserDto(string firstName, string lastName, string email, string parentFirstName, string parentLastName, string parentEmail, long? parentId)
        {
            StudentFirstName = firstName;
            StudentLastName = lastName;
            StudentEmail = email;

            ParentId = parentId;
            ParentFirstName = parentFirstName;
            ParentLastName = parentLastName;
            ParentEmail = parentEmail;
        }
    }
}
