using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Identity;

namespace ElectionMaterialManager.Services
{
    public interface IAuthService
    {
        Task<string> CreateToken(User user);
    }
}
