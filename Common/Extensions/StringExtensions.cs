using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// String custom methods
    /// </summary>
    public static class StringExtensions
    {
        #region Operator pseudo-overloads

        /// <summary>
        /// Performs check against any 2 strings (accounts for null, and trims extra whitespace)
        /// </summary>
        /// <param name="s1">First string</param>
        /// <param name="s2">Second string</param>
        /// <param name="comparison">Optional: Default = CurrentCultureIgnoreCase</param>
        /// <returns>True if they are the same, false if there is a real difference</returns>
        public static bool IsEqual(this string s1, string s2,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => s1.Trimmed().Equals(s2.Trimmed(), comparison);

        /// <summary>
        /// &lt; operator
        /// </summary>
        /// <param name="left">The left value</param>
        /// <param name="right">The right value</param>
        /// <param name="comparison">Optional: Default = CurrentCultureIgnoreCase</param>
        /// <returns>True if the left value is less than the right value (based on comparison/sorting)</returns>
        public static bool IsLessThan(this string left, string right,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => string.Compare(left, right, comparison) < 0;

        /// <summary>
        /// &lt;= operator
        /// </summary>
        /// <param name="left">The left value</param>
        /// <param name="right">The right value</param>
        /// <param name="comparison">Optional: Default = CurrentCultureIgnoreCase</param>
        /// <returns>True if the left value is less than or equal to the right value (based on comparison/sorting)</returns>
        public static bool IsLessThanOrEqual(this string left, string right,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => string.Compare(left, right, comparison) <= 0;

        /// <summary>
        /// &gt; operator
        /// </summary>
        /// <param name="left">The left value</param>
        /// <param name="right">The right value</param>
        /// <param name="comparison">Optional: Default = CurrentCultureIgnoreCase</param>
        /// <returns>True if the left value is less than the right value (based on comparison/sorting)</returns>
        public static bool IsMoreThan(this string left, string right,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => string.Compare(left, right, comparison) > 0;

        /// <summary>
        /// &gt;= operator
        /// </summary>
        /// <param name="left">The left value</param>
        /// <param name="right">The right value</param>
        /// <param name="comparison">Optional: Default = CurrentCultureIgnoreCase</param>
        /// <returns>True if the left value is less than the right value (based on comparison/sorting)</returns>
        public static bool IsMoreThanOrEqual(this string left, string right,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => string.Compare(left, right, comparison) >= 0;

        /// <summary>
        /// Performs a "like" operation (contains, but case insensitive)
        /// </summary>
        /// <param name="str">The original string</param>
        /// <param name="contains">The string to check if the original string contains it</param>
        /// <param name="comp">The comparison to use (Default = CurrentCultureIgnoreCase)</param>
        /// <returns>True if it contains it, false otherwise</returns>
        public static bool Like(this string str, string contains,
            StringComparison comp = StringComparison.CurrentCultureIgnoreCase)
        {
            if (str == null || contains == null)
                return false;
            return str.IndexOf(contains, comp) >= 0;
        }

        #endregion

        #region Trims
        /// <summary>
        /// Removes everything after the given delimiter
        /// </summary>
        /// <param name="s">The string being manipulated</param>
        /// <param name="delimiter">Where to split apart the string</param>
        /// <param name="keepDelimiter">If you should keep the delimiter, or remove it</param>
        /// <returns>The modified string</returns>
        public static string RemoveAfter(this string s, char delimiter, bool keepDelimiter = true)
        {
            if (s == null)
                return null;
            var idx = s.IndexOf(delimiter);
            return idx >= 0 ? s.Substring(0, idx + (keepDelimiter ? 1 : 0)) : s;
        }

        /// <summary>
        /// Removes everything before the given delimiter
        /// </summary>
        /// <param name="s">The string being manipulated</param>
        /// <param name="delimiter">Where to split apart the string</param>
        /// <param name="keepDelimiter">If you should keep the delimiter, or remove it</param>
        /// <returns>The modified string</returns>
        public static string RemoveBefore(this string s, char delimiter, bool keepDelimiter = true)
        {
            if (s == null)
                return null;
            var idx = s.IndexOf(delimiter);
            return idx >= 0 ? s.Substring(idx - (keepDelimiter ? 0 : 1)) : s;
        }

        /// <summary>
        /// Keeps everything before the given delimiter
        /// </summary>
        /// <param name="s">The string being manipulated</param>
        /// <param name="delimiter">Where to split apart the string</param>
        /// <returns>Everything before the delimiter</returns>
        public static string KeepBefore(this string s, char delimiter) => s.RemoveAfter(delimiter, false);

        /// <summary>
        /// Keeps everything after the given delimiter
        /// </summary>
        /// <param name="s">The string being manipulated</param>
        /// <param name="delimiter">Where to split apart the string</param>
        /// <returns>Everything after the delimiter</returns>
        public static string KeepAfter(this string s, char delimiter) => s.RemoveBefore(delimiter, false);

        /// <summary>
        /// Obtains a usable string from a post/request/database, etc (will be not null and trimmed - possibly "")
        /// </summary>
        /// <param name="s">The string being checked</param>
        /// <returns>The not null/trimmed string</returns>
        public static string Trimmed(this string s) => (s ?? "").Trim();

        /// <summary>
        /// This removes all special characters from the string
        /// </summary>
        /// <param name="str">The original string</param>
        /// <returns>The string without special characters</returns>
        public static string RemoveSpecialChars(this string str)
            => str == null
                ? null
                : Regex.Replace(str, @"[^\u0020-\u007E]", string.Empty);
        #endregion

        #region Shorten
        /// <summary>
        /// Shortens a string to the desired length
        /// </summary>
        /// <param name="s">The string being manipulated</param>
        /// <param name="length">The desired length</param>
        /// <returns>The possibly shortened string</returns>
        public static string Shorten(this string s, int length)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;

            s = s.Trim();
            return s.Length > length ? s.Substring(0, length) : s;
        }

        public static string ShortenWithEllipses(this string s, int length)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;

            s = s.Trim();
            return s.Length > length ? $"{s.Substring(0, length - 3)}..." : s;
        }
        #endregion

        #region Split (null-safe, and actual removal of empty/whitespace chars)

        /// <summary>
        /// Splits a string on another string (same as a character split)
        /// </summary>
        /// <param name="s">The string to split</param>
        /// <param name="delimiter">The string which will delimit the original string</param>
        /// <param name="option">How to split the string (Default no options)</param>
        /// <returns>Listing of all strings split by the delimiter</returns>
        public static IEnumerable<string> SafeSplit(this string s, string delimiter,
            StringSplitOptions option = StringSplitOptions.None)
            => s.SafeSplit(delimiter, option, false);

        /// <summary>
        /// Splits a string on another string (same as a character split)
        /// </summary>
        /// <param name="s">The string to split</param>
        /// <param name="delimiter">The string which will delimit the original string</param>
        /// <param name="removeEmpties">If empty elements (IsNullOrWhiteSpace) will be removed from the list (default false)</param>
        /// <returns>Listing of all strings split by the delimiter</returns>
        public static IEnumerable<string> SafeSplit(this string s, string delimiter, bool removeEmpties)
            => s.SafeSplit(delimiter, StringSplitOptions.None, removeEmpties);

        /// <summary>
        /// Splits a string on another string (same as a character split)
        /// </summary>
        /// <param name="s">The string to split</param>
        /// <param name="delimiter">The string which will delimit the original string</param>
        /// <param name="option">How to split the string (Default no options)</param>
        /// <param name="removeEmpties">If empty elements (IsNullOrWhiteSpace) will be removed from the list (default false)</param>
        /// <returns>Listing of all strings split by the delimiter</returns>
        public static IEnumerable<string> SafeSplit(this string s, string delimiter, StringSplitOptions option,
            bool removeEmpties)
        {
            if (s == null)
                return new string[0];
            var items = s.Split(delimiter, option);
            return removeEmpties ? items.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()) : items;
        }

        #endregion

        #region Reverse
        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="str">The original string</param>
        /// <returns>A new string that is reversed</returns>
        public static string Reverse(this string str)
        {
            var a = str.ToCharArray();
            Array.Reverse(a);
            return new string(a);
        }
        #endregion

        #region English Language Manipulations
        /// <summary>
        /// Pluralizes a given word (not fully implemented properly, but works)
        /// </summary>
        /// <param name="s">The word to be pluralized</param>
        /// <returns>The pluralized word</returns>
        public static string Pluralize(this string s) => $"{s}{(s.EndsWith("s") ? "es" : "s")}";

        /// <summary>
        /// Capitalizes the first letter in the string
        /// </summary>
        /// <param name="str">The string to be capitalized</param>
        /// <returns>Capitalized string</returns>
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            var a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Converts the given string to readable format (spaces in camel casing)
        /// </summary>
        /// <param name="s">The string to work on</param>
        /// <returns>The string with spaces in camel casing</returns>
        public static string SplitCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var words = s.Split(' ');
            var sb = new StringBuilder();
            for (var i = 0; i < words.Length; i++)
            {
                if (i > 0)
                    sb.Append(" ");
                sb.Append(SplitCamelCaseWord(words[i]));
            }
            return sb.ToString();
        }
        private static string SplitCamelCaseWord(string word)
        {
            if (word.Length < 2)
                return word;
            return word[0] + InsertSpace(word) + word.Substring(1).SplitCamelCase();
        }
        private static string InsertSpace(string s)
        {
            var prev = s[0];
            var cur = s[1];
            var next = 'A';
            if (s.Length > 2)
                next = s[2];
            if (char.IsUpper(prev) && char.IsUpper(cur) && char.IsLower(next))
                return " ";
            if (!char.IsUpper(prev) && char.IsUpper(cur))
                return " ";
            if (char.IsLetter(prev) && !char.IsLetter(cur))
                return " ";
            return "";
        }
        #endregion

        #region Convert to html/csv/excel
        /// <summary>
        /// This encodes a user typed string as HTML to be displayed directly onto a Web page
        /// </summary>
        /// <param name="str">The string to encode</param>
        /// <returns>The encoded string</returns>
        public static string ToHtml(this string str)
        {
            var sb = new StringBuilder(str);
            sb.Replace("\n", "<br>");
            sb.Replace("\r", "");
            return sb.ToString();
        }

        /// <summary>
        /// This encodes a string for exporting as a CSV file
        /// </summary>
        /// <param name="str">The string to encode</param>
        /// <returns>The encoded string</returns>
        public static string ToCsv(this string str)
        {
            var sb = new StringBuilder(str);
            sb.Replace("\"", "\"\""); // quote becomes double quote ("")
            sb.Insert(0, "\""); // Wrap the string in quotes. eg. "s"
            sb.Append("\"");
            return sb.ToString();
        }

        /// <summary>
        /// This encodes a string for exporting as an excel file
        /// </summary>
        /// <param name="str">The string to encode</param>
        /// <returns>The encoded string</returns>
        public static string ToExcel(this string str)
        {
            var sb = new StringBuilder("=");
            sb.Append(str.ToCsv());
            return sb.ToString();
        }
        #endregion

        #region Encoding
        /// <summary>
        /// Decodes a string that was encoded using javascript's "encodeURIComponent" function
        /// </summary>
        /// <param name="uri">The string to decode</param>
        /// <returns>The decoded string</returns>
        public static string DecodeUri(this string uri) => Uri.UnescapeDataString(uri);

        /// <summary>
        /// Encodes a string so that it will appear as-is when retrieved from the client via the URI
        /// </summary>
        /// <param name="uri">The string to encode</param>
        /// <returns>The encoded string</returns>
        public static string EncodeUri(this string uri) => Uri.EscapeDataString(uri);
        #endregion
    }
}