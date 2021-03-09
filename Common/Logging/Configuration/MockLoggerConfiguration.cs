using Sphyrnidae.Common.Cache;

namespace Sphyrnidae.Common.Logging.Configuration
{
    public class MockLoggerConfiguration : LoggerConfiguration
    {
        public MockLoggerConfiguration(ICache cache) : base(cache) { }

        protected override string DynamicHideKeys() => DefaultHideKeys;

        protected override string DynamicIncludes() => DefaultLogIncludes;

        protected override string DynamicLoggersEnabled() => DefaultLoggers;

        protected override string DynamicLoggerTypesEnabled(string name) => DefaultLogTypes;

        protected override string DynamicTypesEnabled() => DefaultLogTypes;
    }
}
