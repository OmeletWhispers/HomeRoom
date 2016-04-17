using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeRoom.Web.Models
{
    public class DataTableColumnModel
    {
        public string Data { get; set; }

        public bool Orderable { get; set; }

        public DataTableColumnModel(string columnName, bool orderable = true)
        {
            Data = columnName;
            Orderable = orderable;
        }
    }
}