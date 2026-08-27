using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace MiniStoreDemo.Infrastructure.Data;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var connString = configuration.GetConnectionString("MiniStoreDemoConnStr");
        if (string.IsNullOrWhiteSpace(connString))
        {
            throw new InvalidOperationException("Connection string 'MiniStoreDemoConnStr' is not configured.");
        }
        _connectionString = connString;
    }

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}