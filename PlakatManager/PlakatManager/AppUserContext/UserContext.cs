using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElectionMaterialManager.AppUserContext
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ElectionMaterialManagerContext _db;
        public UserContext(IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager, ElectionMaterialManagerContext db)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _db = db;
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
                //throw new InvalidOperationException("Context user not present");
                return null;
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var username = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var userDb = await _db.Users
                .Where(x => x.UserName.Equals(username))
                .Select(x=> new
                {
                    x.Id,
                    x.Email
                })
                .FirstOrDefaultAsync();

            //var email = user.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

            return new CurrentUser(userDb.Id, userDb.Email, roles);
        }
    }
}
