using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace X.Table
{
    public static class TableExtensions
    {
        public static Page<T> Table<T>(this IQueryable<T> queryable, TableParams parameters) where T : class
            => parameters.Paging == null
                ? queryable.HandleNoPaging(parameters.Filters, parameters.Orders)
                : queryable.HandleFilters(parameters.Filters).HandleOrders(parameters.Orders)
                    .Page(parameters.Paging.Index, parameters.Paging.Size);

        public static async Task<Page<T>> TableAsync<T>(
            this IQueryable<T> queryable,
            TableParams parameters,
            CancellationToken ct = default)
            where T : class
        {
            if (parameters.Paging == null) return queryable.HandleNoPaging(parameters.Filters, parameters.Orders);

            Page<T> page = await queryable.HandleFilters(parameters.Filters).HandleOrders(parameters.Orders)
                .PageAsync(parameters.Paging.Index, parameters.Paging.Size, ct);

            return page;
        }

        private static Page<T> HandleNoPaging<T>(this IQueryable<T> queryable, Filters? filters, Orders? orders)
            where T : class
        {
            List<T> list = queryable.HandleFilters(filters).HandleOrders(orders).ToList();
            return new Page<T>(list, 0, list.Count, list.Count);
        }

        private static IQueryable<T> HandleFilters<T>(this IQueryable<T> queryable, Filters? filters) where T : class
            => filters != null ? queryable.Filter(filters) : queryable;

        private static IQueryable<T> HandleOrders<T>(this IQueryable<T> queryable, Orders? orders) where T : class
            => orders is { } ? queryable.Order(orders) : queryable;
    }
}
