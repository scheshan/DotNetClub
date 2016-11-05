using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure.Model
{
    public class PagedResult<T> : Result
    {
        public List<T> Data { get; set; }

        public long Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public static PagedResult<T> SuccessResult(List<T> data, int pageIndex, int pageSize, long total)
        {
            return new PagedResult<T>
            {
                Data = data,
                Success = true,
                Total = total,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public static new PagedResult<T> ErrorResult(string errorMessage)
        {
            return new PagedResult<T>
            {
                ErrorMessage = errorMessage
            };
        }
    }
}
