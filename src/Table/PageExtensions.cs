using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace X.Table {
	[PublicAPI]
	public static class PageExtensions {
		/// <summary>
		/// Return a page and paging information. Also support backward pagination
		/// with negative number
		/// </summary>
		/// <param name="source">IQueryable source</param>
		/// <param name="index">Current page number. Support negative for inverse navigation</param>
		/// <param name="size">Number of items per page. Must be greater of zero</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Source can not be null</exception>
		/// <exception cref="ArgumentException">Item per page can not be less than one</exception>
		public static Page<T> Page<T>(this IQueryable<T> source, int index, int size) {
			if (size < 1)
				throw new ArgumentException("Page size can not be less than one.", nameof(size));

			if (!source.Any())
				return new Page<T>(new List<T>(), index, size, 0);

			List<T> items = index < 0
				? source.SkipLast(-(index + 1) * size).TakeLast(size).ToList()
				: source.Skip(index * size).Take(size).ToList();

			return new Page<T>(items, index, size, source.Count());
		}

		/// <summary>
		/// Return a page and paging information. Also support backward pagination
		/// with negative number
		/// </summary>
		/// <param name="source">IQueryable source</param>
		/// <param name="index">Current page number. Support negative for inverse navigation</param>
		/// <param name="size">Number of items per page. Must be greater of zero</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Source can not be null</exception>
		/// <exception cref="ArgumentException">Item per page can not be less than one</exception>
		public static async Task<Page<T>> PageAsync<T>(
			this IQueryable<T> source,
			int index,
			int size,
			CancellationToken cancellationToken = default
		) {
			if (size < 1)
				throw new ArgumentException("Page size can not be less than one.", nameof(size));

			if (!await source.AnyAsync(cancellationToken))
				return new Page<T>(new List<T>(), index, size, 0);

			var items = index < 0
				? await source.SkipLast(-(index + 1) * size).TakeLast(size)
					.ToListAsync(cancellationToken)
				: await source.Skip(index * size).Take(size)
					.ToListAsync(cancellationToken);

			var total = await source.CountAsync(cancellationToken);

			return new Page<T>(items, index, size, total);
		}
	}
}
