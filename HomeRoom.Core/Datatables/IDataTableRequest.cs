using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.Datatables
{
    public interface IDataTableRequest
    {
        /// <summary>
        /// Gets and sets the draw counter from client-side to give back on the server's response.
        /// </summary>
        int Draw { get; set; }
        /// <summary>
        /// Gets and sets the start record number (count) for paging.
        /// </summary>
        int Start { get; set; }
        /// <summary>
        /// Gets and sets the length of the page (max records per page).
        /// </summary>
        int Length { get; set; }
        /// <summary>
        /// Gets and sets the global search pagameters.
        /// </summary>
        SearchViewModel Search { get; set; }
        /// <summary>
        /// Gets and sets the read-only collection of client-side columns with their options and configs.
        /// </summary>
        ColumnCollection Columns { get; set; }
    }
}
