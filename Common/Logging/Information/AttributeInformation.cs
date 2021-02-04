using System.Collections.Generic;
using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for attributes
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class AttributeInformation : ResultBaseInformation
    {
        public override string LongRunningName => $"Attribute-{AppSettings.Name}-{Message}";

        public AttributeInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings)
        {
            Category = "Attribute";
        }

        public virtual void Initialize(string attributeName, Dictionary<string, string> parameters)
        {
            InitializeResult(TraceEventType.Verbose, attributeName);
            MedProperties = parameters ?? MedProperties;
        }
    }
}