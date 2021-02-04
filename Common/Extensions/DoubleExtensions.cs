using System;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Any Double extension methods
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Determines if 2 doubles are effectively equal
        /// </summary>
        /// <param name="val1">The first value to compare</param>
        /// <param name="val2">The 2nd value to compare</param>
        /// <returns>True if they are effectively equal, false otherwise</returns>
        public static bool IsEqual(this double val1, double val2)
        {
            return Math.Abs(val1 - val2) < double.Epsilon;
        }

        /// <summary>
        /// Determines if a double is effectively less than another value
        /// </summary>
        /// <param name="val1">The first value to compare</param>
        /// <param name="val2">The 2nd value to compare</param>
        /// <returns>True if the first value is less than the 2nd, false otherwise</returns>
        public static bool IsLessThan(this double val1, double val2)
        {
            if (val1.IsEqual(val2))
                return false;
            return val1 < val2;
        }

        /// <summary>
        /// Determines if a double is effectively less than or equal to another value
        /// </summary>
        /// <param name="val1">The first value to compare</param>
        /// <param name="val2">The 2nd value to compare</param>
        /// <returns>True if the first value is less than or equal to the 2nd, false otherwise</returns>
        public static bool IsLessThanOrEqual(this double val1, double val2)
        {
            if (val1.IsEqual(val2))
                return true;
            return val1 < val2;
        }

        /// <summary>
        /// Determines if a double is effectively more than another value
        /// </summary>
        /// <param name="val1">The first value to compare</param>
        /// <param name="val2">The 2nd value to compare</param>
        /// <returns>True if the first value is more than the 2nd, false otherwise</returns>
        public static bool IsMoreThan(this double val1, double val2)
        {
            if (val1.IsEqual(val2))
                return false;
            return val1 > val2;
        }

        /// <summary>
        /// Determines if a double is effectively more than or equal to another value
        /// </summary>
        /// <param name="val1">The first value to compare</param>
        /// <param name="val2">The 2nd value to compare</param>
        /// <returns>True if the first value is more than or equal to the 2nd, false otherwise</returns>
        public static bool IsMoreThanOrEqual(this double val1, double val2)
        {
            if (val1.IsEqual(val2))
                return true;
            return val1 > val2;
        }

        /// <summary>
        /// Determines if a double is between other values
        /// </summary>
        /// <param name="val">The value being compared</param>
        /// <param name="val1">The lower value to compare</param>
        /// <param name="val2">The upper value to compare</param>
        /// <param name="includeLowerValue">Specifies whether the check will include a value equal to val1 (lower)</param>
        /// <param name="includeUpperValue">Specifies whether the check will include a value equal to val2 (upper)</param>
        /// <returns>True if val falls between val1 and val2. False otherwise</returns>
        public static bool Between(this double val, double val1, double val2, bool includeLowerValue = true, bool includeUpperValue = true)
        {
            if (val.IsEqual(val1))
                return includeLowerValue;
            if (val.IsEqual(val2))
                return includeUpperValue;
            if (val < val1)
                return false;
            return !(val > val2);
        }

        /// <summary>
        /// Returns the number to the left of the decimal point
        /// </summary>
        /// <param name="val">The value being used</param>
        /// <returns>The integer to the left of the decimal point</returns>
        public static long IntegerPart(this double val)
        {
            if (val > long.MaxValue)
                throw new Exception($"The value ({val}) is too large to get the integer part");
            if (val < long.MinValue)
                throw new Exception($"The negative value ({val}) is too large to get the integer part");
            return Convert.ToInt64(val.IsLessThan(0.0) ? Math.Ceiling(val) : Math.Floor(val));
        }
    }
}
