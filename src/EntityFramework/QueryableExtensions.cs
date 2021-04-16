using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore {
    public static class QueryableExtensions {
        /// <summary>
        /// Count number of entities per each year in the interval from <paramref name="start"/>
        /// to <paramref name="end"/> year backward.
        /// <remarks>
        /// The months of <paramref name="start"/> and <paramref name="end"/> will be ignored
        /// and assume that the two is at the same months.
        /// </remarks>
        /// </summary>
        public static async Task<IEnumerable<EntityPerDate>> CountPerYear<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, DateTime>> propSelector,
            DateTime start,
            DateTime end,
            CancellationToken token = default
        ) {
            Guard.Against.Null(queryable, nameof(queryable));

            if (start > end) {
                throw new ArgumentException("Start date must be less than the end date.");
            }

            var months = end.Year - start.Year;

            DateTime first;
            DateTime last;

            if (months > 1) {
                first = new DateTime(start.Year, 1, 1);
                last = new DateTime(end.Year, 1, 1);
            }
            else {
                first = new DateTime(end.Year, 1, 1);
                last = new DateTime(end.Year + 1, 1, 1);
            }


            // Make expressions

            var typeParameterExpression = Expression.Parameter(typeof(T), "e");
            var selectedPropertyInfo = (PropertyInfo) ((MemberExpression) propSelector.Body).Member;

            var propertyAccessor = Expression.Property(typeParameterExpression, selectedPropertyInfo);

            // -- start at

            var greaterThanStartExp = Expression.GreaterThan(
                propertyAccessor,
                Expression.Constant(start.AddYears(-1), typeof(DateTime)));

            var greaterThanStartPredicate = Expression.Lambda<Func<T, bool>>(greaterThanStartExp, typeParameterExpression);

            // -- end at

            Expression lessThanEndExp = Expression.LessThan(
                propertyAccessor,
                Expression.Constant(last.AddYears(1)));

            var lessThanEndPredicate = Expression.Lambda<Func<T, bool>>(lessThanEndExp, typeParameterExpression);

            // Query

            var query = queryable
                .Where(greaterThanStartPredicate)
                .Where(lessThanEndPredicate)
                .Select(Expression.Lambda<Func<T, DateTime>>(propertyAccessor,
                    typeParameterExpression))
                .Select(date => new {
                    At   = date,
                    Year = new DateTime(date.Year, 1, 1),
                });

            var list = await query.ToListAsync(token);

            var lookup = list.ToLookup(
                x => x.Year,
                x => x.At);

            return
                from n in Enumerable.Range(1, months)
                let month = first.AddMonths(n)
                select new EntityPerDate(month, lookup[month].Count());
        }

        /// <summary>
        /// Count number of entities per each month in the interval from <paramref name="start"/>
        /// to <paramref name="end"/> months backward.
        /// <remarks>
        /// The day of <paramref name="start"/> and <paramref name="end"/> will be ignored
        /// and assume that the two is at the same day.
        /// </remarks>
        /// </summary>
        public static async Task<IEnumerable<EntityPerDate>> CountPerMonth<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, DateTime>> propSelector,
            DateTime start,
            DateTime end,
            CancellationToken token = default
        ) {
            Guard.Against.Null(queryable, nameof(queryable));

            if (start > end) {
                throw new ArgumentException("Start date must be less than the end date.");
            }

            // Assuming the day of the month is irrelevant (i.e. the diff between 2020.1.1 and 2019.12.31 is one month also)
            var months = ((end.Year - start.Year) * 12) + end.Month - start.Month;

            var last = new DateTime(end.Year, end.Month, 1);
            var first = last.AddMonths(-months);

            // Make expressions

            var typeParameterExpression = Expression.Parameter(typeof(T), "e");
            var selectedPropertyInfo = (PropertyInfo) ((MemberExpression) propSelector.Body).Member;

            var propertyAccessor =
                Expression.Property(typeParameterExpression, selectedPropertyInfo);

            // -- start at

            Expression greaterThanStartExp = Expression.GreaterThan(
                propertyAccessor,
                Expression.Constant(start.AddMonths(-1), typeof(DateTime)));

            var greaterThanStartPredicate =
                Expression.Lambda<Func<T, bool>>(greaterThanStartExp, typeParameterExpression);

            // -- end at

            var lessThanEndExp = Expression.LessThan(
                propertyAccessor,
                Expression.Constant(last.AddMonths(1)));

            var lessThanEndPredicate =
                Expression.Lambda<Func<T, bool>>(lessThanEndExp, typeParameterExpression);

            // Query

            var query = queryable
                .Where(greaterThanStartPredicate)
                .Where(lessThanEndPredicate)
                .Select(Expression.Lambda<Func<T, DateTime>>(propertyAccessor, typeParameterExpression))
                .Select(date => new {
                    At    = date,
                    Month = new DateTime(date.Year, date.Month, 1),
                });

            var list = await query.ToListAsync(token);

            var lookup = list.ToLookup(
                x => x.Month,
                x => x.At);

            return
                from n in Enumerable.Range(1, months)
                let month = first.AddMonths(n)
                select new EntityPerDate(month, lookup[month].Count());
        }

        /// <summary>
        /// Count number of entities per each day in the interval from <paramref name="start"/>
        /// to <paramref name="end"/> months backward.
        /// <remarks>
        /// The time of <paramref name="start"/> and <paramref name="end"/> will be ignored
        /// and assume that the two is at the same time.
        /// </remarks>
        /// </summary>
        public static async Task<IEnumerable<EntityPerDate>> CountPerDay<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, DateTime>> propSelector,
            DateTime start,
            DateTime end,
            CancellationToken token = default
        ) {
            Guard.Against.Null(queryable, nameof(queryable));

            if (start > end) {
                throw new ArgumentException("Start date must be less than the end date.");
            }

            var first = start.Date;
            var last = end.Date;

            // Make expressions
            var typeParameterExpression = Expression.Parameter(typeof(T), "e");

            var selectedPropertyInfo = (PropertyInfo) ((MemberExpression) propSelector.Body).Member;

            var propertyAccessor =
                Expression.Property(typeParameterExpression, selectedPropertyInfo);

            // -- start at

            var greaterThanStartExp = Expression.GreaterThan(
                propertyAccessor,
                Expression.Constant(first.AddDays(-1), typeof(DateTime)));

            var greaterThanPredicate =
                Expression.Lambda<Func<T, bool>>(greaterThanStartExp, typeParameterExpression);

            // -- end at

            var lessThanEndExp = Expression.LessThan(propertyAccessor,
                Expression.Constant(last.AddDays(1), typeof(DateTime)));

            var lessThanPredicate =
                Expression.Lambda<Func<T, bool>>(lessThanEndExp, typeParameterExpression);

            // Query

            var query = queryable
                .Where(greaterThanPredicate)
                .Where(lessThanPredicate)
                .Select(Expression.Lambda<Func<T, DateTime>>(propertyAccessor,
                    typeParameterExpression))
                .Select(date => new {
                    At  = date,
                    Day = new DateTime(date.Year, date.Month, date.Day),
                });

            var list = await query.ToListAsync(token);

            var lookup = list.ToLookup(x => x.Day, x => x.At);

            var days = (last - first).Days;

            return
                from n in Enumerable.Range(1, days)
                let date = first.AddDays(n)
                select new EntityPerDate(date, lookup[date].Count());
        }

        public static async Task<IEnumerable<EntityPerDate>> SumPerDay<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, DateTime>> propSelector,
            Expression<Func<T, int>> propToSumSelector,
            DateTime start,
            DateTime end,
            CancellationToken token = default
        ) {
            Guard.Against.Null(queryable, nameof(queryable));

            if (start > end) {
                throw new ArgumentException("Start date must be less than the end date.");
            }

            var first = start.Date;
            var last = end.Date;

            // Make expressions

            var typeParameterExpression = Expression.Parameter(typeof(T), "e");
            var selectedPropertyInfo = (PropertyInfo) ((MemberExpression) propSelector.Body).Member;

            var propertyAccessor =
                Expression.Property(typeParameterExpression, selectedPropertyInfo);


            // -- start at

            Expression greaterThanStartExp = Expression.GreaterThan(
                propertyAccessor,
                Expression.Constant(first.AddDays(-1), typeof(DateTime)));

            var greaterThanPredicate =
                Expression.Lambda<Func<T, bool>>(greaterThanStartExp, typeParameterExpression);

            // -- end at

            var lessThanEndExp = Expression.LessThan(propertyAccessor,
                Expression.Constant(last.AddDays(1), typeof(DateTime)));

            var lessThanPredicate =
                Expression.Lambda<Func<T, bool>>(lessThanEndExp, typeParameterExpression);

            // -- sum prop

            var sumPropertyInfo = (PropertyInfo) ((MemberExpression) propToSumSelector.Body).Member;
            var sumPropertyAccessor =
                Expression.Property(typeParameterExpression, sumPropertyInfo);

            // select func

            var datePropSelector = Expression
                .Lambda<Func<T, DateTime>>(propertyAccessor, typeParameterExpression).Compile();

            var sumPropSelector = Expression
                .Lambda<Func<T, int>>(sumPropertyAccessor, typeParameterExpression).Compile();

            // Query

            var query = queryable
                .Where(greaterThanPredicate)
                .Where(lessThanPredicate)
                .Select(i => new {
                    Date    = datePropSelector.Invoke(i),
                    IntProp = sumPropSelector.Invoke(i),
                })
                .Select(i => new {
                    i.Date,
                    i.IntProp,
                    Day = new DateTime(i.Date.Year, i.Date.Month, i.Date.Day),
                });

            var list = await query.ToListAsync(token);

            var lookup = list.ToLookup(x => x.Day);

            var days = (last - first).Days;

            return
                from n in Enumerable.Range(1, days)
                let date = first.AddDays(n)
                select new EntityPerDate(date, lookup[date].Sum(i => i.IntProp));
        }
    }
}
