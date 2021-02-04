using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Variable.Interfaces;

// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Variable
{
    /// <inheritdoc />
    public class VariableSettingsMock : IVariableSettings
    {
        public void Setup() { }

        public virtual Task<IEnumerable<SphyrnidaeVariable>> GetAll()
            => new Task<IEnumerable<SphyrnidaeVariable>>(() => new List<SphyrnidaeVariable>());
        public SphyrnidaeVariable GetItem(CaseInsensitiveBinaryList<SphyrnidaeVariable> settingsCollection, string key) => new SphyrnidaeVariable();
        public string GetValue(SphyrnidaeVariable setting) => setting.Value;

        public string Key => "VariableSettings";
        public int CachingSeconds => 1200;
        public int RecheckSeconds => CachingSeconds;
        public bool EnableRecheck => false;
    }
}