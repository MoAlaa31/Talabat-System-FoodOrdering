using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Talabat.Core.Entities.Identity
{
    public class Address
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string AppUserId { get; set; }  // Navigational Property {one} Foreign Key
        //[JsonIgnore]
        //second way to solve the circular reference problem
        public AppUser User { get; set; }    // Navigational Property {one}
    }
}