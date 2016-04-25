using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.DataTableDto;

namespace HomeRoom.Messaging
{
    public interface IAnnouncementService : IApplicationService
    {
        /// <summary>
        /// Gets the latest class announcements.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="dataTableRequest">The data table request.</param>
        /// <returns></returns>
        DataTableResponseDto GetLatestClassAnnouncements(int classId, DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Gets the announcement by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Announcement GetAnnouncementById(int id);

        /// <summary>
        /// Saves the announcement.
        /// </summary>
        /// <param name="announcement">The announcement.</param>
        void SaveAnnouncement(Announcement announcement);

        /// <summary>
        /// Removes the announcement.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void RemoveAnnouncement(int id);
    }
}
