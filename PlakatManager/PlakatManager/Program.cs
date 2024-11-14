
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using PlakatManager.Dtos;
using PlakatManager.Entities;
using PlakatManager.Entities.Seeders;
using PlakatManager.Mappings;
using PlakatManager.Utilities;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlakatManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.RegisterFactories();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddDbContext<PlakatManagerContext>(
                 option => option.UseSqlServer(builder.Configuration.GetConnectionString("pManagerConnectionString"))
            );

            builder.Services.AddScoped<Seeder>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


         
            using var scope = app.Services.CreateScope();
           
            var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
       
            seeder.Seed();

            var dbContext = scope.ServiceProvider.GetService<PlakatManagerContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if(pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }


            app.MapGet("election-items", async (PlakatManagerContext db, int indexRangeStart = 1, int indexRangeEnd = 10) =>
            {
                var electionItems = await db.ElectionItems.Where(x=> x.Id >= indexRangeStart && x.Id <= indexRangeEnd).ToListAsync();

                if (electionItems == null)
                {
                    var errorResponse = new
                    {
                        status = 404,
                        message = $"Election items withing given range not found.",
                        details = "The requested items does not exist in the database."
                    };
                    return Results.NotFound(errorResponse);
                }

                return Results.Ok(electionItems);
            });

            app.MapGet("election-item/{id}", async (int id, PlakatManagerContext db) =>
            {
                var electionItem = await db.ElectionItems.FirstOrDefaultAsync(x => x.Id == id);

                if (electionItem == null)
                {
                    var errorResponse = new
                    {
                        status = 404,
                        message = $"Election item with id {id} not found.",
                        details = "The requested item does not exist in the database."
                    };
                    return Results.NotFound(errorResponse);
                }

                return Results.Ok(electionItem);
            });


            app.MapDelete("election-item/{id}", async (int id, PlakatManagerContext db) =>
            {
                var electionItem = await db.ElectionItems.FirstAsync(x => x.Id.Equals(id));

                if (electionItem == null)
                {
                    var errorResponse = new
                    {
                        status = 404,
                        message = $"Election item with id {id} not found.",
                        details = "The requested item does not exist in the database."
                    };
                    return Results.NotFound(errorResponse);
                }

                db.Remove(electionItem);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });


            app.MapPatch("election-item/{id}", async (int id, PlakatManagerContext db) =>
            {
                var electionItem = await db.ElectionItems.FirstAsync(x=>x.Equals(id));

                if(electionItem == null)
                {
                    var errorResponse = new
                    {
                        status = 404,
                        message = $"Election item with id {id} not found.",
                        details = "The requested item does not exist in the database."
                    };
                    return Results.NotFound(errorResponse);
                }

                db.Remove(electionItem);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapPost("election-item/led", async (LEDRequestDTO dto, PlakatManagerContext db) =>
            {
                var led = new LED
                {
                    Area = dto.Area,

                    Latitude = (double)dto.Latitude,
                    Longitude = (double)dto.Longitude,
                    Priority = (int)dto.Priority,
                    Size = dto.Size,
                    Cost = (decimal)dto.Cost,
                    StatusId = dto.StatusId,
                    AuthorId = dto.AuthorId,
                    RefreshRate = (int)dto.RefreshRate,
                    Resolution = dto.Resolution,
                };
                db.Add(led);
                await db.SaveChangesAsync();

                return led.Id;
            });

            app.MapPost("election-item/poster", async (PosterRequestDTO dto, PlakatManagerContext db) =>
            {
                var poster = new Poster
                {
                    Area = dto.Area,
                    Latitude = (double)dto.Latitude,
                    Longitude = (double)dto.Longitude,
                    Priority = (int)dto.Priority,
                    Size = dto.Size,
                    Cost = (decimal)dto.Cost,
                    StatusId = dto.StatusId,
                    AuthorId = dto.AuthorId,
                    PaperType = dto.PaperType
                    
                };
                db.Add(poster);
                await db.SaveChangesAsync();

                return poster.Id;
            });

            app.MapPost("election-item/billboard", async (BillboardRequestDTO dto, PlakatManagerContext db) =>
            {
                var billboard = new Billboard
                {
                    Area = dto.Area,
                    Latitude = (double)dto.Latitude,
                    Longitude = (double)dto.Longitude,
                    Priority = (int)dto.Priority,
                    Size = dto.Size,
                    Cost = (decimal)dto.Cost,
                    StatusId = dto.StatusId,
                    AuthorId = dto.AuthorId,
                    StartDate = (DateTime)dto.StartDate,
                    EndDate = (DateTime)dto.EndDate
                };
                db.Add(billboard);
                await db.SaveChangesAsync();

                return billboard.Id;
            });

            app.MapPost("election-item", async (ElectionItemRequestDTO dto, PlakatManagerContext db, ElectionItemFactoryRegistry factoryRegistry) =>
            {

                var type = dto.Type;
                var electionItem = factoryRegistry.CreateElectionItem(type, dto);

                db.Add(electionItem);
                await db.SaveChangesAsync();

                return electionItem;
            });

            app.MapGet("data2", (PlakatManagerContext db) =>
            {

                var authorsCommentsCount = db.Comments
                .GroupBy(x => x.AuthorId)
                .Select(g => new { g.Key, Count = g.Count() })
                //.OrderByDescending(x=>x.Count)
                .ToList();

                //return authorsCommentsCount;

                var topAuthor = authorsCommentsCount.First(x => x.Count == authorsCommentsCount.Max(x => x.Count));

                var userDetails = db.Users.First(x => x.Id == topAuthor.Key);
                return new { userDetails, commentsCount = topAuthor.Count };
                //var topAuthor = db.Users.Where(x=>x.Id == topcomments.Key)

            });

            app.Run();

          
        }
    }
}
