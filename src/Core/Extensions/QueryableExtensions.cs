using System.Linq.Expressions;
using Ardalis.GuardClauses;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System.Linq {
    [PublicAPI]
    public static class QueryableExtensions {
        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, bool>> predicate
        ) {
            Guard.Against.Null(query, nameof(query));

            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>(
            [Diagnostics.CodeAnalysis.NotNull] this TQueryable query,
            bool condition,
            Expression<Func<T, bool>> predicate
        )
            where TQueryable : IQueryable<T> {
            Guard.Against.Null(query, nameof(query));

            return condition ? (TQueryable) query.Where(predicate) : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>(
            [Diagnostics.CodeAnalysis.NotNull] this IQueryable<T> query,
            bool condition,
            Expression<Func<T, int, bool>> predicate
        ) {
            Guard.Against.Null(query, nameof(query));

            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static TQueryable WhereIf<T, TQueryable>(
            [Diagnostics.CodeAnalysis.NotNull] this TQueryable query,
            bool condition,
            Expression<Func<T, int, bool>> predicate
        ) where TQueryable : IQueryable<T> {
            Guard.Against.Null(query, nameof(query));

            return condition ? (TQueryable) query.Where(predicate) : query;
        }
    }
}
