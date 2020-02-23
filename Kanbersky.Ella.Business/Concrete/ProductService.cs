using Kanbersky.Ella.Business.Abstract;
using Kanbersky.Ella.Business.DTO.Response;
using Kanbersky.Ella.DAL.Concrete.EntityFramework.GenericRepository;
using Kanbersky.Ella.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Kanbersky.Ella.Business.Concrete
{
    public class ProductService : IProductService
    {
        #region fields

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;

        #endregion

        #region ctor

        public ProductService(IGenericRepository<Product> productRepository,
            IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region methods

        public IEnumerable<ProductCategoryResponse> ProductCategoryResponses()
        {
            var product = _productRepository.GetQueryable();
            var category = _categoryRepository.GetQueryable();

            var productCategory = from p in product
                                  join c in category on p.CategoryId equals c.Id
                                  select new
                                  {
                                      p.Id,
                                      CategoryId = c.Id,
                                      p.ProductName,
                                      c.CategoryName,
                                      p.QuantityPerUnit
                                  };

            var ProductCategoryResponse = productCategory.Select(x =>new ProductCategoryResponse
            { 
                ProductId = x.Id,
                CategoryId = x.CategoryId,
                ProductName = x.ProductName,
                CategoryName = x.CategoryName,
                QuantityPerUnit = x.QuantityPerUnit
            });
            return ProductCategoryResponse.ToList();
        }

        #endregion
    }
}
