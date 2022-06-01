namespace DemonstrateProjects.Application.InputModels;

public class RegisterUserModel
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}