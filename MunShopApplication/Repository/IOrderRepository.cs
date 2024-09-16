using MunShopApplication.Entities;

namespace MunShopApplication.Repository
{
    public interface IOrderRepository
    {
        Task<Order?> Add(Order order);
    }
}
