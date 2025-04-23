using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Common.Parameters
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; init; }
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public bool HasNext => PageNumber * PageSize < TotalCount;
        public bool HasPrevious => PageNumber > 1;
    }
}
