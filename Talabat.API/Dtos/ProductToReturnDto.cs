using Talabat.Core.Entities;

namespace Talabat.API.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public required string Brand { get; set; }   // Navigational Property {one}
        public int CategoryId { get; set; }
        public required string Category { get; set; } //Navigational Property {one}
    }
}
