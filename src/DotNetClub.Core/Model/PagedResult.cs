using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public PagedResult(List<T> data, int pageIndex, int pageSize, int total)
        {
            this.Data = data;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Total = total;
        }
    }
}
