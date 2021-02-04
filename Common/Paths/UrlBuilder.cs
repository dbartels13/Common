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
        private UriBuilder Builder { get; }
        public List<string> Segments { get; } = new List<string>();
        private NameValueCollection Query { get; } = new NameValueCollection();

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
    }
}
