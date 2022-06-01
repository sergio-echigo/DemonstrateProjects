using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Core.Entities;

public class Project
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }
    public int Index { get; init; }


    [StringLength(50)]
    public string Title { get; init; } = null!;

    [StringLength(200)]
    public string Description { get; init; } = null!;
}