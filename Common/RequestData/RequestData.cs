using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpData;
using Sphyrnidae.Common.Microsoft;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.RequestData
{
    /// <inheritdoc />
    public class RequestData : IRequestData
    {
        #region Constructor/Properties
        // ReSharper disable once InconsistentNaming
        private Guid _id { get; }

        protected IHttpData Data { get; }
        /// <inheritdoc />
        public RequestData(IHttpData data)
        {
            Data = data;
            _id = Guid.NewGuid();
        }
        #endregion

        #region Implementations
        /// <inheritdoc />
        public virtual Guid Id => _id;

        public virtual char LoggingOrder { get; set; } = (char)33;

        #region IpAddresses
        public virtual string IpAddress { get; set; }
        public virtual string RemoteIpAddress => Data.Context?.IpAddress();
        #endregion

        #region Url
        private string _url;
        public virtual string DisplayUrl => _url ??= SafeTry.IgnoreException(() => Data.Request?.GetDisplayUrl()) ?? "Unknown";
        #endregion

        #region Route
        private string _route;
        public virtual string Route =>
            _route ??= SafeTry.IgnoreException(() =>
            {
                var ep = GetEndpoint();
                return ep switch
                {
                    null => string.Empty,
                    RouteEndpoint endpoint => endpoint.RoutePattern.RawText,
                    _ => GetEndpointObject<ControllerActionDescriptor>()?.AttributeRouteInfo?.Template ?? string.Empty
                };
            });
        #endregion

        #region Content Data
        private bool _dataSet;
        private string _data;
        public virtual async Task<string> ContentData()
        {
            if (_dataSet)
                return _data;

            try
            {
                _data = await Data.Request.GetBodyAsync();
            }
            catch
            {
                _data = null;
            }
            finally
            {
                _dataSet = true;
            }

            return _data;
        }
        #endregion

        #region Http Verb
        private string _verb;
        public virtual string HttpVerb => _verb ??= SafeTry.IgnoreException(() => Data.Request?.Method, "") ?? "";
        #endregion

        #region Headers
        private NameValueCollection _headers;
        public virtual NameValueCollection Headers => _headers ??= (Data.Request?.Headers?.Count ?? 0) > 0
            ? Data.Request.Headers.ToNameValueCollection() // This seems to work just fine
            : new NameValueCollection();
        #endregion

        #region Query String
        private NameValueCollection _query;
        public virtual NameValueCollection QueryString
        {
            get
            {
                if (_query != null)
                    return _query;

                _query = new NameValueCollection();
                if ((Data.Request?.Query?.Count ?? 0) <= 0)
                    return _query;

                foreach (var (key, value) in Data.Request.Query)
                    _query.Add(key, value);

                return _query;
            }
        }
        #endregion

        #region Form Data
        private NameValueCollection _form;
        public virtual NameValueCollection FormData
        {
            get
            {
                if (_form != null)
                    return _form;

                _form = new NameValueCollection();
                if (!(Data.Request?.HasFormContentType ?? false) || (Data.Request?.Form?.Count ?? 0) <= 0)
                    return _form;

                foreach (var (key, value) in Data.Request.Form)
                    _form.Add(key, value);

                return _form;
            }
        }
        #endregion

        #region Browser
        private string _browser;
        public virtual string Browser =>
            _browser ??= SafeTry.IgnoreException(() =>
            {
                var userAgent = Data.Request.Headers["User-Agent"];
                if (string.IsNullOrWhiteSpace(userAgent))
                    return "Unknown";

                var ua = new UserAgent(userAgent);
                return ua.Browser.Name;
            });
        #endregion

        public virtual T GetEndpointObject<T>()
        {
            var meta = GetEndpoint()?.Metadata;
            if (meta == null)
                return default;

            foreach (var item in meta)
            {
                if (item is T foundItem)
                    return foundItem;
            }

            return default;
        }

        public virtual string GetHeader(string name) => Data.Request?.GetHeader(name);
        public virtual StringValues GetHeaders(string name) => Data.Request?.Headers[name] ?? default;
        #endregion

        #region Helpers
        protected Endpoint GetEndpoint() => Data.Context?.Features?.Get<IEndpointFeature>()?.Endpoint;
        #endregion
    }
}