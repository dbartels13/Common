// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Paths
{
    /// <summary>
    /// Parses out relative paths
    /// </summary>
    public class RelativePathBuilder
    {
        private const string Prefix = "http://a.com/";
        private UrlBuilder Builder { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">The existing relative path (if not provided, will start with empty string)</param>
        public RelativePathBuilder(string path = "")
        {
            if (path.StartsWith("~"))
                path = path.Substring(1);
            if (path.StartsWith("/") || path.StartsWith("\\"))
                path = path.Substring(1);
            Builder = new UrlBuilder($"{Prefix}{path}");
        }

        /// <summary>
        /// If you wish to remove all path segments to build new ones (existing segments can be retrieved via public property)
        /// </summary>
        /// <returns>The RelativePathBuilder so you can chain/build</returns>
        public RelativePathBuilder ClearPathSegments()
        {
            Builder.ClearPathSegments();
            return this;
        }

        /// <summary>
        /// Adds a segment to the relative path
        /// </summary>
        /// <param name="segment">The segment to add</param>
        /// <returns>The RelativePathBuilder so you can chain/build</returns>
        public RelativePathBuilder AddPathSegment(string segment)
        {
            Builder.AddPathSegment(segment);
            return this;
        }

        /// <summary>
        /// Adds a segment to the beginning of the relative path
        /// </summary>
        /// <param name="segment">The segment to add</param>
        /// <returns>The RelativePathBuilder so you can chain/build</returns>
        public RelativePathBuilder AddPathSegmentToBeginning(string segment)
        {
            Builder.AddPathSegmentToBeginning(segment);
            return this;
        }

        /// <summary>
        /// Adds a key and value combination to the query string (parameters are raw text, the actual relative path will have these properly escaped)
        /// </summary>
        /// <param name="key">The key (eg. key=value)</param>
        /// <param name="value">The value (eg. key=value)</param>
        /// <returns>The RelativePathBuilder so you can chain/build</returns>
        public RelativePathBuilder AddQueryString(string key, string value)
        {
            Builder.AddQueryString(key, value);
            return this;
        }

        /// <summary>
        /// Sets the fragment for the relative path
        /// </summary>
        /// <param name="fragment">The fragment to use</param>
        /// <returns>The RelativePathBuilder so you can chain/build</returns>
        public RelativePathBuilder WithFragment(string fragment)
        {
            Builder.WithFragment(fragment);
            return this;
        }

        /// <summary>
        /// Must be called last
        /// </summary>
        /// <returns>The fully built relative path you pieced together using the RelativePathBuilder class</returns>
        public string Build() => Builder.Build().Replace(Prefix, "/");
    }
}
