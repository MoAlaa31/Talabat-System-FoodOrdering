using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext _dbContext)
        {
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                //read JSON file and close it after reading
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                //Deserialize JSON file to List of ProductBrand objects
                //serialize means converting object to JSON
                if (brands?.Count() > 0)
                {
                    brands = brands.Select(b => new ProductBrand
                    {
                        Name = b.Name
                    }).ToList();

                    foreach (var item in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                } 
            }

            if (_dbContext.ProductCategories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                //read JSON file and close it after reading
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
                //Deserialize JSON file to List of ProductBrand objects
                //serialize means converting object to JSON
                if (categories?.Count() > 0)
                {
                    categories = categories.Select(b => new ProductCategory
                    {
                        Name = b.Name
                    }).ToList();

                    foreach (var item in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }

            if (_dbContext.Products.Count() == 0)
            {
                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                //read JSON file and close it after reading
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                //Deserialize JSON file to List of ProductBrand objects
                //serialize means converting object to JSON
                if (products?.Count() > 0)
                {
                    foreach (var item in products)
                    {
                        _dbContext.Set<Product>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
