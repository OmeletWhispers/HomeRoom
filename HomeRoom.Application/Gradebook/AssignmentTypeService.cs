using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.GradeBook;

namespace HomeRoom.Gradebook
{
    public class AssignmentTypeService: HomeRoomAppServiceBase, IAssignmentTypeService
    {
        #region Private Fields

        private readonly IRepository<AssignmentType> _assignmentTypeRepository;

        #endregion


        #region Constructors

        public AssignmentTypeService(IRepository<AssignmentType> assignmentTypeRepository)
        {
            _assignmentTypeRepository = assignmentTypeRepository;
        }

        #endregion


        #region Public Methods

        public DataTableResponseDto GetAllClassAssignmentTypes(int classId, DataTableRequestDto dataTableRequest)
        {
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;

            var assignment = _assignmentTypeRepository.GetAll().Where(x => x.ClassId == classId);

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                assignment = assignment.Where(x => x.Name.ToLower().Contains(searchTerm));
            }

            if (sortedColumns == null)
            {
                assignment = assignment.OrderBy(x => x.Name);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "name":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.Name)
                            : assignment.OrderByDescending(x => x.Name);
                        break;

                    case "percentage":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.Percentage)
                            : assignment.OrderByDescending(x => x.Percentage);
                        break;
                }

            }

            var tableData = assignment.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Percentage = x.Percentage * 100.0
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public List<AssignmentType> GetAllAssignmentTypes(int classId)
        {
            var assignmentTypes = _assignmentTypeRepository.GetAllList(x => x.ClassId == classId);

            return assignmentTypes;
        }

        public AssignmentType GetAssignmentTypeById(int id)
        {
            var assignmentType = _assignmentTypeRepository.Get(id) ?? new AssignmentType();

            return assignmentType;
        }

        public void SaveAssignmentType(AssignmentType assignmentType)
        {
            if (assignmentType.Id == 0)
            {
                _assignmentTypeRepository.Insert(assignmentType);
            }
            else
            {
                _assignmentTypeRepository.Update(assignmentType);
            }
        }

        #endregion

    }
}
