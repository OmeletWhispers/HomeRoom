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
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public long? ParentId { get; set; }
        
        public long StudentId { get; set; }

        public long UserId { get; set; }


        public UserDto(long userId, string firstName, string lastName, string email)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        //public UserDto(long? parentId, long studentId, string firstName, string lastName, string email)
        //{
        //    ParentId = parentId;
        //    StudentId = studentId;
        //    ParentFirstName = firstName;
        //    ParentLastName = lastName;
        //    ParentEmail = email;
        //}

        //public UserDto(string firstName, string lastName, string email, string parentFirstName, string parentLastName, string parentEmail, long? parentId, long studentId)
        //{
        //    StudentId = studentId;
        //    FirstName = firstName;
        //    LastName = lastName;
        //    Email = email;

        //    ParentId = parentId;
        //    ParentFirstName = parentFirstName;
        //    ParentLastName = parentLastName;
        //    ParentEmail = parentEmail;
        //}
    }

    public class StudentDto
    {
        public long StudentId;
        public long? ParentId;

        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentEmail { get; set; }

        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentEmail { get; set; }

        public StudentDto(long studentId, long? parentId, string studentName, string studentLastName, string studentEmail, string parentName, string parentLastName, string parentEmail)
        {
            StudentId = studentId;
            ParentId = parentId;
            StudentFirstName = studentName;
            StudentLastName = studentLastName;
            StudentEmail = studentEmail;

            ParentFirstName = parentName;
            ParentLastName = parentLastName;
            ParentEmail = parentEmail;
        }

    }
}
