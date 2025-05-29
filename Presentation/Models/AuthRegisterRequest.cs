using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AuthRegisterRequest
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ConfirmPassword { get; set; } = null!;
}
