using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Logging.Models;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for any timing (API, Service, Database, Web Service) calls
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public abstract class TimerBaseInformation : BaseLogInformation
    {
        public override string Type => Category;

        /// <summary>
        /// Tells if initial log call completed.
        /// We don't want to log the completion message until that one has completed
        /// It will be up to the logger to correctly utilize this
        /// </summary>
        public bool SetComplete { get; set; }
        private Stopwatch Sw { get; set; }

        protected TimerBaseInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        protected void InitializeTimer(TraceEventType severity, string name)
        {
            InitializeBase(severity);

            SetComplete = false;
            Message = name;
            Sw = Stopwatch.StartNew();
        }
        public void Stop() => Sw.Stop();

        public long? GetElapsed() => SetComplete ? Sw.ElapsedMilliseconds : default(long?);
        public string GetElapsedStr() => SetComplete ? $"Execution Time: {Sw.ElapsedMilliseconds} ms" : null;

        #region Alerts
        public bool? IsLongRunning(long maxMilliseconds)
        {
            if (!SetComplete)
                return null;

            if (maxMilliseconds <= 0)
                return false;

            return Sw.ElapsedMilliseconds >= maxMilliseconds;
        }

        /// <summary>
        /// A name that can be used to determine how many milliseconds is considered long running
        /// </summary>
        public virtual string LongRunningName => null;
        public virtual HttpResponseInfo GetResponseInfo() => null;
        public virtual string GetResponse(CaseInsensitiveBinaryList<string> hideKeys) => null;
        #endregion
    }
}
