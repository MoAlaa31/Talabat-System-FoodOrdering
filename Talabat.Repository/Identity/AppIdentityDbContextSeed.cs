using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            // Seed, if necessary
            if (userManager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "ahmed.naser@linkdev.com",
                    UserName = "Ahmed.Nasr",
                    PhoneNumber = "01000000000"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
        }   }
    }
}
