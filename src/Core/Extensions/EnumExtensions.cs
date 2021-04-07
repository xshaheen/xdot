using System.ComponentModel;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System {
    [PublicAPI]
    public static class EnumExtensions {
        public static string Description(this Enum? value) {
            if (value == null) {
                return string.Empty;
            }

            var attribute = value._GetAttribute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }

        private static T? _GetAttribute<T>(this Enum? value) where T : Attribute {
            if (value == null) {
                return null;
            }

            var member = value.GetType().GetMember(value.ToString());

            var attributes = member[0].GetCustomAttributes(typeof(T), false);

            return (T) attributes[0];
        }
    }
}
