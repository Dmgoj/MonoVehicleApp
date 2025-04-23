using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Common;

namespace Project.Service.Common.Parameters
{
    public class QueryParameters : PagingParameters
    {
        public string Filter { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
