using Sphyrnidae.Common.FeatureToggle;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.WebServices.Interfaces
{
    public interface IFeatureToggleWebService
    {
        Task<IEnumerable<FeatureToggleSetting>> GetAll(string application, string customerId);
    }
}