using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class AppUser: IdentityUser
    {
        public required string DisplayName { get; set; }
        //[JsonIgnore]
        public Address? Address { get; set; }
    }
}
