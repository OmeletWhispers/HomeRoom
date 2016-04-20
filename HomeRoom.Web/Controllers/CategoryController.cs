using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.TestGenerator;
using HomeRoom.Web.Models.TestGenerator;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    public class CategoryController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly ICategoryService _categoryService;
        private readonly ISubjectService _subjectService;


        #endregion

        #region Constructors
        public CategoryController(ICategoryService categoryService, ISubjectService subjectService)
        {
            _categoryService = categoryService;
            _subjectService = subjectService;
        }

        #endregion

        #region Public Methods
        public ActionResult GetDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var categories = _categoryService.GetAllCategories(dataTableRequest);

            return Json(categories.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Category(int? id)
        {
            var subjects = _subjectService.GetAllSubjects();

            if (id.HasValue)
            {
                var category = _categoryService.GetCategoryById(id.Value);
                var model = new CategoryViewModel(category, subjects);

                return PartialView("Forms/_CategoryForm", model);
            }
            else
            {
                var model = new CategoryViewModel(subjects);
                return PartialView("Forms/_CategoryForm", model);
            }
        }

        [HttpPost]
        public JsonResult Category(CategoryViewModel model)
        {
            if (!AbpSession.UserId.HasValue)
                return Json(new {error = true, msg = "You must be logged in to create a category."});

            var category = new Category
            {
                Id = model.Id,
                Name = model.CategoryName,
                SubjectId = model.SubjectId
            };

            _categoryService.SaveCategory(category);

            return Json(new {error = false, msg = "Save Successful!"});
        }


        #endregion
    }
}