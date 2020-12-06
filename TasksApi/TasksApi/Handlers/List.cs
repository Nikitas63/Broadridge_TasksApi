using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tasks.DataLayer.EfClasses;
using Tasks.DataLayer.Models;
using Tasks.DataLayer.Models.Enums;
using TasksApi.Pagination;
using TasksApi.Resources;

namespace TasksApi.Handlers
{
    public static class List
    {
        public class Query : IRequest<Page<TaskDetails>>
        {
            public Query()
            {
                this.Page = 1;
                this.Size = 20;
            }

            /// <summary>
            /// Page number (for paginated response)
            /// </summary>
            public int? Page { get; set; }

            /// <summary>
            /// Page size (for paginated response)
            /// </summary>
            public int? Size { get; set; }

            /// <summary>
            /// Available columns: "name,priority,date"
            /// </summary>
            public string[] OrderAsc { get; set; }

            /// <summary>
            /// Available columns: "name,priority,date"
            /// </summary>
            public string[] OrderDesc { get; set; }

            /// <summary>
            /// Available values: "all,active,completed"
            /// </summary>
            public string Filter { get; set; }
        }

        public class Handler : IRequestHandler<Query, Page<TaskDetails>>
        {
            private readonly TasksDbContext _context;

            public Handler(TasksDbContext context)
            {
                _context = context;
            }

            public async Task<Page<TaskDetails>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<TaskModel> query = _context.Tasks;

                query = BuildSortedQuery(request, query);

                if (request.Filter != null)
                {
                    Enum.TryParse(request.Filter, true, out FilterProperties val);

                    if (val == FilterProperties.Active)
                    {
                        query = query.Where(t => t.Status == TaskModelStatus.Active);
                    }

                    if (val == FilterProperties.Completed)
                    {
                        query = query.Where(t => t.Status == TaskModelStatus.Completed);
                    }
                }

                return await query
                    .ProjectToPageAsync<TaskDetails>(request.Page ?? 1, request.Size ?? 20);
            }

            /// <summary>
            /// Build sorted query by request.
            /// </summary>
            /// <param name="request">Request</param>
            /// <param name="query">Existing query</param>
            /// <returns>Sorted query</returns>
            private IQueryable<TaskModel> BuildSortedQuery(Query request, IQueryable<TaskModel> query)
            {
                var orderAscFieldExpressions = GetOrderExpressions(request, true);
                var orderDescFieldExpressions = GetOrderExpressions(request, false);

                var orderFieldExpressions = orderAscFieldExpressions.Concat(orderDescFieldExpressions);

                if (orderFieldExpressions.Any())
                {
                    var firstExpr = orderFieldExpressions.First();
                    // Ascending sorting
                    if (firstExpr.Item1)
                    {
                        query = query.OrderBy(firstExpr.Item2);
                    }
                    // Descending sorting
                    else
                    {
                        query = query.OrderByDescending(firstExpr.Item2);
                    }


                    foreach (var expr in orderFieldExpressions.Skip(1))
                    {
                        // Ascending sorting
                        if (expr.Item1)
                        {
                            query = ((IOrderedQueryable<TaskModel>)query).ThenBy(expr.Item2);
                        }
                        // Descending sorting
                        else
                        {
                            query = ((IOrderedQueryable<TaskModel>)query).ThenByDescending(expr.Item2);
                        }
                    }
                }

                return query;
            }

            /// <summary>
            /// Gets expression for ordering.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="asc">Value determines whether ordering is ascending (or descending)</param>
            /// <returns>Expression for ordering</returns>
            private List<Tuple<bool, Expression<Func<TaskModel, object>>>> GetOrderExpressions(Query request, bool asc = true)
            {
                var orderFieldExpressions = new List<Tuple<bool, Expression<Func<TaskModel, object>>>>();

                var fields = asc ? request.OrderAsc : request.OrderDesc;

                if (fields != null && fields.Any())
                {
                    foreach (var field in fields)
                    {
                        Enum.TryParse(field, true, out SortingFields val);
                        orderFieldExpressions.Add(new Tuple<bool, Expression<Func<TaskModel, object>>>(asc, GetOrderByExpression(val)));
                    }
                }

                return orderFieldExpressions;
            }

            /// <summary>
            /// Gets expression for ordering.
            /// </summary>
            /// <param name="field">Field</param>
            /// <returns>Expression.</returns>
            private Expression<Func<TaskModel, object>> GetOrderByExpression(SortingFields field)
            {
                switch (field)
                {
                    case SortingFields.Date:
                        {
                            return r => r.CreatedDate;
                        }
                    case SortingFields.Name:
                        {
                            return r => r.Name;
                        }
                    case SortingFields.Priority:
                        {
                            return r => r.Priority;
                        }
                    default:
                        {
                            throw new ArgumentException("Unknown field type");
                        }
                }
            }
        }
    }
}
