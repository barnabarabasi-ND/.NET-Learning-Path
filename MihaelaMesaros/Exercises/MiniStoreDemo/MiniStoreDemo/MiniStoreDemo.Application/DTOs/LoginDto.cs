using System.ComponentModel.DataAnnotations;

namespace MiniStoreDemo.Application.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MaxLength(1000)]
    public string Password { get; set; } = string.Empty;
}