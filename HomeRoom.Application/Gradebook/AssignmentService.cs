using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.GradeBook;

namespace HomeRoom.Gradebook
{
    public class AssignmentService : HomeRoomAppServiceBase, IAssignmentService
    {
        #region Private Fields

        private readonly IRepository<Assignment> _assignmentRepository;

        #endregion


        #region Constructors

        public AssignmentService(IRepository<Assignment> assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        #endregion


        #region Public Methods

        public DataTableResponseDto GetAllClassAssignments(int classId, DataTableRequestDto dataTableRequest)
        {
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;

            var assignment = _assignmentRepository.GetAll().Where(x => x.ClassId == classId);

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                assignment = assignment.Where(x => x.Name.ToLower().Contains(searchTerm) || x.AssignmentType.Name.ToLower().Contains(searchTerm));
            }

            if (sortedColumns == null)
            {
                assignment = assignment.OrderBy(x => x.Name);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "assignmentName":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.Name)
                            : assignment.OrderByDescending(x => x.Name);
                        break;

                    case "assignmentType":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.AssignmentType.Name)
                            : assignment.OrderByDescending(x => x.AssignmentType.Name);
                        break;

                    case "startDate":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.StartDate)
                            : assignment.OrderByDescending(x => x.StartDate);
                        break;

                    case "dueDate":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.DueDate)
                            : assignment.OrderByDescending(x => x.DueDate);
                        break;

                    case "status":
                        assignment = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? assignment.OrderBy(x => x.Status)
                            : assignment.OrderByDescending(x => x.Status);
                        break;
                }

            }

            var tableData = assignment.ToList().Select(x => new
            {
                Id = x.Id,
                AssignmentName = x.Name,
                AssignmentType = x.AssignmentType.Name,
                StartDate = x.StartDate.ToString("d"),
                DueDate = x.DueDate.ToString("d"),
                Status = x.Status.ToString(),
                CanView = x.AssignmentQuestionses.Any(),
                Url = string.Format("/TestGenerator/ViewAssignment?assignmentId={0}", x.Id)
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;

        }

        public List<Assignment> GetAllClassAssignments(int classId)
        {
            var assignments = _assignmentRepository.GetAll().Where(x => x.ClassId == classId);

            return assignments.ToList();
        }

        public List<Assignment> GetCreatedAssignments(int classId)
        {
            var assignments = _assignmentRepository.GetAll().Where(x => x.ClassId == classId && x.Status == AssignmentStatus.Created);

            return assignments.ToList();
        }

        public Assignment GetById(int id)
        {
            var assignment = _assignmentRepository.Get(id) ?? new Assignment();

            return assignment;
        }

        public void SaveAssignment(Assignment assignment)
        {
            if (assignment.Id == 0)
            {
                _assignmentRepository.Insert(assignment);
            }
            else
            {
                _assignmentRepository.Update(assignment);
            }
        }

        #endregion

    }
}