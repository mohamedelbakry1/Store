using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Shared.Common
{
    public sealed class PaginatedResult<TEntity>
    {
        public PaginatedResult(int pageSize, int pageIndex, int count, IReadOnlyList<TEntity> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<TEntity> Data { get; set; }
    }
}
