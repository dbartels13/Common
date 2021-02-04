using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.UserPreference;

namespace Sphyrnidae.Common.WebServices.Interfaces
{
    public interface IUserPreferenceWebService
    {
        Task<IEnumerable<SphyrnidaeUserPreference>> GetAll(string application, int userId);
        Task<bool> Create(string application, int userId, string key, string value);
        Task<bool> Update(string application, int userId, string key, string value);
    }
}