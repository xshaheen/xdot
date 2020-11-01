/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// using FluentValidation.Internal;
// using Microsoft.EntityFrameworkCore;

// https://stackoverflow.com/questions/19052507/how-to-use-a-func-in-an-expression-with-linq-to-entity-framework
// https://stackoverflow.com/questions/46346789/how-to-select-evaluated-value-of-expressionfunct-in-entity-framework-query
namespace Eldwaa.Application._Common.Extensions.Statistics
{
    public class EntityPerDate
    {
        public DateTime Date { get; set; }

        public int Count { get; set; }
    }

    public static class QueryableExtensions
    {
        public static IEnumerable<EntityPerDate> CountPerMonth<T>(
            this IQueryable<T> queryable,
            Func<T, DateTime> keySelector,
            uint numberOfMonthsBeforeNow
        ) where T : class
        {
            var now = DateTime.Now;

            var start = now.AddMonths((int) -numberOfMonthsBeforeNow);

            return queryable.CountPerMonth(keySelector, start, DateTime.Now);
        }

        /// <summary>
        ///     Count number of entities per each month in the interval from <paramref name="start"/>
        ///     to <paramref name="end"/> months backward.
        ///     <remarks>
        ///         The time of <paramref name="start"/> & <paramref name="end"/> will be ignored
        ///         and assume that the two is at the same day.
        ///     </remarks>
        /// </summary>
        public static IEnumerable<EntityPerDate> CountPerMonth<T>(
            this IQueryable<T> queryable,
            Func<T, DateTime> keySelector,
            DateTime start, DateTime end
        ) where T : class
        {
            if (start > end)
                throw new ArgumentException("Start date must be less than the end date.");

            // Assuming the day of the month is irrelevant (i.e. the diff between 2020.1.1 and 2019.12.31 is one month also)
            var months = (end.Year - start.Year) * 12 + end.Month - start.Month;

            var last  = new DateTime(end.Year, end.Month, 1);
            var first = last.AddMonths(-months);

            var query =
                from entity in queryable.AsNoTracking()
                let date = keySelector(entity)
                where date >= first
                where date < last.AddMonths(1)
                select new
                {
                    Entity = entity,
                    Month  = new DateTime(date.Year, date.Month, 1),
                };

            var lookup = query.ToLookup(
                x => x.Month,
                x => x.Entity);

            var counts =
                from n in Enumerable.Range(1, months)
                let month = first.AddMonths(n)
                select new EntityPerDate
                {
                    Date  = month,
                    Count = lookup[month].Count(),
                };

            return counts;
        }

        public static IEnumerable<EntityPerDate> CountPerDay<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, DateTime>> keySelector,
            uint numberOfDaysBeforeNow
        ) where T : class
        {
            var now = DateTime.Now;

            var start = now.AddDays((int) -numberOfDaysBeforeNow);

            return queryable.CountPerDay(keySelector, start, DateTime.Now);
        }

        /// <summary>
        ///     Count number of entities per each day in the interval from <paramref name="start"/>
        ///     to <paramref name="end"/> months backward.
        ///     <remarks>
        ///         The time of <paramref name="start"/> & <paramref name="end"/> will be ignored
        ///         and assume that the two is at the same time.
        ///     </remarks>
        /// </summary>
        public static IEnumerable<EntityPerDate> CountPerDay<T>(
            this IQueryable<T> source,
            Expression<Func<T, DateTime>> propSelector,
            DateTime start, DateTime end
        ) where T : class
        {
            if (start > end) throw new ArgumentException("Start date must be less than the end date.");

            var first = start.Date;
            var last  = end.Date;

            var com = propSelector.Compile();

            MemberInfo member = propSelector.GetMember();


            // var idProperty = typeof(T).GetProperty(propertyInfo.Name);


            var propertyInfo = (PropertyInfo) ((MemberExpression) propSelector.Body).Member;

            ParameterExpression parameter = Expression.Parameter(typeof(T));

            // var expression = Expression.Lambda<Func<T, bool>>(
            //     Expression.Call(
            //         Expression.Constant(ids),
            //         typeof(ICollection<int>).GetMethod("Contains"),
            //         Expression.Property(parameter, propertyInfo)),
            //     parameter);


            var query = source.AsNoTracking()
                .Where(CreateGreaterThanOrEqualFilter(propSelector, first))
                .Where(entity => com(entity) >= first && com(entity) < last.AddDays(1))
                .Select(entity => new
                {
                    Entity = entity,
                    Day    = new DateTime(com(entity).Year, com(entity).Month, com(entity).Day),
                });

            var lookup = query.ToLookup(
                x => x.Day,
                x => x.Entity);

            var days = (last - first).Days;

            var counts =
                from n in Enumerable.Range(1, days)
                let date = first.AddDays(n)
                select new EntityPerDate
                {
                    Date  = date,
                    Count = lookup[date].Count(),
                };

            return counts;
        }

        private static Expression<Func<T, bool>> CreateGreaterThanOrEqualFilter<T, TKey>(
            Expression<Func<T, TKey>> keySelector, TKey valueToCompare)
        {
            MemberInfo member = keySelector.GetMember();

            var propertyInfo = (keySelector.Body as MemberExpression)?.Member as PropertyInfo;

            if (propertyInfo is null) throw new ArgumentException();

            var typeProperty = typeof(T).GetProperty(propertyInfo.Name);

            var parameter = Expression.Parameter(typeof(T));

            var expressionParameter = Expression.Property(parameter, propertyInfo.Name);

            var body = Expression.GreaterThanOrEqual(expressionParameter,
                Expression.Constant(valueToCompare, typeof(TKey)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        // private static Expression<Func<TData, bool>> CreateGreaterThanFilter<TData, TKey>(
        //     Expression<Func<TData, TKey>> selector,
        //     TKey valueToCompare)
        // {
        //     var parameter = Expression.Parameter(typeof(TData));
        //
        //     var expressionParameter = Expression.Property(parameter, propertyInfo.Name);
        //
        //     var body = Expression.GreaterThan(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
        //
        //     return Expression.Lambda<Func<TData, bool>>(body, parameter);
        // }

        private static string GetParameterName<TData, TKey>(Expression<Func<TData, TKey>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
                memberExpression = ((UnaryExpression) expression.Body).Operand as MemberExpression;

            return memberExpression.ToString();
        }
    }
}*/



