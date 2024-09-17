using MunShopApplication.Entities;

namespace MunShopApplication.Repository
{
    public interface ICategoryRepository
    {
        Task<Category?> Add(Category category);
        Task<Category?> Update(Category category);
        Task<bool> Delete(Guid categoryId);
        Task<List<Category>?> GetAll();
        Task<Category?> FindById(Guid categoryId);
    }
}
