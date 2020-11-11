﻿using System;
using System.Globalization;

namespace SharedKernel.Application.Utils.Dates
{
    public static class DateTimeExtensions
    {
        public static int ToWeekNumber(this DateTime dateTime)
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }


        public static string ToReadableString(this TimeSpan? span)
        {
            return span == default || span.Value == default ? "-" : span.Value.ToReadableString();
        }

        public static string ToReadableString(this TimeSpan span)
        {
            //string formatted =
            //    $"{(span.Duration().Days > 0 ? $"{span.Days:0} {CommonRes.TimeSpanDay}{(span.Days == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} {CommonRes.TimeSpanHour}{(span.Hours == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} {CommonRes.TimeSpanMinute}{(span.Minutes == 1 ? string.Empty : "s")}, " : string.Empty)}{(span.Duration().Seconds > 0 ? $"{span.Seconds:0} {CommonRes.TimeSpanSecond}{(span.Seconds == 1 ? string.Empty : "s")}" : string.Empty)}";
            var hours = (int) Math.Truncate(span.Duration().TotalHours);
            var formatted =
                $"{(hours > 0 ? $"{hours:0} hora{(hours == 1 ? string.Empty : "s")}, " : string.Empty)}" +
                $"{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} minuto{(span.Minutes == 1 ? string.Empty : "s")}, " : string.Empty)}" +
                $"{(span.Duration().Seconds > 0 ? $"{span.Seconds:0} segundo{(span.Seconds == 1 ? string.Empty : "s")}" : string.Empty)}";

            if (formatted.EndsWith(", "))
                formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted))
                formatted = " - ";

            return formatted;
        }

        public static int MonthDifference(this DateTime maxValue, DateTime minValue)
        {
            return maxValue.Month - minValue.Month + 12 * (maxValue.Year - minValue.Year);
        }
    }
}