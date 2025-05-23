using Microsoft.AspNetCore.Mvc;
using Presentation.Interfaces;
using Presentation.Models;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationsController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginUserAsync(request);

        return result.Success ? Ok(result) : BadRequest(result.Error);
    }

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
