
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Entities.Seeders;
using ElectionMaterialManager.Mappings;
using ElectionMaterialManager.Services;
using ElectionMaterialManager.Utilities;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

namespace ElectionMaterialManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ElectionAssetManagerAPI",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
               });
            });
            builder.Services.RegisterFactories();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddDbContext<ElectionMaterialManagerContext>(
                 option => option.UseSqlServer(builder.Configuration.GetConnectionString("pManagerConnectionString"))
            );

            builder.Services.ConfigureAuthAndJwt(builder.Configuration);

            builder.Services.AddScoped<Seeder>();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // W³¹cz autoryzacjê i uwierzytelnianie
            app.UseAuthentication();
            app.UseAuthorization();

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

            var dbContext = scope.ServiceProvider.GetService<ElectionMaterialManagerContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if(pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }


            app.MapPost("api/v1/authenticate/login", [AllowAnonymous] async (LoginRequestDTO request, UserManager<IdentityUser> userManager, AuthService service) =>
            {
                var user = await userManager.FindByNameAsync(request.Login);
                if (user == null) return Results.Unauthorized();

                var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
                if (!passwordValid) return Results.Unauthorized();

                string token = service.CreateToken(user);

                return Results.Ok(new { BearerToken = token });

            });

            app.MapPost("api/v1/authenticate/register", async (RegisterRequestDTO request, UserManager<IdentityUser> userManager) =>
            {
                var userFromDb = await userManager.FindByNameAsync(request.Username);
                if (userFromDb != null) return Results.Conflict(new { message = "User already exists" });

                var user = new IdentityUser { UserName = request.Username, Email = request.Email };
                var response = await userManager.CreateAsync(user, request.Password);
                if (!response.Succeeded) return Results.BadRequest(response.Errors);

                return Results.Created();
            });


            app.MapGet("api/v1/election-items", async (ElectionMaterialManagerContext db, int indexRangeStart = 1, int indexRangeEnd = 10) =>
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

            app.MapGet("api/v1/election-item/{id}", async (int id, ElectionMaterialManagerContext db) =>
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


            app.MapDelete("api/v1/election-item/{id}", async (int id, ElectionMaterialManagerContext db) =>
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


            app.MapPatch("api/v1/election-item/{id}", async (int id, ElectionMaterialManagerContext db) =>
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

            app.MapPost("api/v1/election-item/led", async (LEDRequestDTO dto, ElectionMaterialManagerContext db) =>
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

            app.MapPost("api/v1/election-item/poster", async (PosterRequestDTO dto, ElectionMaterialManagerContext db) =>
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

            app.MapPost("api/v1/election-item/billboard", async (BillboardRequestDTO dto, ElectionMaterialManagerContext db) =>
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

            app.MapPost("api/v1/election-item", async (ElectionItemRequestDTO dto, ElectionMaterialManagerContext db, ElectionItemFactoryRegistry factoryRegistry) =>
            {

                var type = dto.Type;
                var electionItem = factoryRegistry.CreateElectionItem(type, dto);

                db.Add(electionItem);
                await db.SaveChangesAsync();

                return electionItem;
            });


            app.MapGet("api/v1/user/comments", async (int id, ElectionMaterialManagerContext db) =>
            {
                var user = await db.Users
                .Where(x=>x.Id == id)
                .Include(x => x.Comments)
                .Select(x => new
                {
                    x.FirstName,
                    x.Comments
                })
                .ToListAsync();

                return user;
            }).RequireAuthorization();


            app.MapGet("data2", (ElectionMaterialManagerContext db) =>
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
