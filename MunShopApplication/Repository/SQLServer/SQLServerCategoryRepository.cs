using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace MunShopApplication.Repository.SQLServer
{
    public class SQLServerCategoryRepository : ICategoryRepository
    {
        private const string INSERT_COMMAND = "INSERT INTO categories(id, name) VALUES (@CategoryId, @Name)";
        private const string UPDATE_COMMAND = "UPDATE categories SET name = @Name WHERE Id = @CategoryId";
        private const string SELECT = "SELECT ";
        private const string FIND_ALL = " id, name FROM categories WHERE (1=1)";
        private const string FIND_BY_ID_QUERY = "SELECT id, name FROM categories WHERE id = @CategoryId";
        private const string DELETE_BY_ID = "DELETE FROM categories WHERE Id = @CategoryId";

        private readonly SqlConnection _connection;
        
        public SQLServerCategoryRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<Category?> Add(Category category)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = INSERT_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)).Value = category.Id;
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 100)).Value = category.Name;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return null;
                }
                return category;
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

        public async Task<Category?> Update(Category category)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = UPDATE_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 100)).Value = category.Name;
                cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)).Value = category.Id;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return null;
                }
                return category;
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

        public async Task<bool> Delete(Guid categoryId)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = DELETE_BY_ID;

                cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)).Value = categoryId;

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

        public async Task<List<Category>?> GetAll()
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = SELECT + FIND_ALL;

                using var reader = await cmd.ExecuteReaderAsync();
                var categories = new List<Category>();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                        });

                    }
                }
                return categories;
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

        public async Task<Category?> FindById(Guid categoryId)
        {
            try
            {
                _connection.Open();
                var cmd = _connection.CreateCommand();

                cmd.CommandText = FIND_BY_ID_QUERY;

                cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier)).Value = categoryId;

                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null )
                {
                    reader.Read();
                    return new Category()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
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
