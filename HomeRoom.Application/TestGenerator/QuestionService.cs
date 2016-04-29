using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.Helpers;
using HomeRoom.TestGenerator.Dto;

namespace HomeRoom.TestGenerator
{
    class QuestionService : HomeRoomAppServiceBase, IQuestionService
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<AssignmentQuestions> _assignmentQuestionRepo; 

        public QuestionService(IRepository<Question> questionRepo, IRepository<AssignmentQuestions> assignmentQuestionRepo)
        {
            _questionRepo = questionRepo;
            _assignmentQuestionRepo = assignmentQuestionRepo;
        }

        public DataTableResponseDto GetAllQuestions(DataTableRequestDto dataTableRequest)
        {
            var userId = AbpSession.UserId;
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;
            var questions = userId.HasValue ? _questionRepo.GetAll().Where(x => x.Category.Subject.TeacherId == userId.Value || x.IsPublic) : _questionRepo.GetAll();

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                questions = questions.Where(x => x.Value.ToLower().Contains(searchTerm) || x.Category.Name.ToLower().Contains(searchTerm));
            }

            // column sorting
            // default sorting
            if (sortedColumns == null)
            {
                questions = questions.OrderBy(x => x.Value);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "question":
                        {
                            questions = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                                ? questions.OrderBy(x => x.Value)
                                : questions.OrderByDescending(x => x.Value);
                        }
                        break;

                    case "category":
                        {
                            questions = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                                ? questions.OrderBy(x => x.Category.Name)
                                : questions.OrderByDescending(x => x.Category.Name);
                        }
                        break;

                    case "questionType":
                    {
                            questions = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                                    ? questions.OrderBy(x => x.QuestionType)
                                    : questions.OrderByDescending(x => x.QuestionType);
                    }
                    break;

                }
            }

            var tableData = questions.ToList().Select(x => new
            {
                Id = x.Id,
                Question = x.Value,
                Category = x.Category.Name,
                QuestionType = EnumHelper<QuestionType>.GetDisplayValue(x.QuestionType),
                IsPublic = x.IsPublic
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public List<Question> GetAllQuestions()
        {
            var userId = AbpSession.UserId;
            var questions = userId.HasValue ? _questionRepo.GetAll().Where(x => x.Category.Subject.TeacherId == userId.Value || x.IsPublic) : _questionRepo.GetAll();

            return questions.ToList();
        }

        public List<Question> GetAllQuestionsInSubject(int subjectId)
        {
            var userId = AbpSession.UserId;
            var questions = userId.HasValue ? _questionRepo.GetAll().Where(x => x.Category.SubjectId == subjectId && (x.Category.Subject.TeacherId == userId.Value || x.IsPublic)) : _questionRepo.GetAll();

            return questions.ToList();
        }

        public List<Question> GetAllQuestionByCategory(int categoryId)
        {
            var userId = AbpSession.UserId;

            var questions = _questionRepo.GetAll().Where(x => x.Category.Subject.TeacherId == userId.Value || x.IsPublic);
            questions = questions.Where(x => x.CategoryId == categoryId);

            return questions.ToList();
        }

        public void SaveQuestion(Question question)
        {
            if (question.Id == 0)
            {
                _questionRepo.Insert(question);
            }
            else
            {
                _questionRepo.Update(question);
            }
        }

        public List<Question> GetAllQuestionsNotAsked(int assignmentId)
        {
            var userId = AbpSession.UserId;
            var questions = _questionRepo.GetAll().Where(x => x.AssignmentQuestionses.Count(y => y.AssignmentId == assignmentId) == 0);

            // get the questions down to the ones for this teacher or the ones that are public
            questions = questions.Where(x => x.Category.Subject.TeacherId == userId.Value || x.IsPublic);

            return questions.ToList();
        }

        public List<AssignmentQuestionDto> GetAllAssignmentQuestions(int assignmentId)
        {
            var assignmentQuestions = _assignmentQuestionRepo.GetAll().Where(x => x.AssignmentId == assignmentId).Select(x => new AssignmentQuestionDto
            {
                Id = x.Id,
                QuestionId = x.QuestionId,
                Question = x.Question.Value,
                PointValue = x.PointValue
            });

            return assignmentQuestions.ToList();
        }

        public List<AssignmentQuestionDto> GetQuestionsForAssignment(int assignmentId)
        {
            var assignmentQuestions = _assignmentQuestionRepo.GetAll().Where(x => x.AssignmentId == assignmentId).OrderBy(x => x.Id).Select(x => new AssignmentQuestionDto
            {
                Id = x.Id,
                QuestionId = x.QuestionId,
                Question = x.Question.Value,
                PointValue = x.PointValue,
                Type = x.Question.QuestionType,
                AnswerChoices = x.Question.AnswerChoiceses.Select(y => new AnswerChoicesDto
                {
                    ChoiceValue = y.Value,
                    Id = y.Id
                }).ToList()
            });

            return assignmentQuestions.ToList();
        }

        public void SaveAssignmentQuestion(AssignmentQuestions question)
        {
            _assignmentQuestionRepo.Insert(question);
        }

        public void DeleteAssignmentQuestions(int assignmentId)
        {
            var assignmentQuestions = _assignmentQuestionRepo.GetAll().Where(x => x.AssignmentId == assignmentId).ToList();

            foreach (var item in assignmentQuestions)
            {
                _assignmentQuestionRepo.Delete(item);
            }
        }
    }
}
