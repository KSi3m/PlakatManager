using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ElectionMaterialManager.AppUserContext
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public UserContext(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<User> GetCurrentIdentityUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("Context user not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var username = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var userI = await _userManager.FindByNameAsync(username);

            return userI;
        }

        public async Task<CurrentUser>? GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("Context user not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var username = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var xd = await _userManager.FindByNameAsync(username);
            var id = xd.Id;
            var email = user.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

            return new CurrentUser(id, email, roles);
        }
    }
}
