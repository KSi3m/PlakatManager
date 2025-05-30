﻿using Xunit;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemPartially.Tests
{
    public class UpdateElectionItemPartiallyCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ShouldReturnNotFound_WhenItemIsNull()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase4")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemPartiallyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );

            var request = new UpdateElectionItemPartiallyCommand();


            var result = await _handler.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(404);

        }

        
        [Fact()]
        public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNull()
        {
            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase4")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemPartiallyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );
            var poster = new Poster() { Id = 2 };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.SaveChangesAsync();

            _userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(null as CurrentUser);
            var request = new UpdateElectionItemPartiallyCommand() { Id = 2 };


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
          .UseInMemoryDatabase(databaseName: "TestDatabase4")
          .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemPartiallyCommandHandler(
                _dbContextMock,
                _mapperMock.Object,
                _userContextMock.Object,
                _districtLocalizationServiceMock.Object
            );
            var poster = new Poster() { Id = 2, AuthorId = "2" };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.SaveChangesAsync();

            _userContextMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(new CurrentUser("1", "test@test.com", ["User"]));
            var request = new UpdateElectionItemPartiallyCommand() { Id = 2 };


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
            .UseInMemoryDatabase(databaseName: "TestDatabase4")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemPartiallyCommandHandler(
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
            var request = new UpdateElectionItemPartiallyCommand { Id = 2, Tags = new List<int> { 1, 2, 3 } };


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
            .UseInMemoryDatabase(databaseName: "TestDatabase4")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemPartiallyCommandHandler(
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
                AuthorId = "1",
                Priority = 5,
                Size = "12x2m",
                Cost = 125.5m,
                StatusId = 1,
                PaperType = "Satin",
                Tags = tags,
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")
            };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();


            var command = new UpdateElectionItemPartiallyCommand()
            {
                Id = 2,
                Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 8,
                PaperType = "Satin2"
            };

            /*  _mapperMock.Setup(x => x.Map<Poster>(command))
                 .Returns(poster);*/
            _mapperMock.Setup(m => m.Map(It.IsAny<UpdateElectionItemPartiallyCommand>(), It.IsAny<ElectionItem>()))
                .Callback<UpdateElectionItemPartiallyCommand, ElectionItem>((cmd, item) =>
                  {
                      if (cmd.Priority != null) item.Priority = cmd.Priority.Value;
                      if (cmd.Size != null) item.Size = cmd.Size;
                      if (cmd.Cost != null) item.Cost = cmd.Cost.Value;
                      if (cmd.StatusId != null) item.StatusId = cmd.StatusId.Value;
                      if (item is Poster poster)
                      {
                          if (cmd.PaperType != null) poster.PaperType = cmd.PaperType;
                      }
                      if (cmd.Location != null) item.Location = new Location
                      {
                          Longitude = cmd.Location.Longitude,
                          Latitude = cmd.Location.Latitude,
                          District = cmd.Location.District,
                          City = cmd.Location.City
                      };
                      if (cmd.StartDate != null) item.StartDate = cmd.StartDate.Value;
                      if (cmd.EndDate != null) item.EndDate = cmd.EndDate.Value;
                  });


            var result = await _handler.Handle(command, CancellationToken.None);

           //var xd =  _dbContextMock.ElectionItems.First(x => x.Id == 2).Priority;
            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(204);
            poster.Priority.Should().Be(8);
            poster.PaperType.Should().Be("Satin2");
            _dbContextMock.Tags.RemoveRange(tags);
            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();
        }
        
        [Fact()]
        public async Task Handle_ShouldUpdateElectionItem_WhenDataIsValidAndUserIsAdmin()
        {

            var options = new DbContextOptionsBuilder<ElectionMaterialManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase4")
            .Options;


            var _userContextMock = new Mock<IUserContext>();
            var _dbContextMock = new ElectionMaterialManagerContext(options);
            var _districtLocalizationServiceMock = new Mock<IDistrictLocalizationService>();
            var _mapperMock = new Mock<IMapper>();

            var _handler = new UpdateElectionItemPartiallyCommandHandler(
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

            var poster = new Poster
            {
                Id = 2,
                AuthorId = "3",
                Priority = 5,
                Size = "12x2m",
                Cost = 125.5m,
                StatusId = 1,
                PaperType = "Satin",
                Tags = tags,
                StartDate = DateTime.Parse("2025-01-28 14:30:00"),
                EndDate = DateTime.Parse("2025-02-28 14:30:00")
            };
            await _dbContextMock.ElectionItems.AddAsync(poster);
            await _dbContextMock.Tags.AddRangeAsync(tags);
            await _dbContextMock.SaveChangesAsync();


            var command = new UpdateElectionItemPartiallyCommand()
            {
                Id = 2,
                Location = new LocationDto() { Latitude = 22.2, Longitude = 52.2 },
                Priority = 8

            };

            _mapperMock.Setup(m => m.Map(It.IsAny<UpdateElectionItemPartiallyCommand>(), It.IsAny<ElectionItem>()))
                .Callback<UpdateElectionItemPartiallyCommand, ElectionItem>((cmd, item) =>
                {
                    if (cmd.Priority != null) item.Priority = cmd.Priority.Value;
                    if (cmd.Size != null) item.Size = cmd.Size;
                    if (cmd.Cost != null) item.Cost = cmd.Cost.Value;
                    if (cmd.StatusId != null) item.StatusId = cmd.StatusId.Value;
                    if (item is Poster poster)
                    {
                        if (cmd.PaperType != null) poster.PaperType = cmd.PaperType;
                    }
                    if (cmd.Location != null) item.Location = new Location
                    {
                        Longitude = cmd.Location.Longitude,
                        Latitude = cmd.Location.Latitude,
                        District = cmd.Location.District,
                        City = cmd.Location.City
                    };
                    if (cmd.StartDate != null) item.StartDate = cmd.StartDate.Value;
                    if (cmd.EndDate != null) item.EndDate = cmd.EndDate.Value;
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(204);
            poster.Priority.Should().Be(8);
            _dbContextMock.Tags.RemoveRange(tags);
            _dbContextMock.ElectionItems.Remove(poster);
            await _dbContextMock.SaveChangesAsync();
        }
    }
}