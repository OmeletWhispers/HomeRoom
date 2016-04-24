using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;
using HomeRoom.Gradebook.GradeBookDto;
using HomeRoom.GradeBook;

namespace HomeRoom.Gradebook
{
    public interface IGradeBookService : IApplicationService
    {
        /// <summary>
        /// Gets all class grades
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllClassGrades(int classId, DataTableRequestDto dataTableRequest);


        /// <summary>
        /// Gets the student assignment type average.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="assignmentTypeId">The assignment type identifier.</param>
        /// <returns></returns>
        double GetStudentAssignmentTypeAverage(long studentId, int assignmentTypeId);

        /// <summary>
        /// Gets the student grade for class.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        double GetStudentGradeForClass(long studentId, int classId);

        /// <summary>
        /// Gets the student grade for class.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="classId">The class identifier.</param>
        /// <param name="assignmentTypes">The assignment types.</param>
        /// <returns></returns>
        double GetStudentGradeForClass(long studentId, int classId, IEnumerable<AssignmentType> assignmentTypes);

        /// <summary>
        /// Gets the grade book.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        GradeBookDto.GradeBookDto GetGradeBook(int classId);

        /// <summary>
        /// Saves the grades.
        /// </summary>
        /// <param name="grades">The grades.</param>
        void SaveGrades(GradeBookDto.GradeBookDto grades);

        /// <summary>
        /// Gets the student grade book.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        StudentGradeBookDto GetStudentGradeBook(long studentId, int classId);

        /// <summary>
        /// Manages the student grades.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        ManageStudentGradesDto ManageStudentGrades(long studentId, int classId);

        /// <summary>
        /// Updates the grades.
        /// </summary>
        /// <param name="studentId">The student identifier</param>
        /// <param name="grades">The grades.</param>
        void UpdateGrades(long studentId, ManageStudentGradesDto grades);

        /// <summary>
        /// Saves the assignment grade.
        /// </summary>
        /// <param name="grade">The grade.</param>
        void SaveAssignmentGrade(Grade grade);
    }
}
