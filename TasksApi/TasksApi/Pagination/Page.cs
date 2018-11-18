using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using X.PagedList;

namespace TasksApi.Pagination
{
    /// <summary>
    /// Page of paginated result.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    public class Page<TData>
    {
        public Page(IPagedList<TData> pagedList)
        {
            Data = pagedList;
            PaginationContext = new PaginationContext(
                page: pagedList.PageNumber,
                size: pagedList.PageSize,
                pages: pagedList.PageCount,
                totalRows: pagedList.TotalItemCount);
        }

        public Page(IEnumerable<TData> data, PaginationContext context)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            PaginationContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        [JsonProperty("data")]
        public IEnumerable<TData> Data { get; }

        [JsonProperty("paginationContext")]
        public PaginationContext PaginationContext { get; }

        public Page<TResult> Select<TResult>(Func<TData, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new Page<TResult>(Data.Select(selector), PaginationContext);
        }
    }
}
