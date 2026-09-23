using Dapper;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Infrastructure.Data;
using MiniStoreDemo.Domain.Entities;
using System.Data;

namespace MiniStoreDemo.Infrastructure.Repositories;

public sealed class ProductQueryRepository : MiniStoreDemo.Application.Abstractions.Persistence.IProductQueryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductQueryRepository(IDbConnectionFactory connectionFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionFactory);

        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize, int? categoryId, bool? isActive, string? keyword, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            CategoryId = categoryId,
            IsActive = isActive,
            Keyword = keyword
        };

        await using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Product>(
            new CommandDefinition(
                "dbo.Product_GetAll",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Product>(
            new CommandDefinition(
                "dbo.Product_GetById",
                new { ProductId = id },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<bool> CheckProductExistsAsync(string productName, int categoryId, int? excludeProductId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add("ProductName", productName);
        parameters.Add("CategoryId", categoryId);
        parameters.Add("ExcludeProductId", excludeProductId);
        parameters.Add("outProductExists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        await using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            new CommandDefinition(
                "dbo.Product_CheckAlreadyExists",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );

        return parameters.Get<bool>("outProductExists");
    }
}
