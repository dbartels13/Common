using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sphyrnidae.Common.Logging.Loggers.Models
{
    public class LogInsert
    {
        public string Type { get; set; }
        public Guid Identifier { get; set; }
        public DateTime Timestamp { get; set; }
        public TraceEventType Severity { get; set; }
        public string Order { get; set; }
        public string RequestId { get; set; }
        public string Session { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string Machine { get; set; }
        public string Application { get; set; }
        public Dictionary<string, string> Other { get; } = new Dictionary<string, string>();
    }
}