using Kanbersky.Ella.Business.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanbersky.Ella.Business.Abstract
{
    public interface IAutoCompleteService<T> where T: ProductCategoryResponse
    {
        Task<SearchResult<T>> Search(string query, int page, int pageSize);

        Task<List<AutoCompleteResult>> AutoComplete(string query);

        Task<bool> CreateIndex(List<T> entity);
    }
}
