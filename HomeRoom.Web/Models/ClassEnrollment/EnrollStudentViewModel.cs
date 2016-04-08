using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;
using HomeRoom.Membership;
using HomeRoom.Users.Dto;

namespace HomeRoom.Web.Models.ClassEnrollment
{
    public class EnrollStudentViewModel : IInputDto
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public long StudentId { get; set; }

        public long? ParentId { get; set; }

        
        [Required]
        public string StudentFirstName { get; set; }

        [Required]
        public string StudentLastName { get; set; }

        [Required]
        public string StudentEmail { get; set; }

        public string ParentFirstName { get; set; }

        public string ParentLastName { get; set; }

        public string ParentEmailAddress { get; set; }

        public EnrollStudentViewModel()
        {
            
        }

        public EnrollStudentViewModel(StudentDto student, int classId)
        {
            ClassId = classId;
            StudentId = student.StudentId;

            StudentFirstName = student.StudentFirstName;
            StudentLastName = student.StudentLastName;
            StudentEmail = student.StudentEmail;
            ParentId = student.ParentId;

            // student has a parent, fill out that information
            if (student.ParentId.HasValue)
            {
                ParentFirstName = student.ParentFirstName;
                ParentLastName = student.ParentLastName;
                ParentEmailAddress = student.ParentEmail;
            }
        }

    }
}