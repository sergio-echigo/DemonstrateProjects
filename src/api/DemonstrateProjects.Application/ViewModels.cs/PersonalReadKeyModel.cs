using System.Text.Json.Serialization;

namespace DemonstrateProjects.Application.ViewModels;

public class PersonalReadKeyModel
{
    public Guid Key { get; set; }
    public DateTimeOffset ExpiresWhen { get; set; }

    [JsonIgnore]
    public Guid UserId { get ; set; }
}