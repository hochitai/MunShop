using MunShopApplication.Entities;
using MunShopApplication.Repository.SQLServer;

namespace MunShopApplication.Services
{
    public class CategoryService
    {
        private readonly SQLServerCategoryRepository _categoryRepository;

        public CategoryService(SQLServerCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category?> Add(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return null;
            }

            category.Id = Guid.NewGuid();

            var result = await _categoryRepository.Add(category);

            return result;

        }
        public async Task<Category?> Update(Category category)
        {
            if (category.Id == null || category.Id == Guid.Empty)
            {
                return null;
            }

            if (string.IsNullOrEmpty(category.Name))
            {
                return null;
            }

            var result = await _categoryRepository.Update(category);

            return result;
        }
        public async Task<bool> Delete(Guid categoryId)
        {
            var categoryInDb = await _categoryRepository.FindById(categoryId);

            if (categoryInDb == null)
            {
                return false;
            }

            var result = await _categoryRepository.Delete(categoryId);

            return result;
        }
        public async Task<List<Category>?> GetAll()
        {
            return await _categoryRepository.GetAll();
        }
        
    }
}
