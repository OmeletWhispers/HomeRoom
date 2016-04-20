using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;

namespace HomeRoom.TestGenerator
{
    public interface ICategoryService : IApplicationService
    {
        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetAllCategories(DataTableRequestDto dataTableRequest);


        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        List<Category> GetAllCategories(int? subjectId);


        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Category GetCategoryById(int id);


        /// <summary>
        /// Saves the category.
        /// </summary>
        /// <param name="category">The category.</param>
        void SaveCategory(Category category);


    }
}
