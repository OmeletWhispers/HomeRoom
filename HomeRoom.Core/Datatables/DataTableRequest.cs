using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.Datatables
{
    public class DataTableRequest : IDataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public SearchViewModel Search { get; set; }
        public ColumnData ColumnData { get; set; }
    }
}
