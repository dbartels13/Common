using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Paths
{
    /// <summary>
    /// There is already a UriBuilder, so didn't want to reuse that name.
    /// This is a much nicer interface though for truly building out a URI
    /// </summary>
    public class UrlBuilder
    {
        #region Properties
        private UriBuilder Builder { get; }
        public List<string> Segments { get; } = new List<string>();
        private NameValueCollection Query { get; } = new NameValueCollection();
        #endregion

        #region Constructor
        /// <summary>
        /// Empty Constructor: You must specify all aspects of the URL
        /// </summary>
        public UrlBuilder() => Builder = new UriBuilder();

        /// <summary>
        /// Constructor: Allows you to modify the URL safely
        /// </summary>
        /// <param name="uri">The initial URL</param>
        public UrlBuilder(string uri)
        {
            Builder = new UriBuilder(uri);
            foreach (var segment in Builder.Uri.Segments)
                AddPathSegment(segment);
            Query = HttpUtility.ParseQueryString(Builder.Query);
        }
        #endregion

        #region Scheme
        /// <summary>
        /// Sets the url scheme to "http"
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder AsHttp() => WithScheme("http");
        /// <summary>
        /// Sets the url scheme to "https"
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder AsHttps() => WithScheme("https");
        /// <summary>
        /// Sets the url scheme to whatever you pass in
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="scheme">The url scheme</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder WithScheme(string scheme)
        {
            Builder.Scheme = scheme;
            return this;
        }
        #endregion

        #region Host
        /// <summary>
        /// Updates the "host" part of the url (eg. the main part)
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="host">The "host" part of the URL</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder WithHost(string host)
        {
            Builder.Host = host;
            return this;
        }
        #endregion

        #region Port
        /// <summary>
        /// Updates the "port" part of the url (eg. 80)
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="port">The port for the host in the url</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder WithPort(int port)
        {
            Builder.Port = port;
            return this;
        }
        #endregion

        #region Segments
        /// <summary>
        /// If you wish to remove all path segments to build new ones (existing segments can be retrieved via public property)
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder ClearPathSegments()
        {
            Segments.Clear();
            return this;
        }

        /// <summary>
        /// Removes the first path segment
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder RemoveFirstSegment()
        {
            if (Segments.Count > 0)
                Segments.RemoveAt(0);
            return this;
        }

        /// <summary>
        /// Removes the last path segment
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder RemoveLastSegment()
        {
            if (Segments.Count > 0)
                Segments.RemoveAt(Segments.Count - 1);
            return this;
        }

        /// <summary>
        /// Adds a segment to the URL
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="segment">The segment to add</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder AddPathSegment(string segment)
        {
            segment = segment.Replace("/", "").Trim();
            if (!string.IsNullOrWhiteSpace(segment))
                Segments.Add(segment);
            return this;
        }

        /// <summary>
        /// Adds a segment to the beginning of the segment part of the URL
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="segment">The segment to add</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder AddPathSegmentToBeginning(string segment)
        {
            segment = segment.Replace("/", "").Trim();
            if (!string.IsNullOrWhiteSpace(segment))
                Segments.Insert(0, segment);
            return this;
        }
        #endregion

        #region Querystring
        /// <summary>
        /// Removes all items from the query string
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder ClearQueryString()
        {
            Query.Clear();
            return this;
        }

        /// <summary>
        /// Removes an item from the query string. If key not found, this does nothing
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="key">The key (eg. key=value)</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder RemoveFromQueryString(string key)
        {
            Query.Remove(key);
            return this;
        }

        /// <summary>
        /// Adds a key and value combination to the query string (parameters are raw text, the actual URL will have these properly escaped)
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="key">The key (eg. key=value)</param>
        /// <param name="value">The value (eg. key=value)</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder AddQueryString(string key, string value)
        {
            Query.Add(key, value);
            return this;
        }
        #endregion

        #region Fragment
        /// <summary>
        /// Sets the fragment for the url
        /// </summary>
        /// <remarks>&lt;scheme&gt;://&lt;host&gt;:&lt;port&gt;/&lt;path segment 1&gt;/&lt;path segment 2&gt;?&lt;query key 1&gt;=&lt;query value 1&gt;&amp;&lt;query key 2&gt;=&lt;query value 2&gt;#&lt;fragment&gt;</remarks>
        /// <param name="fragment">The fragment to use</param>
        /// <returns>The UrlBuilder so you can chain/build</returns>
        public UrlBuilder WithFragment(string fragment)
        {
            Builder.Fragment = fragment;
            return this;
        }
        #endregion

        #region Build
        /// <summary>
        /// Must be called last
        /// </summary>
        /// <returns>The fully built URL you pieced together using the UrlBuilder class</returns>
        public string Build()
        {
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var segment in Segments)
            {
                if (isFirst)
                    isFirst = false; // Slash will automatically be there upon resolving the "Uri"
                else
                    sb.Append("/");
                sb.Append(segment);
            }
            Builder.Path = sb.ToString();

            sb.Clear();
            isFirst = true;
            foreach (var key in Query.AllKeys)
            {
                if (isFirst)
                    isFirst = false; // It will automatically add on "?" upon resolving the "Uri"
                else
                    sb.Append("&");
                sb.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(Query[key])}");
            }
            Builder.Query = sb.ToString();

            return Builder.Uri.ToString();
        }
        #endregion
    }
}
