using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Core.Entities;

public class PersonalReadKey
{
    [Key]
    public Guid Key { get; } = Guid.NewGuid();

    public Guid UserId { get; init; }
    public DateTimeOffset ExpiresWhen { get; init; }
}