using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();   // Navigational Property {many}
    }
}
