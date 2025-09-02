using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.Services
{
    public class ProductService : IProductService
    {
        public Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            //var spec = new ProductWithBrandAndCategorySpecifications();
            throw new NotImplementedException();

        }
        public Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

    }
}
