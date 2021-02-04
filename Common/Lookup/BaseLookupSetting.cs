using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.Lookup
{
    /// <inheritdoc />
    public abstract class BaseLookupSetting<T> : ILookupSettings<T> where T : LookupSetting
    {
        public abstract string Key { get; }

        public abstract int CachingSeconds { get; }

        public abstract Task<IEnumerable<T>> GetAll();

        public virtual T GetItem(CaseInsensitiveBinaryList<T> settingsCollection, string key) => settingsCollection.FindBinary(key);

        public virtual string GetValue(T setting) => setting.Value;
    }
}