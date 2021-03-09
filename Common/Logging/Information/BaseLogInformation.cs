using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Identity;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging.Information
{
    /// <summary>
    /// Collection of information used for any logger methods
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public abstract class BaseLogInformation
    {
        #region Properties
        private bool IsInitialized { get; set; }
        public abstract string Type { get; }

        /// <summary>
        /// Injections
        /// </summary>
        protected ILoggerInformation Info { get; }
        protected IApplicationSettings AppSettings { get; }

        /// <summary>
        /// Set in Constructor (tracking items)
        /// </summary>
        public Guid Identifier { get; }
        public DateTime Timestamp { get; }

        /// <summary>
        ///  Set in Initialization (more tracking items)
        /// </summary>
        public TraceEventType Severity { get; protected set; }
        public string Order { get; private set; }
        public string RequestId { get; private set; }
        public string Session { get; private set; }

        /// <summary>
        /// Set from identity (or from http)
        /// </summary>
        public int? UserId { get; private set; }
        public Dictionary<string, string> IdentityProperties { get; private set; }

        /// <summary>
        /// Set by the caller/inherited classes
        /// </summary>
        public string Message { get; protected set; }
        public string Category { get; protected set; }

        /// <summary>
        /// Set from SetProperties
        /// </summary>
        public string Machine { get; private set; }
        public string Application { get; private set; }

        /// <summary>
        /// Set by inherited classes
        /// </summary>
        public Dictionary<string, string> StaticProperties { get; private set; }
        public Dictionary<string, string> HighProperties { get; private set; }
        public Dictionary<string, string> MedProperties { get; private set; }
        public Dictionary<string, string> LowProperties { get; private set; }
        public Dictionary<string, string> NotResetProperties { get; private set; }
        #endregion

        #region Helper Methods
        public string TypeStr => $"Log Type: {Type}";
        public string IdentifierStr => $"Logging Guid: {Identifier}";
        public string TimestampStr => $"Timestamp: {Timestamp:O}";

        public string SeverityStr => $"Severity: {Enum.GetName(typeof(TraceEventType), Severity)}";
        public string OrderStr => $"Order: {Order}";
        public string RequestStr => string.IsNullOrWhiteSpace(RequestId) ? "" : $"RequestId: {RequestId}";
        public string SessionStr(string prefix = "") => string.IsNullOrWhiteSpace(Session) ? "" : $"{prefix}Session: {Session}";

        public string UserStr => $"User: {UserId ?? 0}";
        public string GetIdentity(CaseInsensitiveBinaryList<string> hideKeys) => IdentityProperties.ToNameValueCollection().AsString(hideKeys);

        public string MessageStr => string.IsNullOrWhiteSpace(Message) ? "" : $"Message: {Message}";
        public string CategoryStr => $"Category: {(string.IsNullOrWhiteSpace(Category) ? "Unknown" : Category)}";

        public string MachineStr => $"Server: {(string.IsNullOrWhiteSpace(Machine) ? "Unknown" : Machine)}";
        public string ApplicationStr => $"Application: {(string.IsNullOrWhiteSpace(Application) ? "Unknown" : Application)}";

        public string GetStatic(CaseInsensitiveBinaryList<string> hideKeys) => StaticProperties.ToNameValueCollection().AsString(hideKeys);
        public string GetHigh(CaseInsensitiveBinaryList<string> hideKeys) => HighProperties.ToNameValueCollection().AsString(hideKeys);
        public string GetMed(CaseInsensitiveBinaryList<string> hideKeys) => MedProperties.ToNameValueCollection().AsString(hideKeys);
        public string GetLow(CaseInsensitiveBinaryList<string> hideKeys) => LowProperties.ToNameValueCollection().AsString(hideKeys);
        #endregion

        #region Constructor/Initialization
        protected BaseLogInformation(ILoggerInformation info, IApplicationSettings appSettings)
        {
            Info = info;
            AppSettings = appSettings;
            Identifier = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            IdentityProperties = new Dictionary<string, string>();
            StaticProperties = new Dictionary<string, string>();
            HighProperties = new Dictionary<string, string>();
            MedProperties = new Dictionary<string, string>();
            LowProperties = new Dictionary<string, string>();
            NotResetProperties = new Dictionary<string, string>();
        }

        protected void InitializeBase(TraceEventType severity)
        {
            InitializeBegin(severity);
            SetRequestId();
            SetSession();
            var identity = Info.Identity;
            SetUser(identity);
            SetIdentityProperties(identity);
            SetStaticProperties();
        }
        protected void InitializeBase(TraceEventType severity, BaseLogInformation prevInfo)
        {
            InitializeBegin(severity);
            RequestId = prevInfo.RequestId;
            Session = prevInfo.Session;
            UserId = prevInfo.UserId;
            IdentityProperties = prevInfo.IdentityProperties;
            StaticProperties = prevInfo.StaticProperties;
            Machine = prevInfo.Machine; // SetProperties not needed??
            Application = prevInfo.Application;
        }
        private void InitializeBegin(TraceEventType severity)
        {
            Severity = severity;
            SetOrder();
            IsInitialized = true;
        }

        private void SetOrder()
        {
            if (Order != null)
                return;

            // Get the prefix string from the request
            Order = SafeTry.IgnoreException(() => Info.LogOrderPrefix) ?? "";

            // Each request will increment this request property
            const string key = "logging_order";
            // I don't think this lock is really necessary, as most everything will occur on main thread so this will be synchronous.
            // But leaving lock in case a thread is trying to log something
            Order += NamedLocker.Lock($"{key}_{Info.RequestId}", num => Info.LoggingOrder++);
        }

        private void SetRequestId()
        {
            if (string.IsNullOrWhiteSpace(RequestId))
                RequestId = SafeTry.IgnoreException(() => Info.RequestId);
        }
        private void SetSession()
        {
            if (string.IsNullOrWhiteSpace(Session))
                Session = SafeTry.IgnoreException(() => Info.SessionId);
        }

        private void SetUser(BaseIdentity identity)
        {
            if (!UserId.HasValue)
                UserId = SafeTry.IgnoreException(() => identity?.Id);
        }
        private void SetIdentityProperties(BaseIdentity identity)
        {
            if ((IdentityProperties?.Count ?? 0) == 0)
                IdentityProperties = identity?.GetCustomLoggingProperties() ?? new Dictionary<string, string>();
        }
        private void SetStaticProperties()
        {
            if ((StaticProperties?.Count ?? 0) == 0)
                StaticProperties = Info.StaticProperties ?? new Dictionary<string, string>();
        }
        #endregion

        #region Virtual Methods
        public virtual void SetProperties(ILoggerConfiguration config)
        {
            if (!IsInitialized)
                throw new Exception("Logging Information Initialization has not occurred");

            SafeTry.IgnoreException(() => Machine = System.Environment.MachineName);
            SafeTry.IgnoreException(() => Application = AppSettings.Name);
        }

        public virtual void UpdateProperties(ILoggerConfiguration config)
        {
            HighProperties = new Dictionary<string, string>();
            MedProperties = new Dictionary<string, string>();
            LowProperties = new Dictionary<string, string>();
        }
        #endregion
    }
}
