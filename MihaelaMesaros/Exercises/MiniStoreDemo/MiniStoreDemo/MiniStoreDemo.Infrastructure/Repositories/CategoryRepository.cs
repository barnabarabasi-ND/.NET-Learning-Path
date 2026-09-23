using Dapper;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Infrastructure.Data;
using System.Data;

namespace MiniStoreDemo.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CategoryRepository(IDbConnectionFactory connectionFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionFactory);

        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CheckCategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add("CategoryId", categoryId);
        parameters.Add("outCategoryExists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        await using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            new CommandDefinition(
                "dbo.Category_CheckExists",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );

        return parameters.Get<bool>("outCategoryExists");
    }
}
