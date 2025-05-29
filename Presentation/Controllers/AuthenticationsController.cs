using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Interfaces;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers;
 
[Route("api/[controller]")]
[ApiController]
public class AuthenticationsController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> FetchUser()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
            return Unauthorized();

        var user = await _authService.GetUserAsync(email);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
    {
        if(!ModelState.IsValid)
        {
            var errors = ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .Select(x => new
            {
                Field = x.Key,
                Errors = x.Value!.Errors.Select(e => e.ErrorMessage)
            });

            return BadRequest(new { message = "Validation failed", errors });
        }

        var result = await _authService.LoginUserAsync(request);

        return result.Success ? Ok(result) : BadRequest(result.Error);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (request.Password != request.ConfirmPassword)
            return BadRequest("Password and Confirm Password do not match.");


        var result = await _authService.RegisterUserAsync(request);
        return result.Success ? Ok(result) : BadRequest(result.Error);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.LogoutUserAsync();
        return result.Success ? Ok(result) : BadRequest(result.Error);
    }
}
