using Microsoft.AspNetCore.Identity;

namespace DemonstrateProjects.Infrastructure.Persistence;

public class AppUser : IdentityUser<Guid>
{
    public new string UserName { get; set; } = null!;
    public new string Email { get; set; } = null!;
    public new string PasswordHash { get; set;} = null!;
}