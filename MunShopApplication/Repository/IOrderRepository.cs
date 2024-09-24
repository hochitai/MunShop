using MunShopApplication.Entities;

namespace MunShopApplication.Repository
{
    public interface IOrderRepository
    {
        Task<Order?> Add(Order order);
        Task<Order?> Update(Order order);
        Task<bool> Cancel(Guid orderId);
        Task<bool> isExistedOrder(Guid orderId);
        Task<Order?> FindById(Guid orderId);
        Task<List<Order>?> Find(OrderFindCreterias creterias);

    }
}
