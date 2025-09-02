using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity;
using System.Text;
using Talabat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Talabat.API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IScheduleService, ScheduleService>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            //adds an entity framework implementation of identity information stores
            //this as a whole code add the default identity system configuration for AppUser and IdentityRole (dependency injection container)

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //use jwt token to authenticate
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //doesnt redirect to login page but to the default challenge scheme
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //default scheme for other auth
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(double.Parse(configuration["JWT:ExpiryInMinutes"]))
                    };
                });

            return services;
        }
    }
}
