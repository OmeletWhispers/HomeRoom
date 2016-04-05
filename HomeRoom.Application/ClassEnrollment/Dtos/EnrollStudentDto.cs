using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeRoom.Users.Dto;

namespace HomeRoom.ClassEnrollment.Dtos
{
    public class EnrollStudentDto
    {
        [Required]
        public int ClassId { get; set; }

        public long? ParentId { get; set; }

        public UserDto User { get; set; }

        public EnrollStudentDto(int classId, UserDto user)
        {
            User = user;
            ClassId = classId;
        }

    }
}
