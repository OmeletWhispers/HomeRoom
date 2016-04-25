using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Messaging;
using HomeRoom.Web.Models.Messages;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    public class AnnouncementController : HomeRoomControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public ActionResult GetClassAnnouncementsDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request, int classId)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;
            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);
            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var announcements = _announcementService.GetLatestClassAnnouncements(classId, dataTableRequest);

            return Json(announcements.ToDataTableResponse(), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public PartialViewResult Announcement(int? id, int classId)
        {
            if (id.HasValue)
            {
                var announcement = _announcementService.GetAnnouncementById(id.Value);

                var model = new AnnouncementViewModel
                {
                    ClassId = announcement.ClassId,
                    Id = announcement.Id,
                    Text = announcement.Text,
                    Title = announcement.Title
                };

                return PartialView("Forms/_AnnouncementForm", model);
            }

            else
            {
                var model = new AnnouncementViewModel
                {
                    ClassId = classId
                };
                return PartialView("Forms/_AnnouncementForm", model);
            }
        }

        [HttpPost]
        public JsonResult Announcement(AnnouncementViewModel model)
        {
            var announcement = new Announcement
            {
                Id = model.Id,
                ClassId = model.ClassId,
                Text = model.Text,
                Title = model.Title
            };

            _announcementService.SaveAnnouncement(announcement);

            return Json(new {msg = "Save Successful!", error = false});
        }

        [HttpPost]
        public JsonResult DeleteAnnouncement(int id)
        {
            _announcementService.RemoveAnnouncement(id);

            return Json(new { msg = "Announcement has been removed" });
        }
    }
}