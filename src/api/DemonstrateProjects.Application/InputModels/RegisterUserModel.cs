using System.ComponentModel.DataAnnotations;
using DemonstrateProjects.Application.ValidationAttributes;

namespace DemonstrateProjects.Application.InputModels;

public class RegisterUserModel
{
    [Required]
    [StringLength(50)]
    [UsernameChars]
    public string Username { get; init; } = null!;

    [Required]
    [EmailAddress]
    [EmailDomain]
    public string Email { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;
}