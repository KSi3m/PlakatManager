using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Utilities
{
    public class UserToAspUsersMigrationUtility
    {

        public async Task Migrate(ElectionMaterialManagerContext dbContext, IServiceProvider services)
        {/*
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
                var tempPassword = $"{user.FirstName[0]}{user.LastName[0]}{user.NormalizedEmail[^1]}123!a";
                await userManager.CreateAsync(appUser, tempPassword);

            }*/

        }


    }
}
