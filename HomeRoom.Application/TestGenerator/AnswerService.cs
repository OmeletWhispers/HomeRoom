using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Castle.MicroKernel;

namespace HomeRoom.TestGenerator
{
    class AnswerService : HomeRoomAppServiceBase, IAnswerService
    {
        private readonly IRepository<AnswerChoices> _answerChoiceRepository;
        private readonly IRepository<AssignmentAnswers> _assignmentAnswers; 

        public AnswerService(IRepository<AnswerChoices> answerChoiceRepository, IRepository<AssignmentAnswers> assignmentAnswers)
        {
            _answerChoiceRepository = answerChoiceRepository;
            _assignmentAnswers = assignmentAnswers;
        }

        public void SaveAnswerChoice(AnswerChoices answer)
        {
            _answerChoiceRepository.Insert(answer);
        }

        public void SaveAnswerChoices(IEnumerable<AnswerChoices> answerChoiceses)
        {
            foreach (var item in answerChoiceses)
            {
                _answerChoiceRepository.Insert(item);
            }
        }

        public void SaveAssignmentAnswer(AssignmentAnswers answer)
        {
            _assignmentAnswers.Insert(answer);
        }
    }
}
