using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace TasksApi.Pagination
{
    /// <summary>
    /// Helper methods for pagination using.
    /// </summary>
    public static class PaginationExtensions
    {
        public static Task<IPagedList<TDestination>> ProjectToPagedListAsync<TDestination>(this IQueryable queryable, int pageNumber, int pageSize)
        {
            return queryable.ProjectTo<TDestination>().ToPagedListAsync(pageNumber, pageSize);
        }

        public static Task<Page<TDestination>> ProjectToPageAsync<TDestination>(this IQueryable queryable, int pageNumber, int pageSize)
        {
            return queryable.ProjectTo<TDestination>().ToPageAsync(pageNumber, pageSize);
        }

        public static async Task<Page<TData>> ToPageAsync<TData>(this IQueryable<TData> queryable, int pageNumber, int pageSize)
        {
            var pagedList = await queryable.ToPagedListAsync(pageNumber, pageSize);
            return new Page<TData>(pagedList);
        }

        public static async Task<Page<TResult>> ToPageAsync<TData, TResult>(this IQueryable<TData> queryable, IMapper mapper, int pageNumber, int pageSize)
        {
            var pagedList = await queryable.ToPageAsync<TData>(pageNumber, pageSize);

            return pagedList.Select(mapper.Map<TData, TResult>);
        }
    }
}
