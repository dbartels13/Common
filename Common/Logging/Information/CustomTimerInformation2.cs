using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomTimerInformation2<T1, T2> : CustomTimerInformation<T1, T2>
    {
        protected CustomTimerInformation2(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }
    }
}