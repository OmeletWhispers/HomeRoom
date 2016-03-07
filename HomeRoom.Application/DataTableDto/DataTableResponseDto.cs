using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeRoom.Datatables;

namespace HomeRoom.DataTableDto
{
    public class DataTableResponseDto
    {
        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the records filtered.
        /// </summary>
        /// <value>
        /// The records filtered.
        /// </value>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableResponseDto"/> class.
        /// </summary>
        /// <param name="draw">The draw.</param>
        /// <param name="recordsFiltered">The records filtered.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <param name="data">The data.</param>
        public DataTableResponseDto(int draw, int recordsFiltered, int totalRecords, IEnumerable data)
        {
            Draw = draw;
            RecordsFiltered = recordsFiltered;
            TotalRecords = totalRecords;
            Data = data;
        }

        /// <summary>
        /// Converts the DataTable DTO Object to the response that the server sends back to the datatable
        /// </summary>
        /// <returns></returns>
        public DataTableResponse ToDataTableResponse()
        {
            return new DataTableResponse(Draw, Data, RecordsFiltered, TotalRecords);
        }
    }
}
