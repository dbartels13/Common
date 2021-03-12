using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Variable;

namespace Sphyrnidae.Settings.Variable
{
    public interface IVariableWebService
    {
        Task<IEnumerable<VariableSetting>> GetAll(string application, string customerId);
    }
}