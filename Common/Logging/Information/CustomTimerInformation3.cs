using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomTimerInformation3<T1, T2> : CustomTimerInformation<T1, T2>
    {
        protected CustomTimerInformation3(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }
    }
}