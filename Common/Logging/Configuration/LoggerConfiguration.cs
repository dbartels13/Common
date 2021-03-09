using System;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Configuration
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of Logger Configurations which read from variables
    /// </summary>
    public abstract class LoggerConfiguration : ILoggerConfiguration
    {
        #region Properties/Constructor
        protected CaseInsensitiveBinaryList<string> TypeEnabledList;
        protected CaseInsensitiveBinaryList<string> LoggingIncludesList;
        protected CaseInsensitiveBinaryList<string> LoggersEnabledList;
        protected CaseInsensitiveBinaryList<string> HideKeyList;

        protected ICache Cache { get; }
        protected LoggerConfiguration(ICache cache) => Cache = cache;
        #endregion

        public bool Enabled { get; set; } = true;

        #region Types Enabled
        protected virtual string DefaultLogTypes => "API;-Attribute;-Database;Exception;Hidden Exception;HTTP Response Error;Long Running;Message;-Middleware;Timer;Unauthorized;-Web Service";

        /// <inheritdoc />
        /// <summary>
        /// Reads in enabled log types
        /// </summary>
        /// <param name="type">The "Type" of the LogInformation which specifies the type of request</param>
        /// <returns>True if the logging mechanism is enabled</returns>
        public virtual bool TypeEnabled(string type)
        {
            TypeEnabledList ??= GetOrderedList("Logging_Enabled_Types", DynamicTypesEnabled);
            return TypeEnabledList.Has(type);
        }

        protected abstract string DynamicTypesEnabled();
        #endregion

        #region Include
        // Nothing is included by default
        protected virtual string DefaultLogIncludes => $@"
-{ApiInformation.HeadersKey}
;-{ApiInformation.QueryStringKey}
;-{ApiInformation.FormKey}
;-{ApiInformation.BrowserKey}
;-{ResultBaseInformation.RequestDataKey}
;-{ResultBaseInformation.ResultKey}
;-{DatabaseInformation.ParametersKey}
";

        /// <inheritdoc />
        /// <summary>
        /// Reads in those optional features that should be included in logs
        /// </summary>
        /// <param name="item">The name of the item to include</param>
        /// <returns>True if the item should be included</returns>
        public virtual bool Include(string item)
        {
            LoggingIncludesList ??= GetOrderedList("Logging_Includes", DynamicIncludes);
            return LoggingIncludesList.Has(item);
        }

        protected abstract string DynamicIncludes();
        #endregion

        #region Loggers
        // No loggers are enabled by default, eg. logging is disabled
        protected virtual string DefaultLoggers => ""; // Debug;etc

        /// <inheritdoc />
        /// <summary>
        /// Reads in if the logger is enabled for a specific logging type
        /// </summary>
        /// <remarks>This is a 2-step process where the logger must be enabled, and the type must be enabled for it</remarks>
        /// <param name="name">The "Name" of the BaseLogger</param>
        /// <param name="type">The "Type" of the BaseLogInformation</param>
        /// <returns>True if the logger is enabled for the given logging type</returns>
        public virtual bool LoggerEnabled(string name, string type)
        {
            LoggersEnabledList ??= GetOrderedList("Logging_Enabled_Loggers", DynamicLoggersEnabled);
            if (!LoggersEnabledList.Has(name))
                return false;

            var list = GetOrderedList($"Logging_Enabled_{name}_Types", () => DynamicLoggerTypesEnabled(name));
            return list.Has(type);
        }

        protected abstract string DynamicLoggersEnabled();
        protected abstract string DynamicLoggerTypesEnabled(string name);
        #endregion

        #region Hide Keys
        protected virtual string DefaultHideKeys => "Authentication;Token;AccountToken;Password;ConfirmPassword";

        /// <inheritdoc />
        /// <summary>
        /// Specifies if a key should be obscured - listing given in Variable "Logging_HideKeys"
        /// </summary>
        /// <returns>True if it should be obscured</returns>
        public virtual CaseInsensitiveBinaryList<string> HideKeys()
            => HideKeyList ??= GetOrderedList("Logging_HideKeys", DynamicHideKeys);

        protected abstract string DynamicHideKeys();
        #endregion

        #region Max Length
        protected virtual int DefaultMaxLength => 1000;

        /// <inheritdoc />
        /// <summary>
        /// Maximum length of a logging item (Used by each logger however it sees fit - eg. Total length, Length of line, key, etc.). Listing given in Configuration "Logging_Loggers_{name}_MaxLength"
        /// </summary>
        /// <param name="name">The "Name" of the BaseLogger</param>
        /// <returns>The maximum length for something to log (Default 1000 chars if not provided or invalid)</returns>
        public virtual int MaxLength(string name) => DefaultMaxLength;
        #endregion

        #region Helper Methods
        protected virtual string UpdateKey(string key) => key;

        protected CaseInsensitiveBinaryList<string> GetOrderedList(string key, Func<string> method)
        {
            // Allow cache to not be a purely hard-coded string
            var wrapperCacheKey = UpdateKey(key);

            Cache.Options.Seconds = CachingSeconds();
            return Caching.Get(
                Cache,
                wrapperCacheKey,
                () =>
                    method()
                        .SafeSplit(";", true)
                        .ToCaseInsensitiveBinaryList()
            );
        }

        protected int CachingSeconds()
        {
            var seconds = DynamicCachingSeconds;
            return seconds < 30 ? 30 : seconds;
        }

        protected virtual int DynamicCachingSeconds => 1200;
        #endregion
    }
}
