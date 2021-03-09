using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomInformation2<T> : CustomInformation<T>
    {
        protected CustomInformation2(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }
    }
}