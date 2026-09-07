using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.Application.Abstractions.Authentication;

public interface ITokenService
{
    LoginResponseDto GenerateToken(User user);
}