using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.ClassEnrollment.Dtos;
using HomeRoom.DataTableDto;

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
        /// Enrolls the student.
        /// </summary>
        /// <param name="enrollStudent">The enroll student.</param>
        void EnrollStudent(EnrollStudentDto enrollStudent);

    }
}
