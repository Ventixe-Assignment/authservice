using Microsoft.AspNetCore.Identity;
using Presentation.Interfaces;
using Presentation.Models;

namespace Presentation.Services;
public class AuthService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ITokenService tokenService) : IAuthService
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;


    public async Task<AuthResult> LoginUserAsync(AuthLoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);

        if (!result.Succeeded)
            return new AuthResult { Success = false, Error = "Invalid login attempt." };

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new AuthResult { Success = false, Error = "User not found." };

        var token = _tokenService.GenerateToken(user);

        return new AuthResult { Success = true, Token = token };

    }

    public async Task<IdentityUser?> GetUserAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return null;

        return user;
    }

    public async Task<AuthResult> RegisterUserAsync(AuthRegisterRequest request)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        return result.Succeeded
            ? new AuthResult { Success = true }
            : new AuthResult { Success = false, Error = "Error during registration" };
    }

    public async Task<AuthResult> LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();
        return new AuthResult { Success = true };
    }
}
