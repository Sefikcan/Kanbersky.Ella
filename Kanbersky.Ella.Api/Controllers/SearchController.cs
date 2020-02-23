using System.Linq;
using System.Threading.Tasks;
using Kanbersky.Ella.Business.Abstract;
using Kanbersky.Ella.Business.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace Kanbersky.Ella.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        #region fields

        private readonly IAutoCompleteService<ProductCategoryResponse> _completeService;
        private readonly IProductService _productService;

        #endregion

        #region ctor

        public SearchController(IAutoCompleteService<ProductCategoryResponse> completeService,
            IProductService productService)
        {
            _completeService = completeService;
            _productService = productService;
        }

        #endregion

        #region methods

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string query, [FromQuery]int page=0, [FromQuery]int pageSize = 10)
        {
            var result = await _completeService.Search(query, page, pageSize);
            return Ok(result);
        }

        [HttpGet("auto-complete")]
        public async Task<IActionResult> Autocomplete([FromQuery]string query)
        {
            var result = await _completeService.AutoComplete(query);
            return Ok(result);
        }

        [HttpPost("create-ella-index")]
        public async Task<IActionResult> CreateIndex()
        {
            var entity = _productService.ProductCategoryResponses();
            var searchResponse = entity.Select(x => new ProductCategoryResponse
            {
                ProductId = x.ProductId,
                CategoryId = x.CategoryId,
                ProductName = x.ProductName,
                CategoryName = x.CategoryName,
                QuantityPerUnit = x.QuantityPerUnit,
                Suggest = new Nest.CompletionField
                {
                    Input = new[] { x.ProductName, x.CategoryName }
                }
            });
            var result = await _completeService.CreateIndex(searchResponse.ToList());
            return Ok(result);
        }

        #endregion
    }
}