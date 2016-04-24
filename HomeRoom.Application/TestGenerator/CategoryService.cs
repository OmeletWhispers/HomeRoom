using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;

namespace HomeRoom.TestGenerator
{
    class CategoryService : HomeRoomAppServiceBase, ICategoryService
    {
        #region Private Fields

        private readonly IRepository<Category> _categoryRepository;



        #endregion

        #region Constructors
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion

        #region Public Methods

        public DataTableResponseDto GetAllCategories(DataTableRequestDto dataTableRequest)
        {
            var userId = AbpSession.UserId;
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;
            var categories = userId.HasValue ? _categoryRepository.GetAll().Where(x => x.Subject.TeacherId == userId.Value) : _categoryRepository.GetAll();

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                categories = categories.Where(x => x.Name.ToLower().Contains(searchTerm) || x.Subject.Name.ToLower().Contains(searchTerm));
            }

            // column sorting
            // default sorting
            if (sortedColumns == null)
            {
                categories = categories.OrderBy(x => x.Name);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "categoryName":
                        {
                            categories = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                                ? categories.OrderBy(x => x.Name)
                                : categories.OrderByDescending(x => x.Name);
                        }
                        break;

                    case "subjectName":
                        {
                            categories = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                                ? categories.OrderBy(x => x.Subject.Name)
                                : categories.OrderByDescending(x => x.Subject.Name);
                        }
                        break;
                }
            }

            var tableData = categories.Select(x => new
            {
                Id = x.Id,
                CategoryName = x.Name,
                SubjectName = x.Subject.Name,
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public List<Category> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll().Where(x => x.Subject.TeacherId == AbpSession.UserId.Value);

            return categories.ToList();
        }

        public List<Category> GetAllCategoriesBySubject(int subjectId)
        {
            var categories = _categoryRepository.GetAll().Where(x => x.SubjectId == subjectId);

            return categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            var category = _categoryRepository.Get(id) ?? new Category();

            return category;
        }

        public void SaveCategory(Category category)
        {
            if (category.Id == 0)
            {
                _categoryRepository.Insert(category);
            }
            else
            {
                _categoryRepository.Update(category);
            }
        }
        #endregion
    }
}
