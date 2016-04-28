using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.Membership;
using HomeRoom.Users.Dto;
using Microsoft.AspNet.Identity;

namespace HomeRoom.Users
{
    /* THIS IS JUST A SAMPLE. */
    public class UserAppService : HomeRoomAppServiceBase, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<Student, long> _studentRepository;
        private readonly IRepository<Parent, long> _parentRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager; 

        public UserAppService(UserManager userManager, IPermissionManager permissionManager, IRepository<Student, long> studentRepository, IRepository<Parent, long> parentRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _userManager = userManager;
            _permissionManager = permissionManager;
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await _userManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await _userManager.RemoveFromRoleAsync(userId, roleName));
        }

        public DataTableResponseDto GetAllUsers(DataTableRequestDto dataTableRequest)
        {
            var search = dataTableRequest.Search;
            var sortedColumn = dataTableRequest.SortedColumns;
            var users = _userManager.Users;

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                // filter by the search term: first name, last name, email address
                users = users.Where(x => x.Name.ToLower().Contains(searchTerm) || x.Surname.ToLower().Contains(searchTerm) || x.EmailAddress.ToLower().Contains(searchTerm));
            }

            // column sorting
            // default sorting
            if (sortedColumn == null)
            {
                users = users.OrderBy(x => x.Name);
            }
            else
            {
                switch (sortedColumn.Data)
                {
                    case "firstName":
                    {
                        users = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant ? users.OrderBy(x => x.Name) : users.OrderByDescending(x => x.Name);
                    }
                        break;
                    case "lastName":
                    {
                        users = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant ? users.OrderBy(x => x.Surname) : users.OrderByDescending(x => x.Surname);
                    }
                        break;
                    case "email":
                    {
                        users = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant ? users.OrderBy(x => x.EmailAddress) : users.OrderByDescending(x => x.EmailAddress);
                    }
                        break;
                }
            }

            // filtering
            


            var tableData = users.Select(x => new
            {
                x.Id,
                FirstName = x.Name,
                LastName = x.Surname,
                Email = x.EmailAddress
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;

        }

        public DataTableResponseDto GetParentStudents(DataTableRequestDto dataTableRequest)
        {
            var userId = AbpSession.GetUserId();

            var parent = _parentRepository.Get(userId);
            var students = parent.Students.Select(x => x.Account);
            var search = dataTableRequest.Search;
            var sortedColumn = dataTableRequest.SortedColumns;

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                // filter by the search term: first name, last name, email address
                students = students.Where(x => x.Name.ToLower().Contains(searchTerm) || x.Surname.ToLower().Contains(searchTerm));
            }

            // column sorting
            // default sorting
            if (sortedColumn == null)
            {
                students = students.OrderBy(x => x.Surname);
            }
            else
            {
                switch (sortedColumn.Data)
                {
                    case "studentName":
                        {
                            students = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant 
                                ? students.OrderBy(x => x.Surname) 
                                : students.OrderByDescending(x => x.Surname);
                        }
                        break;
                }
            }

            // filtering



            var tableData = students.Select(x => new
            {
                x.Id,
                StudentName = x.Name + " " + x.Surname,
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;

        }

        public bool IsUserRegistered(string userName)
        {
            var isRegistered = _userManager.Users.Any(x => x.UserName == userName);

            return isRegistered;
        }

        public StudentDto GetStudentById(long studentId)
        {
            var student = _studentRepository.Get(studentId);
            var account = student.Account;
            var parentAccount = student.ParentId.HasValue ? student.Parent.Account : new User();


            var studentDto = new StudentDto(account.Id, student.ParentId, account.Name, account.Surname, account.EmailAddress, parentAccount.Name, parentAccount.Surname, parentAccount.EmailAddress);

            return studentDto;
        }

        public bool HasStudentAccount(string email)
        {
            var account = _userManager.FindByEmail(email);

            return account != null && account.AccountType == AccountType.Student;
        }

        public long CreateAccountAndGetId(UserDto user)
        {
            var account = _userManager.FindByEmail(user.Email.ToLower());
            var hasParentAccount = account != null && account.AccountType == AccountType.Parent;

            // already has a parent account, just return it's id
            if(hasParentAccount)
                return account.Id;

            // create a user for the parent
            var parent = new User
            {
                AccountType = AccountType.Parent,
                EmailAddress = user.Email.ToLower(),
                UserName = user.Email.ToLower(),
                Name = user.FirstName,
                Surname = user.LastName,
                IsActive = true,
                Password = new PasswordHasher().HashPassword(User.DefaultPassword),
                TenantId = AbpSession.GetTenantId()
            };

            CheckErrors(_userManager.Create(parent));
            _unitOfWorkManager.Current.SaveChanges();

            _parentRepository.Insert(new Parent {Id = parent.Id});
            _unitOfWorkManager.Current.SaveChanges();

            return parent.Id;
        }

        public long CreateStudentAccountAndGetId(UserDto user)
        {
            // create a user for the parent
            var student = new User
            {
                AccountType = AccountType.Student,
                EmailAddress = user.Email.ToLower(),
                UserName = user.Email.ToLower(),
                Name = user.FirstName,
                Surname = user.LastName,
                IsActive = true,
                Password = new PasswordHasher().HashPassword(User.DefaultPassword),
                TenantId = AbpSession.GetTenantId()
            };

            CheckErrors(_userManager.Create(student));

            _studentRepository.Insert(new Student {Id = student.Id});
            _unitOfWorkManager.Current.SaveChanges();

            return student.Id;
        }

        public void InsertParent(long parentUserId, long studentId)
        {
            var student = _studentRepository.Get(studentId);
            student.ParentId = parentUserId;
        }

        public void InsertStudent(long userId)
        {
            _studentRepository.Insert(new Student {Id = userId});
        }

        public void SaveParent(long studentId, UserDto parent)
        {
            var parentAccount = _userManager.FindByEmail(parent.Email);

            parentAccount.EmailAddress = parent.Email.ToLower();
            parentAccount.Name = parent.FirstName;
            parentAccount.Surname = parent.LastName;
        }

        public void UpdateUser(UserDto user)
        {
            var account = _userManager.FindById(user.UserId);

            account.EmailAddress = user.Email.ToLower();
            account.Name = user.FirstName;
            account.Surname = user.LastName;

        }
    }
}