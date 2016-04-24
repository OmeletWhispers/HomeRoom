using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.TestGenerator.Dto;

namespace HomeRoom.TestGenerator
{
    public interface IAnswerService : IApplicationService
    {
        /// <summary>
        /// Saves the answer choice.
        /// </summary>
        /// <param name="answer">The answer.</param>
        void SaveAnswerChoice(AnswerChoices answer);

        /// <summary>
        /// Saves the answer choices.
        /// </summary>
        /// <param name="answerChoiceses">The answer choiceses.</param>
        void SaveAnswerChoices(IEnumerable<AnswerChoices> answerChoiceses);

        /// <summary>
        /// Saves the assignment answer.
        /// </summary>
        /// <param name="answer">The answer.</param>
        void SaveAssignmentAnswer(AssignmentAnswers answer);

        /// <summary>
        /// Gets all assignment answers.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="assignmentId">The assignment identifier.</param>
        /// <returns></returns>
        List<AssignmentAnswersDto> GetAllAssignmentAnswers(long studentId, int assignmentId);

    }
}
