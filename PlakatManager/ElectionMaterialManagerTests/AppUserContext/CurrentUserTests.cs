using Xunit;
using ElectionMaterialManager.AppUserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ElectionMaterialManager.AppUserContext.Tests
{
    public class CurrentUserTests
    {
        [Fact()]
        public void IsInRoleTest_WithRightRole_ShouldReturnTrue()
        {
            var user = new CurrentUser("1", "test@gmail.com", new List<string> { "Admin" });

            var isInRole = user.IsInRole("Admin");

            isInRole.Should().BeTrue();
        }

        [Fact()]
        public void IsInRoleTest_WithoutRightRole_ShouldReturnFalse()
        {
            var user = new CurrentUser("1", "test@gmail.com", new List<string> { "User" });

            var isInRole = user.IsInRole("Admin");

            isInRole.Should().BeFalse();

        }

        [Fact()]
        public void IsInRoleTest_WithRightRoleCaseNotMatching_ShouldReturnFalse()
        {
            var user = new CurrentUser("1", "test@gmail.com", new List<string> { "Admin" });

            var isInRole = user.IsInRole("admin");

            isInRole.Should().BeFalse();

        }

    }
}