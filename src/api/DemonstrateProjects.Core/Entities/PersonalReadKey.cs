using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Core.Entities;

public class PersonalReadKey
{
    [Key]
    public Guid Key { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }
    public DateTimeOffset ExpiresWhen { get; init; }
}