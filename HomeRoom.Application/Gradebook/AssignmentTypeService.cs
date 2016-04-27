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
            //Weird stuff happens and it creates duplicate assignment types this should fix it, also keeps you from updating the name of an assignment type to something that already exists within the same class
            var duplicatequery = _assignmentTypeRepository.GetAll().Where(x => x.ClassId == assignmentType.ClassId && x.Name == assignmentType.Name);
            var duplicateupdatequery = _assignmentTypeRepository.GetAll().Where(x => x.ClassId == assignmentType.ClassId && x.Id != assignmentType.Id && x.Name == assignmentType.Name);

            if (assignmentType.Id == 0 && duplicatequery.Count() == 0)
            {
                _assignmentTypeRepository.Insert(assignmentType);
            }
            else if (assignmentType.Id != 0 && duplicateupdatequery.Count() == 0)
            {
                _assignmentTypeRepository.Update(assignmentType);
            }
            //else display message maybe about duplicate name?
        }

        #endregion

    }
}
