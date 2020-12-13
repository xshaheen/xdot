using System;
using System.ComponentModel;
using System.Reflection;

namespace X.Core.Extensions {
    public static class EnumExtensions {
        public static string Description(this Enum? value) {
            if (value == null) return string.Empty;

            var attribute = value.GetAttribute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }

        private static T? GetAttribute<T>(this Enum? value) where T : Attribute {
            if (value == null) return null;

            MemberInfo[] member = value.GetType().GetMember(value.ToString());

            object[] attributes = member[0].GetCustomAttributes(typeof(T), false);

            return (T) attributes[0];
        }
    }
}