using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace X.Core.Extensions {
    public static class ObjectExtensions {
        /// <summary>
        /// Converts the value of a specified object into a JSON string and return
        /// byte representation of the string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Bytes(this object obj) {
            return Encoding.Default.GetBytes(obj.ToJson());
        }

        /// <summary>
        /// Converts the value of a specified object into a JSON string and return
        /// byte representation of the string.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static byte[] Bytes(this object obj, JsonSerializerOptions options) {
            return Encoding.Default.GetBytes(obj.ToJson(options));
        }

        /// <summary>
        /// Converts the value of a specified type into a JSON string.
        /// </summary>
        /// <param name="obj">The value to convert.</param>
        /// <param name="camelCase">Serialized property name style.</param>
        /// <returns>The JSON string representation of the value.</returns>
        public static string ToJson(this object obj, bool camelCase = true) {
            return JsonSerializer.Serialize(
                obj,
                camelCase
                    ? new JsonSerializerOptions {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    }
                    : null);
        }

        /// <summary>
        /// Converts the value of a specified type into a JSON string.
        /// </summary>
        /// <param name="obj">The value to convert.</param>
        /// <param name="options">Options to control the conversion behavior.</param>
        /// <returns>The JSON string representation of the value.</returns>
        public static string ToJson(this object obj, JsonSerializerOptions options) {
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Used to simplify and beautify casting an object to a type.
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj) {
            return (T) obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,Type)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj) where T : struct {
            if (typeof(T) == typeof(Guid)) {
                return (T) TypeDescriptor.GetConverter(typeof(T))
                    .ConvertFromInvariantString(obj.ToString());
            }

            return (T) Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list) {
            return list.Contains(item);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, IEnumerable<T> list) {
            return list.Contains(item);
        }

        /// <summary>
        /// Can be used to conditionally perform a function on an object and return the modified or the
        /// original object.
        /// It is useful for chained calls.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="condition">A condition</param>
        /// <param name="func">A function that is executed only if the condition is <code>true</code></param>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <returns>
        /// Returns the modified object (by the <paramref name="func"/> if the <paramref name="condition"/> is
        /// <code>true</code>)
        /// or the original object if the <paramref name="condition"/> is <code>false</code>
        /// </returns>
        public static T If<T>(this T obj, bool condition, Func<T, T> func) {
            return condition ? func(obj) : obj;
        }

        /// <summary>
        /// Can be used to conditionally perform an action on an object and return the original object.
        /// It is useful for chained calls on the object.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="condition">A condition</param>
        /// <param name="action">An action that is executed only if the condition is <code>true</code></param>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <returns>
        /// Returns the original object.
        /// </returns>
        public static T If<T>(this T obj, bool condition, Action<T> action) {
            if (condition) {
                action(obj);
            }

            return obj;
        }
    }
}
