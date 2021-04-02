using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace X.Core.Utils {
    public static class ReflectionHelper {
        /// <summary>
        /// Tries to gets an of attribute defined for a class member and it's declaring type including
        /// inherited attributes.
        /// Returns default value if it's not declared at all.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="defaultValue">Default value (null as default)</param>
        /// <param name="inherit">Inherit attribute from base classes</param>
        public static TAttribute? GetSingleAttributeOrDefault<TAttribute>(
            MemberInfo memberInfo,
            TAttribute? defaultValue = default,
            bool inherit = true
        )
            where TAttribute : Attribute
        //Get attribute on the member
        {
            return memberInfo.IsDefined(typeof(TAttribute), inherit)
                ? memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>()
                    .First()
                : defaultValue;
        }

        /// <summary>
        /// Tries to gets an of attribute defined for a class member and it's declaring type including
        /// inherited attributes.
        /// Returns default value if it's not declared at all.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="defaultValue">Default value (null as default)</param>
        /// <param name="inherit">Inherit attribute from base classes</param>
        public static TAttribute? GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(
            MemberInfo memberInfo,
            TAttribute? defaultValue = default,
            bool inherit = true
        )
            where TAttribute : class {
            return memberInfo.GetCustomAttributes(inherit)
                       .OfType<TAttribute>()
                       .FirstOrDefault()
                   ?? memberInfo.DeclaringType?
                       .GetTypeInfo()
                       .GetCustomAttributes(inherit)
                       .OfType<TAttribute>()
                       .FirstOrDefault()
                   ?? defaultValue;
        }

        /// <summary>
        /// Tries to gets attributes defined for a class member and it's declaring type including inherited
        /// attributes.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="inherit">Inherit attribute from base classes</param>
        public static IEnumerable<TAttribute> GetAttributesOfMemberOrDeclaringType<TAttribute>(
            MemberInfo memberInfo,
            bool inherit = true
        ) where TAttribute : class {
            var customAttributes = memberInfo.GetCustomAttributes(inherit).OfType<TAttribute>();

            var declaringTypeCustomAttributes =
                memberInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes(inherit)
                    .OfType<TAttribute>();

            return declaringTypeCustomAttributes != null
                ? customAttributes.Concat(declaringTypeCustomAttributes).Distinct()
                : customAttributes;
        }
    }
}
