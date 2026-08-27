using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}