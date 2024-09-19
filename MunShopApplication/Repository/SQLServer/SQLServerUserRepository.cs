using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace MunShopApplication.Repository.SQLServer
{
    public class SQLServerUserRepository : IUserRepository
    {
        private const string INSERT_COMMAND = "INSERT INTO users(id, name, username, password, salt, email, role_id) " +
            "VALUES (@UserId, @Name, @Username, @Password, @Salt, @Email, @RoleId)";
        private const string UPDATE_COMMAND = "UPDATE users SET name = @Name, email = @Email WHERE Id = @UserId";
        private const string SELECT = "SELECT ";
        private const string FIND_BY_ID_QUERY = "SELECT id, name, email, role_id FROM users WHERE id = @UserId";
        private const string FIND_BY_NAME_QUERY = "SELECT id, name, username, password, email, role_id FROM users WHERE username = @Username";

        private readonly SqlConnection _connection;
        
        public SQLServerUserRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<User?> Add(User user)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = INSERT_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)).Value = user.Id;
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 100)).Value = user.Name;
                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 100)).Value = user.Username;
                cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 255)).Value = user.Password;
                cmd.Parameters.Add(new SqlParameter("@Salt", SqlDbType.NVarChar, 255)).Value = user.Salt;
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = user.Email;
                cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.Int)).Value = user.RoleId;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return null;
                }

                return user;
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

        public async Task<User?> Update(User user)
        {
            try
            {
                await _connection.OpenAsync();

                var cmd = _connection.CreateCommand();
                cmd.CommandText = UPDATE_COMMAND;

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 100)).Value = user.Name;
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = user.Email;

                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)).Value = user.Id;

                if (await cmd.ExecuteNonQueryAsync() < 0)
                {
                    return null;
                }
                return user;
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

        public async Task<User?> GetByUsername(string username)
        {
            try
            {
                _connection.Open();
                var cmd = _connection.CreateCommand();

                cmd.CommandText = FIND_BY_NAME_QUERY;

                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 100)).Value = username;

                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null )
                {
                    reader.Read();
                    return new User()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Username = reader.GetString(2),
                        Password = reader.GetString(3),
                        Email = reader.GetString(4),
                        RoleId = reader.GetInt32(5),
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
