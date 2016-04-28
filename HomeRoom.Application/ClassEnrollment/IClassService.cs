﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.ClassEnrollment.Dtos;
using HomeRoom.DataTableDto;
using HomeRoom.Users;
using HomeRoom.Users.Dto;

namespace HomeRoom.ClassEnrollment
{
    public interface IClassService  : IApplicationService
    {
        /// <summary>
        /// Gets all teacher classes.
        /// </summary>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllTeacherClasses(DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Gets all enrollments.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllEnrollments(int classId, DataTableRequestDto dataTableRequest);


        /// <summary>
        /// Gets all enrollments.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        IEnumerable<User> GetAllEnrollments(int classId);

        /// <summary>
        /// Gets the student classes.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns></returns>
        List<ParentStudentClassesDto> GetStudentClasses(long studentId);

        /// <summary>
        /// Gets the student classes.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllStudentClasses(DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Gets the class by identifier.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        Class GetClassById(int classId);

        /// <summary>
        /// Saves the class.
        /// </summary>
        /// <param name="theClass">The class.</param>
        void SaveClass(Class theClass);

        /// <summary>
        /// Deletes the class.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        void DeleteClass(int classId);

        /// <summary>
        /// Unenrolls the student.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="studentId">The student identifier.</param>
        void UnenrollStudent(int classId, long studentId);

        /// <summary>
        /// Determines whether [is student enrolled] [the specified enrolled student].
        /// </summary>
        /// <param name="enrolledStudent">The enrolled student.</param>
        /// <returns></returns>
        bool IsStudentEnrolled(EnrollStudentDto enrolledStudent);

        /// <summary>
        /// Determines whether [has parent account] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        bool HasParentAccount(UserDto user);

        /// <summary>
        /// Enrolls the student.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="classId">The class identifier.</param>
        void EnrollStudent(long studentId, int classId);

    }
}
