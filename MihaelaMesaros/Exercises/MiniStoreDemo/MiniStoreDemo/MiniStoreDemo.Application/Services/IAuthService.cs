using MiniStoreDemo.Application.DTOs;

namespace MiniStoreDemo.Application.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
}