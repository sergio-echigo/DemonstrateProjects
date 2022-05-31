using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Core.Entities;

public class Project
{
    [Key]
    public int Id { get; }

    public Guid UserId { get; init; }

    [StringLength(50)]
    public string Title { get; init; } = null!;

    [StringLength(200)]
    public string Description { get; init; } = null!;
}