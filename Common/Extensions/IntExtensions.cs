using System;
using System.Globalization;
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Any int extension methods
    /// </summary>
    public static class IntExtensions
    {
        #region Enum
        /// <summary>
        /// Casts an int to an enum
        /// </summary>
        /// <typeparam name="T">An enumeration</typeparam>
        /// <param name="val">The value of the enumeration to check</param>
        /// <returns>The enumeration value</returns>
        public static T ToEnum<T>(this int val) where T : struct, IConvertible
        {
            if (Enum.IsDefined(typeof(T), val))
                return (T)Enum.ToObject(typeof(T), val);
            throw new Exception($"{val} is not a {typeof(T)}");
        }

        /// <summary>
        /// Casts a string to an enum
        /// </summary>
        /// <typeparam name="T">An enumeration</typeparam>
        /// <param name="val">The value of the enumeration to check</param>
        /// <param name="defaultValue">If can not find a match, this will be returned</param>
        /// <returns>The enumeration value</returns>
        public static T ToEnum<T>(this int val, T defaultValue) where T : struct, IConvertible
        {
            if (Enum.IsDefined(typeof(T), val))
                return (T)Enum.ToObject(typeof(T), val);
            return defaultValue;
        }
        #endregion

        #region Comparison
        /// <summary>
        /// Determines if a 
        /// </summary>
        /// <param name="val">The value being checked</param>
        /// <param name="val1">The lower limit</param>
        /// <param name="val2">The upper limit</param>
        /// <param name="includeLowerValue">If true (default), then if the value == the lower value, this will return true</param>
        /// <param name="includeUpperValue">If true (default), then if the value == the upper value, this will return true</param>
        /// <returns></returns>
        public static bool Between(this int val, int val1, int val2, bool includeLowerValue = true, bool includeUpperValue = true)
        {
            if (val < val1)
                return false;
            if (val > val2)
                return false;
            if (val == val1)
                return includeLowerValue;
            return val != val2 || includeUpperValue;
        }
        #endregion

        #region Calendar
        /// <summary>
        /// Takes the month number and converts it to the name of the month
        /// </summary>
        /// <param name="val">The value of the month (1-12)</param>
        /// <param name="useAbbreviation">If true (default), this will return the abbreviated month name instead of the full month</param>
        /// <returns>The month name</returns>
        public static string ToMonthName(this int val, bool useAbbreviation = true)
        {
            if (val < 1 || val > 12)
                throw new Exception($"The value: {val} can not be converted to a month");
            var info = CultureInfo.CurrentCulture.DateTimeFormat;
            return useAbbreviation ? info.GetAbbreviatedMonthName(val) : info.GetMonthName(val);
        }

        /// <summary>
        /// Takes the quarter number and converts it to the name of the quarter
        /// </summary>
        /// <param name="val">The value of the quarter (1-4)</param>
        /// <returns>The quarter name</returns>
        public static string ToQuarterName(this int val)
        {
            if (val < 1 || val > 4)
                throw new Exception($"The value: {val} can not be converted to a quarter");
            return $"Q{val}";
        }

        /// <summary>
        /// Takes the number corresponding to a month and year and converts it to the name
        /// </summary>
        /// <param name="val">The value of the month and year. Format is YYYYMM</param>
        /// <returns>The month and year name</returns>
	    public static string ToMonthYearName(this int val)
        {
            if (val < 100000 || val > 999999)
                throw new Exception($"The value: {val} can not be converted to a month and year");
            return $"{(val % 100).ToMonthName()} {val / 100}";
        }

        /// <summary>
        /// Takes the number corresponding to a quarter and year and converts it to the name
        /// </summary>
        /// <param name="val">The value of the quarter and year. Format is YYYYQQ</param>
        /// <returns>The quarter and year name</returns>
        public static string ToQuarterYearName(this int val)
        {
            if (val < 100000 || val > 999999)
                throw new Exception($"The value: {val} can not be converted to a quarter and year");
            return $"{(val % 100).ToQuarterName()} {val / 100}";
        }
        #endregion
    }
}
