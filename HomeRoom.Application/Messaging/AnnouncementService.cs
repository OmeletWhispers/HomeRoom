using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;

namespace HomeRoom.Messaging
{
    class AnnouncementService : HomeRoomAppServiceBase, IAnnouncementService
    {
        private readonly IRepository<Announcement> _announcementRepository;

        public AnnouncementService(IRepository<Announcement> announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public DataTableResponseDto GetLatestClassAnnouncements(int classId, DataTableRequestDto dataTableRequest)
        {
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;

            var twoWeeksAgo = DateTime.Now.AddDays(-14);

            // all announcements that are no more than two weeks old
            var announcements = _announcementRepository.GetAll().Where(x => x.ClassId == classId && x.DatePosted >= twoWeeksAgo);

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                announcements = announcements.Where(x => x.Title.ToLower().Contains(searchTerm));
            }

            if (sortedColumns == null)
            {
                // newest announcement first
                announcements = announcements.OrderByDescending(x => x.DatePosted);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "title":
                        announcements = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? announcements.OrderBy(x => x.Title)
                            : announcements.OrderByDescending(x => x.Title);
                        break;

                    case "datePosted":
                        announcements = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? announcements.OrderBy(x => x.DatePosted)
                            : announcements.OrderByDescending(x => x.DatePosted);
                        break;
                }

            }

            var tableData = announcements.ToList().Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                DatePosted = x.DatePosted.ToString("d")
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public Announcement GetAnnouncementById(int id)
        {
            var announcement = _announcementRepository.Get(id) ?? new Announcement();

            return announcement;
        }

        public void SaveAnnouncement(Announcement announcement)
        {
            announcement.DatePosted = DateTime.Now;

            if (announcement.Id == 0)
            {
                _announcementRepository.Insert(announcement);
            }
            else
            {
                _announcementRepository.Update(announcement);
            }
        }

        public void RemoveAnnouncement(int id)
        {
            _announcementRepository.Delete(id);
        }
    }
}
