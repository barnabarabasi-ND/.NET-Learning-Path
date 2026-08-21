using Dapper;
using MiniStoreDemo.Data;
using MiniStoreDemo.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniStoreDemo.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(IDbConnectionFactory connectionFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionFactory);

        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        const string query = """
            SELECT ProductId, ProductName, ProductDescription, ProductPrice, CategoryId, IsActive, CreatedAt
            FROM Products
            ORDER BY ProductId;
        """;

        await using var connection = _connectionFactory.CreateConnection();

        var products = await connection.QueryAsync<Product>(new CommandDefinition(query, cancellationToken: cancellationToken));

        return products;
    }


    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string query = """
            SELECT ProductId, ProductName, ProductDescription, ProductPrice, CategoryId, IsActive, CreatedAt
            FROM Products
            WHERE ProductId = @Id;
        """;

        await using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Product>(new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<int> AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        const string query = """
            INSERT INTO Products (ProductName, ProductDescription, ProductPrice, CategoryId, IsActive, CreatedAt)
            OUTPUT INSERTED.ProductId
            VALUES (@ProductName, @ProductDescription, @ProductPrice, @CategoryId, @IsActive, @CreatedAt);
        """;

        await using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(new CommandDefinition(query, product, cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        const string query = """
            UPDATE Products
            SET ProductName = @ProductName,
                ProductDescription = @ProductDescription,
                ProductPrice = @ProductPrice,
                CategoryId = @CategoryId,
                IsActive = @IsActive,
                ModifiedAt = @ModifiedAt
            WHERE ProductId = @ProductId;
        """;

        await using var connection = _connectionFactory.CreateConnection();

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(query, product, cancellationToken: cancellationToken));
        return affectedRows > 0;
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        const string query = """
            DELETE FROM Products
            WHERE ProductId = @Id;
        """;

        await using var connection = _connectionFactory.CreateConnection();

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken));
        return affectedRows > 0;
    }
}