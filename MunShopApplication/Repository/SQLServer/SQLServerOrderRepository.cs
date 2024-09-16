using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using System.Data;
using System.Data.Common;

namespace MunShopApplication.Repository.SQLServer
{
    public class SQLServerOrderRepository : IOrderRepository
    {
        private const string INSERT_COMMAND = "INSERT INTO orders VALUES (@OrderId, @UserId, @Total)";
        private const string INSERT_ITEM_COMMAND = "INSERT INTO orderItems VALUES (@OrderItemId, @OrderId, @ProductId, @Price, @Quantity)";

        private readonly SqlConnection _connection;
        public SQLServerOrderRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<Order?> Add(Order order)
        {
            DbTransaction? transaction = null;
            try
            {
                await _connection.OpenAsync();
                transaction = await _connection.BeginTransactionAsync();
                
                var cmd = _connection.CreateCommand();
                cmd.CommandText = INSERT_COMMAND;

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
            catch (Exception ex) 
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
    }
}
