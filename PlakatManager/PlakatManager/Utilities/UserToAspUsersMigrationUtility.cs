using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Utilities
{
    public class UserToAspUsersMigrationUtility
    {

        public async Task Migrate(ElectionMaterialManagerContext dbContext, IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            var users = await dbContext.LegacyUsers.ToListAsync();

            foreach (var user in users)
            {
                var appUser = new User
                {

                    Id = user.Id.ToString(),
                    UserName = $"{user.FirstName}_{user.LastName}",       
                    NormalizedUserName = $"{user.FirstName}_{user.LastName}".ToUpper(),   
                    Email = user.Email,    
                    NormalizedEmail = user.Email.ToUpper(),  
                    EmailConfirmed = true,            
                    PhoneNumber = "+1234567890",     
                    PhoneNumberConfirmed = false,   
                    TwoFactorEnabled = false,        
                    LockoutEnd = null,               
                    LockoutEnabled = true,           
                    AccessFailedCount = 0,
                    FirstName = user.FirstName,
                    LastName = user.LastName

                 };
                var tempPassword = appUser.FirstName.Take(1).ToString() + appUser.LastName.Take(1).ToString() + appUser.NormalizedEmail.Take(1) + "123!";
                await userManager.CreateAsync(appUser, tempPassword);
               
            }

        }


    }
}
