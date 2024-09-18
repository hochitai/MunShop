using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace MunShopApplication.Repository.SQLServer
{
    public class SQLServerProductRepository : IProductRepository
    {
        private const string INSERT_COMMAND = "INSERT INTO products(id, name, price, description, category_id) VALUES (@ProductId, @Name, @Price, @Description, @CategoryId)";
        private const string UPDATE_COMMAND = "UPDATE products SET name = @Name, price = @Price, description = @Description, category_id = @CategoryId WHERE Id = @ProductId";
        private const string SELECT = "SELECT ";
        private const string FIND_ALL = "id, name, price, description, category_id FROM products WHERE (1=1)";
        private const string FIND_BY_ID_QUERY = "SELECT id, name, price, description, category_id FROM products WHERE id = @ProductId";
        private const string DELETE_BY_ID = "DELETE FROM products WHERE Id = @ProductId";

        private readonly SqlConnection _connection;
        
        public SQLServerProductRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<Product?> Add(Product product)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = INSERT_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier)).Value = product.Id;
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255)).Value = product.Name;
                cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float)).Value = product.Price;
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 255)).Value = product.Description;
                cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)).Value = product.CategoryId;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return null;
                }
                return product;
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

        public async Task<Product?> Update(Product product)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = UPDATE_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255)).Value = product.Name;
                cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float)).Value = product.Price;
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 255)).Value = product.Description;
                cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)).Value = product.CategoryId;
                
                cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier)).Value = product.Id;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return null;
                }
                return product;
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

        public async Task<bool> Delete(Guid productId)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = DELETE_BY_ID;

                cmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier)).Value = productId;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return false;
                }
                return true;
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

        public async Task<List<Product>?> GetAll()
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = SELECT + FIND_ALL;

                using var reader = await cmd.ExecuteReaderAsync();
                var products = new List<Product>();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Price = (float) reader.GetDouble(2),
                            Description = reader.GetString(3),
                            CategoryId = reader.GetGuid(4),
                        });

                    }
                }
                return products;
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

        public async Task<Product?> FindById(Guid productId)
        {
            try
            {
                _connection.Open();
                var cmd = _connection.CreateCommand();

                cmd.CommandText = FIND_BY_ID_QUERY;

                cmd.Parameters.Add(new SqlParameter("@productId", SqlDbType.UniqueIdentifier)).Value = productId;

                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null )
                {
                    reader.Read();
                    return new Product()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Price = (float) reader.GetDouble(2),
                        Description = reader.GetString(3),
                        CategoryId = reader.GetGuid(4),
                    };
                }
                else
                {
                    return null;
                }
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
