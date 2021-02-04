using Sphyrnidae.Common.Cache;

namespace Sphyrnidae.Common.Lookup
{
    /// <summary>
    /// Services required for executing a lookup
    /// </summary>
    public interface ILookupServices<T, TS> where T : ILookupSettings<TS> where TS : LookupSetting
    {
        ICache Cache { get; }
        T Service { get; }
    }
}
