using Azure.Core;
using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using System.Data;
using System.Data.Common;

namespace MunShopApplication.Repository.SQLServer
{
    public class SQLServerOrderRepository : IOrderRepository
    {
        private const string INSERT_COMMAND = "INSERT INTO orders(Id,user_id, total) VALUES (@OrderId, @UserId, @Total)";
        private const string INSERT_ITEM_COMMAND = "INSERT INTO orderItems(id, order_id, product_id, price, quantity) VALUES (@OrderItemId, @OrderId, @ProductId, @Price, @Quantity)";
        private const string CANCEL_ORDER_COMMAND = "UPDATE orderS SET is_canceled = 1 WHERE id = @OrderId";
        private const string SELECT = "SELECT";
        private const string IS_EXISTED_ORDER_QUERY = " id FROM orders WHERE id = @OrderId";
        private const string FIND_BY_ID_QUERY = " id, user_id, total, created_at FROM orders WHERE id = @OrderId AND is_canceled = 0";
        private const string FIND_ITEMS_QUERY = " id, product_id, price, quantity FROM orderItems WHERE order_id = @OrderId";

        private readonly SqlConnection _connection;
        public SQLServerOrderRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<Order?> Add(Order order)
        {
            SqlTransaction? transaction = null;
            try
            {
                await _connection.OpenAsync();
                transaction = _connection.BeginTransaction();
                
                var cmd = _connection.CreateCommand();
                cmd.CommandText = INSERT_COMMAND;
                cmd.Transaction = transaction;

                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier)).Value = order.Id;
                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)).Value = order.UserId;
                cmd.Parameters.Add(new SqlParameter("@Total", SqlDbType.Float)).Value = order.Total;

                if (await cmd.ExecuteNonQueryAsync() > 0)
                {
                    cmd.CommandText = INSERT_ITEM_COMMAND;
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add(new SqlParameter("@OrderItemId", SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float));
                    cmd.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int));
                    
                    cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier)).Value = order.Id;

                    foreach (var item in order.Items) 
                    {
                        cmd.Parameters["@OrderItemId"].Value = item.Id;
                        cmd.Parameters["@ProductId"].Value = item.ProductId;
                        cmd.Parameters["@Price"].Value = item.Price;
                        cmd.Parameters["@Quantity"].Value = item.Quantity;

                        if (await cmd.ExecuteNonQueryAsync() == 0)
                        {
                            throw new Exception("Error inserting OrderItems");
                        }
                    }

                    await transaction.CommitAsync();

                    return order;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                if (transaction != null) 
                { 
                    await transaction.CommitAsync();
                }
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<bool> Cancel(Guid orderId)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = CANCEL_ORDER_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier)).Value = orderId;

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> isExistedOrder(Guid orderId)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = SELECT + IS_EXISTED_ORDER_QUERY;

                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier)).Value = orderId;

                return await cmd.ExecuteScalarAsync() != null;
            }
            catch
            {
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<Order?> FindById(Guid orderId)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = SELECT + FIND_BY_ID_QUERY;

                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier)).Value = orderId;

                var reader = await cmd.ExecuteReaderAsync();
                var order = new Order();

                if (order != null && reader.Read())
                {
                    order.Id = reader.GetGuid(0);
                    order.UserId = reader.GetGuid(1);
                    order.Total = (float) reader.GetDouble(2);
                    order.CreatedAt = reader.GetDateTime(3);
                    reader.Close();

                    cmd.CommandText = SELECT + FIND_ITEMS_QUERY;
                    reader = await cmd.ExecuteReaderAsync();
                    var items = new List<OrderItem>();

                    while (reader != null && reader.Read())
                    {
                        var item = new OrderItem()
                        {
                            Id = reader.GetGuid(0),
                            ProductId = reader.GetGuid(1),
                            Price = (float)reader.GetDouble(2),
                            Quantity = reader.GetInt32(3),
                        };

                        items.Add(item);
                    }

                    order.Items = items;    
                }

                return order;
            }
            catch
            {
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
