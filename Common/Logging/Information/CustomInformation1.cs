using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomInformation1<T> : CustomInformation<T>
    {
        protected CustomInformation1(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }
    }
}