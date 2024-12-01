using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Entities.Seeders
{
    public class Seeder
    {
        private readonly ElectionMaterialManagerContext _dbContext;

        public Seeder(ElectionMaterialManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed(IServiceProvider services)
        {

            if( _dbContext.Database.CanConnect() ) {

                var hasData = _dbContext.Users.Any()
                        || _dbContext.Statuses.Any()
                        || _dbContext.Tags.Any();

                if(!hasData)
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    string[] roleNames = { "Admin", "User", "Moderator" };

                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }


                    var user1 = new User()
                    {
                        FirstName = "Jan",
                        LastName = "Glin",
                        UserName = $"Jan_Glin",       
                        Email = "janglin@gmail.com",
                        Address = new Address()
                        {
                            Country = "Poland",
                            City = "Warsaw",
                            Street = "Krakowskie Przedmiescie",
                            PostalCode = "23-557"
                        }
                    };
                    await userManager.CreateAsync(user1, "JSM123!a");
                    await userManager.AddToRoleAsync(user1, "Admin");
                    //_dbContext.Users.Add(user1);

                    var tags = new List<Tag>()
                    {
                        new Tag() {Value = "Campaign"},
                        new Tag() {Value = "Pre-campaign"},
                        new Tag() {Value = "For people"},
                        new Tag() {Value = "Meetings"}
                    };

                    _dbContext.Tags.AddRange(tags);
                    var statuses = new List<Status>()
                    {
                        new Status() {Name = "To do"},
                        new Status() {Name = "Done"},
                        new Status() {Name = "To be removed"},
                    };
                    _dbContext.Statuses.AddRange(statuses);

                 
                    /*foreach (var x in users)
                    {
                        if (x != null && !await userManager.IsInRoleAsync(x, "Admin"))
                        {
                            await userManager.AddToRoleAsync(x, "User");
                        }
                    }*/



                    _dbContext.SaveChanges();
                }
            
            
            
            
            
            
            
            }

        }
    }
}
