using Kanbersky.Ella.Business.Abstract;
using Kanbersky.Ella.Business.DTO.Response;
using Kanbersky.Ella.Business.Helpers;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbersky.Ella.Business.Concrete
{
    public class AutoCompleteService<T> : IAutoCompleteService<T> where T: ProductCategoryResponse
    {
        #region const

        private readonly string IndexName = "product_search";

        #endregion

        #region fields

        private readonly ElasticClient _elasticClient;

        #endregion

        #region ctor

        public AutoCompleteService(ElasticClientProvider clientProvider)
        {
            _elasticClient = clientProvider.Client;
        }

        #endregion

        #region methods

        public async Task<bool> CreateIndex(List<T> entity)
        {
            var indexIsExists = await _elasticClient.Indices.ExistsAsync(IndexName);

            if (indexIsExists.Exists)
            {
                await _elasticClient.Indices.DeleteAsync(IndexName);
            }

            CreateIndexResponse createIndex = await _elasticClient.Indices.CreateAsync(IndexName, p => p
            .Map(r => r.AutoMap()));

            var indexIsValid = createIndex.IsValid;

            if (indexIsValid)
            {
                var indexCreated = await _elasticClient.IndexManyAsync(entity, index: IndexName);
                return indexCreated.IsValid;
            }

            return false;
        }

        public async Task<SearchResult<T>> Search(string query, int page, int pageSize)
        {
            var dynamicQuery = new List<QueryContainer>
            {
                Query<T>.Match(t => t.Field(new Field("productName")).Query(query)),
                Query<T>.Match(t => t.Field(new Field("categoryName")).Query(query)),
            };

            var result = await _elasticClient.SearchAsync<T>(s =>
            s.From(page)
            .Size(pageSize)
            .Index(IndexName)
            .Query(q => q
            .Bool(b => b
            .Should(dynamicQuery.ToArray()))
            ));

            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }

            //elastic'den dönen değerleri searchresult modelimize set ediyoruz
            return new SearchResult<T>
            {
                Total = result.Total,
                ElapsedMilliseconds = result.Took,
                Page = page,
                PageSize = pageSize,
                Results = result.Documents
            };
        }

        public async Task<List<AutoCompleteResult>> AutoComplete(string query)
        {
            var response = await _elasticClient.SearchAsync<T>(sr => sr
            .Index(IndexName).Suggest(scd => scd
                    .Completion(IndexName, cs => cs
                        .Prefix(query)
                        .Fuzzy(fsd => fsd
                            .Fuzziness(Fuzziness.Auto))
                        .Field(r => r.ProductName)
                        )));

            var suggestions = this.ExtractAutocompleteSuggestions(response);
            return suggestions;
        }

        private List<AutoCompleteResult> ExtractAutocompleteSuggestions(ISearchResponse<T> response)
        {
            List<AutoCompleteResult> results = new List<AutoCompleteResult>();
            var suggestions = response.Suggest["recipe-name-completion"].Select(s => s.Options);
            foreach (var suggestionsCollection in suggestions)
            {
                foreach (var suggestion in suggestionsCollection)
                {
                    var suggestedRecipe = suggestion.Source;

                    var autocompleteResult = new AutoCompleteResult
                    {
                        Id = suggestedRecipe.ProductId.ToString(),
                        Name = suggestedRecipe.ProductName
                    };

                    results.Add(autocompleteResult);
                }
            }

            return results;
        }

        #endregion
    }
}
