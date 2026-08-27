using Microsoft.AspNetCore.Identity;
using MiniStoreDemo.Application.Abstractions.Authentication;
using MiniStoreDemo.Application.Abstractions.Persistence;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Domain.Entities;

namespace MiniStoreDemo.Application.Services;

public sealed class AuthService(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher<User> passwordHasher) : IAuthService
{
    public async Task<LoginResponseDto?> LoginAsync(
        LoginDto loginDto,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(
            loginDto.Username,
            cancellationToken);

        if (user is null || !user.IsActive)
            return null;

        var verificationResult = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            loginDto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return null;

        return tokenService.GenerateToken(user);
    }
}