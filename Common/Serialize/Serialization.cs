using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Serialize
{
    /// <summary>
    /// Handles serialization (and deserialization) of an object.
    /// This effectively is a wrapper around JsonConvert methods.
    /// But also allows for XML serialization
    /// </summary>
    /// <remarks>
    /// There is currently no exception handling for these methods.
    /// Consider wrapping these calls in SafeTry.IgnoreException method
    /// </remarks>
    public static class Serialization
    {
        #region Helpers
        /// <summary>
        /// For XML, get a singleton instance of the serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static class XmlSerializerGeneric<T>
        {
            internal static readonly XmlSerializer Serializer = new XmlSerializer(typeof(T));
        }

        #endregion

        #region Serialize
        /// <summary>
        /// Serializes an object to JSON
        /// </summary>
        /// <param name="item">The item being serialized</param>
        /// <param name="jsonSettings">The json settings to use</param>
        /// <returns>The Json representation of the object</returns>
        public static string SerializeJson<T>(this T item, JsonSerializerSettings jsonSettings = null)
            => JsonConvert.SerializeObject(item, jsonSettings ?? SerializationSettings.Default);

        /// <summary>
        /// Serializes an object to XML
        /// </summary>
        /// <param name="item">The item being serialized</param>
        /// <param name="serializer">The XML Serializer to use</param>
        /// <returns>The XML representation of the object</returns>
        public static string SerializeXml<T>(this T item, XmlSerializer serializer = null)
        {
            serializer ??= XmlSerializerGeneric<T>.Serializer;
            using var writer = new StringWriter();
            serializer.Serialize(writer, item);
            return writer.ToString();
        }
        #endregion

        #region Deserialize
        /// <summary>
        /// Deserializes an object from JSON
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="json">The json to convert</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The converted object</returns>
        public static T DeserializeJson<T>(this string json, JsonSerializerSettings jsonSettings = null)
            => JsonConvert.DeserializeObject<T>(json, jsonSettings ?? SerializationSettings.Default);

        /// <summary>
        /// Deserializes an object from XML
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="xml">The xml to convert</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The converted object</returns>
        public static T DeserializeXml<T>(this string xml, XmlSerializer serializer = null)
        {
            serializer ??= XmlSerializerGeneric<T>.Serializer;
            using var reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader);
        }

        /// <summary>
        /// Deserializes the string and does a convert to the type
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized/converted</typeparam>
        /// <param name="xml">The xml to deserialize</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>An object that is 'converted' to the type</returns>
        public static object DeserializeXmlAs<T>(this string xml, XmlSerializer serializer = null)
            => Convert.ChangeType(xml.DeserializeXml<T>(serializer), typeof(T));
        #endregion
    }
}
