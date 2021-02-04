namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Object custom methods
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks if an object is a default type
        /// </summary>
        /// <typeparam name="T">The type of item you are checking</typeparam>
        /// <param name="value">The item being checked</param>
        /// <remarks>IsDefault is always the opposite of IsPopulated</remarks>
        /// <returns>True if it is the default value, false otherwise</returns>
        public static bool IsDefault<T>(this T value) => value?.Equals(default(T)) ?? true;
        // where T : struct

        /// <summary>
        /// Checks if an object is populated with a non-default value
        /// </summary>
        /// <param name="value">The item being checked</param>
        /// <remarks>IsDefault is always the opposite of IsPopulated</remarks>
        /// <returns>False if it is the default value, true otherwise</returns>
        public static bool IsPopulated<T>(this T value) => !value.IsDefault();
    }
}