using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Application.InputModels;

public class LoginUserModel
{
    [Required]
    public string Main { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}