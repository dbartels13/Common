using System;
using System.Collections.Generic;

namespace Sphyrnidae.Common.Logging.Loggers.Models
{
    public class LogUpdate
    {
        public Guid Identifier { get; set; }
        public long? Elapsed { get; set; }
        public Dictionary<string, string> Other { get; } = new Dictionary<string, string>();
    }
}