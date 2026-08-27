using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Application.Services;

namespace MiniStoreDemo.Api.Controllers;

[ApiController]
[Route("api/authorize")]
public sealed class AuthorizeController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Login endpoint for user authentication.
    /// </summary>
    /// <param name="loginDto">The login credentials of the user.</param>
    /// <returns>The login response containing the authentication token.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(loginDto, cancellationToken);

        if (result is null)
            return Unauthorized();

        return Ok(result);
    }
}