/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace X.Table
{
    public static class FilterExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, Filters? filters) where T : class
            => filters is null
                ? queryable
                : filters.Aggregate(
                    queryable,
                    (current, filter) => current.Filter(filter.Property, filter.Comparison, filter.Value));

        public static IQueryable<T> Filter<T>(
            this IQueryable<T> queryable,
            string property,
            object value)
            where T : class
            => queryable.Filter(property, string.Empty, value);

        public static IQueryable<T> Filter<T>(
            this IQueryable<T> queryable,
            string property,
            string comparison,
            object? value)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(property) || value is null || string.IsNullOrWhiteSpace(value.ToString())) return queryable;

            ParameterExpression parameter = Expression.Parameter(typeof(T));
            Expression          left      = Create(property, parameter);
            try
            {
                value = Change(value, left.Type);
            }
            catch
            {
                return Enumerable.Empty<T>().AsQueryable<T>();
            }

            ConstantExpression constantExpression = Expression.Constant(value, left.Type);
            Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(
                Create(left, comparison, (Expression) constantExpression),
                parameter);
            return queryable.Where<T>(predicate);
        }

        private static object Change(object value, Type type)
        {
            if (type.BaseType == typeof(Enum)) value = Enum.Parse(type, value.ToString());
            return Convert.ChangeType(value, type);
        }

        private static Expression Create(string property, ParameterExpression parameter)
            => ((IEnumerable<string>) property.Split('.')).Aggregate<string, Expression>(
                (Expression) parameter,
                new Func<Expression, string, Expression>(Expression.Property));

        private static Expression Create(
            Expression left,
            string comparison,
            Expression right)
        {
            if (string.IsNullOrWhiteSpace(comparison) && left.Type == typeof(string))
                return Expression.Call(left, "Contains", Type.EmptyTypes, right);

            ExpressionType binaryType = comparison == "=="
                ? ExpressionType.Equal
                : comparison == "<"
                    ? ExpressionType.LessThan
                    : comparison == "<="
                        ? ExpressionType.LessThanOrEqual
                        : comparison == ">"
                            ? ExpressionType.GreaterThan
                            : comparison == ">="
                                ? ExpressionType.GreaterThanOrEqual
                                : comparison == "!="
                                    ? ExpressionType.NotEqual
                                    : ExpressionType.Equal;

            return Expression.MakeBinary(binaryType, left, right);
        }
    }
}
 */


