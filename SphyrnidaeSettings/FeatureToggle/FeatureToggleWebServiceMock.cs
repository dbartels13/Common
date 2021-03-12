using Sphyrnidae.Common.FeatureToggle;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphyrnidae.Settings.FeatureToggle
{
    public class FeatureToggleWebServiceMock : IFeatureToggleWebService
    {
        public Task<IEnumerable<FeatureToggleSetting>> GetAll(string application, string customerId)
            => Task.FromResult(new List<FeatureToggleSetting>().AsEnumerable());
    }
}
