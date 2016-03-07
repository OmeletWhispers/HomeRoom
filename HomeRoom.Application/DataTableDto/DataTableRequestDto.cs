using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeRoom.Datatables;

namespace HomeRoom.DataTableDto
{
    public class DataTableRequestDto
    {
        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the page start.
        /// </summary>
        /// <value>
        /// The page start.
        /// </value>
        public int PageStart { get; set; }

        /// <summary>
        /// Gets or sets the sorted columns.
        /// </summary>
        /// <value>
        /// The sorted columns.
        /// </value>
        public ColumnViewModel SortedColumns { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        public SearchViewModel Search { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableRequestDto"/> class.
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageStart">The page start.</param>
        /// <param name="sortedColumns">The sorted columns.</param>
        /// <param name="search">The search.</param>
        public DataTableRequestDto(int draw, int pageIndex, int pageStart, ColumnViewModel sortedColumns = null, SearchViewModel search = null)
        {
            Draw = draw;
            PageIndex = pageIndex;
            PageStart = pageStart;
            SortedColumns = sortedColumns;
            Search = search;
        }
    }
}
