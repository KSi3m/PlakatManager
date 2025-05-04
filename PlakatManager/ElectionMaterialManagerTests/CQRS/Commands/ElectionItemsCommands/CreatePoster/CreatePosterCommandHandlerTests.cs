using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster.Tests
{
    public class CreatePosterCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNull()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase2")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new CreatePosterCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

            // Arrange
            _userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(null as CurrentUser);
            var request = new CreatePosterCommand();

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
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new CreatePosterCommandHandler(
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
            var request = new CreatePosterCommand { Tags = new List<int> { 1, 2, 3 } };


            var result = await _handler.Handle(request, CancellationToken.None);


            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);

            _dbContextMock.Tags.RemoveRange(tags);
            await _dbContextMock.SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_ShouldCreatePoster_WhenDataIsValid()
        {

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new CreatePosterCommandHandler(
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


            var command = new CreatePosterCommand()
            {
                Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 5,
                Size = "15x2m",
                Cost = 125.5m,
                StatusId = 1,
                PaperType = "Satin",
                Tags = new List<int>() { 1, 2 },
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")

            };
            var poster = new Poster
            {
                Id = 1,
                AuthorId = "1",
                Location = new Location() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 5,
                Size = "15x2m",
                Cost = 125.5m,
                StatusId = 1,
                PaperType = "Satin",
                //Tags = new List<Tag>() { new Tag { Id = 1 }, new Tag { Id = 2 } },
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")
            };

            _mapperMock.Setup(x => x.Map<Poster>(command))
               .Returns(poster);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(201);
            _dbContextMock.Tags.RemoveRange(tags);
            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();
        }
    }
}