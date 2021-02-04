using System.Threading.Tasks;

namespace Sphyrnidae.Common.WebServices.Interfaces
{
    public interface IApiAuthenticationWebService
    {
        Task<bool> IsAuthenticated(string application, string token);
    }
}