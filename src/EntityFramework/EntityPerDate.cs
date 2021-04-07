using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore {
    public record EntityPerDate(DateTime Date, int Count);
}
