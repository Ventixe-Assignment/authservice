using Microsoft.AspNetCore.Identity;

namespace Presentation.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser user);
    }
}