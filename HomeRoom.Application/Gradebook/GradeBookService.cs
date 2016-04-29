using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using HomeRoom.ClassEnrollment;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Gradebook.GradeBookDto;
using HomeRoom.GradeBook;
using HomeRoom.Users;
using Microsoft.AspNet.Identity;

namespace HomeRoom.Gradebook
{
    public class GradeBookService : HomeRoomAppServiceBase, IGradeBookService
    {
        #region Private Fields

        private readonly IRepository<Grade> _gradeBookRepo;
        private readonly IRepository<AssignmentType> _assignmentTypeRepo;
        private readonly IRepository<Enrollment> _classEnrollmentRepo;
        private readonly IRepository<Assignment> _assignmentRepo;
        private readonly UserManager _userManager;

        #endregion

        #region Constructors
        public GradeBookService(IRepository<Grade> gradeBookRepo, IRepository<AssignmentType> assignmentTypeRepo, IRepository<Enrollment> classEnrollmentRepo, IRepository<Assignment> assignmentRepo, UserManager userManager)
        {
            _gradeBookRepo = gradeBookRepo;
            _assignmentTypeRepo = assignmentTypeRepo;
            _classEnrollmentRepo = classEnrollmentRepo;
            _assignmentRepo = assignmentRepo;
            _userManager = userManager;
        }

        #endregion

        #region Public Methods
        public DataTableResponseDto GetAllClassGrades(int classId, DataTableRequestDto dataTableRequest)
        {
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;

            var students = _classEnrollmentRepo.GetAll().Where(x => x.ClassId == classId).Select(x => x.Student).ToList();
            // grab all the assignment types for the assignments that have been graded in this class
            var assignmentTypes = _assignmentTypeRepo.GetAll().Where(x => x.ClassId == classId).ToList();

            var gradeBookTable = new List<GradeBookTableModel>();
            // make the actual gradebook table now
            foreach (var student in students)
            {
                var overallGrade = GetStudentGradeForClass(student.Id, classId, assignmentTypes);
                gradeBookTable.Add(new GradeBookTableModel(student.Id, string.Format("{0} {1}", student.Account.Name, student.Account.Surname), overallGrade));
            }

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                gradeBookTable = gradeBookTable.Where(x => x.StudentName.ToLower().Contains(searchTerm)).ToList();
            }

            if (sortedColumns == null)
            {
                gradeBookTable = gradeBookTable.OrderBy(x => x.StudentName).ToList();
            }
            else
            {
                if (sortedColumns.Data == "studentName")
                {
                    gradeBookTable = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                        ? gradeBookTable.OrderBy(x => x.StudentName).ToList()
                        : gradeBookTable.OrderByDescending(x => x.StudentName).ToList();
                }
                else if (sortedColumns.Data == "currentGrade")
                {
                    gradeBookTable = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                        ? gradeBookTable.OrderBy(x => x.OverallGrade).ToList()
                        : gradeBookTable.OrderByDescending(x => x.OverallGrade).ToList();
                }
            }

