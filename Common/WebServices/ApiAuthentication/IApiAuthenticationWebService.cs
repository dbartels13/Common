using System.Threading.Tasks;

namespace Sphyrnidae.Common.WebServices.ApiAuthentication
{
    public interface IApiAuthenticationWebService
    {
        Task<bool> IsAuthenticated(string application, string token);
    }
}