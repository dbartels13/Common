using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Serialize
{
    /// <summary>
    /// Different serialization settings
    /// </summary>
    public static class SerializationSettings
    {
        /// <summary>
        /// Default settings are to ignore/repopulate default values and only give type names when necessary
        /// </summary>
        public static JsonSerializerSettings Default
            => new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            };

        /// <summary>
        /// Default settings are to ignore/repopulate default values and only give type names when necessary
        /// </summary>
        public static JsonSerializerSettings DefaultWithTypes
            => new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                TypeNameHandling = TypeNameHandling.Auto
            };

        /// <summary>
        /// Minimal settings are to ignore/repopulate default values ignore and null values and empty lists
        /// </summary>
        public static JsonSerializerSettings Minimal
            => new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new IgnoreEmptyEnumerableResolver()
            };

        /// <summary>
        /// Minimal settings are to ignore/repopulate default values ignore and null values and empty lists, but to include type names when needed
        /// </summary>
        public static JsonSerializerSettings MinimalWithTypes
            => new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new IgnoreEmptyEnumerableResolver()
            };
    }
}