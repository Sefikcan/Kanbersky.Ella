using Nest;

namespace Kanbersky.Ella.Business.DTO.Response
{
    public class ProductCategoryResponse
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string QuantityPerUnit { get; set; }

        public CompletionField Suggest { get; set; }
    }
}
