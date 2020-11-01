﻿using System;

namespace X.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ClearTime(this DateTime dateTime)
        {
            return dateTime.Subtract(
                new TimeSpan(
                    0,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond));
        }
    }
}
