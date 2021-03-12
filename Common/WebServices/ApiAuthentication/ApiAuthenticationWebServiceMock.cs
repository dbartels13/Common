using System.Threading.Tasks;

namespace Sphyrnidae.Common.WebServices.ApiAuthentication
{
    public class ApiAuthenticationWebServiceMock : IApiAuthenticationWebService
    {
        public Task<bool> IsAuthenticated(string application, string token) => Task.FromResult(true);
    }
}
