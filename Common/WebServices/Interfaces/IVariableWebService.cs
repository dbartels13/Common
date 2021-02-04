using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Variable;

namespace Sphyrnidae.Common.WebServices.Interfaces
{
    public interface IVariableWebService
    {
        Task<IEnumerable<SphyrnidaeVariable>> GetAll(string application, int customerId);
    }
}