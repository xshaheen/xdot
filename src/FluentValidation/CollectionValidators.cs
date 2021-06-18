using System.Collections.Generic;
using System.Linq;
using X.FluentValidation.Resources;

// ReSharper disable once CheckNamespace
namespace FluentValidation {
    public static class CollectionValidators {
        public static IRuleBuilderOptions<T, ICollection<TElement>> MaximumElements<T, TElement>(
            this IRuleBuilder<T, ICollection<TElement>> builder,
            int maxElements
        ) {
            return builder
                .Must((_, list, context) => {
                    if (list is null || list.Count <= maxElements) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MaxElements", maxElements)
                        .AppendArgument("TotalElements", list.Count);

                    return false;
                })
                .WithErrorCode(nameof(Errors.MaximumElementsValidator))
                .WithMessage(Errors.MaximumElementsValidator);
        }

        public static IRuleBuilderOptions<T, ICollection<TElement>> MinimumElements<T, TElement>(
            this IRuleBuilder<T, ICollection<TElement>> builder,
            int minElements
        ) {
            return builder
                .Must((_, list, context) => {
                    if (list is null || list.Count >= minElements) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MinElements", minElements)
                        .AppendArgument("TotalElements", list.Count);

                    return false;
                })
                .WithErrorCode(nameof(Errors.MinimumElementsValidator))
                .WithMessage(Errors.MinimumElementsValidator);
        }

        public static IRuleBuilderOptions<T, IEnumerable<TElement>> MaximumElements<T, TElement>(
            this IRuleBuilder<T, IEnumerable<TElement>> builder,
            int maxElements
        ) {
            return builder
                .Must((_, list, context) => {
                    if (list is null) {
                        return true;
                    }

                    var arr = list as TElement[] ?? list.ToArray();
                    if (arr.Length <= maxElements) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MaxElements", maxElements)
                        .AppendArgument("TotalElements", arr.Length);

                    return false;
                })
                .WithErrorCode(nameof(Errors.MaximumElementsValidator))
                .WithMessage(Errors.MaximumElementsValidator);
        }

        public static IRuleBuilderOptions<T, IEnumerable<TElement>> MinimumElements<T, TElement>(
            this IRuleBuilder<T, IEnumerable<TElement>> builder,
            int minElements
        ) {
            return builder
                .Must((_, list, context) => {
                    if (list is null) {
                        return true;
                    }

                    var arr = list as TElement[] ?? list.ToArray();
                    if (arr.Length >= minElements) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MinElements", minElements)
                        .AppendArgument("TotalElements", arr.Length);

                    return false;
                })
                .WithErrorCode(nameof(Errors.MinimumElementsValidator))
                .WithMessage(Errors.MinimumElementsValidator);
        }

        public static IRuleBuilderOptions<T, IReadOnlyCollection<TElement>>
            MaximumElements<T, TElement>(
                this IRuleBuilder<T, IReadOnlyCollection<TElement>> builder,
                int maxElements
            ) {
            return builder
                .Must((_, list, context) => {
                    if (list is null || list.Count <= maxElements) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MaxElements", maxElements)
                        .AppendArgument("TotalElements", list.Count);

                    return false;
                })
                .WithErrorCode(nameof(Errors.MaximumElementsValidator))
                .WithMessage(Errors.MaximumElementsValidator);
        }

        public static IRuleBuilderOptions<T, IReadOnlyCollection<TElement>>
            MinimumElements<T, TElement>(
                this IRuleBuilder<T, IReadOnlyCollection<TElement>> builder,
                int minElements
            ) {
            return builder
                .Must((_, list, context) => {
                    if (list is null || list.Count >= minElements) {
                        return true;
                    }

                    context.MessageFormatter
                        .AppendArgument("MinElements", minElements)
                        .AppendArgument("TotalElements", list.Count);

                    return false;
                })
                .WithErrorCode(nameof(Errors.MinimumElementsValidator))
                .WithMessage(Errors.MinimumElementsValidator);
        }
    }
}
