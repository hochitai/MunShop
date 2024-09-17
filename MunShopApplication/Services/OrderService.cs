﻿using MunShopApplication.Entities;
using MunShopApplication.Repository.SQLServer;

namespace MunShopApplication.Services
{
    public class OrderService
    {
        private readonly SQLServerOrderRepository _orderRepository;

        public OrderService(SQLServerOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
    }
}
