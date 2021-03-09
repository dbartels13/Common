using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Configuration;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.Variable.Interfaces;

namespace Sphyrnidae.Settings
{
    /// <inheritdoc />
    public class SphyrnidaeLoggerConfiguration : LoggerConfiguration
    {
        protected IVariableServices Variable { get; }
        protected IIdentityHelper Identity { get; }
        public SphyrnidaeLoggerConfiguration(ICache cache, IVariableServices variable, IIdentityHelper identity) : base(cache)
        {
            Variable = variable;
            Identity = identity;
        }

        protected override string DynamicTypesEnabled()
            => SettingsVariable.Get(Variable, "Logging_Enabled_Types", DefaultLogTypes);

        protected override string DynamicIncludes()
            => SettingsVariable.Get(Variable, "Logging_Includes", DefaultLogIncludes);

        protected override string DynamicLoggersEnabled()
            => SettingsVariable.Get(Variable, "Logging_Enabled_Loggers", DefaultLoggers);

        protected override string DynamicLoggerTypesEnabled(string name)
            => SettingsVariable.Get(Variable, $"Logging_Enabled_{name}_Types", DefaultLogTypes);

        protected override string DynamicHideKeys()
            => SettingsVariable.Get(Variable, "Logging_HideKeys", DefaultHideKeys);

        public override int MaxLength(string loggerName)
            => SettingsVariable.Get(Variable, $"Logging_{loggerName}_MaxLength", DefaultMaxLength.ToString())
                .ToInt(DefaultMaxLength);

        protected override string UpdateKey(string key)
        {
            var identity = (SphyrnidaeIdentity)Identity.Current;
            return identity.IsDefault() ? key : $"{key}_{identity.CustomerId}";
        }

        protected override int DynamicCachingSeconds => Variable.Service.CachingSeconds;
    }
}