using Xunit;
using ElectionMaterialManager.AppUserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FluentAssertions;
using ElectionMaterialManager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace ElectionMaterialManager.AppUserContext.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public async Task GetCurrentUserTest_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Email,"test@test.com"),
                new Claim(ClaimTypes.Role,"Admin"),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"Test"));

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            /*
            using (var context = new ElectionMaterialManagerContext(options))
            {
                context.Users.Add(new User { Id = "1", Email = "test1@test.com" });
                context.Users.Add(new User { Id = "2", Email = "test2@test.com" });
                await context.SaveChangesAsync();
            }*/
            var context = new ElectionMaterialManagerContext(options);
            var userManager = new Mock<UserManager<User>>();
            var userContext = new UserContext(httpContextAccessorMock.Object, userManager.Object,context);

            var currentUser = await userContext.GetCurrentUser()!;

            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().Contain("Admin");


        }
    }
}