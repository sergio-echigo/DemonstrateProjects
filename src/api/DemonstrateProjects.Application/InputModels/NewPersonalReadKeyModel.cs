using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Application.InputModels;

public class NewPersonalReadKeyModel
{
    [Required]
    public DateTimeOffset ExpiresWhen { get; set; }
}