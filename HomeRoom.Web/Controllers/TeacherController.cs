using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.ClassEnrollment;
using HomeRoom.Web.Models.Teacher;

namespace HomeRoom.Web.Controllers
{
    public class TeacherController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly IClassService _classService;

        #endregion

        #region Constructors
        public TeacherController(IClassService classService)
        {
            _classService = classService;
        }
        #endregion

        #region Public Methods

        public ActionResult ManageClass(int classId)
        {
            var course = _classService.GetClassById(classId);
            var courseModel = new ManageClassViewModel
            {
                ClassId = classId,
                ClassName = course.Name
            };

            return View("ManageClass", courseModel);
        }

        #endregion
    }
}