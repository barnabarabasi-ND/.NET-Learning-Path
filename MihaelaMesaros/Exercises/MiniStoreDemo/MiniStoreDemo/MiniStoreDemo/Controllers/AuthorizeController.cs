using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.DTOs;
using MiniStoreDemo.Services;

namespace MiniStoreDemo.Controllers;

[ApiController]
[Route("api/authorize")]
public sealed class AuthorizeController(IAuthService authService) : ControllerBase
{
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