using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomInformation3<T> : CustomInformation<T>
    {
        protected CustomInformation3(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }
    }
}