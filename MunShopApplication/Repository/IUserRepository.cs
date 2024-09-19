using MunShopApplication.Entities;

namespace MunShopApplication.Repository
{
    public interface IUserRepository
    {
        Task<User?> Add(User user);
        Task<User?> GetByUsername(string username);
        Task<User?> Update(User user);
    }
}
