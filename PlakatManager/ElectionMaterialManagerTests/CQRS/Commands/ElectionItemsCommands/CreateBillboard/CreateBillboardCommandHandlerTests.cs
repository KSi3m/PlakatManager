using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using FluentAssertions;
using ElectionMaterialManager.Dtos;
using System.Reflection.Metadata;
using ElectionMaterialManager.Services;
using AutoMapper;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard.Tests
{
    public class CreateBillboardCommandHandlerTests
    {

        [Fact()]
        public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNull()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new Mock<ElectionMaterialManagerContext>(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new CreateBillboardCommandHandler(
                _dbContextMock.Object,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

            // Arrange
            _userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(null as CurrentUser);
            var request = new CreateBillboardCommand();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(401);

        }

        [Fact()]
        public async Task Handle_ShouldReturnBadRequest_WhenTagsAreInvalid()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new CreateBillboardCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

           
          

            _userContextMock.Setup(x => x.GetCurrentUser())
                .ReturnsAsync(new CurrentUser("1", "test@test.com", ["Admin"]));
            var tags = new List<Tag>
            {
                new Tag { Id = 1 },
                new Tag { Id = 2 }
            };

            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();
            var request = new CreateBillboardCommand { Tags = new List<int> { 1, 2, 3 } };

        
            var result = await _handler.Handle(request, CancellationToken.None);

       
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Handle_ShouldCreateBillboard_WhenDataIsValid()
        {

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new CreateBillboardCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

            _userContextMock.Setup(x => x.GetCurrentUser())
                .ReturnsAsync(new CurrentUser("1", "test@test.com", ["Admin"]));
            var tags = new List<Tag>
            {
                new Tag { Id = 1 },
                new Tag { Id = 2 }
            };

            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();

    
            var command = new CreateBillboardCommand()
            {
                Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 5,
                Size = "15x2m",
                Cost = 125.5m,
                StatusId = 1,
                Height = 10,
                Tags = new List<int>() { 1, 2 },
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")

            };
        

             _mapperMock.Setup(x => x.Map<Billboard>(command))
                .Returns(new Billboard { Id = 1, 
                    AuthorId = "1",
                    Location = new Location() { Latitude = 22.2, Longitude = 52.2 },
                    Priority = 5,
                    Size = "15x2m",
                    Cost = 125.5m,
                    StatusId = 1,
                    Height = 10,
                    Tags = new List<Tag>() { new Tag { Id = 1 } , new Tag { Id = 2 } },
                    StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                    EndDate = DateTime.Parse("2025-02-28 14:30:00")
                });
 
            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(201);
        }


        /* [Fact()]
         public async void HandleTest()
         {

             var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

             using (var context = new ElectionMaterialManagerContext(options))
             {
                 context.Users.Add(new User { Id = "1", UserName = "testUser", Email = "test@test.com" });
                 context.SaveChanges();
             }

             var command = new CreateBillboardCommand()
             {
                 Location = new LocationDto() { Latitude = -91, Longitude = 189 },
                 Priority = 11,
                 Size = "15x200000000000000000000000000m",
                 Cost = 1999999.5m,
                 StatusId = 0,
                 Tags = new List<int>(),
                 StartDate = DateTime.Parse("2025-02-28 14:30:00"),
                 EndDate = DateTime.Parse("2025-01-28 14:30:00")

             };

             var userContextMock = new Mock<IUserContext>();
             userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(new CurrentUser(
                 "1", "test@test.com", ["Admin"]
                 ));

             using (var context = new ElectionMaterialManagerContext(options))
             {
                 var userContext = new UserContext(httpContextAccessorMock.Object, null, context);


                 var currentUser = await userContext.GetCurrentUser()!;

                 currentUser.Should().NotBeNull();
                 currentUser.Id.Should().Be("1");
                 currentUser.Email.Should().Be("test@example.com");
                 currentUser.Roles.Should().Contain("Admin");

             }
         }*/
    }
}