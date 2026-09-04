using Dapper;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Domain.Entities;
using MiniStoreDemo.Infrastructure.Data;
using System.Data;

namespace MiniStoreDemo.Infrastructure.Repositories;

public sealed class UserRepository(IDbConnectionFactory connectionFactory) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(
                "dbo.User_GetByUsername",
                new { Username = username },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
        ));

    }
}