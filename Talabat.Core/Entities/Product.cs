using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey(nameof(Brand))]   //foreign key attribute
        public int BrandId { get; set; }    //foreign key column => ProductBrand
        //[InverseProperty(nameof(ProductBrand.Products))]   //inverse property attribute
        public ProductBrand Brand { get; set; }   // Navigational Property {one}

        //[ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; } //Navigational Property {one}
    }
}
