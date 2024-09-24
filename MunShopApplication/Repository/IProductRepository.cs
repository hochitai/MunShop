using MunShopApplication.Entities;

namespace MunShopApplication.Repository
{
    public interface IProductRepository
    {
        Task<Product?> Add(Product product);
        Task<Product?> Update(Product product);
        Task<bool> Delete(Guid productId);
        Task<List<Product>?> GetAll();
        Task<bool> FindById(Guid productId);
        Task<List<Product>?> Find(ProductFindCreterias creterias);
    }
}
