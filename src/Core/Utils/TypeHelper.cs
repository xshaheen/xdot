using System;
using System.Reflection;

namespace X.Core.Utils
{
    public static class TypeHelper
    {
        public static bool IsFunc(object? obj)
        {
            if (obj is null) return false;

            var type = obj.GetType();
            if (!type.GetTypeInfo().IsGenericType) return false;

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        public static bool IsFunc<TReturn>(object? obj) => obj != null && obj.GetType() == typeof(Func<TReturn>);

        public static bool IsNullable(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static T GetDefaultValue<T>() => default!;

        public static object? GetDefaultValue(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

        public static bool IsDefaultValue(object? obj) => obj == null || obj.Equals(GetDefaultValue(obj.GetType()));
    }
}
