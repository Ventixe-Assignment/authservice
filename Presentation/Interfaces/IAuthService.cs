using Microsoft.AspNetCore.Identity;
using Presentation.Models;

namespace Presentation.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityUser?> GetUserAsync(string email);
        Task<AuthResult> LoginUserAsync(AuthLoginRequest request);
        Task<AuthResult> LogoutUserAsync();
        Task<AuthResult> RegisterUserAsync(AuthRegisterRequest request);
    }
}