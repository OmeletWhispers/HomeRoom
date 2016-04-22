using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HomeRoom.DataTableDto;
using HomeRoom.Datatables;

namespace HomeRoom.TestGenerator
{
    class SubjectService : HomeRoomAppServiceBase, ISubjectService
    {
        #region Private Fields

        private readonly IRepository<Subject> _subjectRepository;



        #endregion

        #region Constructors
        public SubjectService(IRepository<Subject> subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        #endregion

        #region Public Methods
        public DataTableResponseDto GetAllSubjects(DataTableRequestDto dataTableRequest)
        {
            var userId = AbpSession.UserId;
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;
            var subjects = userId.HasValue ? _subjectRepository.GetAll().Where(x => x.TeacherId == userId.Value) : _subjectRepository.GetAll();

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                subjects = subjects.Where(x => x.Name.ToLower().Contains(searchTerm));
            }

            // column sorting
            // default sorting
            if (sortedColumns == null)
            {
                subjects = subjects.OrderBy(x => x.Name);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "subjectName":
                        {
                            subjects = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                                ? subjects.OrderBy(x => x.Name)
                                : subjects.OrderByDescending(x => x.Name);
                        }
                        break;
                }
            }

            var tableData = subjects.Select(x => new
            {
                Id = x.Id,
                SubjectName = x.Name,
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public List<Subject> GetAllSubjects()
        {
            var userId = AbpSession.UserId;
            var subjects = userId.HasValue ? _subjectRepository.GetAll().Where(x => x.TeacherId == userId.Value) : _subjectRepository.GetAll();

            return subjects.ToList();
        }

        public Subject GetSubjectById(int id)
        {
            var subject = _subjectRepository.Get(id) ?? new Subject();

            return subject;
        }

        public void SaveSubject(Subject subject)
        {
            if (subject.Id == 0)
            {
                _subjectRepository.Insert(subject);
            }
            else
            {
                _subjectRepository.Update(subject);
            }
        }
        #endregion
    }
}
