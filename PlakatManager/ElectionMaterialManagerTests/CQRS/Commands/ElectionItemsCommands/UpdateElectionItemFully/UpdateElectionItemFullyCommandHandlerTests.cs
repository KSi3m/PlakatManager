using Xunit;
using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using ElectionMaterialManager.Dtos;
using Microsoft.Extensions.Hosting;
using System.Drawing;



namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully.Tests
{
    public class UpdateElectionItemFullyCommandHandlerTests
    {

        [Fact()]
        public async Task Handle_ShouldReturnNotFound_WhenItemIsNull()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase3")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemFullyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

            var request = new UpdateElectionItemFullyCommand();


            var result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(404);

        }


        [Fact()]
        public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNull()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase3")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemFullyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );
            var poster = new Poster() { Id = 2 };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.SaveChangesAsync();

            _userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(null as CurrentUser);
            var request = new UpdateElectionItemFullyCommand() { Id = 2};

           
            var result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(401);

            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();

        }

        [Fact()]
        public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNotAuthorOfItem()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase3")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemFullyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );
            var poster = new Poster() { Id = 2, AuthorId = "2" };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.SaveChangesAsync();

            _userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(new CurrentUser("1","test@test.com", ["User"]));
            var request = new UpdateElectionItemFullyCommand() { Id = 2 };


            var result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(401);

            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();

        }


        [Fact()]
        public async Task Handle_ShouldReturnBadRequest_WhenTagsAreInvalid()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase3")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemFullyCommandHandler(
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
            var poster = new Poster() { Id = 2 };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();
            var request = new UpdateElectionItemFullyCommand { Id = 2, Tags = new List<int> { 1, 2, 3 } };


            var result = await _handler.Handle(request, CancellationToken.None);


            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);

            _dbContextMock.ElectionItems.Remove(poster);
            _dbContextMock.Tags.RemoveRange(tags);
            await _dbContextMock.SaveChangesAsync();
        }

        [Fact()]
        public async Task Handle_ShouldUpdateElectionItem_WhenDataIsValidAndUserIsAuthorOfItem()
        {

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase3")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemFullyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

            _userContextMock.Setup(x => x.GetCurrentUser())
                .ReturnsAsync(new CurrentUser("1", "test@test.com", ["User"]));
            var tags = new List<Tag>
            {
                new Tag { Id = 1 },
                new Tag { Id = 2 }
            };

            var poster = new Poster
            {
                Id = 2,
                AuthorId = "1"
            };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();


            var command = new UpdateElectionItemFullyCommand()
            {
                Id = 2,
                Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 5,
                Size = "12x2m",
                Cost = 125.5m,
                StatusId = 1,
                PaperType = "Satin",
                Tags = new List<int>() { 1, 2 },
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")
            };

            poster.Location = new Location() { Latitude = 22.2, Longitude = 52.2 };
            poster.Priority = 5;
            poster.Size = "12x2m";
            poster.Cost = 125.5m;
            poster.StatusId = 1;
            poster.PaperType = "Satin";
            poster.Tags = _dbContextMock.Tags.Where(x => x.Id.Equals(1)).ToList(); //new List<int>() { 1, 2 };
            poster.StartDate = DateTime.Parse("2025-01-28 14:30:00");
            poster.EndDate = DateTime.Parse("2025-02-28 14:30:00");
            _mapperMock.Setup(x => x.Map<Poster>(command))
               .Returns(poster);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(204);
            _dbContextMock.Tags.RemoveRange(tags);
            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();
        }
        [Fact()]
        public async Task Handle_ShouldUpdateElectionItem_WhenDataIsValidAndUserIsAdmin()
        {

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase3")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemFullyCommandHandler(
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

            var poster = new Poster() { Id = 2, AuthorId = "2" };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();


            var command = new UpdateElectionItemFullyCommand()
            {
                Id = 2,
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
            poster.Location = new Location() { Latitude = 22.2, Longitude = 52.2 };
            poster.Priority = 5;
            poster.Size = "12x2m";
            poster.Cost = 125.5m;
            poster.StatusId = 1;
            poster.PaperType = "Satin";
            poster.Tags = _dbContextMock.Tags.Where(x => x.Id.Equals(1)).ToList(); //new List<int>() { 1, 2 };
            poster.StartDate = DateTime.Parse("2025-01-28 14:30:00");
            poster.EndDate = DateTime.Parse("2025-02-28 14:30:00");
            _mapperMock.Setup(x => x.Map<Poster>(command))
               .Returns(poster);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(204);
            _dbContextMock.Tags.RemoveRange(tags);
            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();
        }
    }
}