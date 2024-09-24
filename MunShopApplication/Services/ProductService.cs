using MunShopApplication.Entities;
using MunShopApplication.Repository;
using MunShopApplication.Repository.SQLServer;

namespace MunShopApplication.Services
{
    public class ProductService
    {
        private readonly SQLServerProductRepository _productRepository;

        public ProductService(SQLServerProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> Add(Product product)
        {
            if (!ValidationProduct(product)) 
            {
                return null;
            }

            product.Id = Guid.NewGuid();

            var result = await _productRepository.Add(product);

            return result;

        }
        public async Task<Product?> Update(Product product)
        {
            if (product.Id == null || product.Id == Guid.Empty)
            {
                return null;
            }

            if (!ValidationProduct(product))
            {
                return null;
            }

            var result = await _productRepository.Update(product);

            return result;
        }
        public async Task<bool> Delete(Guid productId)
        {
            if (!await _productRepository.FindById(productId))
            {
                return false;
            }

            var result = await _productRepository.Delete(productId);

            return result;
        }
        public async Task<List<Product>?> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<List<Product>?> Find(int skip, int take, float minPrice, float maxPrice, string name, Guid categoryId)
        {
            var creterias = new ProductFindCreterias()
            {
                Skip = skip,
                Take = take,
            };
            if (minPrice > 0)
            {
                creterias.MinPrice = minPrice;
            }
            if(maxPrice > 0)
            {
                creterias.MaxPrice = maxPrice;
            }
            if (!string.IsNullOrEmpty(name))
            {
                creterias.Name = name;
            }
            if (categoryId != Guid.Empty && categoryId != null )
            {
                creterias.CategoryId = categoryId;
            }

            return await _productRepository.Find(creterias);
        }

        private static bool ValidationProduct(Product product)
        {
            if (product.Id == null || product.Id == Guid.Empty)
            {
                return false;
            }

            if (string.IsNullOrEmpty(product.Name))
            {
                return false;
            }

            if (!float.IsNormal(product.Price))
            {
                return false;
            }

            if (string.IsNullOrEmpty(product.Description))
            {
                return false;
            }

            if (product.CategoryId == null || product.CategoryId == Guid.Empty)
            {
                return false;
            }
            return true;
        }
    }
}
