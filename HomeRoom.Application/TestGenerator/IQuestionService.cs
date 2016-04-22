using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;

namespace HomeRoom.TestGenerator
{
    public interface IQuestionService : IApplicationService
    {
        /// <summary>
        /// Gets all questions.
        /// </summary>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllQuestions(DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Saves the question.
        /// </summary>
        /// <param name="question">The question.</param>
        void SaveQuestion(Question question);
    }
}
