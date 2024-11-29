using ElectionMaterialManager.Entities;

namespace ElectionMaterialManager.AppUserContext
{
    public interface IUserContext
    {
        Task<CurrentUser>? GetCurrentUser();
        Task<User>? GetCurrentIdentityUser();
    }
}
