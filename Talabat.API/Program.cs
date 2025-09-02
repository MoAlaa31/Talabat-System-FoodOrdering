using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.Extensions;
using Talabat.API.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var WebApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            //ConfigureServices method is used to register services in the container.
            WebApplicationBuilder.Services.AddControllers()/*.AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })*/;
            //A way to solve the problem of reference looping
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            WebApplicationBuilder.Services.AddSwaggerServices();

            WebApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            WebApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            WebApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = WebApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            WebApplicationBuilder.Services.AddIdentityServices(WebApplicationBuilder.Configuration);
            WebApplicationBuilder.Services.AddApplicationServices();

            #endregion

            var app = WebApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();
            //ask CLR for creating object from DBContext Explicitly
            var _identityDbContext =  services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync();  //update database
                await _identityDbContext.Database.MigrateAsync();  //update database
                await StoreContextSeed.SeedAsync(_dbContext);   //Data seeding

                var userManager = services.GetRequiredService<UserManager<AppUser>>();   //Explicitly ask CLR to create object from UserManager
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);   //Data seeding
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
            }

            #region Configure Kestral Middlewares
            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
