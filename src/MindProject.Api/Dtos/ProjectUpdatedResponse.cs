namespace MindProject.Api.Dtos;

public record ProjectUpdatedResponse(
    Guid Id,
    string Name,
    string Description,
    string RepoAddress,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? EndedAt);