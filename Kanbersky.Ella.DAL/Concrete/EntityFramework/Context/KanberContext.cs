using Kanbersky.Ella.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Kanbersky.Ella.DAL.Concrete.EntityFramework.Context
{
    public class KanberContext  :DbContext
    {
        public KanberContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
