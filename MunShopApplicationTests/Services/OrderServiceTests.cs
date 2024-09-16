using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MunShopApplication.Controllers;
using MunShopApplication.Entities;
using MunShopApplication.Repository.SQLServer;
using MunShopApplication.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunShopApplication.Services.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {

        private OrdersController _ordersController;
        private Mock<OrderService> _mockOrderService;
        private Mock<SQLServerOrderRepository> _mockOrderRepository;

        [TestInitialize]
        public void Setup()
        {
            var sqlConnection = new SqlConnection("Server=.;Database=MunShop;Trusted_Connection=True;TrustServerCertificate=True;");
            _mockOrderRepository = new Mock<SQLServerOrderRepository>(sqlConnection);
            _mockOrderService = Substitute.For<OrderService>(_mockOrderRepository);
            _ordersController = new OrdersController(_mockOrderService.Object);
        }

        [TestMethod()]
        public async Task CreateTestAsync()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order()
            {
                Total = 999 + 999,
                UserId = Guid.Empty,
                Id = orderId,
                Items =
                [
                    new OrderItem()
                    {
                        Id = new Guid("00000000-0000-0000-0001-000000000001"),
                        OrderId = orderId,
                        Price = 999,
                        ProductId = new Guid("00000000-0000-0000-0000-000000000001"),
                        Quantity = 1
                    },
                    new OrderItem()
                    {
                        Id = new Guid("00000000-0000-0000-0001-000000000002"),
                        OrderId = orderId,
                        Price = 999,
                        ProductId = new Guid("00000000-0000-0000-0000-000000000002"),
                        Quantity = 1
                    }
                ]
            };

            mockOrderService.Create(order).Returns(order);

            // Act
            var result = await _ordersController.Create(order);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)result).Value, typeof(Order));

        }
    }
}