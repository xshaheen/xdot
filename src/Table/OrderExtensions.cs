using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace X.Table {
    public sealed record Order(bool Ascending, string Property);

    public sealed class Orders : List<Order> {
    }

    public static class OrderExtensions {
        public static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, Orders? orders) {
            if (orders == null || orders.Count == 0) return source.OrderBy(x => 0);

            var (asc, property) = orders[0];

            IOrderedQueryable<T> query = asc
                ? source.OrderBy(property)
                : source.OrderByDescending(property);

            for (var index = 1; index < orders.Count; ++index)
                query = orders[index].Ascending
                    ? source.ThenBy(orders[index].Property)
                    : source.ThenByDescending(orders[index].Property);

            return query;
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order.
        /// </summary>
        /// <exception cref="ArgumentException">If <paramref name="propertyName"/> not valid property name.</exception>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="propertyName">
        /// The property name to order by. You can use '.' to access a child
        /// property.
        /// </param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string propertyName,
            IComparer<object>? comparer = null
        )
            => CallOrderedQueryable(source, nameof(Queryable.OrderBy), propertyName, comparer);

        /// <summary>
        /// Sorts the elements of a sequence in descending order.
        /// </summary>
        /// <exception cref="ArgumentException">If <paramref name="propertyName"/> not valid property name.</exception>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="propertyName">
        /// The property name to order by. You can use '.' to access a child
        /// property.
        /// </param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        public static IOrderedQueryable<T> OrderByDescending<T>(
            this IQueryable<T> source,
            string propertyName,
            IComparer<object>? comparer = null
        )
            => CallOrderedQueryable(source,
                nameof(Queryable.OrderByDescending),
                propertyName,
                comparer);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order.
        /// </summary>
        /// <exception cref="ArgumentException">If <paramref name="propertyName"/> not valid property name.</exception>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="propertyName">
        /// The property name to order by. You can use '.' to access a child
        /// property.
        /// </param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        public static IOrderedQueryable<T> ThenBy<T>(
            this IQueryable<T> source,
            string propertyName,
            IComparer<object>? comparer = null
        )
            => CallOrderedQueryable(source, nameof(Queryable.ThenBy), propertyName, comparer);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order.
        /// </summary>
        /// <exception cref="ArgumentException">If <paramref name="propertyName"/> not valid property name.</exception>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Source</param>
        /// <param name="propertyName">
        /// The property name to order by. You can use '.' to access a child
        /// property.
        /// </param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        public static IOrderedQueryable<T> ThenByDescending<T>(
            this IQueryable<T> source,
            string propertyName,
            IComparer<object>? comparer = null
        )
            => CallOrderedQueryable(source,
                nameof(Queryable.ThenByDescending),
                propertyName,
                comparer);

        /// <summary>
        /// Builds the Queryable functions using a TSource property name.
        /// </summary>
        private static IOrderedQueryable<T> CallOrderedQueryable<T>(
            this IQueryable<T> source,
            string methodName,
            string propertyName,
            IComparer<object>? comparer = null
        ) {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");

            Expression body;

            try {
                body = propertyName.Split('.')
                    .Aggregate<string, Expression>(parameterExpression, Expression.PropertyOrField);
            }
            catch (Exception) {
                throw new InvalidOrderPropertyException(
                    $"'{propertyName}' is invalid sorting property.");
            }

            return comparer != null
                ? (IOrderedQueryable<T>) source.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        source.Expression,
                        Expression.Lambda(body, parameterExpression),
                        Expression.Constant(comparer))
                )
                : (IOrderedQueryable<T>) source.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        source.Expression,
                        Expression.Lambda(body, parameterExpression))
                );
        }
    }

    public class InvalidOrderPropertyException : Exception {
        public InvalidOrderPropertyException(string message) : base(message) { }
    }
}