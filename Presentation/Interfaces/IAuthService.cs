using Presentation.Models;

namespace Presentation.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> LoginUserAsync(AuthLoginRequest request);
        Task<AuthResult> LogoutUserAsync();
        Task<AuthResult> RegisterUserAsync(AuthRegisterRequest request);
    }
}