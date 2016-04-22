using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

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

    }
}
