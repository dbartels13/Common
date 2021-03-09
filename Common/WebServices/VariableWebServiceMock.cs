using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.WebServices.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.WebServices
{
    public class VariableWebServiceMock : IVariableWebService
    {
        public Task<IEnumerable<VariableSetting>> GetAll(string application, string customerId)
            => Task.FromResult(new List<VariableSetting>().AsEnumerable());
    }
}
