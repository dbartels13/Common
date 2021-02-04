using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for a http response message that cause an alert
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class HttpResponseInformation : BaseLogInformation
    {
        public static string StaticType = "HTTP Response Error";
        public override string Type => StaticType;

        public HttpResponseInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(BaseLogInformation prevInfo, string type, string httpMethod, int? httpResponseCode, string result)
        {
            InitializeBase(TraceEventType.Warning, prevInfo);

            Message = $@"Invalid HTTP Response ({httpResponseCode ?? -1}); {type} {httpMethod ?? ""}: {prevInfo.Message}
{result}";
            Category = $"{prevInfo.Category} Http Response Issue";
        }
    }
}