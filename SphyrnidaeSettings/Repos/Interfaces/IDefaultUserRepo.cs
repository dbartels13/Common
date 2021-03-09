using System.Threading.Tasks;

namespace Sphyrnidae.Settings.Repos.Interfaces
{
    public interface IDefaultUserRepo
    {
        Task<SphyrnidaeIdentity> GetDefaultUser();
    }
}
