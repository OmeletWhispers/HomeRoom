using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;

namespace HomeRoom.TestGenerator
{
    public interface ISubjectService: IApplicationService
    {
        /// <summary>
        /// Gets all subjects.
        /// </summary>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllSubjects(DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Gets all subjects.
        /// </summary>
        /// <returns></returns>
        List<Subject> GetAllSubjects();

        /// <summary>
        /// Gets the subject by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Subject GetSubjectById(int id);

        /// <summary>
        /// Saves the subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        void SaveSubject(Subject subject);


    }
}
