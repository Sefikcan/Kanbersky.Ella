using Kanbersky.Ella.Core.Entity;
using Kanbersky.Ella.Entity.Abstract;
using System.Collections.Generic;

namespace Kanbersky.Ella.Entity.Concrete
{
    public class Category : BaseEntity,IEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
