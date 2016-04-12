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
    public interface IAssignmentTypeService : IApplicationService
    {
        /// <summary>
        /// Gets all class assignment types.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllClassAssignmentTypes(int classId, DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Gets all assignment types.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns></returns>
        List<AssignmentType> GetAllAssignmentTypes(int classId);

            /// <summary>
        /// Gets the assignment type by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        AssignmentType GetAssignmentTypeById(int id);

        /// <summary>
        /// Saves the type of the assignment.
        /// </summary>
        /// <param name="assignmentType">Type of the assignment.</param>
        void SaveAssignmentType(AssignmentType assignmentType);
    }
}
