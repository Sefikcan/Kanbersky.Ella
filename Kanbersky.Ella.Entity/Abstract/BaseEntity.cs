using System.ComponentModel.DataAnnotations;

namespace Kanbersky.Ella.Entity.Abstract
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
