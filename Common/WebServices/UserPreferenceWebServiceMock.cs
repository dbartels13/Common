using Sphyrnidae.Common.UserPreference;
using Sphyrnidae.Common.WebServices.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.WebServices
{
    public class UserPreferenceWebServiceMock : IUserPreferenceWebService
    {
        public Task<bool> Create(string application, int userId, string key, string value) => Task.FromResult(true);

        public Task<IEnumerable<UserPreferenceSetting>> GetAll(string application, int userId)
            => Task.FromResult(new List<UserPreferenceSetting>().AsEnumerable());

        public Task<bool> Update(string application, int userId, string key, string value) => Task.FromResult(true);
    }
}
