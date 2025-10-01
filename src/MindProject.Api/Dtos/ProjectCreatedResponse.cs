namespace MindProject.Api.Dtos;

public record ProjectCreatedResponse(
    Guid Id,
    string Name,
    string Description,
    string RepoAddress,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? EndedAt);