using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AuthLoginRequest
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}
