using Sphyrnidae.Common.Cache.Models;
using Sphyrnidae.Common.Lookup;
using Sphyrnidae.Common.Variable.Interfaces;

namespace Sphyrnidae.Common.Variable
{
    public abstract class VariableSettings : BaseLookupSetting<SphyrnidaeVariable>, IVariableSettings
    {
        public override string Key { get; } = "VariableSettings";

        public override int CachingSeconds => CacheOptions.Minute;
    }
}