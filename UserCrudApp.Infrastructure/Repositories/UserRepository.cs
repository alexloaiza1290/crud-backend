using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UserCrudApp.Application.Interfaces;
using UserCrudApp.Domain.Entities;

namespace UserCrudApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (IDbConnection db = CreateConnection())
            {
                var sql = "SELECT Id, Username, Email FROM Users";
                return await db.QueryAsync<User>(sql);
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using (IDbConnection db = CreateConnection())
            {
                var sql = "SELECT Id, Username, Email FROM Users WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(User user)
        {
            using (IDbConnection db = CreateConnection())
            {
                var sql = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email)";
                await db.ExecuteAsync(sql, user);
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (IDbConnection db = CreateConnection())
            {
                var sql = "UPDATE Users SET Username = @Username, Email = @Email WHERE Id = @Id";
                await db.ExecuteAsync(sql, user);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (IDbConnection db = CreateConnection())
            {
                var sql = "DELETE FROM Users WHERE Id = @Id";
                await db.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
