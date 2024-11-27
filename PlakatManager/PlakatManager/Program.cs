

using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Entities.Seeders;
using ElectionMaterialManager.Mappings;
using ElectionMaterialManager.Utilities;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using FluentValidation;
using System.Reflection;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.EditElectionItem;
using FluentValidation.AspNetCore;
using ElectionMaterialManager.Services;

namespace ElectionMaterialManager
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<Seeder>();
            //builder.Services.AddScoped<UserToAspUsersMigrationUtility>();
         

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            builder.Services.AddValidatorsFromAssemblyContaining<EditElectionItemCommand>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            /*builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssembly(typeof(SignUpRequestModelValidator).Assembly);*/

            builder.Services.AddAuthorization();

            var app = builder.Build();

        
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

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
           
           // var userMigration = scope.ServiceProvider.GetRequiredService<UserToAspUsersMigrationUtility>();
           //await userMigration.Migrate(dbContext, scope.ServiceProvider); //!!!!
           


            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if(pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }


                       


            


            /*app.MapGet("api/v1/user/comments", async (int id, ElectionMaterialManagerContext db) =>
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

            });*/

          

            app.Run();

          
        }
    }
}
