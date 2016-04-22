using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace HomeRoom.TestGenerator
{
    class AnswerService : HomeRoomAppServiceBase, IAnswerService
    {
        private readonly IRepository<AnswerChoices> _answerChoiceRepository;

        public AnswerService(IRepository<AnswerChoices> answerChoiceRepository)
        {
            _answerChoiceRepository = answerChoiceRepository;
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
    }
}
