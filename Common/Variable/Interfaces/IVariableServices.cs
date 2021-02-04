using Sphyrnidae.Common.Lookup;

namespace Sphyrnidae.Common.Variable.Interfaces
{
    /// <summary>
    /// Services required for executing a variable lookup
    /// </summary>
    public interface IVariableServices : ILookupServices<IVariableSettings, SphyrnidaeVariable>
    {
    }
}
