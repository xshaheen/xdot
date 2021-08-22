using System.Linq.Expressions;
using Ardalis.GuardClauses;
using JetBrains.Annotations;

#pragma warning disable IDE0130
namespace System.Linq {
#pragma warning restore IDE0130
	[PublicAPI]
	public static class QueryableExtensions {
		/// <summary>Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.</summary>
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

		/// <summary>Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.</summary>
		/// <param name="query">Queryable to apply filtering</param>
		/// <param name="condition">A boolean value</param>
		/// <param name="predicate">Predicate to filter the query</param>
		/// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
		public static TQueryable WhereIf<T, TQueryable>(
			this TQueryable query,
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
			this IQueryable<T> query,
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
			this TQueryable query,
			bool condition,
			Expression<Func<T, int, bool>> predicate
		) where TQueryable : IQueryable<T> {
			Guard.Against.Null(query, nameof(query));

			return condition ? (TQueryable) query.Where(predicate) : query;
		}
	}
}
