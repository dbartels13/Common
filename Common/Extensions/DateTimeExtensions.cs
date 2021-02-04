using System;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Any DateTime extension methods
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Determines if 2 DateTimes are "equal"
        /// </summary>
        /// <param name="val1">The first Datetime to compare</param>
        /// <param name="val2">The 2nd Datetime to compare</param>
        /// <param name="checkDateComponent">If comparison will check the date component</param>
        /// <param name="checkTimeComponent">If comparison will check the time component</param>
        /// <returns>True if equal, false otherwise</returns>
        public static bool IsEqual(this DateTime val1, DateTime val2, bool checkDateComponent = true, bool checkTimeComponent = false)
        {
            if (!checkDateComponent)
                return !checkTimeComponent || val1.TimeOfDay.Equals(val2.TimeOfDay);

            if (!val1.Date.Equals(val2.Date))
                return false;
            return !checkTimeComponent || val1.TimeOfDay.Equals(val2.TimeOfDay);
        }

        /// <summary>
        /// Determines if a Datetime occurred before another
        /// </summary>
        /// <param name="val1">The first Datetime to compare</param>
        /// <param name="val2">The 2nd Datetime to compare</param>
        /// <param name="checkDateComponent">If comparison will check the date component</param>
        /// <param name="checkTimeComponent">If comparison will check the time component</param>
        /// <returns>True if first is less than the 2nd, false otherwise</returns>
        public static bool IsLessThan(this DateTime val1, DateTime val2, bool checkDateComponent = true, bool checkTimeComponent = false)
        {
            if (checkDateComponent)
            {
                if (val1.Date < val2.Date)
                    return true;
                if (val1.Date > val2.Date)
                    return false;
                if (checkTimeComponent)
                    return val1.TimeOfDay < val2.TimeOfDay;
                return false; // Equal
            }
            if (checkTimeComponent)
                return val1.TimeOfDay < val2.TimeOfDay;
            return true; // No checks, so this is default
        }

        /// <summary>
        /// Determines if a Datetime occurred before or at the same time as another
        /// </summary>
        /// <param name="val1">The first Datetime to compare</param>
        /// <param name="val2">The 2nd Datetime to compare</param>
        /// <param name="checkDateComponent">If comparison will check the date component</param>
        /// <param name="checkTimeComponent">If comparison will check the time component</param>
        /// <returns>True if first is before or equal to the 2nd, false otherwise</returns>
        public static bool IsLessThanOrEqual(this DateTime val1, DateTime val2, bool checkDateComponent = true, bool checkTimeComponent = false)
        {
            if (checkDateComponent)
            {
                if (val1.Date < val2.Date)
                    return true;
                if (val1.Date > val2.Date)
                    return false;
                if (checkTimeComponent)
                    return val1.TimeOfDay <= val2.TimeOfDay;
                return true; // Equal
            }
            if (checkTimeComponent)
                return val1.TimeOfDay <= val2.TimeOfDay;
            return true; // No checks, so this is default
        }

        /// <summary>
        /// Determines if a Datetime occurred after another
        /// </summary>
        /// <param name="val1">The first Datetime to compare</param>
        /// <param name="val2">The 2nd Datetime to compare</param>
        /// <param name="checkDateComponent">If comparison will check the date component</param>
        /// <param name="checkTimeComponent">If comparison will check the time component</param>
        /// <returns>True if first is after than the 2nd, false otherwise</returns>
        public static bool IsMoreThan(this DateTime val1, DateTime val2, bool checkDateComponent = true, bool checkTimeComponent = false)
        {
            if (checkDateComponent)
            {
                if (val1.Date > val2.Date)
                    return true;
                if (val1.Date < val2.Date)
                    return false;
                if (checkTimeComponent)
                    return val1.TimeOfDay > val2.TimeOfDay;
                return false; // Equal
            }
            if (checkTimeComponent)
                return val1.TimeOfDay > val2.TimeOfDay;
            return true; // No checks, so this is default
        }

        /// <summary>
        /// Determines if a Datetime occurred after or at the same time as another
        /// </summary>
        /// <param name="val1">The first Datetime to compare</param>
        /// <param name="val2">The 2nd Datetime to compare</param>
        /// <param name="checkDateComponent">If comparison will check the date component</param>
        /// <param name="checkTimeComponent">If comparison will check the time component</param>
        /// <returns>True if first is after or equal to the 2nd, false otherwise</returns>
        public static bool IsMoreThanOrEqual(this DateTime val1, DateTime val2, bool checkDateComponent = true, bool checkTimeComponent = false)
        {
            if (checkDateComponent)
            {
                if (val1.Date > val2.Date)
                    return true;
                if (val1.Date < val2.Date)
                    return false;
                if (checkTimeComponent)
                    return val1.TimeOfDay >= val2.TimeOfDay;
                return true; // Equal
            }
            if (checkTimeComponent)
                return val1.TimeOfDay >= val2.TimeOfDay;
            return true; // No checks, so this is default
        }

        /// <summary>
        /// Determines if a Datetime occurred in a date range
        /// </summary>
        /// <param name="val">The datetime for comparison</param>
        /// <param name="val1">The lower Datetime to compare</param>
        /// <param name="val2">The upper Datetime to compare</param>
        /// <param name="checkDateComponent">If comparison will check the date component</param>
        /// <param name="checkTimeComponent">If comparison will check the time component</param>
        /// <param name="includeLowerValue">Specifies whether the check will include a value equal to val1 (lower)</param>
        /// <param name="includeUpperValue">Specifies whether the check will include a value equal to val2 (upper)</param>
        /// <returns>True if val falls between val1 and val2. False otherwise</returns>
        public static bool Between(this DateTime val, DateTime val1, DateTime val2, bool checkDateComponent = true, bool checkTimeComponent = false, bool includeLowerValue = true, bool includeUpperValue = true)
        {
            if (checkDateComponent)
            {
                if (val.Date < val1.Date)
                    return false;
                if (val.Date > val2.Date)
                    return false;
                if (val1.Date == val2.Date) // All dates are the same
                {
                    if (!checkTimeComponent)
                        return includeLowerValue || includeUpperValue;
                    if (val.TimeOfDay < val1.TimeOfDay)
                        return false;
                    if (val.TimeOfDay > val2.TimeOfDay)
                        return false;
                    if (val.TimeOfDay == val1.TimeOfDay)
                        return includeLowerValue;
                    return val.TimeOfDay != val2.TimeOfDay || includeUpperValue;
                }
                if (val.Date == val1.Date)
                {
                    if (!checkTimeComponent)
                        return includeLowerValue;
                    if (val.TimeOfDay < val1.TimeOfDay)
                        return false;
                    return val.TimeOfDay != val1.TimeOfDay || includeLowerValue;
                }
                if (val.Date != val2.Date)
                    return true;
                if (!checkTimeComponent)
                    return includeUpperValue;
                if (val.TimeOfDay > val2.TimeOfDay)
                    return false;
                return val.TimeOfDay != val2.TimeOfDay || includeUpperValue;
            }

            // No checks, so this is default
            if (!checkTimeComponent)
                return true;
            if (val.TimeOfDay == val1.TimeOfDay)
                return includeLowerValue;
            if (val.TimeOfDay == val2.TimeOfDay)
                return includeUpperValue;
            if (val.TimeOfDay < val1.TimeOfDay)
                return false;
            return val.TimeOfDay <= val2.TimeOfDay;
        }

        /// <summary>
        /// If the value is UTC, this formally makes it UTC
        /// </summary>
        /// <param name="val">The datetime value that was set via a UTC string</param>
        /// <returns>A UTC DateTime</returns>
        public static DateTime AsUtc(this DateTime val) => DateTime.SpecifyKind(val, DateTimeKind.Utc);
    }
}
