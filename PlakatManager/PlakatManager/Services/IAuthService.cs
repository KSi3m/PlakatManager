using Microsoft.AspNetCore.Identity;

namespace ElectionMaterialManager.Services
{
    public interface IAuthService
    {
        string CreateToken(IdentityUser user);
    }
}
