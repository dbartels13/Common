using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.FeatureToggle;

namespace Sphyrnidae.Common.WebServices.Interfaces
{
    public interface IFeatureToggleWebService
    {
        Task<IEnumerable<SphyrnidaeFeatureToggle>> GetAll(string application, int customerId);
    }
}