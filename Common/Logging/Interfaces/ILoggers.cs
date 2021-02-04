using System.Collections.Generic;
using Sphyrnidae.Common.Logging.Loggers;

namespace Sphyrnidae.Common.Logging.Interfaces
{
    /// <summary>
    /// Allows you to "register" multiple loggers
    /// </summary>
    public interface ILoggers
    {
        /// <summary>
        /// Listing of ALL possible loggers
        /// </summary>
        List<BaseLogger> All { get; }
    }
}
