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

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            using (var context = new ElectionMaterialManagerContext(options))
            {
                context.Users.Add(new User { Id = "1", UserName = "testUser", Email = "test@test.com" });
                context.SaveChanges();
            }

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, "testUser"),
                 new Claim(ClaimTypes.Email, "test@test.com"),
                 new Claim(ClaimTypes.Role, "Admin")
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = user
            });

            using (var context = new ElectionMaterialManagerContext(options))
            {
                var userContext = new UserContext(httpContextAccessorMock.Object, null, context);

               
                var currentUser = await userContext.GetCurrentUser()!;

                currentUser.Should().NotBeNull();
                currentUser.Id.Should().Be("1");
                currentUser.Email.Should().Be("test@example.com");
                currentUser.Roles.Should().Contain("Admin");

            }
        }
        [Fact()]
        public async Task GetCurrentUserTest_WithoutAuthenticatedUser_ShouldReturnNull()
        {

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            using (var context = new ElectionMaterialManagerContext(options))
            { 
                context.SaveChanges();
            }

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = null
            });

            using (var context = new ElectionMaterialManagerContext(options))
            {
                var userContext = new UserContext(httpContextAccessorMock.Object, null, context);

                
                var currentUser = await userContext.GetCurrentUser()!;

                currentUser.Should().BeNull();
            }
        }
    }
}