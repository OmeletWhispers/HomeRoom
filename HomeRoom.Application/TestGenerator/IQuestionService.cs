using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;
using HomeRoom.TestGenerator.Dto;

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
        /// Gets all questions.
        /// </summary>
        /// <returns></returns>
        List<Question> GetAllQuestions();

        /// <summary>
        /// Gets all question by category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        List<Question> GetAllQuestionByCategory(int categoryId); 

        /// <summary>
        /// Saves the question.
        /// </summary>
        /// <param name="question">The question.</param>
        void SaveQuestion(Question question);

        /// <summary>
        /// Gets all questions not asked.
        /// </summary>
        /// <param name="assignmentId">The assignment identifier.</param>
        /// <returns></returns>
        List<Question> GetAllQuestionsNotAsked(int assignmentId);

        /// <summary>
        /// Gets all assignment questions.
        /// </summary>
        /// <param name="assignmentId">The assignment identifier.</param>
        /// <returns></returns>
        List<AssignmentQuestionDto> GetAllAssignmentQuestions(int assignmentId);

        /// <summary>
        /// Gets the questions for assignment.
        /// </summary>
        /// <param name="assignmentId">The assignment identifier.</param>
        /// <returns></returns>
        List<AssignmentQuestionDto> GetQuestionsForAssignment(int assignmentId); 

        /// <summary>
        /// Saves the assignment question.
        /// </summary>
        /// <param name="question">The question.</param>
        void SaveAssignmentQuestion(AssignmentQuestions question);

        /// <summary>
        /// Deletes the assignment questions.
        /// </summary>
        /// <param name="assignmentId">The assignment identifier.</param>
        void DeleteAssignmentQuestions(int assignmentId);
    }
}
