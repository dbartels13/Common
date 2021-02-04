using System;
using System.Drawing;
using System.Linq;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// String custom methods which are used to convert the string to another type
    /// </summary>
    public static class StringConvertExtensions
    {
        #region ParseInt (similar to javascript)
        /// <summary>
        /// Converts a string value into an integer, using ParseInt function similar to Javascript (eg. 12ab3 = 12)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted integer value</returns>
        public static int ToIntParsed(this string value, string name) => value.ToIntParsed(name, 0, true);
        /// <summary>
        /// Converts a string value into an integer, using ParseInt function similar to Javascript (eg. 12ab3 = 12)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted integer value (or defaultValue)</returns>
        public static int ToIntParsed(this string value, int defaultValue) => value.ToIntParsed("", defaultValue, false);
        /// <summary>
        /// Converts a string value into an integer, using ParseInt function similar to Javascript (eg. 12ab3 = 12)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails</param>
        /// <returns>The converted integer value (or defaultValue)</returns>
        private static int ToIntParsed(this string value, string name, int defaultValue, bool throwIfBad)
        {
            if (string.IsNullOrWhiteSpace(value))
                return IntParsedError(name, value ?? "NULL", defaultValue, throwIfBad);

            return SafeTry.OnException(
                () =>
                {
                    var intStr = string.Concat(value.TakeWhile(char.IsDigit));
                    return intStr.ToInt(name, defaultValue, throwIfBad);
                },
                ex => IntParsedError(name, value, defaultValue, throwIfBad)
            );
        }
        private static int IntParsedError(string name, string value, int defaultValue, bool throwIfBad)
        {
            if (throwIfBad)
                throw new Exception($"Failed to parse '{name}' ({value}) to an integer");
            return defaultValue;
        }
        #endregion

        #region Int
        /// <summary>
        /// Converts a string value into an integer
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted integer value</returns>
        public static int ToInt(this string value, string name) => value.ToInt(name, 0, true);
        /// <summary>
        /// Converts a string value into an integer
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted integer value (or defaultValue)</returns>
        public static int ToInt(this string value, int defaultValue) => value.ToInt("", defaultValue, false);
        /// <summary>
        /// Converts a string value into an integer
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted integer value (or defaultValue)</returns>
        private static int ToInt(this string value, string name, int defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableInt();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to an integer");
            return defaultValue;
        }
        #endregion

        #region Nullable Int
        /// <summary>
        /// Converts a string value into a nullable integer
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted integer value</returns>
        public static int? ToNullableInt(this string value, string name) => value.ToNullableInt(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable integer
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted integer value (or defaultValue)</returns>
        public static int? ToNullableInt(this string value, int? defaultValue = null) => value.ToNullableInt("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable integer
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted nullable integer value (or defaultValue)</returns>
        private static int? ToNullableInt(this string value, string name, int? defaultValue, bool throwIfBad)
        {
            if (int.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable integer");
            return defaultValue;
        }
        #endregion

        #region Short
        /// <summary>
        /// Converts a string value into a short
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted short value</returns>
        public static short ToShort(this string value, string name) => value.ToShort(name, 0, true);
        /// <summary>
        /// Converts a string value into a short
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted short value (or defaultValue)</returns>
        public static short ToShort(this string value, short defaultValue) => value.ToShort("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a short
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted short value (or defaultValue)</returns>
        private static short ToShort(this string value, string name, short defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableShort();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a short");
            return defaultValue;
        }
        #endregion

        #region Nullable Short
        /// <summary>
        /// Converts a string value into a nullable short
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted short value</returns>
        public static short? ToNullableShort(this string value, string name) => value.ToNullableShort(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable short
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted short value (or defaultValue)</returns>
        public static short? ToNullableShort(this string value, short? defaultValue = null) => value.ToNullableShort("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable short
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted nullable short value (or defaultValue)</returns>
        private static short? ToNullableShort(this string value, string name, short? defaultValue, bool throwIfBad)
        {
            if (short.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable short");
            return defaultValue;
        }
        #endregion

        #region Long
        /// <summary>
        /// Converts a string value into a long
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted long value</returns>
        public static long ToLong(this string value, string name) => value.ToLong(name, 0, true);
        /// <summary>
        /// Converts a string value into a long
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted long value (or defaultValue)</returns>
        public static long ToLong(this string value, long defaultValue) => value.ToLong("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a long
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted long value (or defaultValue)</returns>
        private static long ToLong(this string value, string name, long defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableLong();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a long");
            return defaultValue;
        }
        #endregion

        #region Nullable Long
        /// <summary>
        /// Converts a string value into a nullable long
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted long value</returns>
        public static long? ToNullableLong(this string value, string name) => value.ToNullableLong(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable long
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted long value (or defaultValue)</returns>
        public static long? ToNullableLong(this string value, long? defaultValue = null) => value.ToNullableLong("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable long
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted nullable long value (or defaultValue)</returns>
        private static long? ToNullableLong(this string value, string name, long? defaultValue, bool throwIfBad)
        {
            if (long.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable long");
            return defaultValue;
        }
        #endregion

        #region Unsigned Long
        /// <summary>
        /// Converts a string value into a ulong
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted ulong value</returns>
        public static ulong ToULong(this string value, string name) => value.ToULong(name, 0, true);
        /// <summary>
        /// Converts a string value into a ulong
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted ulong value (or defaultValue)</returns>
        public static ulong ToULong(this string value, ulong defaultValue) => value.ToULong("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a ulong
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted ulong value (or defaultValue)</returns>
        private static ulong ToULong(this string value, string name, ulong defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableULong();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to an unsigned long");
            return defaultValue;
        }
        #endregion

        #region Nullable Unsigned Long
        /// <summary>
        /// Converts a string value into a nullable ulong
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted ulong value</returns>
        public static ulong? ToNullableULong(this string value, string name) => value.ToNullableULong(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable ulong
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted ulong value (or defaultValue)</returns>
        public static ulong? ToNullableULong(this string value, ulong? defaultValue = null) => value.ToNullableULong("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable ulong
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted nullable ulong value (or defaultValue)</returns>
        private static ulong? ToNullableULong(this string value, string name, ulong? defaultValue, bool throwIfBad)
        {
            if (ulong.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable unsigned long");
            return defaultValue;
        }
        #endregion

        #region Double
        /// <summary>
        /// Converts a string value into a double
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted double value</returns>
        public static double ToDouble(this string value, string name) => value.ToDouble(name, 0, true);
        /// <summary>
        /// Converts a string value into a double
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted double value (or defaultValue)</returns>
        public static double ToDouble(this string value, double defaultValue) => value.ToDouble("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a double
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted double value (or defaultValue)</returns>
        private static double ToDouble(this string value, string name, double defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableDouble();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a double");
            return defaultValue;
        }
        #endregion

        #region Nullable Double
        /// <summary>
        /// Converts a string value into a nullable double
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted double value</returns>
        public static double? ToNullableDouble(this string value, string name) => value.ToNullableDouble(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable double
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted double value (or defaultValue)</returns>
        public static double? ToNullableDouble(this string value, double? defaultValue = null) => value.ToNullableDouble("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable double
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted double value (or defaultValue)</returns>
        private static double? ToNullableDouble(this string value, string name, double? defaultValue, bool throwIfBad)
        {
            if (double.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable double");
            return defaultValue;
        }
        #endregion

        #region Decimal
        /// <summary>
        /// Converts a string value into a decimal
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted double value</returns>
        public static decimal ToDecimal(this string value, string name) => value.ToDecimal(name, 0, true);
        /// <summary>
        /// Converts a string value into a decimal
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted decimal value (or defaultValue)</returns>
        public static decimal ToDecimal(this string value, decimal defaultValue) => value.ToDecimal("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a decimal
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted decimal value (or defaultValue)</returns>
        private static decimal ToDecimal(this string value, string name, decimal defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableDecimal();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a decimal");
            return defaultValue;
        }
        #endregion

        #region Nullable Decimal
        /// <summary>
        /// Converts a string value into a nullable decimal
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted decimal value</returns>
        public static decimal? ToNullableDecimal(this string value, string name) => value.ToNullableDecimal(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable decimal
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted decimal value (or defaultValue)</returns>
        public static decimal? ToNullableDecimal(this string value, decimal? defaultValue = null) => value.ToNullableDecimal("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable decimal
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted decimal value (or defaultValue)</returns>
        private static decimal? ToNullableDecimal(this string value, string name, decimal? defaultValue, bool throwIfBad)
        {
            if (decimal.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable decimal");
            return defaultValue;
        }
        #endregion

        #region Float
        /// <summary>
        /// Converts a string value into a float
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted double value</returns>
        public static float ToFloat(this string value, string name) => value.ToFloat(name, 0, true);
        /// <summary>
        /// Converts a string value into a float
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted float value (or defaultValue)</returns>
        public static float ToFloat(this string value, float defaultValue) => value.ToFloat("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a float
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted float value (or defaultValue)</returns>
        private static float ToFloat(this string value, string name, float defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableFloat();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a float");
            return defaultValue;
        }
        #endregion

        #region Nullable Float
        /// <summary>
        /// Converts a string value into a nullable float
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted float value</returns>
        public static float? ToNullableFloat(this string value, string name) => value.ToNullableFloat(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable float
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted float value (or defaultValue)</returns>
        public static float? ToNullableFloat(this string value, float? defaultValue = null) => value.ToNullableFloat("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable float
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted float value (or defaultValue)</returns>
        private static float? ToNullableFloat(this string value, string name, float? defaultValue, bool throwIfBad)
        {
            if (float.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable float");
            return defaultValue;
        }
        #endregion

        #region Bool
        /// <summary>
        /// Converts a string into a bool
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted true/false</returns>
        public static bool ToBool(this string value, string name) => value.ToBool(name, false, true);
        /// <summary>
        /// Converts a string into a bool
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted true/false (or defaultValue)</returns>
        public static bool ToBool(this string value, bool defaultValue) => value.ToBool("", defaultValue, false);
        /// <summary>
        /// Converts a string into a bool
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted true/false (or defaultValue)</returns>
        private static bool ToBool(this string value, string name, bool defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableBool();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a bool");
            return defaultValue;
        }
        #endregion

        #region Nullable Bool
        /// <summary>
        /// Converts a string into a nullable bool
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted true/false</returns>
        public static bool? ToNullableBool(this string value, string name) => value.ToNullableBool(name, false, true);
        /// <summary>
        /// Converts a string into a nullable bool
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted true/false (or defaultValue)</returns>
        public static bool? ToNullableBool(this string value, bool? defaultValue = null) => value.ToNullableBool("", defaultValue, false);
        /// <summary>
        /// Converts a string into a nullable bool
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted true/false (or defaultValue)</returns>
        private static bool? ToNullableBool(this string value, string name, bool? defaultValue, bool throwIfBad)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BoolError(name, value ?? "NULL", defaultValue, throwIfBad);

            var upperValue = value.ToUpper();
            return upperValue switch
            {
                "TRUE" => true,
                "T" => true,
                "YES" => true,
                "Y" => true,
                "ON" => true,
                "1" => true,
                "CHECK" => true,
                "CHECKED" => true,
                "FALSE" => false,
                "F" => false,
                "NO" => false,
                "N" => false,
                "OFF" => false,
                "0" => false,
                "UNCHECK" => false,
                "UNCHECKED" => false,
                _ => BoolError(name, value, defaultValue, throwIfBad)
            };
        }
        private static bool? BoolError(string name, string value, bool? defaultValue, bool throwIfBad)
        {
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value}) to a nullable bool");
            return defaultValue;
        }
        #endregion

        #region DateTime
        /// <summary>
        /// Converts a string value into a datetime
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted datetime value</returns>
        public static DateTime ToDateTime(this string value, string name) => value.ToDateTime(name, DateTime.Now, true);
        /// <summary>
        /// Converts a string value into a datetime
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted datetime value (or defaultValue)</returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue) => value.ToDateTime("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a datetime
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted datetime value (or defaultValue)</returns>
        private static DateTime ToDateTime(this string value, string name, DateTime defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableDateTime();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a date and time");
            return defaultValue;
        }
        #endregion

        #region Nullable DateTime
        /// <summary>
        /// Converts a string value into a nullable datetime
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted datetime value</returns>
        public static DateTime? ToNullableDateTime(this string value, string name) => value.ToNullableDateTime(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable datetime
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted datetime value (or defaultValue)</returns>
        public static DateTime? ToNullableDateTime(this string value, DateTime? defaultValue = null) => value.ToNullableDateTime("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable datetime
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted datetime value (or defaultValue)</returns>
        private static DateTime? ToNullableDateTime(this string value, string name, DateTime? defaultValue, bool throwIfBad)
        {
            if (DateTime.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable date and time");
            return defaultValue;
        }
        #endregion

        #region Char
        /// <summary>
        /// Converts a string value into a char
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted char value</returns>
        public static char ToChar(this string value, string name) => value.ToChar(name, '\0', true);
        /// <summary>
        /// Converts a string value into a char
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted char value (or defaultValue)</returns>
        public static char ToChar(this string value, char defaultValue) => value.ToChar("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a char
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted char value (or defaultValue)</returns>
        private static char ToChar(this string value, string name, char defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableChar();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a char");
            return defaultValue;
        }
        #endregion

        #region Nullable Char
        /// <summary>
        /// Converts a string value into a nullable char
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted char value</returns>
        public static char? ToNullableChar(this string value, string name) => value.ToNullableChar(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable char
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted char value (or defaultValue)</returns>
        public static char? ToNullableChar(this string value, char? defaultValue = null) => value.ToNullableChar("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a char
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted char value (or defaultValue)</returns>
        private static char? ToNullableChar(this string value, string name, char? defaultValue, bool throwIfBad)
        {
            if (value == null)
                return CharError(name, "NULL", defaultValue, throwIfBad);
            return value.Length == 1 ? value[0] : CharError(name, value, defaultValue, throwIfBad);
        }
        private static char? CharError(string name, string value, char? defaultValue, bool throwIfBad)
        {
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value}) to a nullable char");
            return defaultValue;
        }
        #endregion

        #region Guid
        /// <summary>
        /// Converts a string value into a Guid
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted Guid value</returns>
        public static Guid ToGuid(this string value, string name) => value.ToGuid(name, Guid.Empty, true);
        /// <summary>
        /// Converts a string value into a Guid
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted Guid value (or defaultValue)</returns>
        public static Guid ToGuid(this string value, Guid defaultValue) => value.ToGuid("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a Guid
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted Guid value (or defaultValue)</returns>
        private static Guid ToGuid(this string value, string name, Guid defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableGuid();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a guid");
            return defaultValue;
        }
        #endregion

        #region Nullable Guid
        /// <summary>
        /// Converts a string value into a nullable Guid
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error, a nice exception will be thrown)</param>
        /// <returns>The converted Guid value</returns>
        public static Guid? ToNullableGuid(this string value, string name) => value.ToNullableGuid(name, null, true);
        /// <summary>
        /// Converts a string value into a nullable Guid
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <returns>The converted Guid value (or defaultValue)</returns>
        public static Guid? ToNullableGuid(this string value, Guid? defaultValue = null) => value.ToNullableGuid("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a nullable Guid
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the 'value' is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted Guid value (or defaultValue)</returns>
        private static Guid? ToNullableGuid(this string value, string name, Guid? defaultValue, bool throwIfBad)
        {
            if (Guid.TryParse(value, out var myVal))
                return myVal;
            if (throwIfBad)
                throw new Exception($"Failed to convert '{name}' ({value ?? "NULL"}) to a nullable guid");
            return defaultValue;
        }
        #endregion

        #region Enum
        /// <summary>
        /// Casts a string to an enum
        /// </summary>
        /// <typeparam name="T">An enumeration</typeparam>
        /// <param name="str">The value of the enumeration to check</param>
        /// <returns>The enumeration value</returns>
        public static T ToEnum<T>(this string str) where T : struct, IConvertible
        {
            if (!Enum.TryParse(str, true, out T myEnum))
                throw new Exception($"{str} is not a {typeof(T)}");
            return myEnum;
        }

        /// <summary>
        /// Casts a string to an enum
        /// </summary>
        /// <typeparam name="T">An enumeration</typeparam>
        /// <param name="str">The value of the enumeration to check</param>
        /// <param name="defaultValue">If can not find a match, this will be returned</param>
        /// <returns>The enumeration value</returns>
        public static T ToEnum<T>(this string str, T defaultValue) where T : struct, IConvertible
            => !Enum.TryParse(str, true, out T myEnum) ? defaultValue : myEnum;
        #endregion

        #region Color
        /// <summary>
        /// Converts a string value into a Color
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <returns>The converted Color</returns>
        public static Color ToColor(this string value, string name) => value.ToColor(name, default, true);
        /// <summary>
        /// Converts a string value into a Color
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the value is somehow bad, this will be returned instead</param>
        /// <returns>The converted Color (or defaultValue)</returns>
        public static Color ToColor(this string value, Color defaultValue) => value.ToColor("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a Color
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the value is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted Color (or defaultValue)</returns>
        private static Color ToColor(this string value, string name, Color defaultValue, bool throwIfBad)
        {
            var myVal = value.ToNullableColor();
            if (myVal.HasValue)
                return myVal.Value;
            if (throwIfBad)
                throw new Exception("Failed to convert '" + name + "' (" + (value ?? "NULL") + ") to a Color");
            return defaultValue;
        }
        #endregion

        #region Nullable Color
        /// <summary>
        /// Converts a string value into a Color
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <returns>The converted Color</returns>
        public static Color? ToNullableColor(this string value, string name) => value.ToNullableColor(name, null, true);
        /// <summary>
        /// Converts a string value into a Color
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="defaultValue">If the value is somehow bad, this will be returned instead</param>
        /// <returns>The converted Color (or defaultValue)</returns>
        public static Color? ToNullableColor(this string value, Color? defaultValue = null) => value.ToNullableColor("", defaultValue, false);
        /// <summary>
        /// Converts a string value into a Color
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="name">The name of the thing being converted (in case of error)</param>
        /// <param name="defaultValue">If the value is somehow bad, this will be returned instead</param>
        /// <param name="throwIfBad">If true, A nice exception will be thrown if the conversion fails. If false, returns defaultValue</param>
        /// <returns>The converted Color (or defaultValue)</returns>
        private static Color? ToNullableColor(this string value, string name, Color? defaultValue, bool throwIfBad)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ColorError(name, value ?? "NULL", defaultValue, throwIfBad);

            return SafeTry.OnException(
                () => ColorTranslator.FromHtml(value),
                ex => ColorError(name, value, defaultValue, throwIfBad)
            );
        }
        private static Color? ColorError(string name, string value, Color? defaultValue, bool throwIfBad)
        {
            if (throwIfBad)
                throw new Exception("Failed to convert '" + name + "' (" + value + ") to a nullable Color");
            return defaultValue;
        }
        #endregion
    }
}
