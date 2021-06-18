using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using X.FluentValidation.Resources;

// ReSharper disable once CheckNamespace
namespace FluentValidation {
    /* A virus/malware scanner API MUST be used on the file before making the file available to users or other systems. */
    public static class FileValidators {
        public static IRuleBuilderOptions<T, TIFormFile?> MinSize<T, TIFormFile>(
            this IRuleBuilder<T, TIFormFile?> builder,
            int minSize
        ) where TIFormFile : IFormFile {
            return builder
                .Must((_, file, context) => {
                    if (file is null || file.Length >= minSize) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MinSize",
                            (minSize / 1048576).ToString("N1", CultureInfo.CurrentCulture))
                        .AppendArgument("TotalLength",
                            (file.Length / 1048576).ToString("N1", CultureInfo.CurrentCulture));

                    return false;
                })
                .WithErrorCode(nameof(Errors.FileMinimumLengthValidator))
                .WithMessage(Errors.FileMinimumLengthValidator);
        }

        public static IRuleBuilderOptions<T, TIFormFile?> MaxSize<T, TIFormFile>(
            this IRuleBuilder<T, TIFormFile?> builder,
            int maxSize
        ) where TIFormFile : IFormFile {
            return builder
                .Must((_, file, context) => {
                    if (file is null || file.Length <= maxSize) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MaxSize",
                            (maxSize / 1048576).ToString("N1", CultureInfo.InvariantCulture))
                        .AppendArgument("TotalLength",
                            (file.Length / 1048576).ToString("N1",
                                CultureInfo.InvariantCulture));

                    return false;
                })
                .WithErrorCode(nameof(Errors.FileMaximumLengthValidator))
                .WithMessage(Errors.FileMaximumLengthValidator);
        }

        public static IRuleBuilderOptions<T, TIFormFile?> ContentTypes<T, TIFormFile>(
            this IRuleBuilder<T, TIFormFile?> builder,
            params string[] contentTypes
        ) where TIFormFile : IFormFile {
            return builder
                .Must((_, file, context) => {
                    if (file is null
                        || contentTypes.Any(contentType
                            => file.ContentType.Equals(contentType,
                                StringComparison.OrdinalIgnoreCase))) {
                        return true;
                    }

                    context.MessageFormatter.AppendArgument("ContentTypes",
                        contentTypes.Aggregate((p, c) => $"'{p}', '{c}'"));

                    return false;
                })
                .WithErrorCode(nameof(Errors.FileContentTypeValidator))
                .WithMessage(Errors.FileContentTypeValidator);
        }
    }
}
