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

namespace HomeRoom.TestGenerator
{
    class QuestionService : HomeRoomAppServiceBase, IQuestionService
    {
        private readonly IRepository<Question> _questionRepo;

        public QuestionService(IRepository<Question> questionRepo)
        {
            _questionRepo = questionRepo;
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
    }
}
