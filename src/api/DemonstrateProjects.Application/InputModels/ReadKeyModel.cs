using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Application.InputModels;

public class ReadKeyModel
{
    [Required]
    public string Key { get; init; } = null!;
}