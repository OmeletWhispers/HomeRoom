using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Castle.MicroKernel;
using HomeRoom.Enumerations;
using HomeRoom.TestGenerator.Dto;

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

        public List<AssignmentAnswersDto> GetAllAssignmentAnswers(long studentId, int assignmentId)
        {
            var assignmentAnswers = _assignmentAnswers.GetAll().Where(x => x.AssignmentId == assignmentId && x.StudentId == studentId).Select(x => new AssignmentAnswersDto
            {
                QuestionId = x.QuestionId,
                Question = x.Question.Value,
                PointsValue = x.Question.AssignmentQuestionses.Select(y => y.PointValue).FirstOrDefault(),
                AnswerChoiceId = x.AnswerChoiceId,
                AnswerText = x.Text,
                Type = x.Question.QuestionType,
                StudentCorrect = x.Question.QuestionType == QuestionType.MultipleChoice && x.AnswerChoices.IsCorrect,
                AnswerChoices = x.Question.AnswerChoiceses.Select(y => new AnswerChoicesDto
                {
                    Id = y.Id,
                    ChoiceValue = y.Value,
                    IsCorrect = y.IsCorrect
                }).ToList()
            });

            return assignmentAnswers.ToList();
        }
    }
}
