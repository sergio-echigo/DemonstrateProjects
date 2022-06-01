using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Application.InputModels;

public class NewProjectModel
{
    [Required]
    [StringLength(50)]
    public string Title { get; init; } = null!;

    [StringLength(200)]
    public string Description { get; init; } = string.Empty;
}