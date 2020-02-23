using Kanbersky.Ella.Business.DTO.Response;
using System.Collections.Generic;

namespace Kanbersky.Ella.Business.Abstract
{
    public interface IProductService
    {
        IEnumerable<ProductCategoryResponse> ProductCategoryResponses();
    }
}
