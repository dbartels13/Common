using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Extensions
{
    public static class NameValueCollectionExtensions
    {
        #region From NVC to string
        /// <summary>
        /// Converts a NameValueCollection into a human readable string representation
        /// </summary>
        /// <param name="nvc">The NameValueCollection</param>
        /// <param name="hideKeys">If there is any values that need to be hidden</param>
        /// <returns>Human readable string</returns>
        public static string AsString(this NameValueCollection nvc, CaseInsensitiveBinaryList<string> hideKeys) => nvc.AsString(null, hideKeys);

        /// <summary>
        /// Converts a NameValueCollection into a human readable string representation
        /// </summary>
        /// <param name="nvc">The NameValueCollection</param>
        /// <param name="name">The name of the collection</param>
        /// <param name="hideKeys">If there is any values that need to be hidden</param>
        /// <returns>Human readable string</returns>
        public static string AsString(this NameValueCollection nvc, string name = null,
            CaseInsensitiveBinaryList<string> hideKeys = null)
            => SafeTry.IgnoreException(() =>
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    sb.Append(name);
                    sb.AppendLine(": ");
                }

                if (nvc?.IsDefault() ?? true)
                    return sb.ToString();

                if (hideKeys == null)
                    hideKeys = new List<string>().ToCaseInsensitiveBinaryList();

                foreach (var key in nvc.AllKeys)
                {
                    sb.Append(key);
                    sb.Append(" = ");
                    if (hideKeys.Has(key))
                        sb.Append("*****,");
                    else
                        sb.Append(nvc[key] + ",");
                    sb.AppendLine();
                }

                return sb.ToString();
            }, $"Unable to obtain {name ?? "Unknown"}"
            );
        #endregion

        #region To NVC
        public static NameValueCollection DefaultNvc { get; } = new NameValueCollection { { "Error", "Unknown" } };

        public static NameValueCollection ToNameValueCollection(this object o, JsonSerializerSettings jsonSettings = null)
        {
            return SafeTry.IgnoreException(() =>
            {
                var nvc = new NameValueCollection();
                if (o == null)
                    return nvc;

                if (!(o is string s))
                    return ToNameValueCollection(o.SerializeJson(jsonSettings), jsonSettings);

                nvc.Add("String Item", s);
                return nvc;
            }, DefaultNvc);
        }
        public static NameValueCollection ToNameValueCollection(this IDictionary dict, JsonSerializerSettings jsonSettings = null)
        {
            return SafeTry.IgnoreException(() =>
            {
                var nvc = new NameValueCollection();
                if (dict == null)
                    return nvc;

                foreach (var key in dict.Keys)
                {
                    var o = dict[key];
                    var s = o as string;
                    nvc.Add(key.ToString(), s ?? o.SerializeJson(jsonSettings));
                }
                return nvc;
            }, DefaultNvc);
        }
        public static NameValueCollection ToNameValueCollection(this string json, JsonSerializerSettings jsonSettings = null)
        {
            return SafeTry.IgnoreException(() =>
            {
                var nvc = new NameValueCollection();
                if (string.IsNullOrWhiteSpace(json))
                    return nvc;

                // If this is a basic object, attempt that
                var dict = SafeTry.IgnoreException(() => json.DeserializeJson<Dictionary<string, object>>(jsonSettings));
                if (dict.IsPopulated())
                    return ToNameValueCollection(dict, jsonSettings);

                // The JSON could be a collection, but I'm not going to break it down any further
                // Only benefit to breaking this down is to allow more granular ignoreData

                // Don't know, so just log the raw json
                nvc.Add("Raw", json);
                return nvc;
            }, DefaultNvc);
        }
        #endregion
    }
}