            var tableData = gradeBookTable.ToList().Select(x => new
            {
                Id = x.StudentId,
                StudentName = x.StudentName,
                CurrentGrade = x.OverallGrade
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public double GetStudentAssignmentTypeAverage(long studentId, int assignmentTypeId)
        {
            // grab all the grades for the student for this assignmentType
            var studentGrades = _gradeBookRepo.GetAll().Where(x => x.Assignment.AssignmentTypeId == assignmentTypeId && x.StudentId == studentId);
            var numberGrades = studentGrades.Count();
            var assignmentTypeSum = numberGrades != 0 ? studentGrades.Sum(x => x.Value) : 0.0d;

            // if the number of grades is 0 then they don't have an average, just set to 100
            var grade = numberGrades != 0 ? assignmentTypeSum / numberGrades : 100.0d;

            return grade;
        }

        public double GetStudentGradeForClass(long studentId, int classId)
        {
            var assignmentTypes = _assignmentTypeRepo.GetAll().Where(x => x.ClassId == classId).ToList();

            // calculate the average for every assignment type
            var averages = (from item in assignmentTypes
                            let grades = _gradeBookRepo.GetAll().Where(x => x.Assignment.AssignmentTypeId == item.Id && x.StudentId == studentId)
                            let gradesSum = grades.Count() != 0 ? grades.Sum(x => x.Value) : 0.0d
                            let assignmentTypeAverage = grades.Count() != 0 ? gradesSum / grades.Count() : 100.0d
                            select new AssignmentTypeDto { Average = assignmentTypeAverage, Percentage = item.Percentage }).ToList();

            // for all the averages we calculated multiply them by their weighted percentage to get how many points we have received for each assignment type
            var resultPoints = averages.Select(item => item.Average * item.Percentage).ToList();

            // sum all the points together to get the total weighted average
            return resultPoints.Sum();
        }

        public double GetStudentGradeForClass(long studentId, int classId, IEnumerable<AssignmentType> assignmentTypes)
        {
            // calculate the average for every assignment type
            var averages = (from item in assignmentTypes
                            let grades = _gradeBookRepo.GetAll().Where(x => x.Assignment.AssignmentTypeId == item.Id && x.StudentId == studentId)
                            let gradesSum = grades.Count() != 0 ? grades.Sum(x => x.Value) : 0.0d
                            let assignmentTypeAverage = grades.Count() != 0 ? gradesSum/grades.Count() : 100.0d
                            select new AssignmentTypeDto {Average = assignmentTypeAverage, Percentage = item.Percentage}).ToList();

            // for all the averages we calculated multiply them by their weighted percentage to get how many points we have received for each assignment type
            var resultPoints = averages.Select(item => item.Average*item.Percentage).ToList();
            
            // sum all the points together to get the total weighted average
            return resultPoints.Sum();

        }

        public GradeBookDto.GradeBookDto GetGradeBook(int classId)
        {
            // all the assignments where their isn't any grades
            var assignments = _assignmentRepo.GetAll().Where(x => x.ClassId == classId && !x.Grades.Any()).Select(x => new AssignmentDto
            {
                AssignmentId = x.Id,
                AssignmentName = x.Name
            });
            var students = _classEnrollmentRepo.GetAll().Where(x => x.ClassId == classId).Select(x => new GradeDto
            {
                GradeValue = 0,
                StudentId = x.StudentId,
                StudentName = x.Student.Account.Name + " " + x.Student.Account.Surname
            });

            var classAssignments = assignments.ToList();
            var grades = students.ToList();
            var gradeBook = new GradeBookDto.GradeBookDto
            {
                Assignments = classAssignments,
                Grades = grades,
                ClassId = classId
            };

            return gradeBook;
        }

        public void SaveGrades(GradeBookDto.GradeBookDto grades)
        {
            var gradeBook = grades.Grades;
            var assignment = grades.AssignmentId;

            foreach (var item in gradeBook)
            {
                _gradeBookRepo.Insert(new Grade
                {
                    AssignmentId = assignment,
                    StudentId = item.StudentId,
                    Value = item.GradeValue
                });
            }
        }

        public StudentGradeBookDto GetStudentGradeBook(long studentId, int classId)
        {
            var grades = _gradeBookRepo.GetAll().Where(x => x.StudentId == studentId && x.Assignment.ClassId == classId).Select(x => new AssignmentGradeDto
            {
                AssignmentId = x.AssignmentId,
                AssignmentName = x.Assignment.Name,
                AssignmentGrade = x.Value,
                CanView = x.Assignment.AssignmentQuestionses.Any()
            });
            var assignmentTypes = _assignmentTypeRepo.GetAll().Where(x => x.ClassId == classId).ToList();
            var overalGrade = GetStudentGradeForClass(studentId, classId, assignmentTypes);
            var user = _userManager.FindById(studentId);

            var gradeBook = new StudentGradeBookDto
            {
                OverallGrade = overalGrade,
                StudentName = string.Format("{0} {1}", user.Name, user.Surname),
                Assignments = grades.ToList(),
                StudentId = user.Id
            };

            return gradeBook;
        }

        public ManageStudentGradesDto ManageStudentGrades(long studentId, int classId)
        {
            var gradeBook = _gradeBookRepo.GetAll().Where(x => x.StudentId == studentId && x.Assignment.ClassId == classId).OrderBy(x => x.Assignment.AssignmentType.Percentage)
                .Select(x => new StudentAssignmentGradeDto
                {
                    GradeId = x.Id,
                    Value = x.Value,
                    AssignmentId = x.AssignmentId,
                    AssignmentName = x.Assignment.Name
                });

            var studentGrades = new ManageStudentGradesDto
            {
                StudentAssignmentGrades = gradeBook.ToList()
            };

            return studentGrades;
        }

        public void UpdateGrades(long studentId, ManageStudentGradesDto grades)
        {
            var updatedGrades = grades.StudentAssignmentGrades;

            foreach (var updatedGrade in updatedGrades.Select(grade => new Grade
            {
                AssignmentId = grade.AssignmentId,
                Id = grade.GradeId,
                StudentId = studentId,
                Value = grade.Value
            }))
            {
                _gradeBookRepo.Update(updatedGrade);
            }
        }

        public void SaveAssignmentGrade(Grade grade)
        {
            var oldGrade = _gradeBookRepo.GetAll().FirstOrDefault(x => x.AssignmentId == grade.AssignmentId && x.StudentId == grade.StudentId);

            if (oldGrade == null)
            {
                _gradeBookRepo.Insert(grade);
            }
            else
            {
                oldGrade.Value = grade.Value;
            }
        }

        #endregion

    }
}
