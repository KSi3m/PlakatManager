using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Utilities
{
    public class AddRolesUtility
    {

        public async Task AddRoles(ElectionMaterialManagerContext dbContext, IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User", "Moderator" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var userManager = services.GetRequiredService<UserManager<User>>();

            var users = await userManager.Users.ToListAsync();

            var admin = users.FirstOrDefault(x => x.UserName.Equals("Jimmie_Schiller"));
            if (admin != null)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            foreach (var x in users)
            {
                if (x != null && !await userManager.IsInRoleAsync(x, "Admin"))
                {
                    await userManager.AddToRoleAsync(x, "User");
                }
            }

           

        }

    }
}
