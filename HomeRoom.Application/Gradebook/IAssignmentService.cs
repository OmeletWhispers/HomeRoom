using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;
using HomeRoom.GradeBook;

namespace HomeRoom.Gradebook
{
    public interface IAssignmentService : IApplicationService
    {
        /// <summary>
        /// Gets all class assignments.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllClassAssignments(int classId, DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Gets the upcoming assignments.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        List<Assignment> GetUpcomingAssignments(int classId);

            /// <summary>
        /// Gets all class assignments.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        List<Assignment> GetAllClassAssignments(int classId);

        /// <summary>
        /// Gets the created assignments.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        List<Assignment> GetCreatedAssignments(int classId);

            /// <summary>
        /// Gets the assignent by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Assignment GetById(int id);

        /// <summary>
        /// Saves the assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        void SaveAssignment(Assignment assignment);
    }
}
