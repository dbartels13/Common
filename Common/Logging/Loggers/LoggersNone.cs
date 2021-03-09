using Sphyrnidae.Common.Logging.Interfaces;
using System.Collections.Generic;

namespace Sphyrnidae.Common.Logging.Loggers
{
    public class LoggersNone : ILoggers
    {
        public List<BaseLogger> All => new List<BaseLogger>();
    }
}
