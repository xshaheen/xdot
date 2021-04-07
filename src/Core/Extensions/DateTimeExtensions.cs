using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System {
    [PublicAPI]
    public static class DateTimeExtensions {
        public static DateTime ClearTime(this DateTime dateTime) {
            return dateTime.Subtract(new TimeSpan(
                0,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond));
        }
    }
}
