using MunShopApplication.Entities;
using MunShopApplication.Repository;
using MunShopApplication.Repository.SQLServer;

namespace MunShopApplication.Services
{
    public class OrderService
    {
        private readonly SQLServerOrderRepository _orderRepository;
        private readonly SQLServerProductRepository _productRepository;

        public OrderService(SQLServerOrderRepository orderRepository, SQLServerProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Order?> Add(Order order)
        {
            order.Id = Guid.NewGuid();

            foreach (var item in order.Items)
            {
                item.Id = Guid.NewGuid();
            }

            var result = await _orderRepository.Add(order);

            return result;
        }

        public async Task<Order?> Update(Order order)
        {
            if (!await _orderRepository.isExistedOrder(order.Id))
            {
                return null;
            }

            foreach (var item in order.Items)
            {
                if (!await _productRepository.FindById(item.ProductId))
                {
                    return null;
                }
            }

            var result = await _orderRepository.Update(order);

            return result;
        }

        public async Task<bool> Cancel(Guid orderId)
        {
            if (! await _orderRepository.isExistedOrder(orderId))
            {
                return false;
            }
               
            var result = await _orderRepository.Cancel(orderId);

            return result;
        }

        public async Task<Order?> FindByID(Guid orderId)
        {
            var result = await _orderRepository.FindById(orderId);

            return result;
        }

        public async Task<List<Order>?> Find(int skip, int take, DateTime beginDate, DateTime endDate)
        {
            var creterias = new OrderFindCreterias()
            {
                Skip = skip,
                Take = take,
                BeginDate = beginDate,
                EndDate = endDate
            };

            var result = await _orderRepository.Find(creterias);

            return result;
        }
    }
}
