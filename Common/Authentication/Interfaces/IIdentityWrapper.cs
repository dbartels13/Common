namespace Sphyrnidae.Common.Authentication.Interfaces
{
    public interface IIdentityWrapper
    {
        SphyrnidaeIdentity Current { get; set; }
    }
}