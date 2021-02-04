using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Variable.Interfaces;

namespace Sphyrnidae.Common.Variable
{
    /// <inheritdoc />
    public class VariableServices : IVariableServices
    {
        public ICache Cache { get; }
        public IVariableSettings Service { get; }

        public VariableServices(ICache cache, IVariableSettings service)
        {
            Cache = cache;
            Service = service;
        }
    }
}
