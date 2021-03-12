using Sphyrnidae.Common.Variable;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphyrnidae.Settings.Variable
{
    public class VariableWebServiceMock : IVariableWebService
    {
        public Task<IEnumerable<VariableSetting>> GetAll(string application, string customerId)
            => Task.FromResult(new List<VariableSetting>().AsEnumerable());
    }
}
