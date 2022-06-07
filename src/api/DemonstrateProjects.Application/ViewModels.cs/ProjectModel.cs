namespace DemonstrateProjects.Application.ViewModels;

public class ProjectModel
{
    public int Index { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public string Base64Img { get; set; } = null!;
}