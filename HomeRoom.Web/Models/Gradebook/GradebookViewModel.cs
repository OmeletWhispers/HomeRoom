using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeRoom.Web.Models.Gradebook
{
    public class GradebookViewModel
    {
        public IEnumerable<string> GradebookColumns { get; set; }

        public GradebookViewModel(IEnumerable<string> columns)
        {
            GradebookColumns = columns;
        }
    }
}