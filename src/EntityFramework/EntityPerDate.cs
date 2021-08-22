using System;

#pragma warning disable IDE0130
namespace Microsoft.EntityFrameworkCore {
#pragma warning restore IDE0130
    public record EntityPerDate(DateTime Date, int Count);
}
