using Kanbersky.Ella.Core.Entity;
using Kanbersky.Ella.Entity.Abstract;

namespace Kanbersky.Ella.Entity.Concrete
{
    public class Product : BaseEntity,IEntity
    {
        public string ProductName { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public decimal UnitPrice { get; set; }

        public string QuantityPerUnit { get; set; }

        public int UnitsInStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public virtual Category Category { get; set; }
    }
}
