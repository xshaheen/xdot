using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using X.Core.Extensions;

namespace X.Core.Utils
{
    /// <summary>
    /// A collection of common checks clauses, implemented as static methods.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="input"/> is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <returns><paramref name="input"/> if the value is not null.</returns>
        public static T NotNull<T>([NotNull] T? input, string parameterName)
        {
            if (input is null) throw new ArgumentNullException(parameterName);

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="maxLength"></param>
        /// <param name="minLength"></param>
        /// <returns><paramref name="input"/> if the value is not null.</returns>
        public static string NotNull(
            [NotNull] string? input,
            string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (input is null) throw new ArgumentException($"{parameterName} can not be null!", parameterName);

            if (input.Length > maxLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);

            if (minLength > 0 && input.Length < minLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or bigger than {minLength}!",
                    parameterName);

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="maxLength"></param>
        /// <param name="minLength"></param>
        /// <returns></returns>
        public static string NotNullOrWhiteSpace(
            [NotNull] string? input,
            string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);

            if (input.Length > maxLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);

            if (minLength > 0 && input.Length < minLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or bigger than {minLength}!",
                    parameterName);

            return input;
        }

        public static string NotNullOrEmpty(
            [NotNull] string? input,
            string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);

            if (input.Length > maxLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);

            if (minLength > 0 && input.Length < minLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or bigger than {minLength}!",
                    parameterName);

            return input;
        }

        public static ICollection<T> NotNullOrEmpty<T>([NotNull] ICollection<T>? input, string parameterName)
        {
            if (input.IsNullOrEmpty())
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);

            return input;
        }

        public static string? Length(string? input, string parameterName, int maxLength, int minLength = 0)
        {
            if (minLength > 0)
            {
                if (string.IsNullOrEmpty(input))
                    throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);

                if (input.Length < minLength)
                    throw new ArgumentException(
                        $"{parameterName} length must be equal to or bigger than {minLength}!",
                        parameterName);
            }

            if (input is not null && input.Length > maxLength)
                throw new ArgumentException(
                    $"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);

            return input;
        }
    }
}
