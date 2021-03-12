using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Variable
{
    /// <inheritdoc />
    public class VariableSettingsDefault : VariableSettings
    {
        public void Setup() { }

        public override Task<IEnumerable<VariableSetting>> GetAll()
            => new Task<IEnumerable<VariableSetting>>(() => new List<VariableSetting>());
        public override VariableSetting GetItem(CaseInsensitiveBinaryList<VariableSetting> settingsCollection, string key) => new VariableSetting();
        public override string GetValue(VariableSetting setting) => setting.Value;

        public int RecheckSeconds => CachingSeconds;
        public bool EnableRecheck => false;
    }
}