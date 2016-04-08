using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
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

        public UserAppService(UserManager userManager, IPermissionManager permissionManager, IRepository<Student, long> studentRepository, IRepository<Parent, long> parentRepository)
        {
            _userManager = userManager;
            _permissionManager = permissionManager;
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
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
            return _studentRepository.GetAll().Any(x => x.Account.EmailAddress == email);
        }

        public void InsertStudent(long userId)
        {
            _studentRepository.Insert(new Student {Id = userId});
        }

        public void SaveParent(long studentId, UserDto parent)
        {
            if (parent.UserId == 0)
            {
                // create a user for the parent
                var user = new User
                {
                    AccountType = AccountType.Parent,
                    EmailAddress = parent.Email.ToLower(),
                    UserName = parent.Email.ToLower(),
                    Name = parent.FirstName,
                    Surname = parent.LastName,
                    IsActive = true,
                    Password = new PasswordHasher().HashPassword(Helpers.Helpers.GenerateRandomPassword(8))
                };
                // create the actual account for the parent
                _userManager.Create(user);
                // make a parent
                _parentRepository.Insert(new Parent {Id = user.Id});

                // assign this parent to the student
                var student = _userManager.FindById(studentId);
                student.Student.ParentId = user.Id;
                _userManager.Update(student);
            }
            else
            {
                // editing parent information
                var user = _userManager.FindById(parent.UserId);
                user.Name = parent.FirstName;
                user.UserName = parent.Email.ToLower();
                user.Surname = parent.LastName;
                user.EmailAddress = parent.Email.ToLower();

                _userManager.Update(user);
            }
        }

        public void UpdateUser(UserDto user)
        {
            var account = _userManager.FindById(user.UserId);

            account.EmailAddress = user.Email.ToLower();
            account.UserName = user.Email.ToLower();
            account.Name = user.FirstName;
            account.Surname = user.LastName;

            _userManager.Update(account);
        }
    }
}