namespace MindProject.Api.Dtos;

public class CreateNewProjectRequest {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string RepoAddress { get; set; } = null!;
}